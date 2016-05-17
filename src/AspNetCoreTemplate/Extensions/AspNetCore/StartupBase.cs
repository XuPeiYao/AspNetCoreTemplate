using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using System.IO;

namespace AspNetCoreTemplate.Extensions.AspNetCore {
    /// <summary>
    /// 初始化類別的基礎類型，用以簡化啟動流程與增進功能
    /// </summary>
    public class StartupBase {
        /// <summary>
        /// 應用程式配置
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// 錯誤頁面與錯誤代碼對應
        /// </summary>
        public Dictionary<int, string> ErrorPages { get; set; } = new Dictionary<int, string>();
        
        /// <summary>
        /// 設定預設檔案名稱，此方法在執行階段被呼叫，使用此方法設定預設的檔案
        /// </summary>
        /// <param name="app">應用程式建構器</param>
        public void ConfigureDefaultFiles(IApplicationBuilder app) {
            //讀取設定檔中預設檔案設定
            var defaultFiles = Configuration.GetSection("DefaultFiles")?.GetChildren();
            
            //未設定則跳脫
            if (defaultFiles == null) return;

            foreach (var defaultFileItem in defaultFiles) {
                dynamic defaultFileObj = defaultFileItem.ToDynamicObject();
                app.UseDefaultFiles(new DefaultFilesOptions() {
                    RequestPath = defaultFileObj.RequestPath,
                    DefaultFileNames = ((object[])defaultFileObj.DefaultFileNames).Select(item => (string)item).ToList()
                });
            }
        }

        /// <summary>
        /// 設定Mvc路由，此方法在執行階段被呼叫，使用此方法設定MVC預設路由規則
        /// </summary>
        /// <param name="routes">路由建構器</param>
        public void ConfigureMvcRoute(IRouteBuilder routes) {
            //取得所有路由規則
            var rules = Configuration.GetSection("MvcRoutingRules")?.GetChildren();

            //未設定則跳脫
            if (rules == null) return;

            //註冊所有路由規則
            foreach (var rule in rules) {
                //取得子屬性集合
                var attributes = rule.GetChildren();

                //註冊路由
                routes.MapRoute(
                    name: attributes.Where(item => item.Key == "Name").FirstOrDefault()?.Value,
                    template: attributes.Where(item => item.Key == "Template").FirstOrDefault()?.Value,
                    defaults: attributes.Where(item => item.Key == "Defaults").FirstOrDefault()?.ToDynamicObject(),
                    constraints: attributes.Where(item => item.Key == "Constraints").FirstOrDefault()?.ToDynamicObject(),
                    dataTokens: attributes.Where(item => item.Key == "DataTokens").FirstOrDefault()?.ToDynamicObject()
                );
            }
        }
        
        /// <summary>
        /// 設定錯誤代碼與錯誤頁面的對應，此方法在執行階段被呼叫，使用此方法設定錯誤頁面的檔案
        /// </summary>
        /// <param name="app">應用程式建構器</param>
        /// <param name="env">路由建構器</param>
        public void ConfigureErrorPages(IApplicationBuilder app, IHostingEnvironment env) {
            //取得所有錯誤頁面設定
            var pages = Configuration.GetSection("ErrorPages")?.GetChildren();

            //未設定則跳脫
            if (pages == null) return;

            //讀取所有錯誤頁面對應
            foreach (var page in pages) {
                //取得子屬性集合
                dynamic obj = page.ToDynamicObject();

                //存入狀態對應
                ErrorPages[int.Parse(obj.StatusCode)] = obj.FilePath;
            }

            Action<IApplicationBuilder> ErrorHandler = (builder) => {
                builder.Run(handler => {
                    return Task.Run(() => {
                        //取得狀態碼
                        int code = handler.Response.StatusCode;

                        //檢查是否存在指定的對應
                        if (ErrorPages.ContainsKey(code)) {
                            //寫出錯誤頁面內容
                            byte[] Content = File.ReadAllBytes(env.ContentRootPath + "/" + ErrorPages[handler.Response.StatusCode]);
                            handler.Response.ContentType = "text/html";
                            handler.Response.Body.WriteAsync(Content, 0, Content.Length);
                            return;

                            //以跳轉的方式到錯誤頁面
                            //handler.Response.Redirect($"{handler.Request.PathBase}/{ErrorPages[handler.Response.StatusCode]}");
                        }
                    });
                });
            };

            //狀態對應
            app.UseStatusCodePages(ErrorHandler);

            //狀態對應
            app.UseExceptionHandler(ErrorHandler);
        }
    }
}
