using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ChemistryClass.ModUtils;
using Terraria;

namespace ChemistryClass.UI {
    public class DragablePanel : UIPanel {

        private Vector2 offset = Vector2.Zero;
        public bool dragging = false;
        public int[] exemptElements = new int[0];

        public override void MouseDown(UIMouseEvent evt) {

            for (int i = 0; i < Elements.Count; i++) {
                if (exemptElements.Contains(i)) continue;
                if (Elements[i].ContainsMouse()) return;
            }

            dragging = true;
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);

            base.MouseDown(evt);

        }

        public override void MouseUp(UIMouseEvent evt) {

            dragging = false;
            base.MouseUp(evt);

        }

        public override void Update(GameTime gameTime) {

            if (this.ContainsMouse()) {

                Main.LocalPlayer.mouseInterface = true;
                Main.isMouseLeftConsumedByUI = true;

            }

            if (dragging) {

                Left.Set(Main.mouseX - offset.X, 0);
                Top.Set(Main.mouseY - offset.Y, 0);

                Rectangle parentSpace = Parent.GetDimensions().ToRectangle();
                if (!parentSpace.Contains(GetDimensions().ToRectangle())) {

                    CCUtils.Clamp(ref Left.Pixels, 0, parentSpace.Left - Width.Pixels);
                    CCUtils.Clamp(ref Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);

                }

                Recalculate();

            }

        }

    }
}
