using System;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using ChemistryClass.Dusts;
using System.Security.Policy;
using ChemistryClass.Tiles.Blocks;
using System.IO;
using Terraria.ID;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class SulfuricCloud : ModNPCPlus {

        public int AlphaPhase {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        public int SpawnFrame {
            get => (int)npc.ai[1];
            set => npc.ai[1] = value;
        }
        public int AlphaTimerMax {
            get => (int)npc.ai[2];
            set => npc.ai[2] = value;
        }

        public override void SafeSetDefaults() {

            npc.lifeMax = 1;
            npc.defense = 0;

            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.npcSlots = 0.1f;

            //npc.visualOffset = new Vector2(0, 16);

            npc.damage = -1;
            npc.value = 0;

            npc.scale = Main.rand.NextFloat(0.8f, 1.8f);

            npc.width = 32;
            npc.height = 14;

            npc.alpha = 255;

            npc.velocity = new Vector2(Main.rand.NextFloat(0.05f, 0.9f) * Main.rand.Next( new int[] { -1, 1 } ), 0);
            npc.oldVelocity = npc.velocity;

            npc.position -= npc.velocity * Main.rand.Next(100, 151);

            npc.DeathSound = SoundID.NPCDeath3;

            SpawnFrame = Main.rand.Next(0, 3);
            AlphaTimerMax = Main.rand.Next(30, 61);
            //ColorMultiple = Main.rand.NextFloat(1f, 1.2f);

            Main.npcFrameCount[npc.type] = 3;

            npc.aiStyle = -1;

        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
            => false;

        public override void AI() {

            if (ActiveCounter == 0) {

                npc.position -= npc.velocity * Main.rand.Next(150, 226);
                npc.position.Y += Main.rand.NextFloat(-128f, 128f);

            }

            npc.frame.Y = (SpawnFrame + ActiveCounter / (14 - SpawnFrame)) % 3 * 16;
            npc.frame.Height = 14;

            //DEBUGGING
            //ChemistryClass.SparseDebug("SULFURIC CLOUD");

            npc.velocity = npc.oldVelocity;
            npc.direction = 1;

            if (AlphaPhase == 1 || AlphaPhase == 2) {
                npc.damage = 50;
                npc.dontTakeDamage = false;
            } else {
                npc.damage = -1;
                npc.dontTakeDamage = true;
            }

            if (npc.alpha >= 255 && AlphaPhase == 2) npc.active = false;

            if (AlphaPhase == 0) if ((npc.alpha -= 3) <= 100) { AlphaPhase = 1; FrameCounter = 0; }
            if (AlphaPhase == 1) if (FrameCounter >= AlphaTimerMax) AlphaPhase = 2;
            if (AlphaPhase == 2) npc.alpha += 3;

            if (ChemistryClass.TimeIsMultOf(15 + SpawnFrame * 10)) SpawnDusts(1, false);
           
        }

        public override void HitEffect(int hitDirection, double damage)
            => SpawnDusts(Main.rand.Next(8, 11), true);

        public void SpawnDusts(int amt, bool larger) {

            Vector2 randVec = Vector2.UnitX.RotatedByRandom(CCUtils.TWO_PI_FLOAT);

            Dust dust = Dust.NewDustDirect(

                npc.position,
                npc.width,
                npc.height,
                ModContent.DustType<SulfurDust>(),
                randVec.X,
                randVec.Y

                );

            dust.alpha = npc.alpha + Main.rand.Next(-10, 10);
            dust.scale += larger ? Main.rand.NextFloat(0.8f, 1.3f) : 0;

            if (amt > 1) SpawnDusts(--amt, larger);

        }

        public override void DrawEffects(ref Color drawColor) {

            //DEBUGGING
            //drawColor = Color.White;
            //ChemistryClass.SparseDebug("OLD: " + drawColor);

            Vector3 color = new Vector3 {
                X = drawColor.R * 1.1f + SulfuricOreTile.LightColor.X * (255 - npc.alpha) * 2,
                Y = drawColor.G * 1.1f + SulfuricOreTile.LightColor.Y * (255 - npc.alpha) * 2,
                Z = drawColor.B * 1.1f + SulfuricOreTile.LightColor.Z * (255 - npc.alpha) * 2
            };

            drawColor.R = color.X > 255 ? (byte)255 : (byte)color.X;
            drawColor.G = color.Y > 255 ? (byte)255 : (byte)color.Y;
            drawColor.B = color.Z > 255 ? (byte)255 : (byte)color.Z;

            //ChemistryClass.SparseDebug("NEW: " + drawColor);
            //Lighting.AddLight(npc.Center, SulfuricOreTile.LightColor * (255 - npc.alpha) / 64);

        }
    }
}
