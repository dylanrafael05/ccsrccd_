using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Armor {

    [AutoloadEquip(EquipType.Body)]
    public class RustyChestpeice : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A rusty metal vest\nIncreases chemical crit chance by 5%");

        }

        public override void SetDefaults() {

            item.width = 32;
            item.height = 20;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.defense = 7;

        }

        public override void UpdateEquip(Player player) {

            player.Chemistry().ChemicalCritAdd = 5;

        }

        public override void AddRecipes() {

            int rustPowder = ModContent.ItemType<Materials.RustedPowder>();

            this.SetRecipe(

                ModContent.TileType<Tiles.BeakerTile>(),
                1,
                (ItemID.IronChainmail, 1),
                (rustPowder, 24)

                );

            this.SetRecipe(

                ModContent.TileType<Tiles.BeakerTile>(),
                1,
                (ItemID.LeadChainmail, 1),
                (rustPowder, 24)

                );

        }

    }

}
