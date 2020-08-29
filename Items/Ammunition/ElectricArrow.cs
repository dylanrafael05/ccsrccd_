using System;
using ChemistryClass.ModUtils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Ammunition {
    public class ElectricArrow : ModItem {

		public override void SetStaticDefaults() {
			Tooltip.SetDefault("An arrow which strikes enemies with the power and force of lightning.");
		}

		public override void SetDefaults() {
			item.damage = 7;
			item.ranged = true;
			item.width = 14;
			item.height = 32;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 2f;
			item.value = 8;
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.ElectricArrowProjectile>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 20f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes() {

			this.SetRecipe(
                TileID.Anvils,
                50,
                (ModContent.ItemType<Materials.Earlygame.Quartz>(), 1),
                (ItemID.WoodenArrow, 50)
                );

		}

	}
}
