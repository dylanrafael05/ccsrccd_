using System;
using System.IO;
using System.Security.Policy;
using ChemistryClass.ModUtils;
using ChemistryClass.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChemistryClass {
    public class ChemistryClassPlayer : ModPlayer {

        //Chemistry class values
        public float chemicalDamageAdd = 0f;
        public float chemicalDamageMult = 1f;

        public float chemicalKnockbackAdd = 0f;
        public float chemicalKnockbackMult = 1f;

        public float chemicalCritAdd = 0f;
        public float chemicalCritMult = 1f;

        public float decayRateMult = 1f;
        public float decayChanceMult = 1f;

        public bool zoneSulfur = false;

        public bool sulfurHeart = false;
        public bool autoRefine = false;
        public bool prevAutoRefine = false;

        public bool sulfurFire = false;
        public Item autoRefineItem = new Item();

        public override void ResetEffects() {

            chemicalDamageAdd = 0f;
            chemicalDamageMult = 1f;

            chemicalKnockbackAdd = 0f;
            chemicalKnockbackMult = 1f;

            chemicalCritAdd = 0f;
            chemicalCritMult = 1f;

            decayRateMult = 1f;
            decayChanceMult = 1f;

            sulfurHeart = false;
            sulfurFire = false;
            prevAutoRefine = autoRefine;
            autoRefine = false;

        }

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff) {

            if(sulfurHeart) {

                if(player.HeldItem.IsChemistry()) {

                    player.statLifeMax2 += 50;

                }

            }

            if(!autoRefine && prevAutoRefine) {

                ChemistryClass.refinementMenu.menu.autoRefineSlot.TossContents();
                autoRefineItem = new Item();

            }

        }

        public override void PreUpdateBuffs() {

            player.buffImmune[ModContent.BuffType<Buffs.Debuffs.SulfurFire>()] = player.buffImmune[BuffID.OnFire];

            if (sulfurFire) {

                Dust.NewDustDirect(

                    player.position,
                    player.width, player.height,
                    ModContent.DustType<Dusts.SulfurFlame>(),
                    Scale: Main.rand.NextFloat(0.75f, 1.25f)

                    );

            }

        }

        public override void UpdateBadLifeRegen() {

            if (sulfurFire) {

                if (player.lifeRegen > 0) player.lifeRegen = 0;

                player.lifeRegenTime = 0;
                player.lifeRegen -= 14;

            }

        }

        //BIOME SHENANIGANS
        public override void UpdateBiomes() {

            zoneSulfur = ChemistryClassWorld.sulfurCount >= 8 && ChemistryClassWorld.sulfurHeartCount >= 1;

        }

        public override void CopyCustomBiomesTo(Player other) {

            other.GetModPlayer<ChemistryClassPlayer>().zoneSulfur = zoneSulfur;

        }

        public override bool CustomBiomesMatch(Player other)
            => other.GetModPlayer<ChemistryClassPlayer>().zoneSulfur == zoneSulfur;

        public override void SendCustomBiomes(BinaryWriter writer) {

            writer.Write(zoneSulfur);

        }

        public override void ReceiveCustomBiomes(BinaryReader reader) {

            zoneSulfur = reader.ReadBoolean();

        }

        //SAVING AND LOADING
        public override TagCompound Save()
            => new TagCompound {
                { nameof(autoRefineItem), autoRefineItem }
            };

        public override void Load(TagCompound tag) { 
            autoRefineItem = tag.Get<Item>(nameof(autoRefineItem));
            ChemistryClass.refinementMenu.menu.autoRefineSlot.Item = autoRefineItem;
        }

    }
}
