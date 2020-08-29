using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace ChemistryClass.ModUtils {
    public static class LineGeneration {

        public static Vector2 Lightning(ProceduralLineSequence lineSeq, float targetLength, Vector2 realTVec, ref Vector2 curTVec, ref float lastTurn, float angleVariance = CCUtils.QUARTER_PI_FLOAT) {

            Vector2 ret = lineSeq.Endpoint;

            float vecRot = CCUtils.ShortestRotTo(realTVec.ToRotation(), curTVec.ToRotation());

            curTVec = curTVec.RotatedBy(Math.Abs(vecRot) > CCUtils.HALF_PI_FLOAT ? vecRot / 24f : vecRot / 5f);

            float rot = -lastTurn * 2 / 3;
            float addedRot = Main.rand.NextFloat(-angleVariance / 2, angleVariance);

            lastTurn = addedRot;

            ret += curTVec.WithMagnitude(targetLength).RotatedBy(rot + addedRot);

            return ret;

        }

        public static Vector2 LightningBasic(ProceduralLineSequence lineSeq, float targetLength, Vector2 targVec, ref float lastTurn, float angleVariance = CCUtils.QUARTER_PI_FLOAT) {

            Vector2 ret = lineSeq.Endpoint;

            float rot = -lastTurn * 2 / 3;
            float addedRot = Main.rand.NextFloat(-angleVariance / 2, angleVariance);

            lastTurn = addedRot;

            ret += targVec.WithMagnitude(targetLength).RotatedBy(rot + addedRot);

            return ret;

        }

    }
}
