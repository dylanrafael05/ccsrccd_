using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace ChemistryClass {

    public abstract class ChemistryClassItem : ModItem {

        //Clone new instances
        public override bool CloneNewInstances => true;

        //Purity
        public float purity = 1f;
        public bool Impure => purity == 0f;

        //Decay basic stats
        public float minutesToDecay = 3;
        public float decayChance = 100f;

        private float DecayRateReal   => item.useTime / (3600f * minutesToDecay * DecayChanceReal);
        private float DecayChanceReal => decayChance / 100f;

        //Prefix value trackers
        private float decayRateMult   = 1f;
        private float decayChanceMult = 1f;

        //Purity loss instance values
        private float curDecayRate;
        private float curDecayChance;

        private float curMinsToDecay
            => item.useTime / (curDecayRate * 3600f);

        //Purity effect multipliers
        public float maxPurityMult = 1.1f;
        public float minPurityMult = 0.7f;
        public float impureMult = 0.2f;

        //Refinement data
        public List<RefinementItem> refinementData;

        //Purity callout values
        private float previousCallout = 1f;
        public float calloutDivisor = 5f;

        //Static values
        protected static readonly Color calloutColor =
            new Color(120, 255, 255);

        //Valid prefixes (universal, common, and chemical)
        private List<byte> _validPrefixes = null;
        public List<byte> ValidPrefixes {

            get {

                if (_validPrefixes == null) _getPrefixes();

                return _validPrefixes;

            }

        }

        //Chemistry prefixes
        private List<byte> _chemistryPrefixes = null;
        public List<byte> ChemistryPrefixes {

            get {

                if (_chemistryPrefixes == null) _getPrefixes();
                return _chemistryPrefixes;

            }

        }

        //prefix getter
        private void _getPrefixes() {

            _validPrefixes = new List<byte>();
            _chemistryPrefixes = new List<byte>();

            //Main.NewText( string.Join(" :: ", ModPrefix.GetPrefixesInCategory(ChemistryClassPrefix.prefixCategory)) );

            foreach (ModPrefix pre in ModPrefix.GetPrefixesInCategory(ChemistryClassPrefix.prefixCategory)) {

                //skip if not a chemical prefix
                if ( !pre.GetType().IsSubclassOf( typeof(ChemistryClassPrefix) ) ) continue;

                //skip basic chemical prefix
                if ( pre.Name == new ChemistryClassPrefix().Name ) continue;

                _validPrefixes.Add(pre.Type);
                _chemistryPrefixes.Add(pre.Type);

                //DEBUGGING
                //if (pre.GetType() != typeof(Prefixes.ReactivePrefs)) continue;
                //Main.NewText(pre as ChemistryClassPrefix);

            }

            //DEBUGGING
            /*List<byte> bChem = new List<byte>();
            foreach( var chem in _chemistryPrefixes ) { bChem.Add(chem.Type); }

            Main.NewText(string.Join(" ::: ", bChem));
            Main.NewText(string.Join(" ::: ", _validPrefixes));*/

            //Add universal & common prefixes
            _validPrefixes.Add(PrefixID.Keen);
            _validPrefixes.Add(PrefixID.Superior);
            _validPrefixes.Add(PrefixID.Forceful);
            _validPrefixes.Add(PrefixID.Broken);
            _validPrefixes.Add(PrefixID.Damaged);
            _validPrefixes.Add(PrefixID.Shoddy);
            _validPrefixes.Add(PrefixID.Unpleasant);
            _validPrefixes.Add(PrefixID.Weak);
            _validPrefixes.Add(PrefixID.Ruthless);
            _validPrefixes.Add(PrefixID.Godly);
            _validPrefixes.Add(PrefixID.Demonic);
            _validPrefixes.Add(PrefixID.Zealous);
            _validPrefixes.Add(PrefixID.Quick);
            _validPrefixes.Add(PrefixID.Deadly);
            _validPrefixes.Add(PrefixID.Agile);
            _validPrefixes.Add(PrefixID.Nimble);
            _validPrefixes.Add(PrefixID.Murderous);
            _validPrefixes.Add(PrefixID.Slow);
            _validPrefixes.Add(PrefixID.Sluggish);
            _validPrefixes.Add(PrefixID.Lazy);
            _validPrefixes.Add(PrefixID.Annoying);
            _validPrefixes.Add(PrefixID.Nasty);

        }

        //Decay qualification
        protected string DecayQualifier {

            get {

                //DEBUGGING
                //if(ChemistryClass.UnpausedUpdateCount % 60 == 0)
                    //Main.NewText(curDecayRate);

                if (curMinsToDecay > 10)  return "Extremely low";
                if (curMinsToDecay > 5)   return "Very low";
                if (curMinsToDecay > 3)   return "Low";
                if (curMinsToDecay > 2)   return "Average";
                if (curMinsToDecay > 1.5) return "High";
                if (curMinsToDecay > 1)   return "Very high";

                return "Extremely high";

            }

        }

        //Setting defaults
        public virtual void SafeSetDefaults() { }

        public sealed override void SetDefaults() {

            SafeSetDefaults();

            item.melee  = false;
            item.ranged = false;
            item.magic  = false;
            item.summon = false;
            item.thrown = false;

            item.maxStack = 1;

        }

        public void SetRefinementData(params (int, float)[] idValuePairs) {

            refinementData = new List<RefinementItem>();

            foreach( (int, float) value in idValuePairs ) {

                refinementData.Add( (RefinementItem)value );

            }

        }

        //Purity textification
        private double roundOut(double i)
            => Math.Round(i * 10000) / 10000;

        public string DisplayPurity
            => purity == 0f ? "Impure" : roundOut(Math.Round(purity * 100 / calloutDivisor) * calloutDivisor) + "%";

        public string TooltipPurity
            => purity == 0f ? "Impure" : Math.Ceiling(purity * 1000) / 10 + "% pure";

        //Modifying tooltip
        public virtual void SafeModifyTooltips(List<TooltipLine> tooltips) { }

        private void ModifyDamageTooltip(List<TooltipLine> tooltips) {

            int dmgIndex = tooltips.FindIndex(t => t.Name == "Damage" && t.mod == "Terraria");

            if (dmgIndex < 0) return;

            string[] splitTooltip = tooltips[dmgIndex].text.Split(' ');
            string localDamageValue = splitTooltip.First();
            string localDamageWord =  splitTooltip.Last();

            tooltips[dmgIndex].text = localDamageValue + " chemical " + localDamageWord;

        }

        private void AddDecayTooltips(List<TooltipLine> tooltips) {

            string dRText = DecayQualifier + " rate of decay";
            TooltipLine decayRateT = new TooltipLine(mod, "DecayRate", dRText);

            string dCText = Math.Round(curDecayChance * 1000) / 10 + "% chance of decay per use";
            TooltipLine decayChanceT = new TooltipLine(mod, "DecayChance", dCText);

            int targetIndex = tooltips.FindIndex(line => line.Name == "Knockback" && line.mod == "Terraria");

            if (targetIndex < 0) return;

            tooltips.Insert(targetIndex + 1, decayRateT);
            tooltips.Insert(targetIndex + 2, decayChanceT);

        }

        private void AddPurityTooltip(List<TooltipLine> tooltips) {

            TooltipLine tooltip = new TooltipLine(mod, "Purity", TooltipPurity) {

                overrideColor = calloutColor

            };

            int targetIndex = tooltips.FindIndex(line => line.isModifier || line.isModifierBad);

            if (targetIndex < 0) tooltips.Add(tooltip);
            else tooltips.Insert(targetIndex, tooltip);

        }

        private void AddRefinementTooltips(List<TooltipLine> tooltips) {

            if (refinementData.Count == 0) return;

            string text = "Can be refined with:";

            foreach( var entry in refinementData ) {

                Item item = new Item();
                item.SetDefaults(entry.itemID);

                text += "\n- " + item.Name;

                item = null;

            }

            TooltipLine tooltip = new TooltipLine(mod, "Refinement", text) {

                overrideColor = calloutColor

            };

            int targetIndex = tooltips.FindIndex(line => line.Name == "Purity");
            tooltips.Insert(targetIndex + 1, tooltip);

        }

        private void AddPrefixTooltips(List<TooltipLine> tooltips) {

            TooltipLine line;
            float value;

            if (decayRateMult != 1f) {

                value = 100 * (decayRateMult - 1f);
                line = new TooltipLine(
                    mod,
                    "PrefixPurityLoss",
                    (value >= 0 ? "+" : "") + Math.Round(value) + "% purity loss"
                ) { isModifierBad = value >= 0, isModifier = true };
                tooltips.Add(line);

            }

            if (decayChanceMult != 1f) {

                value = 100 * (decayChanceMult - 1f);
                line = new TooltipLine(
                    mod, "PrefixPurityLossChance",
                    (value >= 0 ? "+" : "") + Math.Round(value) + "% purity loss chance"
                ) { isModifierBad = value >= 0, isModifier = true };
                tooltips.Add(line);

            }

        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips) {

            SafeModifyTooltips(tooltips);
            ModifyDamageTooltip(tooltips);
            AddDecayTooltips(tooltips);
            AddPurityTooltip(tooltips);
            AddRefinementTooltips(tooltips);
            AddPrefixTooltips(tooltips);

        }

        //Decay statistics
        private void ClampPurity() => purity = MathHelper.Clamp(purity, 0, 1);

        public void RefreshStats() {

            GetDecayStats(Main.LocalPlayer);
            GetPrefixValues();

        }

        private void GetDecayStats(Player player) {

            //get bases
            curDecayRate = DecayRateReal;
            curDecayChance = DecayChanceReal;

            //get player & factor in prefix
            curDecayRate += player.chemistry().DecayRateAdd;
            curDecayRate *= player.chemistry().DecayRateMult * decayRateMult;

            curDecayChance += player.chemistry().DecayChanceAdd;
            curDecayChance *= player.chemistry().DecayChanceMult * decayChanceMult;

            ModifyDecayStats(ref curDecayRate, ref curDecayChance, player);

        }

        private void GetPrefixValues() {

            decayRateMult = 1f;
            decayChanceMult = 1f;

            //DEBUGGING
            //Main.NewText(item.prefix);

            byte prefixType = item.prefix;
            ChemistryClassPrefix prefix = ModPrefix.GetPrefix(prefixType) as ChemistryClassPrefix;

            if (prefix != null) {

                prefix.SetDecayStats(ref decayRateMult, ref decayChanceMult);

            }

        }

        public virtual void ModifyDecayStats(ref float decay, ref float decayChance, Player player) { }

        //Purity to multiplier mapping
        public float MapPurity(float min, float max)
            => purity * (max - min) + min;

        private float MappedPurity => MapPurity(minPurityMult, maxPurityMult);

        public virtual float PurityDamageMult => !Impure ? MappedPurity : impureMult;
        public virtual float PurityKnockbackMult => !Impure ? MappedPurity : impureMult;
        public virtual float PurityCritMult => !Impure ? MappedPurity : impureMult;

        //Damage modifiction and purity stat retreiving
        public virtual void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) { }
        public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {

            add += player.chemistry().ChemicalDamageAdd;
            mult *= player.chemistry().ChemicalDamageMult * PurityDamageMult;

            SafeModifyWeaponDamage(player, ref add, ref mult, ref flat);

            //RESFRESH STATS
            RefreshStats();

        }

        //Get other stats
        public virtual void SafeGetWeaponKnockback(Player player, ref float knockback) { }
        public sealed override void GetWeaponKnockback(Player player, ref float knockback) {

            float add = 1;
            float mult = 1;

            add += player.chemistry().ChemicalKnockbackAdd;
            mult *= player.chemistry().ChemicalKnockbackMult * PurityKnockbackMult;

            knockback *= add;
            knockback *= mult;

            SafeGetWeaponKnockback(player, ref knockback);

        }

        public virtual void SafeGetWeaponCrit(Player player, ref int crit) { }
        public sealed override void GetWeaponCrit(Player player, ref int crit) {

            float add = 1;
            float mult = 1;

            add += player.chemistry().ChemicalCritAdd;
            mult *= player.chemistry().ChemicalCritMult * PurityCritMult;

            float tempCrit = crit;

            tempCrit *= add;
            tempCrit *= mult;

            crit = (int)Math.Ceiling(tempCrit);

            SafeGetWeaponCrit(player, ref crit);

        }

        //Call out purity when it hits a landmark
        //IMPORTANT: DO NOT CALL EITHER OF THESE BEFORE GetPurityLoss!!
        private float RealCalloutDiv => calloutDivisor / 100f;

        private bool PurityCloseToLandmark
            => purity % RealCalloutDiv < curDecayRate / 2 ||
               purity % RealCalloutDiv > RealCalloutDiv - curDecayRate / 2;

        private void TryCallout(Player player) {

            if( PurityCloseToLandmark && purity != previousCallout ) {

                Rectangle spawn = new Rectangle(0,0,20,20);
                Vector2 position = player.position;
                spawn.X = (int)position.X;
                spawn.Y = (int)position.Y;

                CombatText.NewText(spawn, calloutColor, DisplayPurity, true);

                previousCallout = purity;

            }

        }

        //DEBUGGING
        /*public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {

            string text = "";
            foreach (var i in lines) text += i.Name + " :: ";
            Main.NewText(text);

            return base.PreDrawTooltip(lines, ref x, ref y);

        }*/

        //Get values, reduce purity, and fix damage issues when item is used
        public virtual void SafeUseStyle(Player player) { }
        public sealed override void UseStyle(Player player) {

            if (player.itemAnimation == player.itemAnimationMax - 1) {

                UsePurity(player);

                //DEBUGGING
                //Main.NewText("item purity use");
                //Main.NewText(DecayRateReal);
                //Main.NewText(usesToDecay);

            }

        }

        //Use up purity
        public void UsePurity(Player player) {

            if (Main.rand.NextFloat(0, 1) < curDecayChance)
                purity -= curDecayRate;

            ClampPurity();

            TryCallout(player);

        }

        //Refinement
        public static bool Refine(ref Item chemItemBase, ref Item item) {

            ChemistryClassItem chemItem = chemItemBase.chemistry();

            int itemType = item.type;
            int indexOfItem = chemItem.refinementData.FindIndex(i => i.itemID == itemType);

            //DEBUG
            //Main.NewText(chemItem.ToString());
            //Main.NewText(item.ToString());
            //Main.NewText(indexOfItem);

            if( indexOfItem < 0 ) return false;

            float refinement = chemItem.refinementData[indexOfItem].value;
            float refinementNeeded = 1f - chemItem.purity;

            //DEBUG
            //Main.NewText(refinementNeeded);

            //Fail if no purification is needed
            if (refinementNeeded == 0) return false;

            //Clamp down the stack size if the original size would over-purify.
            if (refinementNeeded < refinement * item.stack) {

                item.stack -= (int)Math.Ceiling(refinementNeeded / refinement);
                chemItem.purity = 1f;

                //DEBUG
                //Main.NewText(item.stack);

                chemItemBase = chemItem.item;

                return true;

            }

            //Add the original stack size purification value if the above two
            //conditions fail.
            chemItem.purity += refinement * item.stack;
            item.TurnToAir();

            chemItemBase = chemItem.item;

            return true;

        }

        //Prefix choosing
        public override int ChoosePrefix(UnifiedRandom rand) {

            //DEBUGGING
            //Main.NewText(string.Join(" ::: ", ValidPrefixes));

            //DEBUGGING
            //Main.NewText(ValidPrefixes.FindIndex(b => ModPrefix.GetPrefix(b).Name.Contains("Reactive")));
            //return ValidPrefixes.FindIndex(b => ModPrefix.GetPrefix(b).Name.Contains("Reactive"));
            //Main.NewText(ValidPrefixes[3]);

            return rand.Next(ValidPrefixes);

        }

        //Purity saving
        public override TagCompound Save()
            => new TagCompound { [nameof(purity)] = purity, };

        public override void NetSend(BinaryWriter writer)
            => writer.Write(purity);

        //Purity loading
        public override void Load(TagCompound tag)
            => purity = tag.GetFloat(nameof(purity));

        public override void NetRecieve(BinaryReader reader)
            => purity = reader.ReadSingle();

    }

}
