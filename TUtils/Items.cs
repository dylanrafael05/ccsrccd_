﻿using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUtils {
    public static class Items {

        public static void SetRecipe(this ModItem item, int requireTile = TileID.WorkBenches, int amount = 1, params (int, int)[] items) {

            ModRecipe recipe = new ModRecipe(item.mod);

            recipe.AddTile(requireTile);

            foreach (var ing in items) {

                recipe.AddIngredient(ing.Item1, ing.Item2);

            }

            recipe.SetResult(item.item.type, amount);

            recipe.AddRecipe();

        }

    }
}
