using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Extensions.AspNetCore {
    /// <summary>
    /// 針對<see cref="HttpClient"/>類型的擴充方法
    /// </summary>
    public static class HttpClientExtension {
        #region Get
        /// <summary>
        /// 將 GET 要求傳送至指定的 URI，並透過非同步作業，以JToken形式傳回回應內容
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> GetJsonAsync(this HttpClient Obj, string requestUri) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await Obj.GetStringAsync(requestUri)
                )
            );
        }

        /// <summary>
        /// 將 GET 要求傳送至指定的 URI，並透過非同步作業，以JToken形式傳回回應內容
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> GetJsonAsync(this HttpClient Obj, Uri requestUri) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await Obj.GetStringAsync(requestUri)
                )
            );
        }
        #endregion

        #region Post
        /// <summary>
        /// 以非同步作業的方式，傳送 POST 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <param name="cancellationToken">取消語彙基元，可由其他物件或執行緒使用以接收的取消通知</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PostJsonAsync(this HttpClient Obj, string requestUri, HttpContent content, CancellationToken cancellationToken) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PostAsync(requestUri, content, cancellationToken)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 POST 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <param name="cancellationToken">取消語彙基元，可由其他物件或執行緒使用以接收的取消通知</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PostJsonAsync(this HttpClient Obj, Uri requestUri, HttpContent content, CancellationToken cancellationToken) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PostAsync(requestUri, content, cancellationToken)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 POST 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PostJsonAsync(this HttpClient Obj, string requestUri, HttpContent content) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PostAsync(requestUri, content)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 POST 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PostJsonAsync(this HttpClient Obj, Uri requestUri, HttpContent content) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PostAsync(requestUri, content)).Content.ReadAsStringAsync()
                )
            );
        }
        #endregion

        #region Delete
        /// <summary>
        /// 以非同步作業的方式，傳送 DELETE 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="cancellationToken">取消語彙基元，可由其他物件或執行緒使用以接收的取消通知</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> DeleteJsonAsync(this HttpClient Obj, string requestUri, CancellationToken cancellationToken) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.DeleteAsync(requestUri, cancellationToken)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 DELETE 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="cancellationToken">取消語彙基元，可由其他物件或執行緒使用以接收的取消通知</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PostJsonAsync(this HttpClient Obj, Uri requestUri, CancellationToken cancellationToken) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.DeleteAsync(requestUri, cancellationToken)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 DELETE 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> DeleteJsonAsync(this HttpClient Obj, string requestUri) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.DeleteAsync(requestUri)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 DELETE 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PostJsonAsync(this HttpClient Obj, Uri requestUri) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.DeleteAsync(requestUri)).Content.ReadAsStringAsync()
                )
            );
        }
        #endregion

        #region Put
        /// <summary>
        /// 以非同步作業的方式，傳送 PUT 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <param name="cancellationToken">取消語彙基元，可由其他物件或執行緒使用以接收的取消通知</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PutJsonAsync(this HttpClient Obj, string requestUri, HttpContent content, CancellationToken cancellationToken) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PutAsync(requestUri, content, cancellationToken)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 PUT 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <param name="cancellationToken">取消語彙基元，可由其他物件或執行緒使用以接收的取消通知</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PutJsonAsync(this HttpClient Obj, Uri requestUri, HttpContent content, CancellationToken cancellationToken) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PutAsync(requestUri, content, cancellationToken)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 PUT 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PutJsonAsync(this HttpClient Obj, string requestUri, HttpContent content) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PutAsync(requestUri, content)).Content.ReadAsStringAsync()
                )
            );
        }

        /// <summary>
        /// 以非同步作業的方式，傳送 PUT 要求和取消語彙基元
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="requestUri">要求被傳送到的 URI</param>
        /// <param name="content">傳送至伺服器的 HTTP 要求內容</param>
        /// <returns>回應內容</returns>
        public static async Task<JToken> PutJsonAsync(this HttpClient Obj, Uri requestUri, HttpContent content) {
            return await Task.FromResult<JToken>(
                JContainer.Parse(
                    await (await Obj.PutAsync(requestUri, content)).Content.ReadAsStringAsync()
                )
            );
        }
        #endregion
    }
}
