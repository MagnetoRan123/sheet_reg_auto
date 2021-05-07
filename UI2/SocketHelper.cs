using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraServer
{
    public class SocketHelper
    {
        //Socket服务器信息
        private static Socket client_socket;
       
        //集合：存储客户端信息
        public static Dictionary<string, Socket> clientConnectionItems = new Dictionary<string, Socket>();
        public static Queue queue = new Queue();
        private static RichTextBox _RichTextBox;

        /// <summary>
        /// 开启服务
        /// </summary>
        public static void Open(string SocketServer_Ip,int SocketServer_Port, RichTextBox richTextBox)
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse(SocketServer_Ip);

            IPEndPoint point = new IPEndPoint(address, Convert.ToInt32(SocketServer_Port));

            client_socket.Bind(point);

            client_socket.Listen(10);

            Task.Run(watchconnecting);

            _RichTextBox = richTextBox;
        }

        /// <summary>
        /// 监听客户端发来的请求
        /// </summary>
        static void watchconnecting()
        {
            Socket connection = null;
            //持续不断监听客户端发来的请求
            while (true)
            {
                connection = client_socket.Accept();
                //50秒客户端未响应，则关闭连接
                connection.ReceiveTimeout = 50000;
                var ep = (IPEndPoint)connection.RemoteEndPoint;
                _RichTextBox.Invoke(new EventHandler(delegate {
                    _RichTextBox.Text += $"{DateTime.Now} 客户端{ep.Address}:{ep.Port}连接\r\n";
                }));

                Task.Run(() => recvAsync(connection));
            }
        }

        static async Task recvAsync(object socketclientpara)
        {
            var socketServer = socketclientpara as Socket;
            while (true)
            {
                await Task.Delay(1);
                byte[] arrServiceRecMsg = new byte[1024 * 1024 * 10];
                try
                {
                    //添加客户端信息(key是设备号，value是socket对象)
                    if (!clientConnectionItems.ContainsKey("001"))
                    {
                        clientConnectionItems.Add("001", socketServer);
                    }

                    int length = socketServer.Receive(arrServiceRecMsg);
                    if (length > 0)
                    {
                        queue.Enqueue(arrServiceRecMsg.Take(length).ToArray());
                    }
                }
                catch (Exception ex)
                {
                    var key = clientConnectionItems.FirstOrDefault(a => a.Value == socketServer).Key;
                    if (!string.IsNullOrEmpty(key))
                    {
                        _RichTextBox.Invoke(new EventHandler(delegate {
                            _RichTextBox.Text += $"客户端断开连接\r\n";
                        }));

                        clientConnectionItems.Remove(key);
                        socketServer.Disconnect(true);
                        socketServer.Close();
                    }

                    break;
                }
            }
        }
    }
}
