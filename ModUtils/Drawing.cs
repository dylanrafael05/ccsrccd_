﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using IL.Terraria.Achievements;
using Microsoft.SqlServer.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ChemistryClass.ModUtils {
    public static partial class Drawing {

        public static float GetFixedScale(float orig, int width)
            => (float)Math.Ceiling(orig * width) / width;
        public static Vector2 GetFixedScale(Vector2 orig, Vector2 size)
            => new Vector2(GetFixedScale(orig.X, CCUtils.RoundToInt(size.X)), GetFixedScale(orig.Y, CCUtils.RoundToInt(size.Y)));

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

        public static Point OffsetBy(this Point pt, Point other) {
            pt.X += other.X;
            pt.Y += other.Y;
            return pt;
        }
        public static Point Offset(this ref Point pt, Point other) => pt = pt.OffsetBy(other);

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
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), sourceRectangle, color, rotation, origin, GetFixedScale(scale, texture.Width), spriteEffects, layerDepth);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), sourceRectangle, color, rotation, origin, GetFixedScale(scale, texture.Size()), spriteEffects, layerDepth);

        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
            => spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
            => spriteBatch.Draw(Main.magicPixel, rectangle, color);
        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 size)
            => spriteBatch.DrawRect(position, size, Color.White);
        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rectangle)
            => spriteBatch.DrawRect(rectangle, Color.White);
        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color, float rotation, float layerDepth)
            => spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, rotation, Vector2.Zero, SpriteEffects.None, layerDepth);
        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color, float rotation, Vector2 origin, float layerDepth)
            => spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, rotation, origin, SpriteEffects.None, layerDepth);

        public static Rectangle RectFromCorners(Vector2 a, Vector2 b)
            => new Rectangle((int)Math.Min(a.X, b.X), (int)Math.Min(a.Y, b.Y), (int)Math.Abs(a.X - b.X), (int)Math.Abs(a.Y - b.Y));
        public static Rectangle RectFromCorners(Point a, Point b)
            => RectFromCorners(a.ToVector2(), b.ToVector2());
        public static Rectangle RectFromCorners(Point16 a, Point16 b)
            => RectFromCorners(a.ToVector2(), b.ToVector2());

        public static Rectangle CenteredRescale(this Rectangle rect, float xFac, float yFac)
            => new Rectangle(rect.X + (int)(rect.Width * (1 - xFac)) / 2, rect.Y + (int)(rect.Height * (1 - yFac)) / 2, (int)(rect.Width * xFac), (int)(rect.Height * yFac));
        public static Rectangle CenteredRescale(this Rectangle rect, Vector2 fac)
            => rect.CenteredRescale(fac.X, fac.Y);
        public static Rectangle CenteredRescale(this Rectangle rect, float fac)
            => rect.CenteredRescale(fac, fac);

    }

    //public class LineGraphic {

    //    public Texture2D EndTexture { get; } = DefaultEndTexture;
    //    public static Texture2D DefaultEndTexture => ModContent.GetTexture("ChemistryClass/ModUtils/LineEnd");

    //    public Vector2 Start;
    //    public Vector2 End;
    //    public int Width;

    //    public Vector2 Vector => End - Start;
    //    public float Length => Vector2.Distance(Start, End);

    //    public bool IsValid => Width > 0;

    //    public Vector2 Scaling => new Vector2(Width, (int)Math.Ceiling(Length));

    //    public float Rotation => Vector.ToRotation();// + CCUtils.HALF_PI_FLOAT;
    //    public LinearFunction LinearFunc => LinearFunction.FromPoints(Start, End);

    //    public float CapScaling => (float)Width / EndTexture.Width;
    //    public float CapHeight => CapScaling * EndTexture.Height;
    //    public float CapWidth => CapScaling * EndTexture.Width;

    //    public Vector2 StartDP => Start - Main.screenPosition;
    //    public Vector2 EndDP => End - Main.screenPosition;

    //    public LineGraphic(Vector2 start, Vector2 end, int width) {
    //        Start = start;
    //        End = end;
    //        Width = width;
    //    }

    //    public bool Colliding(Rectangle rect)
    //        => Collision.CheckAABBvLineCollision(rect.TopLeft(), rect.Size(), Start, End);
    //    public LineGraphic GetSubsection(float start, float end) {
    //        start.Clamp(0, 1);
    //        end.Clamp(start, 1);
    //        return new LineGraphic(Start + Vector * start, Start + Vector * end, Width);
    //    }

    //    public static LineGraphic FromVector(Vector2 start, Vector2 vector, int width)
    //        => new LineGraphic(start, Drawing.OffsetBy(start, vector.X, vector.Y), width);

    //}

    //public class LineStructure {

    //    private Vector2[] verts;
    //    public Vector2[] Vertices {
    //        get => verts;
    //        set {
    //            verts = value;
    //            //if (!AutoTotal) return;
    //            TotalLength = 0;
    //            for (int i = 0; i < SegmentCount; i++) {
    //                TotalLength += this[i].Length;
    //            }
    //        }
    //    }
    //    public int Width;

    //    //public float StartOffset;
    //    //public float EndOffset;

    //    //public bool AutoTotal = true;

    //    public int VertexCount => Vertices.Length;
    //    public int SegmentCount => VertexCount - 1;

    //    public Vector2 Endpoint => Vertices[SegmentCount];

    //    public bool IsValid => SegmentCount > 0 && Width > 0;

    //    public float TotalLength { get; protected set; }

    //    public LineStructure(Vector2[] vertices, int width) {
    //        Vertices = vertices;
    //        Width = width;
    //    }
    //    public LineStructure(int width, params Vector2[] vertices) {
    //        Vertices = vertices;
    //        Width = width;
    //    }
    //    public LineStructure() : this(-1) { }

    //    public LineGraphic this[int i] => new LineGraphic(Vertices[i], Vertices[i + 1], Width);

    //    public LineStructure GetSubsection(float start, float end) {

    //        start.Clamp(0f, 1f);
    //        end.Clamp(start, 1f);

    //        List<Vector2> temp = new List<Vector2>();

    //        float startLenExact = start * TotalLength;
    //        float endLenExact = end * TotalLength;

    //        int startIndex = -1;
    //        int endIndex = -1;

    //        float startLT = -1;
    //        float endLT = -1;

    //        float startLineL = -1;
    //        float endLineL = -1;

    //        float curLength;
    //        float lengthTotal = 0;

    //        for (int i = 0; i < SegmentCount; i++) {

    //            curLength = this[i].Length;
    //            lengthTotal += this[i].Length;

    //            if (lengthTotal >= startLenExact && startIndex == -1) {
    //                startIndex = i;
    //                startLT = lengthTotal - curLength;
    //                startLineL = curLength;
    //            }
    //            if (lengthTotal >= endLenExact && endIndex == -1) {
    //                endIndex = i;
    //                endLT = lengthTotal - curLength;
    //                endLineL = curLength;
    //            }

    //            if (startIndex != -1 && endIndex != -1) break;
    //        }

    //        if (startIndex == -1) startIndex = SegmentCount - 1;
    //        if (endIndex == -1) endIndex = SegmentCount - 1;

    //        float startSub = (startLenExact - startLT) / startLineL;
    //        float endSub = (endLenExact - endLT) / endLineL;

    //        //FILTER OUT BABEES
    //        if (startIndex == endIndex) {

    //            LineGraphic gph = this[startIndex].GetSubsection(startSub, endSub);
    //            temp.Add(gph.Start);
    //            temp.Add(gph.End);

    //        } else {

    //            temp.Add(this[startIndex].Start + this[startIndex].Vector * startSub);

    //            for (int i = startIndex + 1; i <= endIndex; i++) {
    //                temp.Add(this[i].Start);
    //            }

    //            temp.Add(this[endIndex].Start + this[endIndex].Vector * endSub);

    //        }

    //        return new LineStructure(temp.ToArray(), Width);

    //    }

    //    public bool Colliding(Rectangle rect) {
    //        for (int i = 0; i < SegmentCount; i++) {
    //            if (this[i].Colliding(rect)) return true;
    //        }
    //        return false;
    //    }

    //    //IMPLEMENT THAT THE POINTS OF THE LINE CANNOT CHANGE BY MORE THAN 45˚ AT A TIME
    //    //this will be to prevent sharp curves for MoveLightningProcedural.
    //    //mess around with values here
    //    public static LineStructure NewLightning(Vector2 pointA, Vector2 pointB, float minLength, float maxLength, int width) {

    //        List<Vector2> vecs = new List<Vector2>();
    //        Vector2 runningTotal = pointA;

    //        float curDist, randRot;
    //        Vector2 toAdd;

    //        int iter = 0;

    //        vecs.Add(pointA);

    //        while (iter < 100) {

    //            curDist = Vector2.Distance(runningTotal, pointB);

    //            if (curDist < maxLength) break;

    //            toAdd = (pointB - runningTotal).GetClampMagnitude(0, Main.rand.NextFloat(minLength, maxLength));

    //            randRot = Main.rand.NextFloat(-CCUtils.QUARTER_PI_FLOAT, CCUtils.QUARTER_PI_FLOAT);
    //            toAdd = toAdd.RotatedBy(randRot);

    //            runningTotal += toAdd;
    //            vecs.Add(runningTotal);

    //            iter++;

    //        }

    //        vecs.Add(pointB);

    //        return new LineStructure(vecs.ToArray(), width);

    //    }

    //    //CREATE "NewLightningRaw" => raw Vector2[] data of NewLightning

    //    //CREATE "MergeWith" => combine two structures, merging start-to-end if possible

    //    //REDO BELOW:
    //    //The method should return the structure to draw using GetSubsection() for safety
    //    //New segments will be added using NewLightningRaw and MergeWith, and old segments will simple be deleted
    //    //The parameters for GetSubsection will be calculated using a formula of some kind
    //    public bool MoveLightningProcedural(Vector2 defaultVec, Vector2? target, float distanceToTravel, float targetLen, float maxTurn = CCUtils.HALF_PI_FLOAT, bool isEnding = false, bool isStarting = false) {

    //        if (SegmentCount < 1) return false;

    //        //TAIL
    //        Vector2 toAdd = this[0].Vector;
    //        float segmentLength = this[0].Length;

    //        if (isStarting) goto ADD_HEAD;

    //        if (segmentLength < distanceToTravel) {

    //            Vertices = Vertices.Where((vec, i) => i != 0).ToArray();
    //            if (SegmentCount < 1) return false;

    //            Vertices[0] += toAdd.WithMagnitude(distanceToTravel - segmentLength);

    //        } else Vertices[0] += toAdd.WithMagnitude(distanceToTravel);

    //        //HEAD
    //        ADD_HEAD:
    //        if (isEnding) return true;

    //        toAdd = this[SegmentCount - 1].Vector;
    //        segmentLength = this[SegmentCount - 1].Length;

    //        if (targetLen - segmentLength < distanceToTravel) {

    //            List<Vector2> verts = Vertices.ToList();

    //            if (target == null) {
    //                toAdd = defaultVec;
    //            } else {
    //                toAdd = target.Value - verts[SegmentCount];
    //            }

    //            toAdd = this[SegmentCount - 1].Vector.RotatedBy(
    //                //CCUtils.PI_FLOAT +
    //                CCUtils.ShortestRotTo(toAdd.ToRotation(), this[SegmentCount - 1].Rotation).GetClamp(-maxTurn, maxTurn) +
    //                Main.rand.NextFloat(-maxTurn / 2, maxTurn / 2));

    //            verts[SegmentCount] = Vertices[SegmentCount - 1] + this[SegmentCount - 1].Vector.WithMagnitude(targetLen);
    //            verts.Add(verts[SegmentCount] + toAdd.WithMagnitude(distanceToTravel + segmentLength - targetLen));

    //            Vertices = verts.ToArray();

    //        } else Vertices[SegmentCount] += toAdd.WithMagnitude(distanceToTravel);

    //        return true;

    //    }

    //    public override string ToString()
    //        => string.Join(" => ", Vertices);

    //}

    //public static partial class Drawing {

    //    public static void Draw(this SpriteBatch spriteBatch, LineGraphic line, Color color) {

    //        if (!line.IsValid) return;

    //        float rot = line.Rotation + CCUtils.HALF_PI_FLOAT;
    //        Vector2 orig = new Vector2(line.EndTexture.Width / 2, line.EndTexture.Height);
    //        Vector2 orig2 = new Vector2(0.5f, 0f);

    //        spriteBatch.Draw(
    //            line.EndTexture,
    //            line.StartDP,
    //            null,
    //            color,
    //            rot + CCUtils.PI_FLOAT,
    //            orig,
    //            line.CapScaling,
    //            SpriteEffects.None,
    //            1f
    //            );

    //        spriteBatch.DrawRect(
    //            line.StartDP,
    //            line.Scaling,
    //            color,
    //            rot + CCUtils.PI_FLOAT,
    //            orig2,
    //            1f
    //            );

    //        spriteBatch.Draw(
    //            line.EndTexture,
    //            line.EndDP,
    //            null,
    //            color,
    //            rot,
    //            orig,
    //            line.CapScaling,
    //            SpriteEffects.None,
    //            1f
    //            );

    //    }

    //    public static void Draw(this SpriteBatch spriteBatch, LineStructure line, Color color) {
    //        if (!line.IsValid) return;
    //        for (int i = 0; i < line.SegmentCount; i++) {
    //            spriteBatch.Draw(line[i], color);
    //        }
    //    }

    //    public static void Write(this BinaryWriter writer, LineGraphic gph) {
    //        writer.Write(gph.Start.Y);
    //        writer.Write(gph.Start.X);
    //        writer.Write(gph.End.Y);
    //        writer.Write(gph.End.X);
    //        writer.Write(gph.Width);
    //    }

    //    public static LineGraphic ReadLineGraphic(this BinaryReader reader) {
    //        int wdth = reader.Read();
    //        Vector2 end = new Vector2(reader.ReadSingle(), reader.ReadSingle());
    //        Vector2 start = new Vector2(reader.ReadSingle(), reader.ReadSingle());

    //        return new LineGraphic(start, end, wdth);
    //    }

    //    public static void Write(this BinaryWriter writer, LineStructure structure) {
    //        foreach(var vec in structure.Vertices) {
    //            writer.Write(vec.Y);
    //            writer.Write(vec.X);
    //        }
    //        writer.Write(structure.VertexCount);
    //        writer.Write(structure.Width);
    //    }

    //    public static LineStructure ReadLineStructure(this BinaryReader reader) {
    //        int wdth = reader.Read();
    //        int len = reader.Read();
    //        Vector2[] vecs = new Vector2[len];

    //        for (int i = len - 1; i >= 0; i--) {
    //            vecs[i] = new Vector2(reader.ReadSingle(), reader.ReadSingle());
    //        }

    //        return new LineStructure(vecs, wdth);
    //    }

    //}

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

            if (MaxFrameX == -1 || MaxFrameY == -1) {

                if (Vertical) {
                    ret.Y = (FrameHeight + EdgeSize) * frame;
                } else {
                    ret.X = (FrameWidth + EdgeSize) * frame;
                }

            } else {

                int frX;
                int frY;

                if (Vertical) {
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

    public static partial class Drawing {

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, TextureFramer framer, int frame, Color color)
            => spriteBatch.Draw(texture, destinationRectangle, framer.GetFrame(frame), color);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color)
            => spriteBatch.Draw(texture, position, framer.GetFrame(frame), color);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, destinationRectangle, framer.GetFrame(frame), color, rotation, origin, effects, layerDepth);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position, framer.GetFrame(frame), color, rotation, origin, GetFixedScale(scale, texture.Width), effects, layerDepth);
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position, framer.GetFrame(frame), color, rotation, origin, GetFixedScale(scale, texture.Size()), effects, layerDepth);

        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color)
            => spriteBatch.Draw(texture, position.Floor(), framer, frame, color);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), framer, frame, color, rotation, origin, scale, effects, layerDepth);
        public static void DrawFixed(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, TextureFramer framer, int frame, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => spriteBatch.Draw(texture, position.Floor(), framer, frame, color, rotation, origin, scale, effects, layerDepth);

    }

}
