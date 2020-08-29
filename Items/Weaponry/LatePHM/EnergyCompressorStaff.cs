using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class EnergyCompressorStaff : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "Uses the piezoelectric powers of Quartz to\n" +
                "devastate foes with lightning"

                );

            Item.staff[item.type] = true;

        }

        public override void SafeSetDefaults() {

            item.damage = 18;
            item.crit = 0;
            item.knockBack = 2;

            item.width = 32;
            item.height = 32;

            item.useTime = 40;
            item.useAnimation = 40;
            item.UseSound = SoundID.Item91;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;

            item.noMelee = true;

            item.rare = 2;
            item.value = Item.buyPrice(0, 0, 45, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.CompressedEnergy>();
            item.shootSpeed = 7;

            minutesToDecay = 6f;

            SetRefinementData(

                (ModContent.ItemType<Materials.Earlygame.Quartz>(), 1f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            TileID.Anvils,
                            1,
                            (ModContent.ItemType<Materials.Earlygame.Quartz>(), 8),
                            ("IronBar", 3)

            );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            position.X += speedX * 4;
            position.Y += speedY * 4;

            return true;

        }

    }
}
