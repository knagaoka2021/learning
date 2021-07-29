using System;
using System.Collections.Generic;
using UnityEngine;

namespace CUtil {
    public static class DateTimeUtil {
        /// <summary>
        /// 年月日時刻取得(yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public static string GetDateTime () {
            DateTime dt = DateTime.Now;
            return dt.ToString ("yyyyMMdd_HHmmss");
        }
        /// <summary>
        /// 年月日取得(yyyy-MM-dd)
        /// </summary>
        public static string GetDate () {
            DateTime dt = DateTime.Now;
            return dt.ToString ("yyyy_MM_dd");
        }
    }
}