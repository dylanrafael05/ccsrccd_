using System;
using Terraria;
using Terraria.UI;

namespace ChemistryClass.UI {
    public class RefinementMenuState : UIState {

        public RefinementMenuPanel menu;

        public override void OnInitialize() {

            menu = new RefinementMenuPanel() {

                HAlign = 0.5f,
                Top = new StyleDimension(24, 0)

            };

            menu.Activate();

            Append(menu);

        }

    }
}
