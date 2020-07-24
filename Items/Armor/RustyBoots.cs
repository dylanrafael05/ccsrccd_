using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Armor {

    [AutoloadEquip(EquipType.Legs)]
    public class RustyBoots : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("Protective and rusted boots\nIncreases chemical knockback by 8%");

        }

        public override void SetDefaults() {

            item.width = 26;
            item.height = 20;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.defense = 6;

        }

        public override void UpdateEquip(Player player) {

            player.Chemistry().ChemicalKnockbackAdd += 0.08f;

        }

        public override void AddRecipes() {

            int rustPowder = ModContent.ItemType<Materials.RustedPowder>();

            this.SetRecipe(

                ModContent.TileType<Tiles.BeakerTile>(),
                1,
                (ItemID.IronGreaves, 1),
                (rustPowder, 16)

                );

            this.SetRecipe(

                ModContent.TileType<Tiles.BeakerTile>(),
                1,
                (ItemID.LeadGreaves, 1),
                (rustPowder, 16)

                );

        }

    }

}