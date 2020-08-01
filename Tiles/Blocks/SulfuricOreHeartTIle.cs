using System;
using Terraria;

namespace ChemistryClass.Tiles.Blocks {

    public class SulfuricOreHeartTile : SulfuricOreTile {

        public override bool Autoload(ref string name, ref string texture) {

            texture = "ChemistryClass/Tiles/Blocks/SulfuricOreTile";

            return true;

        }

        public override void SetDefaults() {

            base.SetDefaults();
            this.mineResist = 16f;

            Main.tileMerge[Type][ChemistryClassWorld.SulfurOreType] = true;
            Main.tileMerge[ChemistryClassWorld.SulfurOreType][Type] = true;

            Main.tileMerge[Type][ChemistryClassWorld.SulfurHeartType] = true;
            Main.tileMerge[ChemistryClassWorld.SulfurHeartType][Type] = true;

        }

    }
}
