using System;
using System.Net;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Tiles.Multitiles {
    public class SulfurHeartTile : ModTile {

        internal const int frameCount = 9;
        internal const int frameWidth = 36;

        public override void SetDefaults() {

            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;

            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileValue[Type] = 460;

            this.dustType = ModContent.DustType<Dusts.SulfurDust>();
            this.minPick = int.MaxValue;
            this.soundType = SoundID.Tink;

            Main.tileFrameImportant[Type] = true;

            Main.tileLighted[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);

            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile, 2, 0);

            TileObjectData.newTile.AnchorValidTiles = new int[] { ChemistryClassWorld.SulfurOreHeartType };

            TileObjectData.addTile(Type);

            ModTranslation text = CreateMapEntryName();
            text.AddTranslation(GameCulture.English, "Sulfur Heart");
            AddMapEntry(new Color(0.85f, 0.85f, 0.3f), text);

            //individualFrame = Main.rand.Next(20);

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {

            r = Blocks.SulfuricOreTile.LightColor.X * 2f;
            g = Blocks.SulfuricOreTile.LightColor.Y * 2f;
            b = Blocks.SulfuricOreTile.LightColor.Z * 2f;

        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {

            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Accessories.Earlygame.SulfurHeart>());

        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {

            int frame = Main.tileFrame[Type];
            frameXOffset = frame * frameWidth;

        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {

            if (++frameCounter > 6) {

                frame = ++frame % frameCount;
                frameCounter = 0;

            }

        }

        //SULFUR CLOUD SPAWNING : CONSTS
        private const int sulfurXRange = 100;
        private const int sulfurYRange = 50;
        private const int sulfurSkip = 4;
        private const int sulfurCalc = 15;
        private static readonly RandChance sulfurChance = new RandChance(0.02f);
        private int NearbyCalls = 0;
        //Passes per frame: 100 * 50 / (4*15) or 83. Decent?.

        public override void NearbyEffects(int i, int j, bool closer) {

            if (Main.gamePaused || Main.gameInactive) return;
            if (++NearbyCalls > sulfurCalc || !closer) return;

            NearbyCalls = 0;

            int offset = (int)(ChemistryClass.UnpausedUpdateCount % sulfurSkip);

            int realI = i + offset;
            int realJ = j + offset;

            Tile curTile;

            for (int curI = realI - sulfurXRange / 2; curI < realI + sulfurXRange / 2; curI += sulfurSkip) {
                for (int curJ = realJ - sulfurYRange / 2; curJ < realJ + sulfurYRange / 2; curJ += sulfurSkip) {

                    curTile = Framing.GetTileSafely(curI, curJ);

                    if ( sulfurChance.Evaluate() && curTile.type == ChemistryClassWorld.SulfurOreType) {

                        NPC.NewNPC(curI * 16 + 8, curJ * 16 + 8, ModContent.NPCType<Projectiles.EarlygameFL.SulfuricCloud>());

                    }

                }
            }

        }

    }
}
