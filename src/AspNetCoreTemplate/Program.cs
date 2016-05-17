using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCoreTemplate {
    /// <summary>
    /// 啟動類別
    /// </summary>
    public class Program {
        /// <summary>
        /// 程式進入點
        /// </summary>
        /// <param name="args">執行參數</param>
        public static void Main(string[] args) {
            var host = new WebHostBuilder()//WebHost建構器
                .UseKestrel()//可使用Kestrel
                .UseContentRoot(Directory.GetCurrentDirectory())//設定根目錄
                .UseIISIntegration()//可使用IIS
                .UseStartup<Startup>()//設定初始化類別
                .Build();//建構WebHost

            host.Run();//開始執行WebHost
        }
    }
}
