using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TUtils.WorldGeneration;
using TUtils;
using System.Security.Policy;
using Microsoft.Xna.Framework;

namespace ChemistryClass {
    public static partial class ChemistryClassStructures {

        public static Structure SulfurHeart = new Structure(

            new Dictionary<char, TileReplacer>() {

                { 'S', new TileReplacer(ChemistryClassWorld.SulfurOreHeartType) },
                { 'H', TileReplacer.Multitile(ChemistryClassWorld.SulfurHeartType) }

            },

            new string[] {

                "SSSS",
                "SH S",
                "S  S",
                "SSSS"

            },

            WGUtils.NoCoordAction,
            WGUtils.RemoveTilesInArea,
            WGUtils.CoordinateAny,
            WGUtils.AreaAny

            );

        //public static Delegates.CoordinatePredicate SulfurDepoCanGenerate(int i, int j) => _sulfurDepoCanGenerate(i, j);
        private static bool _sulfurDepoCanGenerate(Tile curTile)
            => (curTile.type == TileID.Stone || curTile.type == TileID.Dirt ||
                curTile.type == TileID.Stalactite || curTile.type == TileID.Ruby ||
                curTile.type == TileID.Emerald || curTile.type == TileID.Amethyst ||
                curTile.type == TileID.Diamond || curTile.type == TileID.Topaz ||
                curTile.type == TileID.Iron || curTile.type == TileID.Lead ||
                curTile.type == TileID.Tin || curTile.type == TileID.Copper ||
                curTile.type == TileID.Gold || curTile.type == TileID.Platinum ||
                curTile.type == TileID.Tungsten || curTile.type == TileID.Silver ||
                curTile.type == TileID.Silt || curTile.type == TileID.ClayBlock) &&
                curTile.active();

        public static Structure SulfurDeposit = new Structure(

            WGUtils.NoCoordAction,
            WGUtils.NoAreaAction,
            WGUtils.CoordinateIsActive,
            rec => WGUtils.PollAreaAll(Mathematics.CenteredRescale(ref rec, 8, 8), _sulfurDepoCanGenerate),
            ts => FadeStructureGeneration.MapToTileData(

                    FadeStructureGeneration.MakeRandomEllipse(
                        Main.rand.Next(20, 51),
                        Main.rand.Next(20, 51)
                    ),
                    new TileReplacer(ChemistryClassWorld.SulfurOreType, keepTileForm: true)
                  )

            );
        
    }

    public class ChemistryClassWorld : ModWorld {

        //BIOME SHIZ
        public static ushort SulfurOreHeartType => (ushort)ModContent.TileType<Tiles.Blocks.SulfuricOreHeartTile>();
        public static ushort SulfurOreType => (ushort)ModContent.TileType<Tiles.Blocks.SulfuricOreTile>();
        public static ushort SulfurHeartType => (ushort)ModContent.TileType<Tiles.Multitiles.SulfurHeartTile>();

        public static int sulfurCount;
        public static int sulfurHeartCount;

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

        }

        private void GenerateSulphuricDeposit(GenerationProgress progress) {

            progress.Message = "Generating Sulfuric Deposits";

            //VARIABLE DECALRATIONS
            int worldCenter = Main.maxTilesX / 2;
            int tileSpread = Main.maxTilesX / 4;

            int iCenter, jCenter;

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

                //GENERATE
                if (ChemistryClassStructures.SulfurDeposit.Generate(iCenter, jCenter)) {
                    ChemistryClassStructures.SulfurHeart.Generate(iCenter, jCenter);
                } else {
                    goto RETRY;
                }

                //SET PROGRESS
                progress.Set(deposit / (float)amount);

            }
             
        }

    }
}
