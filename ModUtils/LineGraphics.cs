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
            value |= t.active() && Main.tileSolid[t.type] && !Main.tileSolidTop[t.type];
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
