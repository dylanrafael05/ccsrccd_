using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace ChemistryClass {
    public static class ChemistryClassExtensions {

        public static ChemistryClassPlayer chemistry(this Player player)
            => player.GetModPlayer<ChemistryClassPlayer>();

        public static ChemistryClassItem chemistry(this Item item)
            => item.modItem as ChemistryClassItem;

        public static bool isChemistry(this Item item)
            => item.modItem is ChemistryClassItem;

        public static StyleDimension styleDimen(this float i)
            => new StyleDimension(i, 0);

        public static float MinDimension(this Rectangle i)
            => Math.Min(i.Width, i.Height);
        public static float MinDimension(this Texture2D i)
            => Math.Min(i.Width, i.Height);

        public static float MaxDimension(this Rectangle i)
            => Math.Max(i.Width, i.Height);
        public static float MaxDimension(this Texture2D i)
            => Math.Max(i.Width, i.Height);

        public static void AddMargin(this Rectangle i, int margin) {

            i.X += margin;
            i.Y += margin;
            i.Width  -= margin * 2;
            i.Height -= margin * 2;

        }

        public static void AddMargin(this Rectangle i, float margin)
            => i.AddMargin((int)margin);

        public static void ShrinkBy(this Rectangle i, float margin)
            => new Rectangle(

                (int)(i.Center.X - (i.Width / 2)*(1 - margin)),
                (int)(i.Center.Y - (i.Height / 2)*(1 - margin)),
                (int)(i.Width * (1 - margin)),
                (int)(i.Height * (1 - margin))

                );

        public static void SetRecipe( this ModItem item, int requireTile = TileID.WorkBenches, int amount = 1, params (int, int)[] items ) {

            ModRecipe recipe = new ModRecipe(item.mod);

            recipe.AddTile(requireTile);
            foreach( var ing in items ) {

                recipe.AddIngredient(ing.Item1, ing.Item2);

            }

            recipe.SetResult(item.item.type, amount);

            recipe.AddRecipe();

        }


    }
}
