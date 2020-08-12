using System;
using ChemistryClass.ModUtils;
using ChemistryClass.ModUtils.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class SpearOfResolveProjectile : ModProjectilePlus {

        public override void SafeSetDefaults() {

            projectile.width = 
            projectile.height = 64;
            projectile.penetrate = -1;

            projectile.damage = 10;
            projectile.knockBack = 2;

            projectile.scale = 1.25f;

            projectile.friendly = true;
            projectile.tileCollide = false;

            //projectile.aiStyle = ProjectileAIStyle.Spear;

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            float _ = default;
            return Collision.CheckAABBvLineCollision(
                    targetHitbox.Location.ToVector2(), targetHitbox.Size(),
                    projectile.Center + projectile.velocity.WithMagnitude(projectile.width * CCUtils.SQRT_2 / 2f), Owner.Center,
                    8f, ref _);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Rusted>(), 180);
            target.immune[projectile.owner] = 13;

        }

        public float MovementAmt {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public Player Owner => Main.player[projectile.owner];

        public override void AI() {

            projectile.direction = Owner.direction;
            Owner.heldProj = projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation;

            projectile.position = Owner.RotatedRelativePoint(Owner.MountedCenter);

            if(MovementAmt == 0f) {
                MovementAmt = 24f;
                projectile.netUpdate = true;
            } else if(Owner.itemAnimation > Owner.itemAnimationMax / 3) {
                MovementAmt += 4f;
            } else {
                MovementAmt -= 6f;
            }

            projectile.position += projectile.velocity.WithMagnitude(MovementAmt);

            if (Owner.itemAnimation == 0) {
                projectile.Kill();
            }

            projectile.rotation = projectile.velocity.ToRotation() + CCUtils.HALF_PI_FLOAT + CCUtils.QUARTER_PI_FLOAT;
            if (projectile.spriteDirection == -1) projectile.rotation -= CCUtils.HALF_PI_FLOAT;

            //FIX COLLISION JANK
            projectile.Hitbox = new Rectangle(

                (int)projectile.Center.X - projectile.width,
                (int)projectile.Center.Y - projectile.width,
                projectile.width,
                projectile.width

                );

        }

        //thank you example mod
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            Texture2D texture = Main.projectileTexture[projectile.type];
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            Color drawColor = projectile.GetAlpha(lightColor);

            Main.spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY),
                sourceRectangle, drawColor, projectile.rotation, new Vector2(projectile.width / 2), projectile.scale, spriteEffects, 0f);

            return false;

        }

    }
}
