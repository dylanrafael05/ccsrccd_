using System;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using ChemistryClass.ModUtils;
using Terraria.GameContent.UI.States;

namespace ChemistryClass.UI {
    public class UIRefineButton : UIElement {

        public const string inactive = "ChemistryClass/UI/RefineButtonInactive";
        public const string active = "ChemistryClass/UI/RefineButtonActive";

        public static Texture2D inactiveTexture => ModContent.GetTexture(inactive);
        public static Texture2D activeTexture   => ModContent.GetTexture(active);

        private bool previousMouseOver = false;
        //private bool previousItemUse = false;
        private Texture2D curTexture = inactiveTexture;

        protected override void DrawSelf(SpriteBatch spriteBatch) {

            bool mouseOver = this.ContainsMouse();

            if( mouseOver && !previousMouseOver ) {

                Main.PlaySound(SoundID.MenuTick);
                curTexture = activeTexture;

            }

            if( !mouseOver && previousMouseOver ) {

                Main.PlaySound(SoundID.MenuTick);
                curTexture = inactiveTexture;

            }

            previousMouseOver = mouseOver;

            spriteBatch.Draw(curTexture, GetDimensions().ToRectangle(), Color.White);

        }

    }
}
