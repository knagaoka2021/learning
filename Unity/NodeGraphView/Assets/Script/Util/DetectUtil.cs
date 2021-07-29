using System;
using UnityEngine;

namespace CUtil {
    public enum DETECT {
        NONE,
        DistanceGreater,
        DistanceGreaterEqual,
        DistanceLess,
        DistanceLessEqual
    }

    public static class DetectUtil {
        /// <summary>
        /// 距離判定
        /// </summary>
        public static float DistanceDetect (Vector3 position, Vector3 target) {
            float distance = (position - target).sqrMagnitude;
            return distance;
        }
        // <summary>
        /// 距離判定(>:2点間距離が一定距離より大きい)
        /// </summary>
        public static bool DistanceGreater (Vector3 position, Vector3 target, float fixedDistance) {
            return CalculateUtil.Greater (DistanceDetect (position, target), fixedDistance * fixedDistance);
        }
        // <summary>
        /// 距離判定(>=:2点間距離が一定距離以上)
        /// </summary>
        public static bool DistanceGreaterEqual (Vector3 position, Vector3 target, float fixedDistance) {
            return CalculateUtil.GreaterEqual (DistanceDetect (position, target), fixedDistance * fixedDistance);
        }
        // <summary>
        /// 距離判定(<:2点間距離が一定距離より小さい)
        /// </summary>
        public static bool DistanceLess (Vector3 position, Vector3 target, float fixedDistance) {
            return CalculateUtil.Less (DistanceDetect (position, target), fixedDistance * fixedDistance);
        }
        // <summary>
        /// 距離判定(<=:2点間距離が一定距離以下)
        /// </summary>
        public static bool DistanceLessEqual (Vector3 position, Vector3 target, float fixedDistance) {
            return CalculateUtil.LessEqual (DistanceDetect (position, target), fixedDistance * fixedDistance);
        }
    }
}