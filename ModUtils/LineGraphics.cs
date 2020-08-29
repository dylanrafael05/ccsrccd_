using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Microsoft.SqlServer.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.ModUtils {

    /*
    [Flags] public enum LineDrawType : byte {
        Normal = 0b_00,
        Capped = 0b_01,
        Tiled  = 0b_10
    }

    public class LineGraphicNEW {

        //FIELDS//
        public Vector2 Start;
        public Vector2 End;

        public int Width;

        public float StartOff = 0f;
        public float EndOff = 1f;

        //CONSTRUCTORS AND CONSTRUCTION METHODS//
        public LineGraphicNEW(Vector2 start, Vector2 end, int width) {
            Start = start;
            End = end;
            Width = width;
        }

        public static LineGraphicNEW FromVector(Vector2 start, Vector2 vec, int width) {
            LineGraphicNEW ret = new LineGraphicNEW(start, start + vec, width);
            return ret;
        }

        //MEMBERS//
        public Vector2 FullVector => End - Start;
        public float FullLength => FullVector.Length();

        public Vector2 RealStart => Start + FullVector * StartOff;
        public Vector2 RealEnd   => Start + FullVector * EndOff;
        public Vector2 RealVector => RealEnd - RealStart;
        public float RealLength => RealVector.Length();

        public Vector2 DrawStart => RealStart - Main.screenPosition;
        public Vector2 DrawEnd   => RealEnd - Main.screenPosition;

        public float Rotation => FullVector.ToRotation();

        public bool RequiresOffsetReload => StartOff != 0f && EndOff != 1f;

        public bool IsValid => FullLength > 0 && EndOff > StartOff;

        //METHODS//
        public float GetOffsetFromLength(float length)
            => length / FullLength;
        public void SetStartOffFromLength(float length)
            => StartOff = GetOffsetFromLength(length);
        public void SetEndOffFromLength(float length)
            => EndOff = GetOffsetFromLength(length);

        public bool LengthIsInFullBounds(float length, float startLength)
            => length - startLength <= FullLength && length - startLength >= 0;
        public bool LengthIsInCurrentBounds(float length, float startLength)
            => length - startLength <= RealLength && length - startLength >= 0;

        //DRAWING//
        public void DrawEndsRaw(SpriteBatch spriteBatch, Color color, Texture2D head, Rectangle headFrame, Vector2 headOrigin, Texture2D tail, Rectangle tailFrame, Vector2 tailOrigin) {

            bool shouldDrawHead = head != null;
            bool shouldDrawTail = tail != null;

            float rotation = Rotation;

            if(shouldDrawHead) {

                spriteBatch.DrawFixed(
                    
                    head,
                    DrawStart,
                    headFrame,
                    color,
                    rotation + CCUtils.PI_FLOAT,
                    headOrigin,
                    (float)Width / headFrame.Width,
                    SpriteEffects.None,
                    1f

                    );

            }

            if (shouldDrawTail) {

                spriteBatch.DrawFixed(

                    tail,
                    DrawEnd,
                    tailFrame,
                    color,
                    rotation,
                    tailOrigin,
                    (float)Width / tailFrame.Width,
                    SpriteEffects.None,
                    1f

                    );

            }

        }

        public void DrawEndsCentered(SpriteBatch spriteBatch, Color color, Texture2D head, Rectangle headFrame, Texture2D tail, Rectangle tailFrame)
            => DrawEndsRaw(spriteBatch, color, head, headFrame, headFrame.Size() / 2, tail, tailFrame, tailFrame.Size() / 2);

        public void DrawEndsCapped(SpriteBatch spriteBatch, Color color, Texture2D head, Rectangle headFrame, Texture2D tail, Rectangle tailFrame)
            => DrawEndsRaw(spriteBatch, color, head, headFrame, new Vector2(headFrame.Width / 2f, 0), tail, tailFrame, new Vector2(tailFrame.Width / 2f, 0));

        public void DrawBodyFill(SpriteBatch spriteBatch, Color color, Texture2D body, Rectangle bodyFrame, Vector2 offsetHead, Vector2 offsetTail) {

            Vector2 scale = new Vector2(

                (RealEnd.X - RealStart.X - offsetHead.X - offsetTail.X) / bodyFrame.Width,
                Width / (float)bodyFrame.Height

                );

            spriteBatch.DrawFixed(
                body,
                DrawStart + offsetHead,
                bodyFrame,
                color,
                Rotation + CCUtils.HALF_PI_FLOAT,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                1f
                );

        }

        public void DrawBodyTiled(SpriteBatch spriteBatch, Color color, Texture2D body, Rectangle bodyFrame, Vector2 offsetHead, Vector2 offsetTail) {

            float realLen = RealLength - offsetHead.Length() - offsetTail.Length();

            int batches = (int)realLen / bodyFrame.Width;

            batches.EnforceMin(1);
            if(batches > 50) batches.EnforceMax((int)Math.Sqrt(Main.screenHeight ^ 2 + Main.screenWidth ^ 2) / (int)realLen);

            float scaleFactor = realLen / batches;
            Vector2 toAdd = RealVector.WithMagnitude(realLen) / batches;

            for (int i = 0; i < batches; i++) {
                spriteBatch.DrawFixed(
                    body,
                    DrawStart + toAdd * i + offsetHead,
                    bodyFrame,
                    color,
                    Rotation + CCUtils.PI_FLOAT,
                    Vector2.Zero,
                    new Vector2(scaleFactor, Width / (float)bodyFrame.Height),
                    SpriteEffects.None,
                    1f
                    );
            }

        }

        public void DrawBodyTyped(SpriteBatch spriteBatch, Color color, Texture2D body, Rectangle bodyFrame, Vector2 offsetHead, Vector2 offsetTail, LineDrawType type) {

            if (!type.HasFlag(LineDrawType.Tiled)) DrawBodyFill(spriteBatch, color, body, bodyFrame, offsetHead, offsetTail);
            else DrawBodyTiled(spriteBatch, color, body, bodyFrame, offsetHead, offsetTail);

        }

        public void Draw(SpriteBatch spriteBatch, Color color, Texture2D body = null, Rectangle? bodyFrame = null, Texture2D head = null, Rectangle? headFrame = null, Texture2D tail = null, Rectangle? tailFrame = null, LineDrawType type = LineDrawType.Normal) {

            bodyFrame = bodyFrame ?? body.Bounds;
            headFrame = headFrame ?? head.Bounds;
            tailFrame = tailFrame ?? tail.Bounds;

            body = body ?? LineGraphicHelpers.SimpleBody;
            head = head ?? LineGraphicHelpers.RoundedEnd;
            tail = tail ?? LineGraphicHelpers.RoundedEnd;

            bool isCapped = type.HasFlag(LineDrawType.Capped);

            Vector2 offsetHead = isCapped ? new Vector2(0, -headFrame.Value.Height / 2).RotatedBy(Rotation) : Vector2.Zero;
            Vector2 offsetTail = isCapped ? new Vector2(0, -tailFrame.Value.Height / 2).RotatedBy(Rotation) : Vector2.Zero;

            if (isCapped) DrawEndsCapped(spriteBatch, color, head, headFrame.Value, tail, tailFrame.Value);
            else DrawEndsCentered(spriteBatch, color, head, headFrame.Value, tail, tailFrame.Value);

            DrawBodyTyped(spriteBatch, color, body, bodyFrame.Value, offsetHead, offsetTail, type);

        }


    }*/

    public struct LineSegment {

        public readonly Vector2 Start;
        public readonly Vector2 End;

        public LineSegment(Vector2 start, Vector2 end) {
            Start = start;
            End = end;
        }

        public Vector2 Vector => End - Start;
        public float Rotation => Vector.ToRotation();
        public float Length => Vector.Length();

        public LineSegment GetSubsection(float start, float end)
            => new LineSegment(Start + Vector * start.GetClamp(0, 1), Start + Vector * end.GetClamp(0, 1));
        public LineSegment GetLengthSubsection(float start, float end)
            => GetSubsection(start / Length, end / Length);

        public LineSegment ToOnscreen() => this - Main.screenPosition;

        public static LineSegment operator +(LineSegment seg) => seg;
        public static LineSegment operator -(LineSegment seg) => new LineSegment(seg.End, seg.Start);

        public static LineSegment operator +(LineSegment seg, Vector2 vec) => new LineSegment(seg.Start + vec, seg.End + vec);
        public static LineSegment operator -(LineSegment seg, Vector2 vec) => seg +- vec;

        public bool Plot(int width, Utils.PerLinePoint method)
            => Utils.PlotLine(Start.ToPoint().OffsetBy(Vector.WithMagnitude(-width).ToPoint()), End.ToPoint().OffsetBy(Vector.WithMagnitude(width).ToPoint()), method);
        public bool PlotTile(int width, Utils.PerLinePoint method)
            => Utils.PlotTileLine(Start + Vector.WithMagnitude(-width), End + Vector.WithMagnitude(width), width, method);

    }

    public class ProceduralLineSequence : IEnumerable<LineSegment> {

        public List<Vector2> Vertices { get; private set; } = new List<Vector2>();

        public List<float> CurrentOffsets { get; private set; } = new List<float>();

        public float StartLength = 0f;
        public float EndLength = 0f;

        public int VertexCount => Vertices.Count;
        public int SegmentCount => VertexCount - 1;

        public float PastLength => CurrentOffsets.Count > 0 ? CurrentOffsets[0] : 0;
        public float TotalLength => CurrentOffsets.Count > 0 ? CurrentOffsets.Last() : 0;

        public Vector2 Endpoint => Vertices[SegmentCount];
        public Vector2 Startpoint => Vertices[0];

        public Vector2 VisualEndpoint => this.LastOrDefault().End;
        public Vector2 VisualStartpoint => this.FirstOrDefault().End;

        public bool IsValid => Vertices.Count > 1;

        public LineSegment this[int index]
            => new LineSegment(Vertices[index], Vertices[index + 1]).GetLengthSubsection(StartLength - CurrentOffsets[index], EndLength - CurrentOffsets[index]);

        public IEnumerator<LineSegment> GetEnumerator() {
            List<LineSegment> list = new List<LineSegment>();
            for (int i = 0; i < SegmentCount; i++) {
                list.Add(this[i]);
            }
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            List<LineSegment> list = new List<LineSegment>();
            for (int i = 0; i < SegmentCount; i++) {
                list.Add(this[i]);
            }
            return list.GetEnumerator();
        }

        public void ExtendSequence(Vector2 vertex) {

            if (VertexCount > 0) {
                float len = (Vertices.Last() - vertex).Length();
                CurrentOffsets.Add(len + CurrentOffsets.Last());
            } else {
                CurrentOffsets.Add(0f);
            }

            Vertices.Add(vertex);

        }

        public void ShortenSequence() {

            float curLen = PastLength;
            CurrentOffsets.RemoveAt(0);

            Vertices.RemoveAt(0);

        }

        public void AdvanceSequence(float lengthToAdvance, float targetLength, Vector2 vertexToAdd, bool shouldContinue = true) {

            bool shouldShorten = TotalLength < targetLength ? !shouldContinue : true;

            if (shouldContinue) {
                EndLength += lengthToAdvance;
                while (EndLength > TotalLength) ExtendSequence(vertexToAdd);
            }
            if (shouldShorten) {
                StartLength += lengthToAdvance;
                if (CurrentOffsets.Count < 1) return;
                while (StartLength > CurrentOffsets[1]) ShortenSequence();
            }

        }

        public void AdvanceSequence(float lengthToAdvance, float targetLength, Func<Vector2> vertexGetter, bool shouldContinue = true) {

            bool shouldShorten = TotalLength < targetLength ? !shouldContinue : true;

            //if (shouldShorten) StartLength += lengthToAdvance;
            //if (shouldExtend) EndLength += lengthToAdvance;

            //if (EndLength > TotalLength && shouldExtend) ExtendSequence(vertexGetter());
            //if (CurrentOffsets.Count < 1) return;
            //if (StartLength > CurrentOffsets[1] && shouldShorten) ShortenSequence();

            if (shouldContinue) {
                EndLength += lengthToAdvance;
                if (EndLength > TotalLength) ExtendSequence(vertexGetter());
            }
            if (shouldShorten) {
                StartLength += lengthToAdvance;
                if (CurrentOffsets.Count < 1) return;
                if (StartLength > CurrentOffsets[1]) ShortenSequence();
            }

        }

        public bool RequiresNewVertex(float lengthToAdvance)
            => EndLength + lengthToAdvance > TotalLength;

        public ProceduralLineSequence(Vector2 a, params Vector2[] verts) {
            ExtendSequence(a);
            foreach (var vert in verts) {
                ExtendSequence(vert);
            }
        }

        public ProceduralLineSequence(Vector2 a, Vector2 b) {
            ExtendSequence(a);
            ExtendSequence(b);
        }

        public ProceduralLineSequence(Vector2 a) {
            ExtendSequence(a);
        }

        public ProceduralLineSequence() {}

        //SENDING
        public void Send(BinaryWriter writer) {
            foreach(var item in CurrentOffsets) {
                writer.Write(item);
            }
            writer.Write(CurrentOffsets.Count);
            foreach (var item in Vertices) {
                writer.WriteVector2(item);
            }
            writer.Write(VertexCount);
        }

        public static ProceduralLineSequence ReadSeq(BinaryReader reader) {

            List<Vector2> vertex = new List<Vector2>();
            int length = reader.Read();
            for(int i = 0; i < length; i++) {
                vertex.Add(reader.ReadVector2());
            }

            List<float> curOffs = new List<float>();
            length = reader.Read();
            for (int i = 0; i < length; i++) {
                curOffs.Add(reader.ReadSingle());
            }

            return new ProceduralLineSequence() {

                Vertices = vertex,
                CurrentOffsets = curOffs

            };

        }

        //PLOTTING
        public bool Plot(int width, Utils.PerLinePoint method)
            => this.All(line => line.Plot(width, method));
        public bool PlotTile(int width, Utils.PerLinePoint method)
            => this.All(line => line.PlotTile(width, method));

    }

    public static class LineSegmentDrawing {

        public static Texture2D RoundedEnd { get; } = ModContent.GetTexture("ChemistryClass/ModUtils/LineEnd");
        public static Texture2D RoundedEnd8PX { get; } = ModContent.GetTexture("ChemistryClass/ModUtils/LineEnd8");

        public static void DrawLine(this SpriteBatch spriteBatch, LineSegment line, int width, Texture2D headTexture = null, Rectangle? headFrame = null, Texture2D tailTexture = null, Rectangle? tailFrame = null, Color color = default) {

            width.EnforceMin(1);

            headTexture = headTexture ?? RoundedEnd;
            tailTexture = tailTexture ?? RoundedEnd;

            Rectangle hFrame = headFrame ?? headTexture.Bounds;
            Rectangle tFrame = tailFrame ?? tailTexture.Bounds;

            float rot = line.Rotation + CCUtils.HALF_PI_FLOAT;
            Vector2 originHead = new Vector2(hFrame.Width / 2, hFrame.Height);
            Vector2 originBody = new Vector2(0.5f, 0f);
            Vector2 originTail = new Vector2(tFrame.Width / 2, tFrame.Height);

            spriteBatch.Draw(
                headTexture,
                line.Start,
                hFrame,
                color,
                rot + CCUtils.PI_FLOAT,
                originHead,
                (float)width / hFrame.Width,
                SpriteEffects.None,
                1f
                );

            spriteBatch.DrawRect(
                line.Start,
                new Vector2(width, (int)Math.Ceiling(line.Length)),
                color,
                rot + CCUtils.PI_FLOAT,
                originBody,
                1f
                );

            spriteBatch.Draw(
                tailTexture,
                line.End,
                tFrame,
                color,
                rot,
                originTail,
                (float)width / hFrame.Width,
                SpriteEffects.None,
                1f
                );

        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, int width, Texture2D headTexture = null, Rectangle? headFrame = null, Texture2D tailTexture = null, Rectangle? tailFrame = null, Color color = default)
            => spriteBatch.DrawLine(new LineSegment(start, end), width, headTexture, headFrame, tailTexture, tailFrame, color);

        public static void DrawLineSequence(this SpriteBatch spriteBatch, ProceduralLineSequence lineSequence, int width, Vector2 offset, Texture2D headTexture = null, Rectangle? headFrame = null, Texture2D tailTexture = null, Rectangle? tailFrame = null, Color color = default) {

            foreach(var line in lineSequence) {
                if (!CCUtils.ScreenRectangle.Contains(line.Start.ToPoint()) && !CCUtils.ScreenRectangle.Contains(line.End.ToPoint())) continue;
                spriteBatch.DrawLine(line + offset, width, headTexture, headFrame, tailTexture, tailFrame, color);
            }

        }

    }

    public static class LineSegmentCollision {

        public static bool CheckAAABCollision(Rectangle rect, LineSegment line)
            => Collision.CheckAABBvLineCollision(rect.Location.ToVector2(), rect.Size(), line.Start, line.End);
        public static bool CheckAAABCollision(Vector2 aaabPos, Vector2 aaabSize, LineSegment line)
            => Collision.CheckAABBvLineCollision(aaabPos, aaabSize, line.Start, line.End);
        public static bool CheckAAABCollision(Vector2 aaabPos, Vector2 aaabSize, LineSegment line, float lineWidth, ref float collisionPoint)
            => Collision.CheckAABBvLineCollision(aaabPos, aaabSize, line.Start, line.End, lineWidth, ref collisionPoint);

        public static Vector2[] CheckLineCollision(LineSegment lineA, LineSegment lineB)
            => Collision.CheckLinevLine(lineA.Start, lineA.End, lineB.Start, lineB.End);

        public static bool TileCollisionPPL(int x, int y, ref bool value) {
            Tile t = Framing.GetTileSafely(x, y);
            value |= t.active() && Main.tileSolid[t.type];
            return true;
        }

        public static bool CheckTileCollision(LineSegment line, int width) {
            bool ret = false;
            Utils.PlotTileLine(line.Start + line.Vector.WithMagnitude(-width), line.End + line.Vector.WithMagnitude(width), width, (x, y) => TileCollisionPPL(x, y, ref ret));
            return ret;
        }

    }

    public static class LineSequenceCollision {

        public static bool CheckAAABCollision(Rectangle rect, ProceduralLineSequence lineSequence) {
            foreach(var line in lineSequence) {
                if (LineSegmentCollision.CheckAAABCollision(rect, line)) return true;
            }
            return false;
        }

        public static bool CheckAAABCollision(Vector2 aaabPos, Vector2 aaabSize, ProceduralLineSequence lineSequence) {
            foreach (var line in lineSequence) {
                if (LineSegmentCollision.CheckAAABCollision(aaabPos, aaabSize, line)) return true;
            }
            return false;
        }

        public static bool CheckAAABCollision(Rectangle rect, ProceduralLineSequence lineSequence, float lineWidth, ref float collision) {
            foreach (var line in lineSequence) {
                if (LineSegmentCollision.CheckAAABCollision(rect.Location.ToVector2(), rect.Size(), line, lineWidth, ref collision)) return true;
            }
            return false;
        }

        public static bool CheckAAABCollision(Vector2 aaabPos, Vector2 aaabSize, ProceduralLineSequence lineSequence, float lineWidth, ref float collision) {
            foreach (var line in lineSequence) {
                if (LineSegmentCollision.CheckAAABCollision(aaabPos, aaabSize, line, lineWidth, ref collision)) return true;
            }
            return false;
        }

        public static bool CheckTileCollision(ProceduralLineSequence lineSequence, int width) {
            foreach (var line in lineSequence) {
                if (LineSegmentCollision.CheckTileCollision(line, width)) return true;
            }
            return false;
        }

    }

}
