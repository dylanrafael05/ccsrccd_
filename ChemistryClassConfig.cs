using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;

namespace ChemistryClass {
    public enum DecayDisplayMode {
        Callout,
        Meter,
        Both
    }

    [Label("Client Configurations")]
    public class ChemistryClassConfig : ModConfig {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Decay Display Options")]
        [Label("Decay Display Mode")]
        [Tooltip("Chooses which method of displaying weapon purity to the player will be used.")]
        [DrawTicks]
        [DefaultValue(DecayDisplayMode.Callout)]
        public DecayDisplayMode DecayDisplay;

        [Label("Decay Meter Length")]
        [Tooltip("Changes how large the decay meter will be.")]
        [Increment(1)]
        [Range(1, 8)]
        [DefaultValue(3)]
        public int DecayMeterLength;

        [Label("Decay Meter Offset")]
        [Tooltip("Changes the position of the Decay Meter")]
        [Range(-100f, 100f)]
        [Increment(1f)]
        public Vector2 DecayMeterOffset;

        [Header("Features")]
        [ReloadRequired]
        [Label("Modify Vanilla Pickaxe Values")]
        [Tooltip("De-randomizes vanilla pickaxe values and spaces out values to better fit in Quartz into tile progression.")]
        [DefaultValue(true)]
        public bool ChangePick;

        public override void OnChanged() {
            if (ChemistryClass.decayMeter != null) {
                ChemistryClass.decayMeter.decayMeter.length = DecayMeterLength;
                ChemistryClass.decayMeter.decayMeter.offset = DecayMeterOffset;
            }
        }

    }
}
