using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.ModUtils {

    public struct RecipeEntry {

        public readonly int? ItemType;
        public readonly string GroupType;
        public readonly int Amount;

        public RecipeEntry(int? itemType, string groupType, int amount) {
            ItemType = itemType;
            GroupType = groupType;
            Amount = amount;
        }

        public RecipeEntry(int itemType, int amount) {
            ItemType = itemType;
            GroupType = null;
            Amount = amount;
        }

        public RecipeEntry(string groupType, int amount) {
            ItemType = null;
            GroupType = groupType;
            Amount = amount;
        }

        public static implicit operator RecipeEntry( (int, int) item )
            => new RecipeEntry(item.Item1, item.Item2);
        public static implicit operator RecipeEntry((string, int) item)
            => new RecipeEntry(item.Item1, item.Item2);

    }

    public static class RecipeHelping {

        public static void AddEntry(this ModRecipe recipe, RecipeEntry entry) {
            if (entry.ItemType.HasValue) {
                recipe.AddIngredient(entry.ItemType.Value, entry.Amount);
            } else {
                string group = entry.GroupType;
                if (group[0] == '\0') {
                    group = "ChemistryClass:" + group.Substring(1);
                }
                recipe.AddRecipeGroup(group, entry.Amount);
            }
        }

        public static void SetRecipe(this ModItem item, int requireTile = TileID.WorkBenches, int amount = 1, params RecipeEntry[] items) {

            ModRecipe recipe = new ModRecipe(item.mod);

            recipe.AddTile(requireTile);
            foreach (var ing in items) recipe.AddEntry(ing);

            recipe.SetResult(item.item.type, amount);

            recipe.AddRecipe();

        }

    }

}
