using System;
using System.Collections.Generic;
using System.Security.Policy;
using Terraria;

namespace TUtils.Timers {
    public static class TimerUtils {

        //METHODS
        /*public static Timer UpdateTimer(ref uint timer, uint by = 1 ) { unchecked { timer += by; } }

        public static Timer UpdateTimerByPassedTime(ref uint timer, ref uint lastUpdate) {

            if (lastUpdate == activeUpdateTimer.Value) return;

            UpdateTimer(ref timer, activeUpdateTimer.Value - lastUpdate);
            lastUpdate = activeUpdateTimer.Value;

        }*/

        /// <summary>
        /// To be called in Mod.PostUpdateEverything
        /// </summary>
        public static void UpdateActiveTimer() {

            if (Main.gameInactive || Main.gamePaused) return;

            activeUpdateTimer.Update();

        }

        //Update Count
        public static readonly ReadonlyTimer activeUpdateTimer = new ReadonlyTimer();
        
    }

    //CLASSES
    /// <summary>
    /// The Timer class represents an incrementing, overflowable value.
    /// It's <c>Update()</c> method is used to increment the timer when needed.
    /// <c>Value</c> holds the current value of the timer.
    /// </summary>
    public class Timer {

        private protected uint _timer;

        public Timer() => _timer = 0;

        public uint Value => _timer;
        public virtual uint Update(uint value = 1) {

            unchecked { _timer += value; }
            return _timer;

        }

        public void Reset() => Set(0);
        public virtual void Set(uint value) => _timer = value;

        public static implicit operator uint(Timer timer) => timer.Value;

    }

    /// <summary>
    /// The ActiveTimer class is a variant of the Timer class which, when updated, increments to match time passed since its creation.
    /// WARNING: All ActiveTimers will refuse to update if <c>TUrils.TimerUtils.UpdateActiveTimer()</c> is not called in <c>Mod.PostUpdateEverything()</c>.
    /// </summary>
    public class ActiveTimer : Timer {

        private protected uint _lastUpdateTime;

        public ActiveTimer() : base() => _lastUpdateTime = TimerUtils.activeUpdateTimer;

        public uint Update() {

            if (_lastUpdateTime == TimerUtils.activeUpdateTimer) return _timer;

            unchecked { _timer += TimerUtils.activeUpdateTimer - _lastUpdateTime; }
            _lastUpdateTime = TimerUtils.activeUpdateTimer;

            return _timer;

        }
        private new uint Update(uint value = 1) => 0;

        public override void Set(uint value) {
            _lastUpdateTime = TimerUtils.activeUpdateTimer;
            base.Set(value);
        }

    }

    /// <summary>
    /// The ReadonlyTimer class is a variant of the Timer class which cannot be reset or set from non-internal code
    /// </summary>
    public class ReadonlyTimer : Timer {

        internal ReadonlyTimer() : base() { }

        private new void Reset() { }
        private new void Set(uint value = 1) { }

    }

}
