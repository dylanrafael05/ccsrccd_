using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class RustyKnife : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "Infused with rust, a blade like this makes enemies' defenses drop\n" +
                "Inflicts \"Rusted\", which drains enemy life slowly\n" +
                $"and decreases their defense by {ChemistryClassGlobalNPC.rustedDef}"

                );

        }

        public override void SafeSetDefaults() {

            item.damage = 9;
            item.crit = 4;
            item.knockBack = 2;

            item.width = 32;
            item.height = 32;

            item.useTime = 15;
            item.useAnimation = 15;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = false;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 15, 0);

            minutesToDecay = 5f;

            SetRefinementData(

                (ModContent.ItemType<Materials.Earlygame.RustedPowder>(), 1 / 18f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            ChemistryClass.BeakerTileID,
                            1,
                            (ModContent.ItemType<Materials.Earlygame.RustedPowder>(), 10),
                            (ItemID.SilverBar, 6)

            );

            this.SetRecipe(

                            ChemistryClass.BeakerTileID,
                            1,
                            (ModContent.ItemType<Materials.Earlygame.RustedPowder>(), 10),
                            (ItemID.TungstenBar, 6)

            );

        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Rusted>(), 600);

        }

    }
}