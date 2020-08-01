using System;
using System.IO;
using System.Security.Policy;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUtils;

namespace ChemistryClass {
    public class ChemistryClassPlayer : ModPlayer {

        //Chemistry class values
        public float ChemicalDamageAdd = 0f;
        public float ChemicalDamageMult = 1f;

        public float ChemicalKnockbackAdd = 0f;
        public float ChemicalKnockbackMult = 1f;

        public float ChemicalCritAdd = 0f;
        public float ChemicalCritMult = 1f;

        public float DecayRateMult = 1f;
        public float DecayChanceMult = 1f;

        public bool ZoneSulfur = false;

        public bool SulfurHeart = false;

        public override void ResetEffects() {

            ChemicalDamageAdd = 0f;
            ChemicalDamageMult = 1f;

            ChemicalKnockbackAdd = 0f;
            ChemicalKnockbackMult = 1f;

            ChemicalCritAdd = 0f;
            ChemicalCritMult = 1f;

            DecayRateMult = 1f;
            DecayChanceMult = 1f;

            SulfurHeart = false;

            //DEBUGGING
            int _ = 0;
            int _2 = 20;
            ChemistryClass.SparseDebug(Mathematics.Clamp(ref _, 5, 15));
            ChemistryClass.SparseDebug(Mathematics.Clamp(ref _2, 5, 15));

        }

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff) {

            if(SulfurHeart) {

                if(player.HeldItem.IsChemistry()) {

                    player.statLifeMax2 += 50;

                }

            }

        }

        //BIOME SHENANIGANS
        public override void UpdateBiomes() {

            ZoneSulfur = ChemistryClassWorld.sulfurCount >= 8 && ChemistryClassWorld.sulfurHeartCount >= 1;

        }

        public override void CopyCustomBiomesTo(Player other) {

            other.GetModPlayer<ChemistryClassPlayer>().ZoneSulfur = ZoneSulfur;

        }

        public override bool CustomBiomesMatch(Player other)
            => other.GetModPlayer<ChemistryClassPlayer>().ZoneSulfur == ZoneSulfur;

        public override void SendCustomBiomes(BinaryWriter writer) {

            writer.Write(ZoneSulfur);

        }

        public override void ReceiveCustomBiomes(BinaryReader reader) {

            ZoneSulfur = reader.ReadBoolean();

        }

    }

    public static class PlayerExtensions {

        public static ChemistryClassPlayer Chemistry(this Player pl) => pl.GetModPlayer<ChemistryClassPlayer>();

    }
}
