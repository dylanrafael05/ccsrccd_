using System;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;

namespace ChemistryClass.UI {
    public class UIRefineButton : UIElement {

        public static string inactive = null;
        public static string active = null;

        public static Texture2D inactiveTexture => ModContent.GetTexture(inactive);
        public static Texture2D activeTexture   => ModContent.GetTexture(active);

        private bool? previousMouseOver = null;
        private Texture2D curTexture = null;

        public override void OnActivate() {

            inactive = "ChemistryClass/UI/RefineButtonInactive";
            active = "ChemistryClass/UI/RefineButtonActive";

            curTexture = inactiveTexture;
            previousMouseOver = false;
            base.OnActivate();

        }

        public override void OnDeactivate() {

            inactive = null;
            active = null;

            curTexture = null;
            previousMouseOver = null;

            base.OnDeactivate();

        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {

            bool mouseOver = ContainsPoint(Main.MouseScreen);

            if( mouseOver && !previousMouseOver.Value ) {

                Main.PlaySound(SoundID.MenuTick);
                curTexture = activeTexture;

            }

            if( !mouseOver && previousMouseOver.Value ) {

                Main.PlaySound(SoundID.MenuTick);
                curTexture = inactiveTexture;

            }

            previousMouseOver = mouseOver;

            spriteBatch.Draw(curTexture, GetDimensions().ToRectangle(), Color.White);

        }

    }
}
