using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ChemistryClass.UI {
    [Obsolete]
    public class RefinementMenuStateWithAR : RefinementMenuState {

        public UIItemSlot autoRefineSlot;
        public UIText autoRefineTitle;

        public override void OnInitialize() {

            base.OnInitialize();

            menu.Width = new StyleDimension(Main.screenWidth / 7f, 0);
            menu.Height = new StyleDimension(5 * Main.screenHeight / 21f, 0);

            //NEW UI
            autoRefineTitle = new UIText("Auto Refine") {

                HAlign = 0.5f,
                VAlign = 0.55f

            };
            autoRefineTitle.Activate();

            autoRefineSlot = new UIItemSlot(scale: 0.8f) {

                HAlign = 0.5f,
                VAlign = 0.87f,
                validItem = i => true

            };
            autoRefineSlot.Activate();

            //PATCH UI TO MAKE ROOM
            menu.chemHolder.VAlign *= 0.3f;
            menu.matrHolder.VAlign *= 0.3f;
            menu.button.VAlign *= 0.3f;
            menu.title.VAlign *= 0.3f;

            menu.Append(autoRefineTitle);
            menu.Append(autoRefineSlot);

            Append(menu);

        }

        //public override void Update(GameTime gameTime) {

        //    Main.LocalPlayer.Chemistry().autoRefineItem = autoRefineSlot.Item;

        //}

    }
}
