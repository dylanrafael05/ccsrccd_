using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace ChemistryClass.UI {
    public class RefinementMenuPanel : DragablePanel {

        public UIText title;

        public UIItemSlot chemHolder;
        public UIItemSlot matrHolder;

        public UIRefineButton button;

        public override void OnInitialize() {

            title = new UIText("Chemical Refinement", 0.9f) {

                VAlign = 0.05f,
                HAlign = 0.5f

            };

            chemHolder = new UIItemSlot(scale: 0.8f) {

                validItem = i => i.modItem is ChemistryClassItem || i.IsAir,
                tossContentsOnClose = true,
                VAlign = 0.75f,
                HAlign = 0.1f

            };

            matrHolder = new UIItemSlot(scale: 0.8f) {

                tossContentsOnClose = true,
                VAlign = 0.75f,
                HAlign = 0.5f

            };

            button = new UIRefineButton() {

                VAlign = 0.75f,
                HAlign = 0.9f,
                Height = 30f.ToStyleDimension(),
                Width = 30f.ToStyleDimension()

            };

            button.OnClick += Refine;

            Append(title);
            Append(chemHolder);
            Append(matrHolder);
            Append(button);

        }

        private void Refine(UIMouseEvent evt, UIElement listener) {

            if (chemHolder.item.IsAir || matrHolder.item.IsAir) return;

            bool refined = ChemistryClassItem.Refine(ref chemHolder.item, ref matrHolder.item);

            if (refined) Main.PlaySound(SoundID.Item37);

        }

    }
}
