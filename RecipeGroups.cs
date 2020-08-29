using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ChemistryClass {
    public partial class ChemistryClass {

        public override void AddRecipeGroups() {

            RecipeGroup newGroup;

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Copper Bar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });
            RecipeGroup.RegisterGroup("ChemistryClass:CopperBar", newGroup);

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar", new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar
            });
            RecipeGroup.RegisterGroup("ChemistryClass:SilverBar", newGroup);

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("ChemistryClass:GoldBar", newGroup);

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Iron Helmet", new int[]
            {
                ItemID.IronHelmet,
                ItemID.LeadHelmet
            });
            RecipeGroup.RegisterGroup("ChemistryClass:IronHelmet", newGroup);

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Iron Chainmail", new int[]
            {
                ItemID.IronChainmail,
                ItemID.LeadChainmail
            });
            RecipeGroup.RegisterGroup("ChemistryClass:IronChainmail", newGroup);

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Iron Greaves", new int[]
            {
                ItemID.IronGreaves,
                ItemID.LeadGreaves
            });
            RecipeGroup.RegisterGroup("ChemistryClass:IronGreaves", newGroup);

            newGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Iron Ore", new int[]
            {
                ItemID.IronOre,
                ItemID.LeadOre
            });
            RecipeGroup.RegisterGroup("ChemistryClass:IronOre", newGroup);

        }

    }
}
