using System;
using Terraria;

namespace ChemistryClass {
    public class RefinementItem {

        public RefinementItem(int itemID, float value) {

            this.itemID = itemID;
            this.value = value;

        }

        public int   itemID;
        public float value;

        public static explicit operator RefinementItem( (int, float) v )
            => new RefinementItem(v.Item1, v.Item2);

    }
}
