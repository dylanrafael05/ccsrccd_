﻿using System;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.UI {
    public class DecayMeterState : UIState {

        public DecayMeter decayMeter;

        public override void OnInitialize() {

            decayMeter = new DecayMeter() {

                Left = 0f.ToStyleDimension(),
                Top = 0f.ToStyleDimension(),
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill

            };
            decayMeter.Activate();

            Append(decayMeter);

        }

    }

    public class DecayMeter : UIElement {

        public int length = 3;

        public const float scale = 1f;
        public const int frameSize = 24;
        public const int barYOffset = 10;
        public const int barY = 4;

        public Texture2D Texture { get; } = ModContent.GetTexture("ChemistryClass/UI/DecayMeter");
        public TextureFramer Framer { get; } = new TextureFramer(frameSize);
        public Color SubtractColor { get; } = new Color(0xFF, 0x10, 0x10);
        public Color AddColor { get; } = new Color(0x40, 0xD0, 0x30);

        public bool isAdding = false;

        private float _dP = 1f;
        public float DisplayPurity {

            get => _dP;
            set {

                DisplayPurityReal = value;
                _dP = MathHelper.Lerp(_dP, DisplayPurityReal, 0.1f);

                isAdding = _dP < DisplayPurityReal;

            }

        }
        public float DisplayPurityReal { get; private set; } = 1f;
        public bool toggled = false;

        public override void Update(GameTime gameTime) {

            if (Main.dedServ) return;

            Player plr = Main.LocalPlayer;

            toggled = !plr.HeldItem.IsChemistry();

            if (!toggled) DisplayPurity = plr.HeldItem.Chemistry().purity;

        }

        public override void Draw(SpriteBatch spriteBatch) {

            if (toggled) return;
            if (!Main.LocalPlayer.active) return;

            Vector2 drawStart =
                CCUtils.ScreenRectangle.Size() / 2 +
                new Vector2(scale * Main.UIScale * frameSize * -(length + 1.5f) / 2f, -Main.screenHeight / 9f);
            Vector2 drawOff = Vector2.UnitX * scale * frameSize * Main.UIScale;

            Rectangle mainBarRect = new Rectangle(

                (int)(drawStart.X + drawOff.X),
                (int)(drawStart.Y + barYOffset * scale * Main.UIScale),
                (int)(scale * frameSize * Main.UIScale * length * DisplayPurity),
                (int)(scale * barY * Main.UIScale)

                );

            Rectangle otherBarRect = Drawing.RectFromCorners(

                mainBarRect.TopRight().OffsetBy(isAdding ? 0 : 1, 0),
                drawStart.OffsetBy(0, (barYOffset + barY) * Main.UIScale) + drawOff * (length * DisplayPurityReal + 1)

                );

            spriteBatch.DrawFixed(

                Texture,
                drawStart,
                Framer,
                0,
                Color.White,
                0f,
                Vector2.Zero,
                scale * Main.UIScale,
                SpriteEffects.None,
                1f

            );

            for(int i = 0; i < length; i++) {

                spriteBatch.DrawFixed(

                    Texture,
                    drawStart + drawOff * (i + 1),
                    Framer,
                    1,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale * Main.UIScale,
                    SpriteEffects.None,
                    1f

                );

            }

            spriteBatch.DrawFixed(

                Texture,
                drawStart + drawOff * (length + 1),
                Framer,
                2,
                Color.White,
                0f,
                Vector2.Zero,
                scale * Main.UIScale,
                SpriteEffects.None,
                1f

            );

            //FILL LINE
            spriteBatch.DrawRect(

                mainBarRect,
                ChemistryClassItem.CalloutColor

            );

            if (otherBarRect.Width <= 1) return;

            spriteBatch.DrawRect(

                otherBarRect,
                isAdding ? AddColor : SubtractColor

            );

        }

    }
}
