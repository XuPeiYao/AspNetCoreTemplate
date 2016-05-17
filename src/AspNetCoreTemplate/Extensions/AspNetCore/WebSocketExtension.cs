using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Extensions.AspNetCore {
    /// <summary>
    /// 針對<see cref="WebSocket"/>類型的擴充方法
    /// </summary>
    public static class WebSocketExtension {
        /// <summary>
        /// 非同步接收文字
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="Token">散佈通知，表示不應取消作業</param>
        /// <param name="BufferSize">緩衝區大小</param>
        /// <returns>字串與原始結果</returns>
        public static async Task<Tuple<string, WebSocketReceiveResult>> ReceiveTextAsync(this WebSocket Obj, CancellationToken Token, int BufferSize = 1024 * 4) {
            byte[] Buffer = new byte[BufferSize];
            WebSocketReceiveResult ReceiveResult = await Obj.ReceiveAsync(new ArraySegment<byte>(Buffer), Token);
            string Result = Encoding.UTF8.GetString(Buffer);
            int End = Result.IndexOf("\0");
            if (End == -1) End = Result.Length;
            Result = Result.Substring(0, End);
            return new Tuple<string, WebSocketReceiveResult>(Result, ReceiveResult);
        }

        /// <summary>
        /// 非同步送出文字
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="Data">文字內容</param>
        /// <returns>字串與原始結果</returns>
        public static async Task SendTextAsync(this WebSocket Obj, string Data) {
            await Obj.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(Data)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 透過WebSocket以非同步的方式傳送資料
        /// </summary>
        /// <param name="Obj">擴充對象</param>
        /// <param name="buffer">要透過連線傳送的緩衝區</param>
        /// <param name="messageType">表示應用程式正在傳送二進位或文字訊息</param>
        /// <param name="endOfMessage">指示「緩衝區」中的資料是否為訊息的最後一部分</param>
        /// <param name="cancellationToken">散佈通知的語彙基元，該通知表示不應取消作業</param>
        /// <returns>字串與原始結果</returns>
        public static async Task SendAsync(this WebSocket Obj, byte[] buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken) {
            await Obj.SendAsync(new ArraySegment<byte>(buffer), messageType, endOfMessage, cancellationToken);
        }
    }
}
