using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass {
    public class ChemistryClassGlobalNPC : GlobalNPC {

        public override bool InstancePerEntity => true;

        public bool rusted;
        public static int rustedDef => 5;
        public static int rustedLD => 7;
        public static Color RustedColor => new Color(210, 140, 85);

        public bool sulfurFire;

        //public Color colorOnRusted;

        public override void SetDefaults(NPC npc) {

            npc.buffImmune[ModContent.BuffType<Buffs.Debuffs.Rusted>()] =
                npc.buffImmune[BuffID.Poisoned] || npc.buffImmune[BuffID.Ichor];

        }

        public override void ResetEffects(NPC npc) {

            if (rusted) {
                npc.defense += rustedDef;
                //npc.color = colorOnRusted;
            }

            rusted = false;
            sulfurFire = false;

        }

        public override bool PreAI(NPC npc) {

            if (rusted) npc.defense -= rustedDef;

            //DEBUGGING
            //if (ChemistryClass.TimeIsMultOf(60))
            //  if (npc.type == NPCID.KingSlime)
            //      Main.NewText( $"col: r{npc.color.R}, g{npc.color.G}, b{npc.color.B}, a{npc.color.A}");

            return true;

        }

        public override void DrawEffects(NPC npc, ref Color drawColor) {

            //DEBUGGING
            //ChemistryClass.SparseDebug(drawColor);
            //ChemistryClass.SparseDebug(Color.Lerp(drawColor, RustedColor, 0.5f));

            if (rusted) drawColor = Color.Lerp(drawColor, RustedColor, 0.5f);

            if ( rusted && ChemistryClass.TimeIsMultOf(20) ) {

                Dust.NewDust(

                    npc.position,
                    npc.width, npc.height,
                    DustID.Iron,
                    Main.rand.NextFloat(-1, 1),
                    Main.rand.NextFloat(-4, 0),
                    0

                    );

            }

            if(sulfurFire) {

                Dust.NewDustDirect(

                    npc.position,
                    npc.width, npc.height,
                    ModContent.DustType<Dusts.SulfurFlame>(),
                    Scale: Main.rand.NextFloat(0.75f, 1.25f)

                    );

            }

        }

        public override void UpdateLifeRegen(NPC npc, ref int damage) {

            if(rusted) {

                if (npc.lifeRegen > 0) npc.lifeRegen = 0;

                npc.lifeRegen -= rustedLD;

                if (damage < 1) damage = 1;
                else damage += 1;

            }

            if (sulfurFire) {

                if (npc.lifeRegen > 0) npc.lifeRegen = 0;

                npc.lifeRegen -= 14;

                if (damage < 1) damage = 1;
                else damage += 1;

            }

        }

        //EoC PRISMATIC LENS DROP
        public override void NPCLoot(NPC npc) {

            if (Main.expertMode) return;

            if( npc.type == NPCID.EyeofCthulhu) {

                Item.NewItem(npc.Hitbox, ModContent.ItemType<Items.Materials.Earlygame.PrismaticLens>(), Main.rand.Next(5, 10));

            }

        }

    }
}
