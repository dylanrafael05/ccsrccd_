using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using TUtils.Constants;
using ChemistryClass;
using Terraria.ModLoader;
using System.ComponentModel;

namespace TUtils.WorldGeneration {

    //CLASS FOR REPLACING A TILE

    public struct TileReplacer {

        public enum LiquidType : byte {
            Water,
            Honey,
            Lava
        }

        public enum FormType : byte {
            Basic,
            Half,
            DownRight,
            DownLeft,
            UpRight,
            UpLeft
        }

        public bool keepTile;
        public bool keepWall;
        public bool keepTileForm;
        public bool keepMechanics;

        public bool smoothSlope;

        public byte liquidLevel;
        public LiquidType liquidType;

        public FormType form;

        public ushort tileType;
        public ushort wallType;

        public bool redWire;
        public bool blueWire;
        public bool greenWire;
        public bool yellowWire;
        public bool actuator;

        public TileReplacer(ushort tileType, ushort wallType = 0, FormType form = FormType.Basic, byte liquidLevel = 0,
                            LiquidType liquidType = LiquidType.Water, bool redWire = false, bool blueWire = false,
                            bool greenWire = false, bool yellowWire = false, bool actuator = false, bool keepTile = false,
                            bool keepWall = true, bool keepTileForm = false, bool keepMechanics = false,
                            bool smoothSlope = false) {

            this.tileType = tileType;
            this.wallType = wallType;
            this.form = form;
            this.liquidLevel = liquidLevel;
            this.liquidType = liquidType;
            this.redWire = redWire;
            this.blueWire = blueWire;
            this.greenWire = greenWire;
            this.yellowWire = yellowWire;
            this.actuator = actuator;
            this.keepTile = keepTile;
            this.keepWall = keepWall;
            this.keepTileForm = keepTileForm;
            this.keepMechanics = keepMechanics;
            this.smoothSlope = smoothSlope;

        }

        public static readonly TileReplacer KeepAll = new TileReplacer(0, keepTile: true, keepWall: true, smoothSlope: false);
        public static TileReplacer Multitile(ushort multitileType) => new TileReplacer(multitileType, keepTileForm: true, keepMechanics: true);

        public void ReplaceTileAt(int i, int j) {

            Tile toReplace = Framing.GetTileSafely(i, j);

            if( !keepTile ) {

                WorldGen.PlaceTile(i, j, tileType, true, true);

                if ( liquidLevel != 0 ) {

                    toReplace.liquid = liquidLevel;
                    toReplace.honey(liquidType == LiquidType.Honey);
                    toReplace.lava(liquidType == LiquidType.Lava);

                }

                if ( !keepTileForm ) {

                    toReplace.slope(form == FormType.Half ? (byte)0 : (byte)form);
                    toReplace.halfBrick(form == FormType.Half);

                }

                if( !keepMechanics ) {

                    toReplace.wire (redWire);
                    toReplace.wire2(blueWire);
                    toReplace.wire3(greenWire);
                    toReplace.wire4(yellowWire);
                    toReplace.actuator(actuator);

                }

            }

            if ( !keepWall ) {

                toReplace.wall = 0;
                WorldGen.PlaceWall(i, j, wallType, true);

            }

            if ( smoothSlope ) {

                Tile.SmoothSlope(i, j);

            }

        }

        public override string ToString()
            => $"Tile: {tileType} ({keepTile}). Wall: {wallType} ({keepWall}). " +
               $"Liquid: {liquidType} @ {liquidLevel}. Form: {form} ({keepTileForm}). " +
               $"Mechanics: {redWire}, {blueWire}, {greenWire}, {yellowWire} w/ {actuator} " +
               $"({keepMechanics}).";

    }

    public static class WGUtils {

        //GENERAL AREA EFFECTS
        public static void ClearTile(int i, int j) {
            WorldGen.KillTile(i, j, false, false, true);
            WorldGen.KillWall(i, j, false);
        }

        public static void ForceClearTile(int i, int j) {
            Tile c = Framing.GetTileSafely(i, j);
            c.ClearEverything();
        }

        public static void FillTile(int i, int j, ushort type, ushort wall = 0) {
            WorldGen.PlaceTile(i, j, type, true, true);
            WorldGen.PlaceWall(i, j, wall);
        }

        public static void ForceFillTile(int i, int j, ushort type, ushort wall = 0) {
            Tile c = Framing.GetTileSafely(i, j);
            c.active(true);
            c.type = type;
            c.wall = wall;
        }

        public static void ClearArea(int i, int j, int w, int h) {

            for (int rI = i; rI < i + w; rI++) {
                for (int rJ = j; rJ < j + h; rJ++) {
                    ClearTile(rI, rJ);
                }
            }

        }

        public static void ClearArea(Rectangle rectangle)
            => ClearArea(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        public static void ForceClearArea(int i, int j, int w, int h) {

            for (int rI = i; rI < i + w; rI++) {
                for (int rJ = j; rJ < j + h; rJ++) {
                    ForceClearTile(rI, rJ);
                }
            }

        }

        public static void ForceClearArea(Rectangle rectangle)
            => ForceClearArea(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        public static void FillArea(int i, int j, int w, int h, ushort type, ushort wall = 0) {

            for (int rI = i; rI < i + w; rI++) {
                for (int rJ = j; rJ < j + h; rJ++) {
                    FillTile(rI, rJ, type, wall);
                }
            }

        }

        public static void FillArea(Rectangle rectangle, ushort type, ushort wall = 0)
            => FillArea(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, type, wall);

        public static void ForceFillArea(int i, int j, int w, int h, ushort type, ushort wall = 0) {

            for (int rI = i; rI < i + w; rI++) {
                for (int rJ = j; rJ < j + h; rJ++) {
                    ForceFillTile(rI, rJ, type, wall);
                }
            }

        }

        public static void ForceFillArea(Rectangle rectangle, ushort type, ushort wall = 0)
            => ForceFillArea(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, type, wall);

        //AREA POLLING
        public static bool PollAreaAny(int i, int j, int w, int h, Delegates.TilePredicate poll) {

            for (int rI = i; rI < i + w; rI++) {
                for (int rJ = j; rJ < j + h; rJ++) {
                    if (poll(Main.tile[rI, rJ])) return true;
                }
            }

            return false;

        }

        public static bool PollAreaAny(Rectangle rectangle, Delegates.TilePredicate poll)
            => PollAreaAny(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, poll);

        public static bool PollAreaAll(int i, int j, int w, int h, Delegates.TilePredicate poll) {

            for (int rI = i; rI < i + w; rI++) {
                for (int rJ = j; rJ < j + h; rJ++) {
                    if (!poll(Main.tile[rI, rJ])) return false;
                }
            }

            return true;

        }

        public static bool PollAreaAll(Rectangle rectangle, Delegates.TilePredicate poll)
            => PollAreaAll(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, poll);

        public static readonly Delegates.CoordinateAction NoCoordAction = (i, j) => { };
        public static readonly Delegates.CoordinateAction RemoveTileAtCoord = (i, j) => ForceClearTile(i, j);
        public static readonly Delegates.AreaAction NoAreaAction = rec => { };
        public static readonly Delegates.AreaAction RemoveTilesInArea = rec => ForceClearArea(rec);
        public static readonly Delegates.TilePredicate TileAny = t => true;
        public static readonly Delegates.TilePredicate TileIsEmpty = t => !t.active();
        public static readonly Delegates.TilePredicate TileIsActive = t => !TileIsEmpty(t);
        public static readonly Delegates.CoordinatePredicate CoordinateAny = (i, j) => true;
        public static readonly Delegates.CoordinatePredicate CoordinateIsEmpty = (i, j) => !Framing.GetTileSafely(i, j).active();
        public static readonly Delegates.CoordinatePredicate CoordinateIsActive = (i, j) => !CoordinateIsEmpty(i, j);
        public static readonly Delegates.AreaPredicate AreaAny = rec => true;
        public static readonly Delegates.AreaPredicate AreaIsEmpty = rec => PollAreaAll(rec, TileIsEmpty);
        public static readonly Delegates.AreaPredicate AreaIsActive = rec => PollAreaAll(rec, TileIsActive);
        public static Delegates.TilePredicate CreateTileIs(params int[] ids) => t => ids.Contains(t.type);
        public static Delegates.CoordinatePredicate CreateCoordinateIs(params int[] ids) => (i, j) => CreateTileIs(ids)(Framing.GetTileSafely(i, j));
        public static Delegates.AreaPredicate CreateAreaIs(params int[] ids) => rec => PollAreaAll(rec, CreateTileIs(ids));

    }

    //STRUCTURE
    public class Structure { //: IStructure {

        public delegate TileReplacer[,] EditTileData(TileReplacer[,] @in);
        public static EditTileData NoEdits = @in => @in;

        public static TileReplacer[,] TileData { get; set; }
        private protected EditTileData editTileData;
        private protected Delegates.CoordinateAction prePlace;
        private protected Delegates.AreaAction preGenerate;
        private protected Delegates.CoordinatePredicate canPlace;
        private protected Delegates.AreaPredicate canGenerate;

        public Structure(Dictionary<char, TileReplacer> pairing, string[] data,
                         Delegates.CoordinateAction prePlace, Delegates.AreaAction preGenerate,
                         Delegates.CoordinatePredicate canPlace, Delegates.AreaPredicate canGenerate,
                         EditTileData editTileData) {

            this.editTileData = editTileData;
            this.prePlace = prePlace;
            this.preGenerate = preGenerate;
            this.canPlace = canPlace;
            this.canGenerate = canGenerate;

            Parse(data, pairing);

        }

        public Structure(Dictionary<char, TileReplacer> pairing, string[] data,
                         Delegates.CoordinateAction prePlace, Delegates.AreaAction preGenerate,
                         Delegates.CoordinatePredicate canPlace, Delegates.AreaPredicate canGenerate) {

            this.editTileData = NoEdits;
            this.prePlace = prePlace;
            this.preGenerate = preGenerate;
            this.canPlace = canPlace;
            this.canGenerate = canGenerate;

            Parse(data, pairing);

        }

        public Structure(Delegates.CoordinateAction prePlace, Delegates.AreaAction preGenerate,
                         Delegates.CoordinatePredicate canPlace, Delegates.AreaPredicate canGenerate,
                         EditTileData editTileData) {

            this.editTileData = editTileData;
            this.prePlace = prePlace;
            this.preGenerate = preGenerate;
            this.canPlace = canPlace;
            this.canGenerate = canGenerate;

            TileData = null;

        }

        public bool Generate(int i, int j) {

            TileData = editTileData(TileData);

            int w = TileData.GetLength(0);
            int h = TileData.GetLength(1);
            int wldI, wldJ;

            i -= w / 2;
            j -= h / 2;

            Rectangle effectArea = new Rectangle(i, j, w, h);

            if (!canGenerate( effectArea )) return false;
            preGenerate( effectArea );

            for (int iInd = 0; iInd < w; iInd++) {
                for (int jInd = 0; jInd < h; jInd++) {

                    wldI = i + iInd;
                    wldJ = j + jInd;

                    if (!canPlace(wldI, wldJ)) continue;
                    prePlace(wldI, wldJ);

                    TileData[iInd, jInd].ReplaceTileAt(wldI, wldJ);
                    //WorldGen.PlaceTile(wldI, wldJ, TileData[iInd, jInd].tileType);

                }
            }

            return true;

        }
         
        private protected void Parse(string[] data, Dictionary<char, TileReplacer> pairing) {

            TileData = new TileReplacer[data.Length, data.Max(s => s.Length)];

            for (int i = 0; i < data.Length; i++) {
                for (int j = 0; j < data.Max(s => s.Length); j++) {

                    if (pairing.ContainsKey(data[i].GetVoided(j))) {
                        TileData[i, j] = pairing[data[i].GetVoided(j)];
                    } else {
                        TileData[i, j] = TileReplacer.KeepAll;
                    }

                    //DEBUGGING
                    ChemistryClass.ChemistryClass.ModLogger.Debug(TileData[i, j].ToString());

                }
            }
        }
    }

    public static class FadeStructureGeneration {

        public static TileReplacer[,] MapToTileData(float[,] map, TileReplacer replacement) {

            int mW = map.GetLength(0);
            int mH = map.GetLength(1);

            TileReplacer[,] ret = new TileReplacer[mW, mH];

            for (int i = 0; i < mW; i++) {
                for (int j = 0; j < mH; j++) {

                    if (Logic.EvaluateChanceWGen(map[i, j])) {

                        ret[i, j] = replacement;

                    } else {

                        ret[i, j] = TileReplacer.KeepAll;

                    }

                }
            }

            return ret;

        }

        public static float[,] AddMaps(ref float[,] main, float[,] other) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] += other.GetVoided(i, j);

                }
            }

            return ClampValues(ref main);

        }

        public static float[,] SubtractMaps(ref float[,] main, float[,] other) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] -= other.GetVoided(i, j);

                }
            }

            return ClampValues(ref main);

        }

        public static float[,] ScaleValues(ref float[,] main, float factor) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] *= factor;

                }
            }

            return ClampValues(ref main);

        }

        public static float[,] ExponentiateValues(ref float[,] main, float factor) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] = (float)Math.Pow(main[i, j], factor);

                }
            }

            return main;

        }

        public static float[,] ClampValues(ref float[,] main) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    Mathematics.LimitMin(ref main[i, j], 0);

                }
            }

            return main;

        }

        public static float[,] ResizeCanvas(ref float[,] main, int w, int h, int displacementX = 0, int displacementY = 0) {

            float[,] newData = new float[w, h];

            int realDX = displacementX + w / 2 - main.GetLength(0) / 2;
            int realDY = displacementY + h / 2 - main.GetLength(1) / 2;

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {

                    newData[i, j] = main.GetVoided(i - realDX, j - realDY);

                }
            }

            return main = ClampValues( ref newData );

        }

        public static float[,] MakeRandomEllipse(int w, int h, int steps = 20) {

            float[,] newData = MakeEllipseGradient(w, h);
            ResizeCanvas(ref newData, w, h);

            for (int step = 0; step < steps; step++) {

                int randW = WorldGen.genRand.Next(w / 16, w / 6);
                int randH = WorldGen.genRand.Next(w / 16, w / 6);

                Mathematics.Clamp(ref randW, 2, w);
                Mathematics.Clamp(ref randH, 2, h);

                int randI = WorldGen.genRand.Next(0, w);
                int randJ = WorldGen.genRand.Next(0, h);

                if (newData[randI, randJ] < 0.1f || newData[randI, randJ] > 0.9f) {
                    step--;
                    continue;
                }

                float[,] randEllipse = MakeEllipseGradient(randW, randH);
                ResizeCanvas(ref randEllipse, w, h, randI, randJ);
                ScaleValues(ref randEllipse, 0.8f);

                if (newData[randI, randJ] > 0.5f && Logic.EvaluateChanceWGen(0.9f)) AddMaps(ref newData, randEllipse);
                else SubtractMaps(ref newData, randEllipse);

            }

            Smooth(ref newData, 5);
            ScaleValues(ref newData, 1.3f);

            return ClampValues(ref newData);

        }

        public static float[,] MakeCircleGradient(int size) {

            float[,] newData = new float[size, size];
            Vector2 center = new Vector2(size / 2f, size / 2f);
            Vector2 curVec;

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {

                    curVec = new Vector2(i, j);
                    newData[i, j] = 1f - 2 * Vector2.Distance(curVec, center) / size;

                }
            }

            return ClampValues(ref newData);

        }

        public static float[,] MakeEllipseGradient(int w, int h) {

            float[,] newData = MakeCircleGradient(Math.Min(w, h));
            Resize(ref newData, w, h);

            return ClampValues(ref newData);

        }

        public static float[,] Smooth(ref float[,] map, int passes = 1) {

            float[,] newData = map;

            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(1); j++) {

                    newData[i, j] =
                        (map.GetBounded(i, j) + map.GetBounded(i + 1, j) +
                         map.GetBounded(i, j + 1) + map.GetBounded(i - 1, j) +
                         map.GetBounded(i, j - 1)) / 5f;

                }
            }

            if (--passes > 0) return Smooth(ref map, passes);
            else return map = ClampValues(ref newData);

        }

        public static float[,] Resize(ref float[,] map, int w, int h) {

            float[,] newData = new float[w, h];

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {

                    double iOrig = Mathematics.Map(i, 0d, w - 1, 0, map.GetLength(0) - 1);// + 0.01;
                    double jOrig = Mathematics.Map(j, 0d, h - 1, 0, map.GetLength(1) - 1);// + 0.01;

                    //ChemistryClass.ChemistryClass.ModLogger.Debug($"From Resize: {iOrig}, {jOrig}");

                    //try {
                    newData[i, j] = map.GetRounded(iOrig, jOrig);
                    //} catch (Exception e) {
                      //  ChemistryClass.ChemistryClass.ModLogger.Error($"Attempting to access index {iOrig}, {jOrig} of array {map}");
                    //}

                }
            }

            return map = Smooth(ref newData);

        }

    }

    /*

    public abstract class SpreadStructure : IStructure {

        public static float[,] tileChances { get; protected set; }
        public static ushort tileType;

        private protected SpreadStructure() { }

        public abstract bool CanGenerate(int i, int j);
        public abstract bool CanPlace(int i, int j);
        public abstract IStructure GetInstance();

        public virtual void PreGenerate() { }
        public virtual bool Generate(int i, int j) {

            if (!CanGenerate(i, j)) return false;
            PreGenerate();

            for (int iOff = 0; iOff < tileChances.GetLength(0); iOff++) {
                for (int jOff = 0; jOff < tileChances.GetLength(1); jOff++) {

                    if (!CanPlace(i + iOff, j + jOff)) continue;

                    if (Logic.EvaluateChanceWGen(tileChances[0, 1])) {

                        Framing.GetTileSafely(i + iOff, j + jOff).type = tileType;

                    }

                }
            }

            return true;

        }

        //MAPPING IMPLEMENTATION
        protected static float[,] AddMaps(ref float[,] main, float[,] other) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] += other.GetVoided(i, j);

                }
            }

            return main;

        }

        protected static float[,] SubtractMaps(ref float[,] main, float[,] other) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] -= other.GetVoided(i, j);

                }
            }

            return main;

        }

        protected static float[,] ScaleValues(ref float[,] main, float factor) {

            for (int i = 0; i < main.GetLength(0); i++) {
                for (int j = 0; j < main.GetLength(1); j++) {

                    main[i, j] *= factor;

                }
            }

            return main;

        }

        protected static float[,] ResizeCanvas(ref float[,] main, int w, int h, int displacementX, int displacementY) {

            float[,] newData = new float[w, h];

            int realDX = displacementX + w / 2 - main.GetLength(0) / 2;
            int realDY = displacementY + h / 2 - main.GetLength(1) / 2;

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {

                    newData[i, j] = main.GetVoided(i - realDX, j - realDY);

                }
            }

            return main = newData;

        }

        protected static float[,] MakeRandomEllipse(int w, int h, int steps = 20) {

            float[,] newData = MakeEllipseGradient(2 * w / 3, 2 * h / 3);
            Resize(ref newData, w, h);

            for (int step = 0; step < steps; step++) {

                int randW = WorldGen.genRand.Next(w / 16, w / 6);
                int randH = WorldGen.genRand.Next(w / 16, w / 6);

                int randI = WorldGen.genRand.Next(0, w);
                int randJ = WorldGen.genRand.Next(0, h);

                if (newData[randI, randJ] < 0.1f || newData[randI, randJ] > 0.7f) {
                    step--;
                    continue;
                }

                float[,] randEllipse = MakeEllipseGradient(randW, randH);
                ResizeCanvas(ref randEllipse, w, h, randI, randJ);
                ScaleValues(ref randEllipse, 0.8f);

                if (Logic.EvaluateChanceWGen(newData[randI, randJ])) AddMaps(ref newData, randEllipse);
                else SubtractMaps(ref newData, randEllipse);

            }

            Smooth(ref newData, 3);
            ScaleValues(ref newData, 1.2f);

            return newData;

        }

        protected static float[,] MakeCircleGradient(int size) {

            float[,] newData = new float[size, size];
            Vector2 center = new Vector2(size / 2f, size / 2f);
            Vector2 curVec;

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {

                    curVec = new Vector2(i, j);
                    newData[i, j] = (1f - 2 * Vector2.Distance(curVec, center) / size);

                }
            }

            return newData;

        }

        protected static float[,] MakeEllipseGradient(int w, int h) {

            float[,] newData = MakeCircleGradient(Math.Min(w, h));
            Resize(ref newData, w, h);

            return newData;

        }

        protected static float[,] Smooth(ref float[,] map, int passes = 1) {

            float[,] newData = map;

            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(1); j++) {

                    newData[i, j] =
                        (map.GetBounded(i, j) + map.GetBounded(i + 1, j) +
                         map.GetBounded(i, j + 1) + map.GetBounded(i - 1, j) +
                         map.GetBounded(i, j - 1)) / 5f;

                }
            }

            if (--passes > 0) return Smooth(ref map, passes);
            else return map = newData;

        }

        protected static float[,] Resize(ref float[,] map, int w, int h) {

            float[,] newData = new float[w, h];

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {

                    float iOrig = (float)i / w * map.GetLength(0);
                    float jOrig = (float)j / h * map.GetLength(1);
                    newData[i, j] = map.GetRounded(iOrig, jOrig);

                }
            }

            return map = Smooth(ref newData);

        }

    }*/

}
