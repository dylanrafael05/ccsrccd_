using System;
using Terraria;
using Terraria.UI;

namespace ChemistryClass.UI {
    public class RefinementMenuState : UIState {

        public RefinementMenuPanel menu;

        public override void OnInitialize() {

            menu = new RefinementMenuPanel() {

                HAlign = 0.5f,
                Top = new StyleDimension(24, 0),
                Width = new StyleDimension(Main.screenWidth / 7f, 0),
                Height = new StyleDimension(100, 0)

            };

            menu.Activate();

            Append(menu);

        }

    }
}
