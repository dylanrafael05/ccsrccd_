﻿using System;
using Terraria.ID;
using Terraria;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class RustyKnife : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A dull blade made useful by its rusted state.");

        }

        public override void SafeSetDefaults() {

            item.damage = 8;
            item.crit = 4;
            item.knockBack = 2;

            item.width = 32;
            item.height = 32;

            item.useTime = 15;
            item.useAnimation = 15;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);

            minutesToDecay = 5;

            SetRefinementData(

                (ItemID.IronBar, 0.5f),
                (ItemID.IronOre, 0.15f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            ChemistryClass.BeakerTileID,
                            1,
                            (ItemID.IronBar, 10),
                            (ItemID.IronOre, 6)

            );

            this.SetRecipe(

                            ChemistryClass.BeakerTileID,
                            1,
                            (ItemID.LeadBar, 10),
                            (ItemID.LeadOre, 6)

            );

        }

    }
}