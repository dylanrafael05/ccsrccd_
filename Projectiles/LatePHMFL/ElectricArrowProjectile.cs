using System;
using System.Collections.Generic;
using System.IO;
using ChemistryClass.ModUtils;
using ChemistryClass.Projectiles.LatePHMFL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class ElectricArrowProjectile : ModProjectilePlus {

        public static readonly Color OuterColor = new Color(130, 180, 180);
        public static readonly Color InnerColor = new Color(200, 255, 255);

        public int DustType => DustID.Electric;
        public override string Texture => "Terraria/Item_0";

        public Vector2 previousPos2, previousPos3;

        public float Counter {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void SafeSetDefaults() {

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.damage = 6;

            projectile.width = 8;
            projectile.height = 8;

            projectile.damage = 6;

            projectile.ignoreWater = true;

        }

        public override void FirstFrame() {
            previousPos2 =
            previousPos3 = projectile.position;
        }

        public override void AI() {

            projectile.rotation = projectile.velocity.ToRotation() - CCUtils.HALF_PI_FLOAT;

            projectile.velocity.Y += 0.15f;
            projectile.velocity.Y.Clamp(-30, 30);

            Lighting.AddLight(projectile.Center, 0, 0.8f, 0.8f);

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            int rand = Main.rand.Next(2, 5);
            for (int __ = 0; __ < rand; __++) Projectile.NewProjectile(
                projectile.Center, Main.rand.NextVector2CircularEdge(6, 6),
                ModContent.ProjectileType<ShortLightning>(), damage / 3, 0f, projectile.owner);
            for (int _ = 0; _ < 5; _++) newDust();
        }

        public override void OnHitPvp(Player target, int damage, bool crit) {
            int rand = Main.rand.Next(2, 5);
            for (int __ = 0; __ < rand; __++) Projectile.NewProjectile(
                projectile.Center, Main.rand.NextVector2CircularEdge(6, 6),
                ModContent.ProjectileType<ShortLightning>(), damage / 3, 0f, projectile.owner);
            for (int _ = 0; _ < 5; _++) newDust();
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            for (int _ = 0; _ < 5; _++) newDust();
            return true;
        }

        public override void SafeSendExtraAI(BinaryWriter writer) {
            writer.WriteVector2(previousPos3);
            writer.WriteVector2(previousPos2);
        }

        public override void SafeReceiveExtraAI(BinaryReader reader) {
            previousPos2 = reader.ReadVector2();
            previousPos3 = reader.ReadVector2();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {

            if (Counter > 0) {
                spriteBatch.DrawLine(
                    projectile.position - Main.screenPosition, projectile.oldPosition - Main.screenPosition, 8,
                    LineSegmentDrawing.RoundedEnd8PX, null, LineSegmentDrawing.RoundedEnd8PX, null,
                    OuterColor);
                spriteBatch.DrawLine(
                    projectile.position - Main.screenPosition, projectile.oldPosition - Main.screenPosition, 4,
                    LineSegmentDrawing.RoundedEnd8PX, null, LineSegmentDrawing.RoundedEnd8PX, null,
                    InnerColor);
            }

            if(Counter > 1) {
                spriteBatch.DrawLine(
                    projectile.oldPosition - Main.screenPosition, previousPos2 - Main.screenPosition, 8,
                    LineSegmentDrawing.RoundedEnd8PX, null, LineSegmentDrawing.RoundedEnd8PX, null,
                    OuterColor * 0.6f);
                spriteBatch.DrawLine(
                    projectile.oldPosition - Main.screenPosition, previousPos2 - Main.screenPosition, 4,
                    LineSegmentDrawing.RoundedEnd8PX, null, LineSegmentDrawing.RoundedEnd8PX, null,
                    InnerColor * 0.6f);
            }

            if (Counter > 2) {
                spriteBatch.DrawLine(
                    previousPos2 - Main.screenPosition, previousPos3 - Main.screenPosition, 8,
                    LineSegmentDrawing.RoundedEnd8PX, null, LineSegmentDrawing.RoundedEnd8PX, null,
                    OuterColor * 0.2f);
                spriteBatch.DrawLine(
                    previousPos2 - Main.screenPosition, previousPos3 - Main.screenPosition, 4,
                    LineSegmentDrawing.RoundedEnd8PX, null, LineSegmentDrawing.RoundedEnd8PX, null,
                    InnerColor * 0.2f);
            }

            previousPos3 = previousPos2;
            previousPos2 = projectile.oldPosition;

            Counter++;

            return false;

        }

        private void newDust(int type = -1) {

            if (type == -1) type = DustType;

            Dust.NewDust(projectile.Center,
                             0, 0, type,
                             Main.rand.NextFloat(-1, 1),
                             Main.rand.NextFloat(-1, 1));

        }

    }
}
