using System;
using Terraria;
using ReLogic.Utilities;
using Terraria.ModLoader;
using TUtils.Timers;
using System.IO;
using System.Diagnostics;
using TUtils;

//ENSURE THIS NAMESPACE MATCHES YOUR MOD
namespace ChemistryClass.ModContents {

    public class ModProjectileWithTimer : ModProjectile {

        public ActiveTimer activeTimer = null;
        public Timer timer = null;

        public virtual bool SafeAutoload(ref string name) => true;
        public sealed override bool Autoload(ref string name) {

            if (this.GetType().IsSubclassOf(typeof(ModProjectileWithTimer)) && this.GetType() != typeof(ModProjectileWithTimer)) return SafeAutoload(ref name);
            return false;

        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults() {
            SafeSetDefaults();
            if (Logic.IsNull(activeTimer)) {
                activeTimer = new ActiveTimer();
            }
            if (Logic.IsNull(timer)) {
                timer = new Timer();
            }
        }

        public virtual void SafePostAI() { }
        public sealed override void PostAI() {

            SafePostAI();

            activeTimer.Update();
            timer.Update();

        }

        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer) {

            writer.Write(activeTimer);
            writer.Write(timer);

            SafeSendExtraAI(writer);

        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader) {

            SafeReceiveExtraAI(reader);

            timer.Set(reader.ReadUInt32());
            activeTimer.Set(reader.ReadUInt32());

        }

    }

    public class ModProjectileWithCrit : ModProjectile {

        public int critChance = 0;
        public ActiveTimer activeTimer = null;
        public Timer timer = null;

        public virtual bool SafeAutoload(ref string name) => true;
        public sealed override bool Autoload(ref string name) {

            if (this.GetType().IsSubclassOf(typeof(ModProjectileWithCrit)) && this.GetType() != typeof(ModProjectileWithCrit)) return SafeAutoload(ref name);
            return false;

        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults() {
            SafeSetDefaults();
            if (Logic.IsNull(activeTimer)) {
                activeTimer = new ActiveTimer();
            }
            if (Logic.IsNull(timer)) {
                timer = new Timer();
            }
            critChance = Main.player[projectile.owner].HeldItem.crit;
        }

        public virtual void SafePostAI() { }
        public sealed override void PostAI() {

            SafePostAI();

            activeTimer.Update();
            timer.Update();

        }

        public virtual void SafeModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) { }
        public sealed override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {

            crit = critChance > Main.rand.Next(101);

            SafeModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);

        }

        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer) {

            writer.Write(activeTimer);
            writer.Write(timer);
            writer.Write(critChance);

            SafeSendExtraAI(writer);

        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader) {

            SafeReceiveExtraAI(reader);

            critChance = reader.Read();
            timer.Set(reader.ReadUInt32());
            activeTimer.Set(reader.ReadUInt32());

        }

    }

    public class ModNPCWithTimer : ModNPC {

        public ActiveTimer activeTimer = null;
        public Timer timer = null;

        public virtual void SafeSetDefaults() { }
        public override void SetDefaults() {
            SafeSetDefaults();

            activeTimer = new ActiveTimer();
            timer = new Timer();
        }

        public virtual bool SafeAutoload(ref string name) => true;
        public sealed override bool Autoload(ref string name) {

            if (this.GetType().IsSubclassOf(typeof(ModNPCWithTimer)) && this.GetType() != typeof(ModNPCWithTimer)) return SafeAutoload(ref name);
            return false;

        }

        public virtual bool SafePreAI() => true;
        public override bool PreAI() {

            if (!SafePreAI()) return false;

            activeTimer.Update();

            return true;

        }

        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer) {

            writer.Write(activeTimer);
            writer.Write(timer);

            SafeSendExtraAI(writer);

        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader) {

            SafeReceiveExtraAI(reader);

            activeTimer.Set(reader.ReadUInt32());
            timer.Set(reader.ReadUInt32());

        }

    }
}
