using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChemistryClass.Tiles.Multitiles {
    public class MetallurgyStandTile : ModTile {

        internal const int frameCount = 19;
        internal const int frameWidth = 36;
        //internal int individualFrame = 0;

        public override void SetDefaults() {

            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;

            this.minPick = 0;
            this.dustType = DustID.Iron;

            Main.tileFrameImportant[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };

            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Metallurgic Stand");
            AddMapEntry(new Color(100,100,100), name);

            //individualFrame = Main.rand.Next(20);

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {

            r = 1f;
            g = 0.5f;
            b = 0f;

        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {

            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Placeable.Crafting.MetallurgyStand>());

        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {

            int frame = Main.tileFrame[Type];

            frame += (i + j) % 2 * 5;
            frame += (i + j) % 3 * 5;
            frame += (i + j) % 4 * 5;
            frame += (i + j) % 5 * 5;
            frame += (i + j) % 6 * 5;

            frame %= frameCount;

            frameXOffset = frame * frameWidth;

        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {

            if (++frameCounter > 4) {

                //individualFrame = individualFrame++ % frameCount;
                frame = ++frame % frameCount;
                frameCounter = 0;

            }

            //DEBUGGING
            //if(ChemistryClass.TimeIsMultOf(60))
                //Main.NewText(frame);

        }

    }
}