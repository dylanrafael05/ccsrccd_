using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUtils;

namespace ChemistryClass.Items.Armor.Rusty {

    [AutoloadEquip(EquipType.Head)]
    public class RustyHeadgear : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A ferric face covering lined with rust");

        }

        public override void SetDefaults() {

            item.width = 22;
            item.height = 20;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.defense = 3;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == item.type &&
               body.type == ModContent.ItemType<RustyChestpeice>() &&
               legs.type == ModContent.ItemType<RustyBoots>();

        public override void UpdateArmorSet(Player player) {

            player.Chemistry().DecayRateMult -= 0.08f;
            player.setBonus = "Reduces all decay by 8%";

        }

        public override void AddRecipes() {

            int rustPowder = ModContent.ItemType<Materials.Earlygame.RustedPowder>();

            this.SetRecipe(

                ModContent.TileType<Tiles.Multitiles.BeakerTile>(),
                1,
                (ItemID.IronHelmet, 1),
                (rustPowder, 21)

                );

            this.SetRecipe(

                ModContent.TileType<Tiles.Multitiles.BeakerTile>(),
                1,
                (ItemID.LeadHelmet, 1),
                (rustPowder, 21)

                );

        }

    }

}
