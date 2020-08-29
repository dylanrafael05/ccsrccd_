using System;
using System.IO;
using ChemistryClass.ModUtils;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Steamworks;
using Terraria.ID;
using log4net.Util;

namespace ChemistryClass.Projectiles.PreHMHS {
    public class LightningHostile : LightningBase {

        public override int Width => 12;
        public override bool TileCollide => false;
        public override int TimeLeft => 20;
        public override float DustChance => 0.01f;

        public override void SaferSetDefaults() {

            projectile.hostile = true;
            projectile.friendly = false;

            projectile.damage = 10;
            projectile.knockBack = 2;

        }

        public override Entity GetTarget() {
            Entity ret = null;
            float retDist = float.MaxValue;
            float curDist;
            Vector2 end = sequence.VisualEndpoint;
            foreach (Player player in Main.player) {
                if (player == null) continue;
                curDist = player.Distance(end);
                if (curDist > 200f) continue;
                if (!player.active || player.dead) continue;
                if (curDist >= retDist) continue;
                ret = player;
                retDist = ret.Distance(end);
            }
            return ret;
        }

    }
}
