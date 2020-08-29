using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class SulfuricSpitter : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "Spews sulfuric flames onto enemies"

                );

        }

        public override void SafeSetDefaults() {

            item.damage = 18;
            item.crit = 4;
            item.knockBack = 1;

            item.width = 44;
            item.height = 16;

            item.useTime = 15;
            item.useAnimation = 15;
            item.UseSound = SoundID.Item34;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;

            item.noMelee = true;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 55, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.SulfurFlame>();
            item.shootSpeed = 15;

            minutesToDecay = 2.8f;

            minPurityMult = 1.05f;
            maxPurityMult = 0.8f;

            SetRefinementData(

                (ModContent.ItemType<Placeable.Blocks.SulfurClump>(), 1 / 6f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            ModContent.TileType<Tiles.Multitiles.MetallurgyStandTile>(),
                            1,
                            (ModContent.ItemType<Placeable.Blocks.SulfurClump>(), 18),
                            ("IronBar", 5)

            );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            position.X += speedX * 3;
            position.Y += speedY * 3;

            return true;

        }

    }
}
