using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.ModUtils {
    public class ModProjectilePlus : ModProjectile {

        private int? _critChance;
        public int CritChance {
            get => _critChance.Value;
            set => _critChance = value;
        }
        private int? _frameCounter;
        public int FrameCounter {
            get => _frameCounter.Value;
            set => _frameCounter = value;
        }
        private int? _activeCounter;
        public int ActiveCounter {
            get => _activeCounter.Value;
            set => _activeCounter = value;
        }

        public Player OwningPlayer => Main.player[projectile.owner];

        public virtual bool SafeAutoload(ref string name) => true;
        public override bool Autoload(ref string name) {

            if (GetType() == typeof(ModProjectilePlus)) return false;
            return SafeAutoload(ref name);

        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults() {

            _critChance = 0;
            _frameCounter = -1;
            _activeCounter = -1;

            SafeSetDefaults();

        }

        public virtual void FirstFrame() { }
        public virtual bool SafePreAI() => true;
        public override bool PreAI() {

            if (ActiveCounter == -1) {

                CritChance = OwningPlayer.HeldItem.crit;
                FirstFrame();

            }

            FrameCounter++;
            ActiveCounter++;

            return SafePreAI();

        }

        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer) {

            writer.Write(CritChance);
            writer.Write(FrameCounter);
            writer.Write(ActiveCounter);

            SafeSendExtraAI(writer);

        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader) {

            ActiveCounter = reader.Read();
            FrameCounter = reader.Read();
            CritChance = reader.Read();

            SafeReceiveExtraAI(reader);

        }

        public virtual void SafeModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) { }
        public sealed override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {

            crit = CritChance > Main.rand.Next(101);

            SafeModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);

        }

    }

    public class ModNPCPlus : ModNPC {

        private int? _frameCounter;
        public int FrameCounter {
            get => _frameCounter.Value;
            set => _frameCounter = value;
        }
        private int? _activeCounter;
        public int ActiveCounter {
            get => _activeCounter.Value;
            set => _activeCounter = value;
        }

        public virtual bool SafeAutoload(ref string name) => true;
        public override bool Autoload(ref string name) {

            if (GetType() == typeof(ModNPCPlus)) return false;
            return SafeAutoload(ref name);

        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults() {

            _frameCounter = -1;
            _activeCounter = -1;

            SafeSetDefaults();

        }

        public virtual bool SafePreAI() => true;
        public override bool PreAI() {

            FrameCounter++;
            ActiveCounter++;

            return SafePreAI();

        }

        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer) {

            writer.Write(FrameCounter);
            writer.Write(ActiveCounter);

            SafeSendExtraAI(writer);

        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader) {

            ActiveCounter = reader.Read();
            FrameCounter = reader.Read();

            SafeReceiveExtraAI(reader);

        }

    }
}
