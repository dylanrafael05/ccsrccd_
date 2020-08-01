using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ChemistryClass;
using Terraria;
using TUtils;

namespace ChemistryClass.UI {
    public class DragablePanel : UIPanel {

        private Vector2 offset;
        public bool dragging;

        public override void MouseDown(UIMouseEvent evt) {

            if (Elements.Any(el => el.ContainsPoint(Main.MouseScreen))) return;

            base.MouseDown(evt);
            dragging = true;

            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);

        }

        public override void MouseUp(UIMouseEvent evt) {

            base.MouseUp(evt);
            dragging = false;

        }

        private void ClampAndRecalculate() {

            Rectangle parentSpace = Parent.GetDimensions().ToRectangle();
            if( !GetDimensions().ToRectangle().Intersects(parentSpace) ) {

                Mathematics.Clamp(ref Left.Pixels, 0, parentSpace.Left - Width.Pixels);
                Mathematics.Clamp(ref Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);

            }

            Recalculate();

        }

        public override void Update(GameTime gameTime) {

            if( ContainsPoint(Main.MouseScreen) ) {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging) {
                Left.Set(Main.mouseX - offset.X, 0);
                Top.Set(Main.mouseY - offset.Y, 0);
            }

            ClampAndRecalculate();

        }

    }
}
