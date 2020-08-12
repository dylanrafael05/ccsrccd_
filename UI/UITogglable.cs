using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace ChemistryClass.UI {
    public class UITogglable : UIElement {

        public bool toggled = false;

        protected virtual void SafeDrawSelf(SpriteBatch spriteBatch) { }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if (!toggled) base.DrawSelf(spriteBatch);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch) {
            if(!toggled) base.DrawChildren(spriteBatch);
        }

    }
}
