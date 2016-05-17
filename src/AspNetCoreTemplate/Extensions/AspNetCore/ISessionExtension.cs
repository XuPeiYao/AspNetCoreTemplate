using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Extensions.AspNetCore {
    /// <summary>
    /// 針對<see cref="ISession"/>類型的擴充方法
    /// </summary>
    public static class ISessionExtension {
        /// <summary>
        /// 對指定Key寫入值
        /// </summary>
        /// <param name="obj">擴充對象</param>
        /// <param name="key">主鍵</param>
        /// <param name="value">值</param>
        public static void Set(this ISession obj, string key, string value) {
            obj.Set(key, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 嘗試取得指定Key的值
        /// </summary>
        /// <param name="obj">擴充對象</param>
        /// <param name="key">主鍵</param>
        /// <param name="value">取得結果</param>
        /// <returns>是否成功取得</returns>
        public static bool TryGetStringValue(this ISession obj, string key, out string value) {
            byte[] result;
            value = null;
            if (!obj.TryGetValue(key, out result)) return false;
            value = Encoding.UTF8.GetString(result);
            return true;
        }

        /// <summary>
        /// 對指定Key寫入值
        /// </summary>
        /// <param name="obj">擴充對象</param>
        /// <param name="key">主鍵</param>
        /// <param name="value">值</param>
        public static void Set(this ISession obj, string key, JToken value) {
            obj.Set(key, value.ToString());
        }

        /// <summary>
        /// 嘗試取得指定Key的值
        /// </summary>
        /// <param name="obj">擴充對象</param>
        /// <param name="key">主鍵</param>
        /// <param name="value">取得結果</param>
        /// <returns>是否成功取得</returns>

        public static bool TryGetJTokenValue(this ISession obj, string key, out JToken value) {
            string result;
            value = null;
            if (!obj.TryGetStringValue(key, out result)) return false;
            value = JToken.Parse(result);
            return true;
        }
    }
}
