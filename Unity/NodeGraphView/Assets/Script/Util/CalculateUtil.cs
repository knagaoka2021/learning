using System;
using System.Collections.Generic;
using UnityEngine;

namespace CUtil {
    public enum CALCULATE {
        NONE,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        Equal,
        PercentGreater,
        PercentGreaterEqual,
        PercentLess,
        PercentLessEqual
    }
    public static class CalculateUtil {
        public static Dictionary<CALCULATE, Func<double,double,double,bool>> MethodDictionary
        = new Dictionary<CALCULATE, Func<double,double,double,bool>>(){
            {CALCULATE.Greater,(double current,double total,double threshold) => {return Greater(current,threshold);}},
            {CALCULATE.GreaterEqual,(double current,double total,double threshold) => {return GreaterEqual(current,threshold);}},
            {CALCULATE.Less,(double current,double total,double threshold) => {return Less(current,threshold);}},
            {CALCULATE.LessEqual,(double current,double total,double threshold) => {return LessEqual(current,threshold);}},
            {CALCULATE.Equal,(double current,double total,double threshold) => {return Equal((int)current,(int)threshold);}},
            {CALCULATE.PercentGreater,(double current,double total,double threshold) => {return PercentGreater(current,total,threshold);}},
            {CALCULATE.PercentGreaterEqual,(double current,double total,double threshold) => {return PercentGreaterEqual(current,total,threshold);}},
            {CALCULATE.PercentLess,(double current,double total,double threshold) => {return PercentLess(current,total,threshold);}},
            {CALCULATE.PercentLessEqual,(double current,double total,double threshold) => {return PercentLessEqual(current,total,threshold);}}
        };

        /// <summary>
        /// 判定(>)
        /// </summary>
        public static bool Greater (double percentage, double threshold) {
            return percentage > threshold;
        }
        public static bool Greater (int percentage, int threshold) {
            return percentage > threshold;
        }
        public static bool Greater (float percentage, float threshold) {
            return percentage > threshold;
        }
        /// <summary>
        /// 判定(>=)
        /// </summary>
        public static bool GreaterEqual (double percentage, double threshold) {
            return percentage >= threshold;
        }
        public static bool GreaterEqual (int percentage, int threshold) {
            return percentage >= threshold;
        }
        public static bool GreaterEqual (float percentage, float threshold) {
            return percentage >= threshold;
        }
        /// <summary>
        /// 判定(<)
        /// </summary>
        public static bool Less (double percentage, double threshold) {
            return percentage < threshold;
        }
        public static bool Less (int percentage, int threshold) {
            return percentage < threshold;
        }
        public static bool Less (float percentage, float threshold) {
            return percentage < threshold;
        }
        /// <summary>
        /// 判定(<=)
        /// </summary>
        public static bool LessEqual (double percentage, double threshold) {
            return percentage <= threshold;
        }
        public static bool LessEqual (int percentage, int threshold) {
            return percentage <= threshold;
        }
        public static bool LessEqual (float percentage, float threshold) {
            return percentage <= threshold;
        }
        /// <summary>
        /// 判定(=)
        /// </summary>
        public static bool Equal (int percentage, int threshold) {
            return percentage == threshold;
        }
        /// <summary>
        /// 百分率計算
        /// </summary>
        public static double PercentCalculation (double current, double total) {
            return Math.Round ((current / total) * 100);
        }
        /// <summary>
        /// 百分率判定(>)
        /// </summary>
        public static bool PercentGreater (double current, double total, double threshold) {
            return Greater (PercentCalculation (current, total), threshold);
        }
        /// <summary>
        /// 百分率判定(>=)
        /// </summary>
        public static bool PercentGreaterEqual (double current, double total, double threshold) {
            return GreaterEqual (PercentCalculation (current, total), threshold);
        }
        /// <summary>
        /// 百分率判定(<)
        /// </summary>
        public static bool PercentLess (double current, double total, double threshold) {
            return Less (PercentCalculation (current, total), threshold);
        }
        /// <summary>
        /// 百分率判定(<=)
        /// </summary>
        public static bool PercentLessEqual (double current, double total, double threshold) {
            return LessEqual (PercentCalculation (current, total), threshold);
        }
    }
}