using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace ChemistryClass.UI {
    public class UIItemSlot : UIElement {

        internal Item item;
        internal Predicate<Item> validItem = i => true;
        internal bool allowAir = true;

        internal const float margin = 0.4f;

        internal bool tossContentsOnClose = false;
        internal bool Visible {

            get => _visible;
            set {

                if (!value && _visible && tossContentsOnClose) TossContents();
                _visible = value;

            }

        }

        private bool _visible = true;
        private readonly int _context;
        private readonly float _scale;

        public UIItemSlot(int context = ItemSlot.Context.BankItem, float scale = 1f, bool tossContentsOnClose = false) {

            _context = context;
            _scale = scale;

            this.tossContentsOnClose = tossContentsOnClose;

            item = new Item();
            item.SetDefaults();

            Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
            Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);

        }

        protected void TossContents() {

            Main.LocalPlayer.QuickSpawnClonedItem(item, item.stack);
            item = new Item();
            item.SetDefaults();

        }

        public override void OnDeactivate() {

            if (tossContentsOnClose) TossContents();

        }

        public bool SetItem(Item item) {

            if (validItem(item)) {

                this.item = item;
                return true;

            } else return false;

        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {

            if (!Visible) return;

            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;

            if( ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface ) {

                Main.LocalPlayer.mouseInterface = true;
                if (validItem(Main.mouseItem) || (Main.mouseItem.IsAir && allowAir))
                    ItemSlot.Handle(ref item, _context);

            }

            ItemSlot.Draw(spriteBatch, ref item, _context, GetDimensions().ToRectangle().TopLeft());

            Main.inventoryScale = oldScale;

        }

    }
}
