using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChemistryClass.ModUtils {
    public static class EntityHelpers {

        public static Rectangle ToTileHitbox(this Rectangle rect)
            => new Rectangle(rect.X / 16, rect.Y / 16, (int)Math.Ceiling(rect.Width / 16f), (int)Math.Ceiling(rect.Height / 16f));

        public static Rectangle GetPositionedHitbox(this Entity entity)
            => new Rectangle((int)entity.position.X, (int)entity.position.Y, entity.Hitbox.Width, entity.Hitbox.Height);

        public static Rectangle GetTileHitbox(this Entity entity)
            => entity.GetPositionedHitbox().ToTileHitbox();

    }
}
