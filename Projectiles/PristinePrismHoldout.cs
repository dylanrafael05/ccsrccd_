using System;
using ChemistryClass.Items.Weaponry.Earlygame;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles {
    public class PristinePrismHoldout : ModProjectile {

        //TEXTURE
        public override string Texture => "ChemistryClass/Items/Weaponry/Earlygame/PristinePrism";

        //ENCLOSERS
        public Player owner => Main.player[projectile.owner];
        public int FrameCounter {

            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;

        }
        public int ActiveCounter {

            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;

        }
        public int UUID => Projectile.GetByUUID(projectile.owner, projectile.whoAmI);

        //SETTING DEFAULTS
        public override void SetStaticDefaults() {

            ProjectileID.Sets.NeedsUUID[projectile.type] = true;

            DisplayName.SetDefault("Pristine Prism Holdout");

        }

        public override void SetDefaults() {

            //projectile.CloneDefaults(ProjectileID.LastPrism);
            projectile.width = 32;
            projectile.height = 32;

            projectile.damage = 3;
            projectile.knockBack = 0;

            projectile.hostile = false;
            projectile.friendly = false;

            projectile.tileCollide = false;

        }

        //FORCE LACK OF PROJECTILE MOVEMENT
        public override bool ShouldUpdatePosition() => false;

        //AI
        public override void AI() {

            //DEBUGGING
            //if(ChemistryClass.TimeIsMultOf(60))
            //    Main.NewText("TEST.");

            //UPDATE DRAW VARIABLES
            projectile.Center = owner.RotatedRelativePoint(owner.MountedCenter, true);
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
            projectile.spriteDirection = projectile.direction;
            owner.ChangeDir(projectile.direction);
            owner.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            //HELD PROJ, ENSURE ITEM CAN BE REUSED
            owner.heldProj = projectile.whoAmI;
            owner.itemTime = FrameCounter % 20;
            owner.itemAnimation = owner.itemTime;

            //UPDATE VELOCITY
            projectile.velocity =

                Vector2.Normalize(
                    Vector2.Lerp(
                        Vector2.Normalize(Main.MouseScreen + Main.screenPosition - projectile.Center),
                        projectile.velocity,
                        0.15f
                    )
                ) * 32f;

            //SPAWN BEAMS ON FIRST FRAME
            if (ActiveCounter < 1) {

                SpawnBeams();

            }

            //UPDATE BEAMS

            foreach( Projectile proj in Main.projectile ) {

                if(proj.modProjectile is PristinePrismBeam beam && beam.HoldoutUUID == UUID ) {

                    beam.UpdateBeam();

                }

            }

            //KILL PROJ IF ITEM NO LONGER IN USE
            if (!owner.channel || owner.noItems || owner.CCed || !(owner.HeldItem.modItem is PristinePrism)) {

                projectile.Kill();

            }

            //RESET PROJECTILE TIMEOUT TIME, AND INCREMENT FRAME COUNTER
            projectile.timeLeft = 2;
            FrameCounter++;
            ActiveCounter++;

        }

        //SPAWN BEAMS
        private void SpawnBeams() {

            for (int i = 0; i < 8; i++) {

                int proj = Projectile.NewProjectile(

                    projectile.position,
                    projectile.velocity,
                    ModContent.ProjectileType<PristinePrismBeam>(),
                    projectile.damage,
                    projectile.knockBack,
                    owner.whoAmI,
                    UUID,
                    i

                    );

                PristinePrismBeam beam = Main.projectile[proj].modProjectile as PristinePrismBeam;

                beam.FrameCounter = FrameCounter;
                beam.UpdateBeam();

            }

        }

        //CUSTOM DRAW CODE
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {

            Vector2 position = projectile.Center - Main.screenPosition;
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = ModContent.GetTexture(Texture);

            spriteBatch.Draw(

                texture,
                position,
                null,
                Color.White,
                projectile.rotation,
                new Vector2(texture.Width / 2, texture.Height),
                1f,
                effects,
                0f

                );

            return false;

        }

    }
}
