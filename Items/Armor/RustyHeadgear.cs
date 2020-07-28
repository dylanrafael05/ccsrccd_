using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Armor {

    [AutoloadEquip(EquipType.Head)]
    public class RustyHeadgear : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A ferric face covering lined with rust\nIncreases chemical damage by 7%");

        }

        public override void SetDefaults() {

            item.width = 22;
            item.height = 20;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.defense = 7;

        }

        public override void UpdateEquip(Player player) {

            player.Chemistry().ChemicalDamageAdd += 0.07f;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == item.type &&
               body.type == ModContent.ItemType<RustyChestpeice>() &&
               legs.type == ModContent.ItemType<RustyBoots>();

        public override void UpdateArmorSet(Player player) {

            player.Chemistry().DecayRateMult -= 0.15f;
            player.setBonus = "Reduces all decay by 15%";

        }

        public override void AddRecipes() {

            int rustPowder = ModContent.ItemType<Materials.RustedPowder>();

            this.SetRecipe(

                ModContent.TileType<Tiles.BeakerTile>(),
                1,
                (ItemID.IronHelmet, 1),
                (rustPowder, 21)

                );

            this.SetRecipe(

                ModContent.TileType<Tiles.BeakerTile>(),
                1,
                (ItemID.LeadHelmet, 1),
                (rustPowder, 21)

                );

        }

    }

}
