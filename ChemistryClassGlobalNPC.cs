using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass {
    public class ChemistryClassGlobalNPC : GlobalNPC {

        public override bool InstancePerEntity => true;

        public bool rusted;
        public static int rustedDef => 10;
        public static int rustedLD => 8;
        public static Color rustedColor => new Color(212, 187, 169);

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

            if( rusted ) drawColor = rustedColor;

            if ( rusted && ChemistryClass.TimeIsMultOf(20) ) {

                int dust = Dust.NewDust(

                    npc.position,
                    npc.width, npc.height,
                    DustID.Iron,
                    Main.rand.NextFloat(-1, 1),
                    Main.rand.NextFloat(-4, 0),
                    0, rustedColor

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

        }

        //EOC PRISMATIC DROP
        public override void NPCLoot(NPC npc) {

            if(npc.type == NPCID.EyeofCthulhu) {

                Item.NewItem(npc.Hitbox, ModContent.ItemType<Items.Materials.PrismaticLens>(), Main.rand.Next(5, 10));

            }

        }

    }
}
