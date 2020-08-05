using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using ChemistryClass.ModUtils;
using System.Security.Cryptography.X509Certificates;

namespace ChemistryClass.UI {
    public class RefinementMenuPanel : DragablePanel {

        public UIText title;
        public UIItemSlot chemHolder;
        public UIItemSlot matrHolder;
        public UIRefineButton button;

        public UIItemSlot autoRefineSlot;
        public UITitleTogglable autoRefineTitle;

        public override void OnInitialize() {

            Width = new StyleDimension(Main.screenWidth / 7f, 0);
            Height = new StyleDimension(Main.screenHeight / 7f, 0);

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
                Height = new StyleDimension(30, 0),
                Width = new StyleDimension(30, 0)

            };

            autoRefineTitle = new UITitleTogglable("Auto Refine") {

                HAlign = 0.5f,
                VAlign = 0.55f

            };

            autoRefineSlot = new UIItemSlot(scale: 0.8f) {

                HAlign = 0.5f,
                VAlign = 0.87f,
                validItem = i => true

            };

            button.OnClick += Refine;

            Append(title);
            Append(chemHolder);
            Append(matrHolder);
            Append(button);
            Append(autoRefineTitle);
            Append(autoRefineSlot);

        }

        public override void Update(GameTime gameTime) {

            if (Main.dedServ) return;

            ChemistryClassPlayer cPlayer = Main.LocalPlayer.Chemistry();

            autoRefineTitle.toggled = !cPlayer.autoRefine;
            autoRefineSlot.Visible = cPlayer.autoRefine;

            Width = new StyleDimension(Main.screenWidth / 7f, 0);
            Height = cPlayer.autoRefine ?
                        new StyleDimension(5 * Main.screenHeight / 21f, 0) :
                        new StyleDimension(Main.screenHeight / 7f, 0);

            title.VAlign = cPlayer.autoRefine ? 0.05f * 0.4f : 0.05f;
            matrHolder.VAlign = chemHolder.VAlign = button.VAlign =
                cPlayer.autoRefine ?
                    0.75f * 0.4f :
                    0.75f;

        }

        private void Refine(UIMouseEvent evt, UIElement listener) {

            if (chemHolder.Item.IsAir || matrHolder.Item.IsAir) return;

            Item ch = chemHolder.Item;
            Item mt = matrHolder.Item;

            bool refined = ChemistryClassItem.Refine(ref ch, ref mt);

            chemHolder.Item = ch;
            matrHolder.Item = mt;

            if (refined) Main.PlaySound(SoundID.Item37);

        }

    }
}
