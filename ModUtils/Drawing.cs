using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;

namespace ChemistryClass.ModUtils {
    public struct TextureFramer {

        public int FrameWidth;
        public int FrameHeight;
        public int EdgeSize;
        public bool Vertical;
        public int MaxFrameX;
        public int MaxFrameY;

        public TextureFramer(int FrameWidth, int FrameHeight, int EdgeSize, bool Vertical, int MaxFrameX, int MaxFrameY) {
            this.FrameWidth = FrameWidth;
            this.FrameHeight = FrameHeight;
            this.EdgeSize = EdgeSize;
            this.Vertical = Vertical;
            this.MaxFrameX = MaxFrameX;
            this.MaxFrameY = MaxFrameY;
        }

        public TextureFramer(int FrameWidth, int FrameHeight, int EdgeSize, bool Vertical)
            : this(FrameWidth, FrameHeight, EdgeSize, Vertical, -1, -1) { }

        public TextureFramer(int FrameWidth, int FrameHeight, int EdgeSize)
            : this(FrameWidth, FrameHeight, EdgeSize, false) { }

        public TextureFramer(int FrameWidth, int FrameHeight)
            : this(FrameWidth, FrameHeight, 2) { }

        public TextureFramer(int FrameWidth, int FrameHeight, bool Vertical)
            : this(FrameWidth, FrameHeight) { this.Vertical = Vertical; }

        public TextureFramer(int FrameSize)
            : this(FrameSize, FrameSize) { }

        public Rectangle GetFrame(int frame) {

            Rectangle ret = new Rectangle(0, 0, FrameWidth, FrameHeight);

            if(MaxFrameX == -1 || MaxFrameY == -1) {

                if(Vertical) {
                    ret.Y = (FrameHeight + EdgeSize) * frame;
                } else {
                    ret.X = (FrameWidth + EdgeSize) * frame;
                }

            } else {

                int frX;
                int frY;

                if(Vertical) {
                    frY = frame % MaxFrameX;
                    frX = (int)Math.Floor(frame / (float)MaxFrameX);
                } else {
                    frX = frame % MaxFrameX;
                    frY = (int)Math.Floor(frame / (float)MaxFrameX);
                }

                ret.X = (FrameHeight + EdgeSize) * frX;
                ret.Y = (FrameHeight + EdgeSize) * frY;

            }

            return ret;

        }

    }

    public static class Drawing {

        public static Vector2 OffsetBy(this Vector2 vec, float x, float y) {
            vec.X += x;
            vec.Y += y;
            return vec;
        }
        public static Vector2 Offset(this ref Vector2 vec, float x, float y) => vec = vec.OffsetBy(x, y);

        public static Point OffsetBy(this Point pt, int x, int y) {
            pt.X += x;
            pt.Y += y;
            return pt;
        }
        public static Point Offset(this ref Point pt, int x, int y) => pt = pt.OffsetBy(x, y);

        public static Rectangle OffsetBy(this Rectangle rec, int x, int y) {
            rec.X += x;
            rec.Y += y;
            return rec;
        }
        public static Rectangle Offset(this ref Rectangle rec, int x, int y) => rec = rec.OffsetBy(x, y);

        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color)
            => spriteBatch.Draw(texture, position.Floor(), color);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
            => spriteBatch.Draw(texture, position.Floor(), sourceRectangle, color);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color)
            => spriteBatch.Draw(texture, position.Floor(), framer, frame, color);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), framer, frame, color, rotation, origin, scale, effects, layerDepth);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), framer, frame, color, rotation, origin, scale, effects, layerDepth);

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, TextureFramer framer, int frame, Color color)
            => spriteBatch.Draw(texture, destinationRectangle, framer.GetFrame(frame), color);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color)
            => spriteBatch.Draw(texture, position, framer.GetFrame(frame), color);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, destinationRectangle, framer.GetFrame(frame), color, rotation, origin, effects, layerDepth);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position, framer.GetFrame(frame), color, rotation, origin, scale, effects, layerDepth);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position, framer.GetFrame(frame), color, rotation, origin, scale, effects, layerDepth);

        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
            => spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
            => spriteBatch.Draw(Main.magicPixel, rectangle, color);
        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 size)
            => spriteBatch.DrawRect(position, size, Color.White);
        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rectangle)
            => spriteBatch.DrawRect(rectangle, Color.White);

        public static Rectangle RectFromCorners(Vector2 a, Vector2 b)
            => new Rectangle((int)Math.Min(a.X, b.X), (int)Math.Min(a.Y, b.Y), (int)Math.Abs(a.X - b.X), (int)Math.Abs(a.Y - b.Y));
        public static Rectangle RectFromCorners(Point a, Point b)
            => RectFromCorners(a.ToVector2(), b.ToVector2());
        public static Rectangle RectFromCorners(Point16 a, Point16 b)
            => RectFromCorners(a.ToVector2(), b.ToVector2());

    }
}
