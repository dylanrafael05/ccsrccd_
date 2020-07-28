using System;
using Terraria;
using Terraria.UI;
using ChemistryClass.ModUtils;

namespace ChemistryClass.UI {
    public class RefinementMenuState : UIState {

        public RefinementMenuPanel menu;

        public override void OnInitialize() {

            menu = new RefinementMenuPanel() {

                HAlign = 0.5f,
                Top = 24f.ToStyleDimension(),
                Width = (Main.screenWidth / 7f).ToStyleDimension(),
                Height = 100f.ToStyleDimension()

            };

            menu.Activate();

            Append(menu);

        }

    }
}
