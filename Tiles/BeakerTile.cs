using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChemistryClass.Tiles {
    public class BeakerTile : ModTile {

        public override void SetDefaults() {

            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;

            this.minPick = 0;
            this.dustType = DustID.Ice;

            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.RandomStyleRange = 2;

            TileObjectData.addTile(Type);

            //this.drop = ModContent.ItemType<Items.Placeable.Beaker>();

        }

        public override bool Drop(int i, int j) {

            Item.NewItem(i*16, j*16, 0, 0, ModContent.ItemType<Items.Placeable.Beaker>());
            return false;

        }

        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance) {

            wormChance = 0;
            grassHopperChance = 0;
            jungleGrubChance = 0;

        }

    }
}
