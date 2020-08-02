using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Armor.Rusty {

    [AutoloadEquip(EquipType.Body)]
    public class RustyChestpeice : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A rusty metal vest");

        }

        public override void SetDefaults() {

            item.width = 32;
            item.height = 20;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.defense = 3;

        }

        public override void AddRecipes() {

            int rustPowder = ModContent.ItemType<Materials.Earlygame.RustedPowder>();

            this.SetRecipe(

                ModContent.TileType<Tiles.Multitiles.BeakerTile>(),
                1,
                (ItemID.IronChainmail, 1),
                (rustPowder, 24)

                );

            this.SetRecipe(

                ModContent.TileType<Tiles.Multitiles.BeakerTile>(),
                1,
                (ItemID.LeadChainmail, 1),
                (rustPowder, 24)

                );

        }

    }

}
