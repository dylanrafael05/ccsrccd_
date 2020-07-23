﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles {
    public class PristinePrismBeam : ModUtils.ModProjectilePlus {

        //CONSTS
        public const string innerTextureStr = "ChemistryClass/Projectiles/PristinePrismBeamInside";
        public override string Texture => "ChemistryClass/Projectiles/PristinePrismBeam";

        public const double beamSpread = CCUtils.PI / 10;
        private double waveSpread
            => beamSpread * (1 + Math.Cos(ChemistryClass.UnpausedUpdateCount / 20f) / 6f);
        private double curSpread;
        public const double rotSpeed = 1f / 20;
        public const float maxLength = 200;

        public static readonly Color[] colors = new Color[] //Basically a const
        {
            new Color(255, 102, 102, 100),
            new Color(255, 186, 102, 100),
            new Color(255, 240, 102, 100),
            new Color(145, 255, 102, 100),
            new Color(102, 204, 255, 100),
            new Color(102, 112, 255, 100),
            new Color(219, 102, 255, 100)
        };

        //ENCLOSERS
        public int HoldoutUUID => (int)projectile.ai[0];
        public int MotionOffset => (int)projectile.ai[1];

        public Color BeamColor => colors[MotionOffset];
        public Projectile HostProjectile => Main.projectile[HoldoutUUID];

        //SETTING DEFAULTS
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Pristine Prism Beam");
        }

        public override void SafeSetDefaults() {

            projectile.friendly = false;
            projectile.hostile = false;

            projectile.width = 8;
            projectile.height = 32;

            projectile.penetrate = -1;
            projectile.maxPenetrate = -1;

            projectile.damage = 2;
            projectile.knockBack = 0;

            projectile.tileCollide = false;

        }

        //FORCE LACK OF PROJECTILE MOVEMENT
        public override bool ShouldUpdatePosition() => false;

        //KILL IF HOLDOUT IS GONE AND ENSURE NO DESPAWNS OCCUR
        public void UpdateBeam() {

            //If anything goes wrong, delete the projectile
            if (

                HostProjectile == null ||
                !(HostProjectile.modProjectile is PristinePrismHoldout) ||
                !Main.player[projectile.owner].channel ||
                !HostProjectile.active

                ) projectile.Kill();

            //intro animation
            //Main.NewText(FrameCounter);

            if (FrameCounter == 0) {
                curSpread = 0;
            }

            if (FrameCounter > 0 && FrameCounter <= 40) {
                curSpread = waveSpread * (1 - Math.Pow(1.002, -Math.Pow(FrameCounter, 2)));
            }

            if (FrameCounter > 40) {
                curSpread = waveSpread;
                projectile.friendly = true;
            }

            //Setup velocity for this frame
            projectile.velocity = HostProjectile.velocity.RotatedBy(

                    curSpread *
                    Math.Sin(ChemistryClass.UnpausedUpdateCount * rotSpeed + CCUtils.TWO_PI * MotionOffset / 7 )

                    );

            //Snap to the host's position
            projectile.Center = HostProjectile.Center;

            //Rotate toward velocity
            projectile.rotation = projectile.velocity.ToRotation() - CCUtils.HALF_PI;

            //reset time left timer
            projectile.timeLeft = 2;

            //DEBUGGING
            //if (ChemistryClass.TimeIsMultOf(60)) Main.NewText("BEAM: " + MotionOffset);

        }

        //ENDPOINT DATA
        private (int, Vector2, bool) EndPointData() {

            int ret = 0;
            Vector2 vec = projectile.Center;
            Vector2 tileVec = vec;
            Vector2 beamVec = Vector2.Zero;
            bool returnEarly = false;

            while(beamVec.Length() < maxLength) {

                vec += projectile.velocity;
                beamVec += projectile.velocity;
                ret++;

                for ( int _ = 0; _ < 3; _++ ) {

                    tileVec += projectile.velocity / 3f;

                    Tile tile = Framing.GetTileSafely(tileVec.Floor());

                    if (tile.active() && Main.tileSolid[tile.type]) {
                        returnEarly = true;
                        break;
                    }

                }

                if (returnEarly) break;

            }

            return (ret, vec, returnEarly);

        }

        public int EndPointUnits => EndPointData().Item1;
        public Vector2 EndPointPos => EndPointData().Item2;
        public bool EndPointParticles => EndPointData().Item3;

        //COLLISION
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {

            return Collision.CheckAABBvLineCollision(

                targetHitbox.Location.ToVector2(),
                new Vector2(targetHitbox.Width, targetHitbox.Height),
                projectile.position,
                EndPointPos + projectile.velocity

                );

        }

        //IMMUNITY
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.immune[projectile.owner] = 4;

        }

        //DRAWING
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {

            Texture2D texture = Main.projectileTexture[projectile.type];
            Texture2D innerTexture = ModContent.GetTexture(innerTextureStr);

            Rectangle headSrc = new Rectangle(0, 00, 16, 32);
            Rectangle bodySrc = new Rectangle(0, 32, 16, 32);
            Rectangle tailSrc = new Rectangle(0, 64, 16, 32);

            Vector2 beamStart = projectile.Center - Main.screenPosition;
            Vector2 beamEnd = EndPointPos - Main.screenPosition;

            Vector2 origin = new Vector2(8, 0);

            Vector2 scaleInner = new Vector2(1f, 1f);
            Vector2 scaleOuter = new Vector2(1f, 1f);
            Color colorInner = new Color(255, 255, 255, 100);

            //DRAW HEADS
            spriteBatch.Draw(

                texture,
                beamStart,
                headSrc,
                BeamColor,
                projectile.rotation,
                origin,
                scaleOuter,
                SpriteEffects.None,
                0f

                );

            spriteBatch.Draw(

                innerTexture,
                beamStart,
                headSrc,
                colorInner,
                projectile.rotation,
                origin,
                scaleInner,
                SpriteEffects.None,
                1f

                );

            //DRAW BODIES
            for (int mult = 1; mult < EndPointUnits; mult++) {

                spriteBatch.Draw(

                    texture,
                    beamStart + mult * projectile.velocity,
                    bodySrc,
                    BeamColor,
                    projectile.rotation,
                    origin,
                    scaleOuter,
                    SpriteEffects.None,
                    0f

                    );

                spriteBatch.Draw(

                    innerTexture,
                    beamStart + mult * projectile.velocity,
                    bodySrc,
                    colorInner,
                    projectile.rotation,
                    origin,
                    scaleInner,
                    SpriteEffects.None,
                    1f

                    );

            }

            //DRAW TAILS
            spriteBatch.Draw(

                texture,
                beamEnd,
                tailSrc,
                BeamColor,
                projectile.rotation,
                origin,
                scaleOuter,
                SpriteEffects.None,
                0f

                );

            spriteBatch.Draw(

                innerTexture,
                beamEnd,
                tailSrc,
                colorInner,
                projectile.rotation,
                origin,
                scaleInner,
                SpriteEffects.None,
                1f

                );

            //PARTICLES
            if( EndPointParticles ) {

                int dust = Dust.NewDust(

                    EndPointPos + projectile.velocity,
                    10, 10,
                    ModContent.DustType<Dusts.PristineDust>(),
                    Main.rand.NextFloat(-1, 1),
                    Main.rand.NextFloat(-1, 1),
                    0, BeamColor,
                    Main.rand.NextFloat(0.5f, 1.25f)

                    );

            }

            //LIGHT UP SURROUNDING AREA
            DelegateMethods.v3_1 = BeamColor.ToVector3();
            Utils.PlotTileLine(projectile.Center, EndPointPos, 1f, new Utils.PerLinePoint(DelegateMethods.CastLight));

            return false;

        }

    }
}
