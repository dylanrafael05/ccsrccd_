using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ChemistryClass.UI {
    public class UITitleTogglable : UIText {

        public UITitleTogglable(string text) : base(text) { }

        public bool toggled = false;

        public override void Draw(SpriteBatch spriteBatch) {
            if(!toggled) base.Draw(spriteBatch);
        }

    }
}
