using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using System.Linq;
using ChemistryClass.ModUtils;
using System.Collections;
using System.Drawing.Drawing2D;

namespace ChemistryClass {
    public class ChemistryClassWorld : ModWorld {

        //BIOME SHIZ
        public static ushort SulfurOreHeartType => (ushort)ModContent.TileType<Tiles.Blocks.SulfuricOreHeartTile>();
        public static ushort SulfurOreType => (ushort)ModContent.TileType<Tiles.Blocks.SulfuricOreTile>();
        public static ushort SulfurHeartType => (ushort)ModContent.TileType<Tiles.Multitiles.SulfurHeartTile>();

        public static int sulfurCount;
        public static int sulfurHeartCount;

        //QUARTZ LIGHTNING MANAGEMENT
        public const int quartzLightningTimer = 30;
        public const int quartzLightningMax = 2;

        private static int _qLC = 0;
        public static int QuartzLightningCount {
            get => _qLC;
            set {
                _qLC = value;
                QuartzLightningValid &= _qLC < quartzLightningMax;
            }
        }

        public static bool QuartzLightningValid { get; private set; } = false;

        //BIOME INFORMATION
        public override void ResetNearbyTileEffects() {

            sulfurCount = 0;
            sulfurHeartCount = 0;
            base.ResetNearbyTileEffects();

        }

        public override void TileCountsAvailable(int[] tileCounts) {

            sulfurCount = tileCounts[SulfurOreHeartType] + tileCounts[SulfurOreType];
            sulfurHeartCount = tileCounts[SulfurHeartType];
            base.TileCountsAvailable(tileCounts);

        }

        //WORLD GEN TASKS
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {

            int index = tasks.FindIndex(t => t.Name == "Spider Caves");

            if(index > -1) {

                PassLegacy pass = new PassLegacy("Sulfuric Deposit", GenerateSulphuricDeposit);
                tasks.Insert(++index, pass);

            }

            index = tasks.FindIndex(t => t.Name == "Gem Caves");

            if (index > -1) {

                PassLegacy pass = new PassLegacy("Quartz Caves", GenerateQuartzCaves);
                tasks.Insert(++index, pass);

            }

        }

        //GENERATION OF SULFUR DEPOSITS
        private void GenerateSulphuricDeposit(GenerationProgress progress) {

            progress.Message = "Generating Sulfuric Deposits";

            //VARIABLE DECALRATIONS
            int worldCenter = Main.maxTilesX / 2;
            int tileSpread = Main.maxTilesX / 4;

            int iCenter, jCenter;
            int w, h;

            Tile curTile;

            int amount = WorldGen.genRand.Next(2, 4) + (Main.maxTilesX - 4200) / 1500;

            //DEBUGGING
            //amount = 20;

            int deposit = 0;
            int realDeposit = 0;

            //GENRATE DEPOSIT
            while(deposit < amount && realDeposit < 1000 * amount) {

                //INCREMENT DEPOSIT COUNTER
                deposit++;

                //LABEL TO GOTO WHEN GENERATION NEEDS TO BE RETRIED
                RETRY:
                realDeposit++;

                //CHOOSE CENTER POINT
                iCenter = WorldGen.genRand.Next(worldCenter - tileSpread, worldCenter + tileSpread);
                jCenter = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 200);

                //ENSURE THAT ONLY GENERATES IN UNDERGROUND BIOME, AND WHERE THERE IS AN 10x10 FILLED SPACE
                for(int iOff = -5; iOff < 5; iOff++) {
                    for (int jOff = -5; jOff < 5; jOff++) {

                        curTile = Framing.GetTileSafely(iCenter + iOff, jCenter + jOff);

                        if (curTile.type != TileID.Stone && curTile.type != TileID.Dirt &&
                            curTile.type != TileID.Stalactite && curTile.type != TileID.Ruby &&
                            curTile.type != TileID.Emerald && curTile.type != TileID.Amethyst &&
                            curTile.type != TileID.Diamond && curTile.type != TileID.Topaz &&
                            curTile.type != TileID.Iron && curTile.type != TileID.Lead &&
                            curTile.type != TileID.Tin && curTile.type != TileID.Copper &&
                            curTile.type != TileID.Gold && curTile.type != TileID.Platinum &&
                            curTile.type != TileID.Tungsten && curTile.type != TileID.Silver &&
                            curTile.type != TileID.Silt && curTile.type != TileID.ClayBlock ||
                            !curTile.active()) goto RETRY;

                    }
                }

                //CHOOSE WIDTH AND HEIGHT
                w = WorldGen.genRand.Next(20, 51);
                h = WorldGen.genRand.Next(20, 51);

                //GENERATE AND PLACE RANDOM MAPPING
                RandChance[,] depositMap = RandomMapping.EllipseRandom(w, h);
                depositMap.PlaceInWorld(SulfurOreType, iCenter - w / 2, jCenter - h / 2,
                    tile => tile.active() && tile.type != SulfurHeartType && tile.type != SulfurOreType);

                //ENSURE FILL IN CENTER 4x4 TILE AREA
                for (int iOff = -2; iOff < 2; iOff++) {
                    for (int jOff = -2; jOff < 2; jOff++) {

                        curTile = Framing.GetTileSafely(iCenter + iOff, jCenter + jOff);

                        if ((iOff == -1 || iOff == 0) && (jOff == -1 || jOff == 0)) {
                            curTile.active(false);
                        } else {
                            curTile.type = SulfurOreHeartType;
                            curTile.slope(0);
                        }

                    }
                }

                //PLACE HEART
                WorldGen.PlaceTile(iCenter - 1, jCenter - 1, SulfurHeartType, true, true);

                //SET PROGRESS
                progress.Set(deposit / (float)amount);

            }
             
        }

        private void GenerateQuartzCaves(GenerationProgress pogress) {

            pogress.Message = "Zippy-zappy Caves";

            int maxY = Main.maxTilesY - 300;
            int minY = (int)WorldGen.rockLayerLow;
            int maxX = Main.maxTilesX - 100;
            int minX = 100;

            int iCen, jCen;

            Tile curTile;

            TileRun caveStructure;

            int amount = Main.rand.Next(8, 12);
            int attempts = 0;
            int sAttempts = 0;
            const int maxAttempts = 7_500;

            const float maxWeight = 40f;
            const float weightPer = 7f;
            const float weightPerAir = 21f;
            const float randWeight = 7f;
            const float randWeightAir = 2.1f;

            bool cont;
            bool smooth = false;

            while (attempts++ < maxAttempts && sAttempts < amount) {

                iCen = Main.rand.Next(minX, maxX);
                jCen = Main.rand.Next(minY, maxY);

                //ChemistryClass.Logging.Debug("A");

                cont = true;
                for (int i = -5; i <= 5; i++) {
                    for (int j = -5; j <= 5; j++) {
                        curTile = Framing.GetTileSafely(iCen + i, jCen + j);
                        cont &= curTile.active() &&
                            new[] { TileID.Stone, TileID.Dirt, TileID.Mud, TileID.Tin,
                            TileID.Copper, TileID.Iron, TileID.Lead, TileID.Silver, TileID.Tungsten,
                            TileID.Gold, TileID.Platinum, TileID.Amethyst, TileID.Topaz, TileID.Diamond,
                            TileID.Ruby, TileID.Sapphire, TileID.Silt }.Contains(curTile.type);
                        if (!cont) goto CONT;
                    }
                }
                CONT:
                if (!cont) continue;

                //ChemistryClass.Logging.Debug("B");

                caveStructure = new TileRun(TileRunItem.FromTile(iCen, jCen, 0));
                if (!caveStructure.ExecuteFull(
                     weightPer, maxWeight,
                     obj => obj.Items.Count > 300,
                     randWeight)) continue;

                //ChemistryClass.Logging.Debug("C");

                foreach (var item in caveStructure) {
                    WorldGen.KillTile(item.I, item.J, false, false, true);
                    WorldGen.PlaceWall(item.I, item.J, Main.rand.Next(WallID.CaveUnsafe, WallID.Cave2Unsafe + 1), true);
                }

                //ChemistryClass.Logging.Debug("D");

                caveStructure = new TileRun(TileRunItem.FromTile(iCen, jCen, 0));
                caveStructure.ExecuteFull(
                     weightPerAir, maxWeight,
                     obj => obj.Items.Count > 350,
                     randWeightAir);

                //ChemistryClass.Logging.Debug("E");

                foreach (var item in caveStructure) {
                    if (item.Tile) {
                        WorldGen.PlaceTile(item.I, item.J, ModContent.TileType<Tiles.Blocks.QuartzStone>(), true, true);
                    }
                    if (item.Border && !item.RunBorder && Main.rand.NextFloat() < 0.8f) {
                        //WorldGen.PlaceTile(item.I, item.J, ModContent.TileType<Tiles.Blocks.QuartzPlaced>(), false, true, -1, pStyle);
                        WorldGen.PlaceTile(item.I, item.J, ModContent.TileType<Tiles.Blocks.QuartzPlaced>(), false, true, -1, Main.rand.Next(0, 3));
                        continue;
                    }
                }

                //ChemistryClass.Logging.Debug("F");

                foreach (var item in caveStructure) {
                    curTile = Framing.GetTileSafely(item.I, item.J);
                    if(curTile.type == ModContent.TileType<Tiles.Blocks.QuartzStone>()) {
                        smooth = true;
                        curTile = Framing.GetTileSafely(item.I + 1, item.J);
                        smooth &= curTile.type != ModContent.TileType<Tiles.Blocks.QuartzPlaced>();
                        curTile = Framing.GetTileSafely(item.I - 1, item.J);
                        smooth &= curTile.type != ModContent.TileType<Tiles.Blocks.QuartzPlaced>();
                        curTile = Framing.GetTileSafely(item.I, item.J + 1);
                        smooth &= curTile.type != ModContent.TileType<Tiles.Blocks.QuartzPlaced>();
                        curTile = Framing.GetTileSafely(item.I, item.J - 1);
                        smooth &= curTile.type != ModContent.TileType<Tiles.Blocks.QuartzPlaced>();
                        if(smooth) {
                            Tile.SmoothSlope(item.I, item.J, false);
                        }
                    }
                }

                //ChemistryClass.Logging.Debug("G");

                sAttempts++;

                pogress.Value = MathHelper.Lerp(1, (float)sAttempts / amount, 1f - (float)attempts / maxAttempts);

            }

        }

        //QUARTZ MANAGEMENT
        public override void PreUpdate() {

            QuartzLightningValid = ChemistryClass.TimeIsMultOf(quartzLightningTimer);
            QuartzLightningCount = 0;

        }

    }
}
