using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using ChemistryClass.ModUtils;
using ChemistryClass.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
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

        private float DecayRateReal => item.useTime / (3600f * minutesToDecay);

        //Prefix value trackers
        private float decayRateMult = 1f;

        //Purity loss instance values
        private float curDecayRate;

        //Item value trackers
        public int currentDamage { get; private set; }
        public int currentCrit { get; private set; }
        public float currentKB { get; private set; }

        //DECAY STATS
        private float curMinsToDecay
            => item.useTime / (curDecayRate * 3600f);

        //Purity effect multipliers
        public float maxPurityMult = 1.1f;
        public float minPurityMult = 0.7f;
        public float impureMult = 0.2f;

        //Refinement data
        public List<RefinementItem> refinementData;

        //Fan name
        public string fanName = null;

        //Purity callout values
        private float previousCallout = 1f;
        public float calloutDivisor = 5f;

        //Static values
        public static Color CalloutColor { get; } = new Color(120, 0xFF, 0xFF);
        public static Color FanColor { get; } = new Color(0xFF, 0x50, 0xA0);

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
                if (!pre.GetType().IsSubclassOf(typeof(ChemistryClassPrefix))) continue;

                //skip basic chemical prefix
                if (pre.Name == new ChemistryClassPrefix().Name) continue;

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

                if (curMinsToDecay > 10) return "Extremely low";
                if (curMinsToDecay > 5) return "Very low";
                if (curMinsToDecay > 3) return "Low";
                if (curMinsToDecay > 2) return "Average";
                if (curMinsToDecay > 1.5) return "High";
                if (curMinsToDecay > 1) return "Very high";

                return "Extremely high";

            }

        }

        //Setting defaults
        public virtual void SafeSetDefaults() { }

        public sealed override void SetDefaults() {

            SafeSetDefaults();

            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.summon = false;
            item.thrown = false;

            item.maxStack = 1;

        }

        public void SetRefinementData(params (int, float)[] idValuePairs) {

            refinementData = new List<RefinementItem>();

            foreach ((int, float) value in idValuePairs) {

                refinementData.Add((RefinementItem)value);

            }

        }

        public bool CanBeRefinedWith(int itemID) {
            foreach(RefinementItem item in refinementData) {
                if (item.itemID == itemID) return true;
            }
            return false;
        }

        public float GetRefinementValue(int itemID) {
            foreach (RefinementItem item in refinementData) {
                if (item.itemID == itemID) return item.value;
            }
            return 0f;
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
            string localDamageWord = splitTooltip.Last();

            tooltips[dmgIndex].text = localDamageValue + " chemical " + localDamageWord;

        }

        private void AddDecayTooltips(List<TooltipLine> tooltips) {

            string dRText = DecayQualifier + " rate of decay";
            TooltipLine decayRateT = new TooltipLine(mod, "DecayRate", dRText);

            int targetIndex = tooltips.FindIndex(line => line.Name == "Knockback" && line.mod == "Terraria");

            if (targetIndex < 0) return;

            tooltips.Insert(targetIndex + 1, decayRateT);

        }

        private void AddPurityTooltip(List<TooltipLine> tooltips) {

            TooltipLine tooltip = new TooltipLine(mod, "Purity", TooltipPurity) {

                overrideColor = CalloutColor

            };

            int targetIndex = tooltips.FindIndex(line => line.isModifier || line.isModifierBad);

            if (targetIndex < 0) tooltips.Add(tooltip);
            else tooltips.Insert(targetIndex, tooltip);

        }

        private void AddRefinementTooltips(List<TooltipLine> tooltips) {

            string text = "Can be refined with:";

            if (refinementData == null || refinementData.Count < 1) {

                text = "Cannot be refined by normal means.";
                goto RETURN;

            }

            foreach (var entry in refinementData) {

                Item item = new Item();
                item.SetDefaults(entry.itemID);

                text += "\n- " + item.Name;

                item = null;

            }

            RETURN:
            TooltipLine tooltip = new TooltipLine(mod, "Refinement", text) {

                overrideColor = CalloutColor

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

        }

        private void AddFanByTootlip(List<TooltipLine> tooltips) {

            if (fanName != null) {

                tooltips.Insert(
                    1,
                    new TooltipLine(mod, "FanBy", $"[Fan idea by {fanName}]")
                    { overrideColor = FanColor }
                    );

            }

        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips) {

            SafeModifyTooltips(tooltips);
            ModifyDamageTooltip(tooltips);
            AddDecayTooltips(tooltips);
            AddPurityTooltip(tooltips);
            AddRefinementTooltips(tooltips);
            AddPrefixTooltips(tooltips);
            AddFanByTootlip(tooltips);

        }

        //Decay statistics
        private void ClampPurity() => purity.Clamp(0, 1);

        public void RefreshStats() {

            GetDecayStats(Main.LocalPlayer);
            GetPrefixValues();

        }

        private void GetDecayStats(Player player) {

            //get bases
            curDecayRate = DecayRateReal;

            //get player & factor in prefix
            curDecayRate *= player.Chemistry().decayRateMult * decayRateMult;

            ModifyDecayStats(ref curDecayRate, player);

            //limit stats
            float minValue = (float)Math.Pow(10, -5);
            curDecayRate.EnforceMin(minValue);

        }

        private void GetPrefixValues() {

            decayRateMult = 1f;

            //DEBUGGING
            //Main.NewText(item.prefix);

            byte prefixType = item.prefix;
            ChemistryClassPrefix prefix = ModPrefix.GetPrefix(prefixType) as ChemistryClassPrefix;

            if (prefix != null) {

                prefix.SetDecayStats(ref decayRateMult);

            }

        }

        public virtual void ModifyDecayStats(ref float decay, Player player) { }

        //Purity to multiplier mapping
        protected float MapPurity(float min, float max)
            => purity.Map(0, 1, min, max);
        protected float DefaultMapPurity => MapPurity(minPurityMult, maxPurityMult);

        public virtual float PurityDamageMult => !Impure ? DefaultMapPurity : impureMult;
        public virtual float PurityKnockbackMult => !Impure ? DefaultMapPurity : impureMult;
        public virtual float PurityCritMult => !Impure ? DefaultMapPurity : impureMult;

        //Damage modifiction and purity stat retreiving
        public virtual void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) { }
        public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {

            //RESFRESH STATS
            RefreshStats();

            add += player.Chemistry().chemicalDamageAdd;
            mult *= player.Chemistry().chemicalDamageMult * PurityDamageMult;

            SafeModifyWeaponDamage(player, ref add, ref mult, ref flat);

            currentDamage = (int)(item.damage * add * mult + flat);

        }

        //Get other stats
        public virtual void SafeGetWeaponKnockback(Player player, ref float knockback) { }
        public sealed override void GetWeaponKnockback(Player player, ref float knockback) {

            float add = 1;
            float mult = 1;

            add += player.Chemistry().chemicalKnockbackAdd;
            mult *= player.Chemistry().chemicalKnockbackMult * PurityKnockbackMult;

            knockback *= add;
            knockback *= mult;

            SafeGetWeaponKnockback(player, ref knockback);

            currentKB = knockback;

        }

        public virtual void SafeGetWeaponCrit(Player player, ref int crit) { }
        public sealed override void GetWeaponCrit(Player player, ref int crit) {

            float add = 1;
            float mult = 1;

            add += player.Chemistry().chemicalCritAdd;
            mult *= player.Chemistry().chemicalCritMult * PurityCritMult;

            float tempCrit = crit;

            tempCrit += add;
            tempCrit *= mult;

            crit = (int)Math.Ceiling(tempCrit);

            SafeGetWeaponCrit(player, ref crit);

            currentCrit = crit;

        }

        //Call out purity when it hits a landmark
        //IMPORTANT: DO NOT CALL EITHER OF THESE BEFORE GetPurityLoss!!
        private float RealCalloutDiv => calloutDivisor / 100f;

        private bool PurityCloseToLandmark
            => purity % RealCalloutDiv < curDecayRate / 2 ||
               purity % RealCalloutDiv > RealCalloutDiv - curDecayRate / 2;

        private void TryCallout(Player player) {

            if (ChemistryClass.Configuration.DecayDisplay == DecayDisplayMode.Meter) return;

            if (PurityCloseToLandmark && purity != previousCallout) {

                Rectangle spawn = new Rectangle(0, 0, 20, 20);
                Vector2 position = player.position;
                spawn.X = (int)position.X;
                spawn.Y = (int)(position.Y - (ChemistryClass.Configuration.DecayDisplay == DecayDisplayMode.Both ? Main.screenHeight / 18f : 0));
                spawn.Location = spawn.Location.OffsetBy( (int)ChemistryClass.Configuration.DecayMeterOffset.X, (int)ChemistryClass.Configuration.DecayMeterOffset.Y );

                CombatText.NewText(spawn, CalloutColor, DisplayPurity, true);

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
        public virtual bool PrePurityOnWeaponUse(Player player) => true;
        public virtual void UseItemExact(Player player) { }
        public sealed override void UseStyle(Player player) {

            SafeUseStyle(player);

            if (player.itemAnimation == player.itemAnimationMax - 1) {

                if (PrePurityOnWeaponUse(player))
                    UsePurity(ref player);

                UseItemExact(player);

            }

        }

        //Use up purity
        public void UsePurity(ref Player player) {

            purity -= curDecayRate;

            ClampPurity();

            TryAutoRefine(ref player);

            TryCallout(player);

        }

        //Refinement
        internal void TryAutoRefine(ref Player player) {

            ChemistryClassPlayer chemPlayer = player.Chemistry();
            RefinementMenuState availableMenu = ChemistryClass.refinementMenu;

            if (!chemPlayer.autoRefine) return;
            chemPlayer.autoRefineItem = availableMenu.menu.autoRefineSlot.Item;

            if (!CanBeRefinedWith(chemPlayer.autoRefineItem.type)) return;
            if (purity > 0.5f) return;

            ChemistryClassItem newItem = this;
            Item newAutoRefineItem = chemPlayer.autoRefineItem;

            Refine(ref newItem, ref newAutoRefineItem, 0.5f);

            purity = newItem.purity;
            player.Chemistry().autoRefineItem = newAutoRefineItem;

            Main.PlaySound(SoundID.Item37);

            if (availableMenu == null) return;
            availableMenu.menu.autoRefineSlot.Item = newAutoRefineItem;

        }

        public static bool Refine(ref ChemistryClassItem chemItem, ref Item item, float maxPurity = 1f) {

            //DEBUG
            //Main.NewText(chemItem.ToString());
            //Main.NewText(item.ToString());
            //Main.NewText(indexOfItem);

            if (!chemItem.CanBeRefinedWith(item.type)) return false;

            float refinement = chemItem.GetRefinementValue(item.type);
            float refinementNeeded = maxPurity - chemItem.purity;

            //DEBUG
            //Main.NewText(refinementNeeded);

            //Fail if no purification is needed
            if (refinementNeeded == 0) return false;

            //Clamp down the stack size if the original size would over-purify.
            if (refinementNeeded < refinement * item.stack) {

                int useAmt = (int)(Math.Ceiling(refinementNeeded / refinement) + 0.5f);

                item.stack -= useAmt;
                chemItem.purity += useAmt * refinement;
                chemItem.purity.EnforceMax(1);

                //DEBUG
                //Main.NewText(item.stack);

                return true;

            }

            //Add the original stack size purification value if the above two
            //conditions fail.
            chemItem.purity += refinement * item.stack;
            item.TurnToAir();

            return true;

        }

        public static bool Refine(ref Item chemItem, ref Item item, float maxPurity = 1f) {

            ChemistryClassItem ccI = chemItem.Chemistry();

            bool ret = Refine(ref ccI, ref item, maxPurity);

            chemItem = ccI.item;
            return ret;

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
