using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace ChemistryClass.Tiles.Blocks {
    public class QuartzPlaced : ModTile {

        public override void SetDefaults() {

            this.dustType = DustID.Platinum;
            this.minPick = 60;
            this.soundType = SoundID.Dig;
            this.drop = ModContent.ItemType<Items.Materials.Earlygame.Quartz>();

            Main.tileFrameImportant[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 200;
            Main.tileShine[Type] = 1000;
            Main.tileSolid[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 18 };
            TileObjectData.newTile.CoordinateWidth = 20;

            for (int i = 0; i < 3; i++) {
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.AlternateTile, 1, 0);
                TileObjectData.newTile.AnchorAlternateTiles = new[] { 124 };
                TileObjectData.addAlternate(i);
            }

            for (int j = 6; j < 9; j++) {
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom | AnchorType.AlternateTile, 1, 0);
                TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
                TileObjectData.newTile.AnchorAlternateTiles = new[] { 124 };
                //TileObjectData.newAlternate.DrawYOffset = -2;
                TileObjectData.addAlternate(j);
            }

            for (int k = 3; k < 6; k++) {
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, 1, 0);
                TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
                TileObjectData.newAlternate.AnchorTop = AnchorData.Empty;
                TileObjectData.newTile.AnchorAlternateTiles = new[] { 124 };
                TileObjectData.addAlternate(k);
            }

            for (int l = 9; l < 12; l++) {
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, 1, 0);
                TileObjectData.newAlternate.AnchorTop = AnchorData.Empty;
                TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
                TileObjectData.newAlternate.AnchorRight = AnchorData.Empty;
                TileObjectData.newTile.AnchorAlternateTiles = new[] { 124 };
                TileObjectData.addAlternate(l);
            }

            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.addTile(Type);

            ModTranslation text = CreateMapEntryName();
            text.AddTranslation(GameCulture.English, "Quartz");
            AddMapEntry(new Color(0.6f, 0.6f, 0.85f), text);

        }

        public override bool CanExplode(int i, int j)
            => false;

        public float ProjChance => 0.36f;

        public override void NearbyEffects(int i, int j, bool closer) {

            if (!closer || Main.gameInactive || Main.gamePaused) return;
            if (!ChemistryClassWorld.QuartzLightningValid || Main.rand.NextFloat() > ProjChance) return;

            Vector2 realPos = new Point(i, j).ToWorldCoordinates();
            Player clPl = CCUtils.Min(Main.player, p => p.Distance(realPos));
            if (clPl.Distance(realPos) > 300) return;
            if (clPl.Distance(realPos) < 32) return;

            Projectile.NewProjectileDirect(

                realPos,
                (clPl.Center - realPos).WithMagnitude(Main.rand.NextFloat(7, 10)),
                ModContent.ProjectileType<Projectiles.PreHMHS.LightningHostile>(),
                10,
                3f

                );

            ChemistryClassWorld.QuartzLightningCount++;
            Main.PlaySound(SoundID.Item12.WithVolume(0.4f));

            //Main.NewText(proj.Center);

        }

    }
}
