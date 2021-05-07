using CameraServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_MainTable());
        }

        public static void takepicture()
        {
            var res = SocketHelper.clientConnectionItems.TryGetValue("001", out Socket socketServer);
            if (res && socketServer != null)
            {
                List<byte> list1 = new List<byte>
                {
                    0x01//拍照指令
                };

                try
                {
                    socketServer.Send(list1.ToArray());
                }
                catch (Exception ee)
                {
                    throw;
                }
            }

            List<byte> list2 = new List<byte>();
            void timer1_Tick(object sender, EventArgs e)
            {
                while (SocketHelper.queue.Count > 0)
                {
                    var recevBuff = (byte[])SocketHelper.queue.Dequeue();
                    foreach (var item in recevBuff)
                    {
                        list2.Add(item);
                    }

                    if (SocketHelper.queue.Count == 0)
                    {
                        ShowImg(list2.ToArray());
                        list2.Clear();
                    }
                }
            }

            void ShowImg(byte[] buffer)
            {
                try
                {
                    string ImagePath = @"D:/image/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    MemoryStream ms = new MemoryStream(buffer);
                    Bitmap bitmap = new Bitmap(ms);
                    bitmap.Save(ImagePath, ImageFormat.Bmp);

                    bitmap.Dispose();
                    ms.Close();

                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}
