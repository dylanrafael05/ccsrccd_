using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Armor.Rusty {

    [AutoloadEquip(EquipType.Legs)]
    public class RustyBoots : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("Protective and rusted boots");

        }

        public override void SetDefaults() {

            item.width = 26;
            item.height = 20;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.defense = 2;

        }

        public override void AddRecipes() {

            int rustPowder = ModContent.ItemType<Materials.Earlygame.RustedPowder>();

            this.SetRecipe(

                ModContent.TileType<Tiles.Multitiles.BeakerTile>(),
                1,
                ("\0IronGreaves", 1),
                (rustPowder, 16)

                );

        }

    }

}