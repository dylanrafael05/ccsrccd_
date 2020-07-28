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

namespace ChemistryClass {
    public class ChemistryClassWorld : ModWorld {

        //BIOME SHIZ
        public static ushort SulfurStoneType => (ushort)ModContent.TileType<Tiles.Blocks.SulfurStoneTile>();
        public static ushort SulfurOreType => (ushort)ModContent.TileType<Tiles.Blocks.SulfuricOreTile>();
        public static ushort SulfurHeartType => (ushort)ModContent.TileType<Tiles.SulfurHeartTile>();

        public static int sulfurCount;
        public static int sulfurHeartCount;

        public override void ResetNearbyTileEffects() {

            sulfurCount = 0;
            sulfurHeartCount = 0;
            base.ResetNearbyTileEffects();

        }

        public override void TileCountsAvailable(int[] tileCounts) {

            sulfurCount = tileCounts[SulfurStoneType] + tileCounts[SulfurOreType];
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

        }

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
                jCenter = WorldGen.genRand.Next((int)WorldGen.rockLayer, (int)(Main.maxTilesY * 0.9f) - 100);

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
                            curTile.type = SulfurOreType;
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

    }
}
