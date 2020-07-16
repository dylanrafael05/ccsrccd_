using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace ChemistryClass.UI {
    public class RefinementMenu : UIState {

        public UIPanel backPanel;
        public UIText title;

        public UIItemSlot chemHolder;
        public UIItemSlot matrHolder;

        public UIRefineButton button;

        public bool Visible = true;

        private void SetVisible(bool v) {

            Visible = v;
            chemHolder.Visible = v;
            matrHolder.Visible = v;

        }

        public void GoVisible()
            => SetVisible(true);

        public void GoInvisible()
            => SetVisible(false);

        public override void OnInitialize() {

            backPanel = new UIPanel() {

                Top = 0f.styleDimen(),
                Left = 0f.styleDimen(),
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill

            };

            title = new UIText("Chemical Refinement", 0.9f) {

                VAlign = 0.15f,
                HAlign = 0.5f

            };

            chemHolder = new UIItemSlot(scale: 0.8f) {

                validItem = i => i.modItem is ChemistryClassItem || i.IsAir,
                tossContentsOnClose = true,
                VAlign = 0.65f,
                HAlign = 0.2f

            };

            matrHolder = new UIItemSlot(scale: 0.8f) {

                tossContentsOnClose = true,
                VAlign = 0.65f,
                HAlign = 0.5f

            };

            button = new UIRefineButton() {

                VAlign = 0.65f,
                HAlign = 0.8f,
                Height = 30f.styleDimen(),
                Width = 30f.styleDimen()

            };

            button.OnClick += Refine;

            backPanel.Append(button);

            Append(backPanel);
            Append(title);
            Append(chemHolder);
            Append(matrHolder);

        }

        public override void OnActivate() {

            GoVisible();
            base.OnActivate();

        }

        public override void OnDeactivate() {

            GoInvisible();

        }

        private void Refine(UIMouseEvent evt, UIElement listener) {

            

            if (chemHolder.item.IsAir || matrHolder.item.IsAir) return;

            bool refined = ChemistryClassItem.Refine(ref chemHolder.item, ref matrHolder.item);

            if (refined) Main.PlaySound(SoundID.Item37);

        }

        public override void Draw(SpriteBatch spriteBatch) {

            if (!Visible) return;

            base.Draw(spriteBatch);

        }

    }
}
