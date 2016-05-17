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
using AspNetCoreTemplate.Extensions.AspNetCore;
using AspNetCoreTemplate.Controllers;
//using Microsoft.Framework.DependencyInjection;

namespace AspNetCoreTemplate {
    /// <summary>
    /// 初始化類別
    /// </summary>
    public class Startup : StartupBase{
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()//設定檔建構器
                .SetBasePath(env.ContentRootPath)//設定根目錄
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)//加入Json類型的設定檔如果有的話，並且檔案變更時重新自動載入
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);//加入Json類型且為特定執行環境的設定檔如果有的話

            if (env.IsDevelopment()) {//是否為開發模式
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();//加入環境變數
            Configuration = builder.Build();//建構設定檔
        }

        /// <summary>
        /// 設定服務，此方法在執行階段被呼叫
        /// </summary>
        /// <param name="services">服務集合</param>
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //加入Session
            services.AddSession();

            //加入MVC服務
            services.AddMvc();
            
            /*DI
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            */
        }

        /// <summary>
        /// 設定AspNetCore
        /// </summary>
        /// <param name="app">應用程式建構器</param>
        /// <param name="env">環境變數</param>
        /// <param name="loggerFactory">記錄檔處理類別</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            #region Development Configure
            //加入記錄主控台設定
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            
            //除錯模式
            loggerFactory.AddDebug();
            #endregion
            
            #region StaticFiles Configure
            //設定預設檔案
            ConfigureDefaultFiles(app);

            if (!env.IsDevelopment()) {//是否為開發模式
                app.UseDeveloperExceptionPage();//使用開發模式錯誤頁面
                app.UseDatabaseErrorPage();//使用資料庫錯誤頁面
                app.UseBrowserLink();//使用瀏覽器連結
            } else {
                ConfigureErrorPages(app, env);//設定錯誤頁面對應
            }


            //使用靜態檔案
            app.UseStaticFiles();
            #endregion

            //使用Session
            app.UseSession();

            //使用MVC服務，並且載入預設路由設定
            app.UseMvc(ConfigureMvcRoute);

            //WebSocket設定
            app.UseWebSockets<TestChatHanlder>();
        }
    }
}
