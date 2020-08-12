using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles.LatePHMFL {
	public class RottingBeing : ModProjectilePlus {

		private protected virtual int ReqItem => ModContent.ItemType<Items.Weaponry.LatePHM.ToothballStaff>();

		public override void SetStaticDefaults() {

			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SafeSetDefaults() {
			projectile.width = 32;
			projectile.height = 32;

			// These below are needed for a minion weapon
			// Only controls if it deals damage to enemies on contact (more on that later)
			projectile.friendly = true;
			// Only determines the damage type
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 0f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
		}

		public override bool? CanCutTiles() => false;

		public override bool MinionContactDamage() => true;

        public Player Owner => Main.player[projectile.owner];

        public int TargetIndex {
			get => (int)projectile.ai[0];
			set => projectile.ai[0] = value;
        }

		public override void AI() {

			#region ACTIVE CHECK

			if (
				Owner.HeldItem.type != ReqItem &&
				!Owner.HeldItem.IsAir
				) {

				projectile.Kill();

			}

			#endregion

			#region GET STATS
			ChemistryClassItem staff = Owner.HeldItem.Chemistry();

			projectile.damage = staff.currentDamage;
			projectile.knockBack = staff.currentKB;
			#endregion

			#region PURITY
			if (ActiveCounter.IsMultipleOf(staff.item.useTime * 2))
				staff.UsePurity(ref Main.player[projectile.owner]);
			#endregion

			#region VISUALS
			projectile.frame = CCUtils.BlinkPong(ActiveCounter, 4, 120, 2);
			projectile.rotation = MathHelper.Lerp(projectile.velocity.X * 0.05f, projectile.rotation, 0.1f);
            #endregion

            #region GET TARGET
            bool hasTarget = false;
			NPC target = null;

			if (Owner.HasMinionAttackTargetNPC) {
				target = Main.npc[Owner.MinionAttackTargetNPC];
				hasTarget = Owner.Distance(target.Center) < 750f;
			}

            if (!hasTarget) {

				NPC[] possibleTargets =
					(from npc in Main.npc
					 where
					 npc.CanBeChasedBy() && !npc.friendly &&
					 Owner.Distance(npc.Center) < 750f &&
					 (Collision.CanHitLine(projectile.position - new Vector2(10, 10), projectile.width + 20, projectile.height + 20, npc.position, npc.width, npc.height)
                     || projectile.Distance(npc.Center) < 100f)
					 select npc).ToArray();

				if (possibleTargets.Length > 0) {
					target = CCUtils.Min(possibleTargets, t => t.Distance(projectile.Center));
					hasTarget = true;
				}
			}
			#endregion

			TargetIndex = target == null ? -1 : target.whoAmI;

			projectile.timeLeft = 2;
			projectile.velocity *= 0.97f;

			#region IDLE BEHAVIOR
			if (hasTarget) goto ACTIVE;

			Vector2 idleT = Owner.Center;
			idleT.Y -= 60;

			float distanceToIdle = Vector2.Distance(projectile.Center, idleT);

			if (distanceToIdle > 1000f)
				projectile.position = idleT;
            else
			    projectile.velocity.ChargeTargetAndSet(
				    projectile.Center, idleT,
					distanceToIdle > 500f ? 25f : 17f,
                    60f
				);

			if (projectile.velocity.Length() < 0.15f) projectile.velocity.SetMagnitude(0f);

			return;
		    #endregion

		    #region ACTIVE BEHAVIOR
		    ACTIVE:

			projectile.velocity.ChargeTargetAndSet(
				projectile.Center, target.Center,
                17f,
				60f
				);

			if (projectile.velocity.Length() < 0.15f) projectile.velocity.SetMagnitude(0f);
			#endregion

		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void Kill(int timeLeft) {
			Main.PlaySound(SoundID.NPCDeath13.WithVolume(0.5f));
            for(int _ = 0; _ < 10; _++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SomethingRed, newColor: Color.Maroon);
            }
        }

    }
}
