using AspNetCoreTemplate.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Threading;

namespace AspNetCoreTemplate.Controllers {
    public class TestChatHanlder : WebSocketHandler {
        //WebSocket集合，用以儲存目前線上所有使用者。
        public static List<WebSocket> WebSocketList = new List<WebSocket>();

        //建構子，設定Request路徑
        public TestChatHanlder() : base(RequestPath: "/api/chat") {
            //加入事件
            this.OnConnected += TestChatHanlder_OnConnected;
            this.OnDisconnected += TestChatHanlder_OnDisconnected;
            this.OnReceive += TestChatHanlder_OnReceive;
        }

        private void TestChatHanlder_OnConnected(HttpContext Context, WebSocket Socket) {
            //加入聊天室使用者集合
            WebSocketList.Add(Socket);
        }

        private void TestChatHanlder_OnDisconnected(HttpContext Context, WebSocket Socket) {
            //自聊天使使用者集合中移除
            WebSocketList.Remove(Socket);
        }

        private void TestChatHanlder_OnReceive(WebSocket Socket, WebSocketMessageType Type, byte[] ReceiveMessage) {
            //並行廣播給所有使用者，也可以轉發給指定使用者
            Parallel.ForEach(WebSocketList, async socket => {
                await socket.SendAsync(new ArraySegment<byte>(ReceiveMessage), Type, true, CancellationToken.None);
            });
        }

        protected override bool AcceptConditions(HttpContext Context) {
            //總是允許連線，此處可以透過Context中的QueryString或者Cookie等來判別是否允許連線
            return true;
        }
    }
}
