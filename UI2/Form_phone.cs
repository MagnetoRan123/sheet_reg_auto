using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using UI2;

namespace CameraServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string ImagePath;// = @"D:/image/shit.png";
        //private readonly string ip = ConfigurationManager.AppSettings["IP"];
        //private readonly int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //获取本机IP地址
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址,AddressFamily.InterNetwork表示此IP为IPv4,AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    tbIP.Text = address.ToString();
            }
            //关闭对文本框的非法线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;

            //初始化参数
            tbPort.Text = "8000";

            txtWidth.Text = "1200";
            txtHeight.Text = "1600";

        }

        List<byte> list = new List<byte>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            while (SocketHelper.queue.Count > 0)
            {
                var recevBuff = (byte[])SocketHelper.queue.Dequeue();
                foreach (var item in recevBuff)
                {
                    list.Add(item);
                }

                if (SocketHelper.queue.Count == 0)
                {
                    richTextBox1.Text += "成功收到图片信息  \r\n";
                    ShowImg(list.ToArray());
                    list.Clear();
                }
            }
        }

        void ShowImg(byte[] buffer)
        {
            try
            {
                ImagePath = @"D:/image/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                MemoryStream ms = new MemoryStream(buffer);
                Bitmap bitmap = new Bitmap(ms);
                bitmap.Save(ImagePath, ImageFormat.Bmp);

                bitmap.Dispose();
                ms.Close();

                pictureBox1.Image = Image.FromFile(ImagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        /// <summary>
        /// 开启socket服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSocket_Click(object sender, EventArgs e)
        {
            var ip = tbIP.Text.Trim();
            var portStr = tbPort.Text.Trim();
            var res = int.TryParse(portStr, out int port);
            if (!res)
            {
                MessageBox.Show("端口号必须是数字");
                return;
            }

            SocketHelper.Open(ip, port, richTextBox1);
            richTextBox1.Text += "socket服务已开启  \r\n";
            btnSocket.Enabled = false;
        }

        public  void takepicture()
        {
            var width = int.Parse(txtWidth.Text.Trim());
            var height = int.Parse(txtHeight.Text.Trim());
            var res = SocketHelper.clientConnectionItems.TryGetValue("001", out Socket socketServer);
            if (res && socketServer != null)
            {
                List<byte> list = new List<byte>
                {
                    0x01,//拍照指令
                    (byte)(width & 0xff),
                    (byte)((width & 0xff00) >> 8),
                    (byte)(height & 0xff),
                    (byte)((height & 0xff00) >> 8)
                };

                try
                {
                    socketServer.Send(list.ToArray());
                    richTextBox1.Text += "成功发送拍照指令  \r\n";
                }
                catch (Exception ee)
                {
                    richTextBox1.Text += $"发送拍照指令出现异常：{ee.Message}  \r\n";
                }
            }
            else
            {
                richTextBox1.Text += $"无设备连接  \r\n";
            }
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            /*var width = int.Parse(txtWidth.Text.Trim());
            var height = int.Parse(txtHeight.Text.Trim());
            var res = SocketHelper.clientConnectionItems.TryGetValue("001", out Socket socketServer);
            if (res && socketServer != null)
            {
                List<byte> list = new List<byte>
                {
                    0x01,//拍照指令
                    (byte)(width & 0xff),
                    (byte)((width & 0xff00) >> 8),
                    (byte)(height & 0xff),
                    (byte)((height & 0xff00) >> 8)
                };

                try
                {
                    socketServer.Send(list.ToArray());
                    richTextBox1.Text += "成功发送拍照指令  \r\n";
                }
                catch (Exception ee)
                {
                    richTextBox1.Text += $"发送拍照指令出现异常：{ee.Message}  \r\n";
                }
            }
            else
            {
                richTextBox1.Text += $"无设备连接  \r\n";
            }*/
            takepicture();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var res = SocketHelper.clientConnectionItems.TryGetValue("001", out Socket socketServer);
            if (res && socketServer != null)
            {
                List<byte> list = new List<byte>
                {
                    0x02  //开启闪光灯
                };

                try
                {
                    socketServer.Send(list.ToArray());
                    richTextBox1.Text += "成功发送开启闪光灯指令  \r\n";
                }
                catch (Exception ee)
                {
                    richTextBox1.Text += $"发送开启闪光灯指令出现异常：{ee.Message}  \r\n";
                }
            }
            else
            {
                richTextBox1.Text += $"无设备连接  \r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = SocketHelper.clientConnectionItems.TryGetValue("001", out Socket socketServer);
            if (res && socketServer != null)
            {
                List<byte> list = new List<byte>
                {
                    0x03  //关闭闪光灯
                };

                try
                {
                    socketServer.Send(list.ToArray());
                    richTextBox1.Text += "成功发送关闭闪光灯指令  \r\n";
                }
                catch (Exception ee)
                {
                    richTextBox1.Text += $"发送关闭闪光灯指令出现异常：{ee.Message}  \r\n";
                }
            }
            else
            {
                richTextBox1.Text += $"无设备连接  \r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.takepicture();
        }

        
    }
}
