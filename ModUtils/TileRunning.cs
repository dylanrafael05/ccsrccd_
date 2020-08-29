using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChemistryClass.ModUtils {
    [Flags] public enum TileRunStatus : byte {
        EMPTY         = 0b_0_0001,
        BORDER        = 0b_0_0010,
        TILE          = 0b_0_0100,
        RUN_COMPLETED = 0b_0_1000,
        RUN_BORDER    = 0b_1_0000
    }

    public struct TileRunItem {

        public int I;
        public int J;
        public TileRunStatus Status;
        public float Weight;

        public TileRunItem(int i, int j, float w, TileRunStatus s) {
            this.I = i;
            this.J = j;
            Weight = w;
            Status = s;
        }

        public static TileRunItem FromTile(int i, int j, float w = -1) {
            Tile curTile = Framing.GetTileSafely(i, j);
            return new TileRunItem(i, j, w, curTile.active() ? TileRunStatus.TILE : TileRunStatus.EMPTY);
        }

        public void AddStatus(TileRunStatus s) => Status |= s;
        public void RemoveStatus(TileRunStatus s) => Status &= ~s;

        public bool Empty {
            get => Status.HasFlag(TileRunStatus.EMPTY);
            set {
                if (value) AddStatus(TileRunStatus.EMPTY);
                else RemoveStatus(TileRunStatus.EMPTY);
            }
        }

        public bool Tile {
            get => Status.HasFlag(TileRunStatus.TILE);
            set {
                if (value) AddStatus(TileRunStatus.TILE);
                else RemoveStatus(TileRunStatus.TILE);
            }
        }

        public bool Border {
            get => Status.HasFlag(TileRunStatus.BORDER);
            set {
                if (value) AddStatus(TileRunStatus.BORDER);
                else RemoveStatus(TileRunStatus.BORDER);
            }
        }

        public bool RunComplete {
            get => Status.HasFlag(TileRunStatus.RUN_COMPLETED);
            set {
                if (value) AddStatus(TileRunStatus.RUN_COMPLETED);
                else RemoveStatus(TileRunStatus.RUN_COMPLETED);
            }
        }

        public bool RunBorder {
            get => Status.HasFlag(TileRunStatus.RUN_BORDER);
            set {
                if (value) AddStatus(TileRunStatus.RUN_BORDER);
                else RemoveStatus(TileRunStatus.RUN_BORDER);
            }
        }

        public override string ToString()
            => $"[{RunComplete}]: ({I}, {J}) => {Status} @ {Weight}";
    }

    public struct TileRun : IEnumerable<TileRunItem> {
        public List<TileRunItem> Items;
        public TileRunItem this[int i, int j]
        {
            get => Items.First(item => item.I == i && item.J == j);
            set => Items[Items.FindIndex(item => item.I == i && item.J == j)] = value;
        }

        public TileRun(List<TileRunItem> items) => Items = items;
        public TileRun(params TileRunItem[] items) => Items = items.ToList();
        public TileRun(IEnumerable<TileRunItem> items) => Items = items.ToList();

        public IEnumerator<TileRunItem> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public bool ContainsPoint(int i, int j) => Items.Exists(item => item.I == i && item.J == j);

        public object[] GetLoadedNeighbors(int i, int j) {
            object[] ret = new object[4];
            ret[0] = ContainsPoint(i - 1, j) ? this[i - 1, j] as object : new Point(i - 1, j) as object;
            ret[1] = ContainsPoint(i + 1, j) ? this[i + 1, j] as object : new Point(i + 1, j) as object;
            ret[2] = ContainsPoint(i, j - 1) ? this[i, j - 1] as object : new Point(i, j - 1) as object;
            ret[3] = ContainsPoint(i, j + 1) ? this[i, j + 1] as object : new Point(i, j + 1) as object;
            return ret;
        }

        public byte CountLoadedNeighbors(int i, int j) {
            byte ret = 0;
            if (ContainsPoint(i - 1, j)) ret++;
            if (ContainsPoint(i + 1, j)) ret++;
            if (ContainsPoint(i, j - 1)) ret++;
            if (ContainsPoint(i, j + 1)) ret++;
            return ret;
        }

        public bool ExecuteOn(ref TileRunItem center, float weightToAdd, float maxWeight, float randWeight = 0) {

            if (center.RunComplete) return false;

            object[] neighbors = GetLoadedNeighbors(center.I, center.J);
            byte nSolidCount = 0;
            byte nInvalidCount = 0;

            List<TileRunItem> toAdd = new List<TileRunItem>();

            foreach(var neighborObj in neighbors) {

                if (neighborObj is Point neighbor) {

                    TileRunItem newItem = TileRunItem.FromTile(neighbor.X, neighbor.Y, center.Weight);

                    if (newItem.Tile) {
                        nSolidCount++;
                        newItem.Weight += weightToAdd + Main.rand.NextFloat(-randWeight, randWeight);
                    }

                    if (newItem.Weight <= maxWeight) toAdd.Add(newItem);
                    else nInvalidCount++;

                } else if(neighborObj is TileRunItem neighborI) {
                    nInvalidCount++;
                    if (neighborI.Tile) nSolidCount++;
                } else nInvalidCount++;

            }

            if (nSolidCount < 4) center.Border = true;

            Items.AddRange(toAdd);
            center.RunComplete = true;

            return nInvalidCount < 4;
        }

        public bool Execute(float weightToAdd, float maxWeight, float randWeight = 0f) {
            bool ret = false;
            int len = Items.Count;
            for (int i = 0; i < len; i++) {
                TileRunItem newItem = Items[i];
                ret |= ExecuteOn(ref newItem, weightToAdd, maxWeight, randWeight);
                Items[i] = newItem;
            }
            return ret;
        }

        public void SetBorders() {
            for (int i = 0; i < Items.Count; i++) {
                if (CountLoadedNeighbors(Items[i].I, Items[i].J) < 4) {
                    TileRunItem item = Items[i];
                    item.RunBorder = true;
                    Items[i] = item;
                }
            }
        }

        public bool ExecuteFull(float weightToAdd, float maxWeight, Predicate<TileRun> endEarly, float randWeight = 0f) {
            int attempts = 0;
            while(!endEarly(this) && attempts++ < 10_000) {
                if(!Execute(weightToAdd, maxWeight, randWeight)) {
                    //ChemistryClass.Logging.Debug($"{Items.Count}: {Items.Count(i => i.RunComplete)}");
                    SetBorders();
                    //ChemistryClass.Logging.Debug($"{Items.Count}: {Items.Count(i => i.RunComplete)}");
                    return true;
                }
                //ChemistryClass.Logging.Debug($"{Items.Count}: {Items.Count(i => i.RunComplete)}");
            }
            SetBorders();
            return false;
        }

        public override string ToString()
            => string.Join("; ", Items);
    }
}
