using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenCvSharp;
using Tesseract;
using System.Collections;
using FirebirdSql.Data.FirebirdClient;



namespace UI2
{
    public partial class Form_MainTable : Form
    {
        static Mat transformed_pic = new Mat();//透视变换后的图片

        string[] header = new string[5];//表头信息    客户，日期，地址电话，经手人，表单ID

        //List<int> y_loc = new List<int>();

        static double line_scale = 0.1;

        Mat debug_src = new Mat(@"D:\XD\1-dis\pic\proceed.jpg");

        public static FbConnection conn = new FbConnection(GetConnectionString());

        int length = 0;

        public Form_MainTable()
        {
            InitializeComponent();
            label1.Text = "欢迎使用表单自动录入系统！";
            if (File.Exists(@"D:\XD\1-dis\UI2\firebird\AUTOSHEET.FDB"))
            {
                File.Delete(@"D:\XD\1-dis\UI2\firebird\AUTOSHEET.FDB");
            }

            FbConnection.CreateDatabase(GetConnectionString());
            conn.Open();
            CreatClient(conn);
            CreatSellingTable(conn);
            CreatSellingTableInfo(conn);
            
        }


        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 打开设置的窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ConfigTable config = new Form_ConfigTable();
            config.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (Form_ConfigTable.IfPic)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "请选择文件";
                fileDialog.Filter = "(图片文件)|*.jpg;*.jpeg;*.png";
                this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    folder_path.Text = fileDialog.FileName;//返回文件的完整路径   
                    

                    if (true)
                    {
                        /*--要不要加这个判断条件呢--*/
                        transformed_pic = Cv_PersprctiveTrans(new Mat(fileDialog.FileName));
                        MemoryStream f = transformed_pic.ToMemoryStream(".png");
                        this.pictureBox1.Image = Image.FromStream(f);
                    }
                    else
                    {
                        //this.pictureBox1.Load(fileDialog.FileName);
                    }
                }
            }
            else
            {
                CameraServer.Form1 phone = new CameraServer.Form1();
                phone.ShowDialog();
                transformed_pic = Cv_PersprctiveTrans(new Mat(CameraServer.Form1.ImagePath));
                MemoryStream f = transformed_pic.ToMemoryStream(".png");
                this.pictureBox1.Image = Image.FromStream(f);
            }


        }
        /// <summary>
        /// 识别图象。把图像处理的代码复制进来。然后还需要把表格和表头信息填了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            

            ItemsInfo 商品全名 = new ItemsInfo("商品全名");
            ItemsInfo 规格 = new ItemsInfo("规格");
            ItemsInfo 型号 = new ItemsInfo("型号");
            ItemsInfo 单位 = new ItemsInfo("单位");
            ItemsInfo 数量 = new ItemsInfo("数量");
            ItemsInfo 单价 = new ItemsInfo("单价");
            ItemsInfo 金额 = new ItemsInfo("金额");
            ItemsInfo 单据备注 = new ItemsInfo("单据备注");
                        //处理表头信息
                        header = Get_header(transformed_pic);

                        //客户，日期，地址电话，经手人，表单ID
                        TextBox_ClientName.Text = header[0];
                        TextBox_Date.Text = header[1];
                        TextBox_AddressNPhonenumber.Text = header[2];
                        TextBox_Dealer.Text = header[3];
                        TextBox_SheetID.Text = header[4];
            
            //处理表中的信息
            Get_items_loc(transformed_pic, out List<double> cen_y_left, out List<double> k);//1开始，length - 3 为止
            int loc;
            length = k.Count - 4;
            for (int i = 1; i < k.Count - 3; i++)
            {
                int index = this.dataGridView1.Rows.Add();
                if (true)
                {
                    //loc = (int)(cen_y_left[i] - k[i] * (商品全名.x - (transformed_pic.Width * line_scale)) );
                    //dataGridView1.Rows[index].Cells[0].Value = Seg_img_to_text(transformed_pic, 商品全名.x , loc , 商品全名.width, 商品全名.height);
                    dataGridView1.Rows[index].Cells[0].Value = Seg_img_to_text(transformed_pic, 商品全名.x, 商品全名.y + (i - 1) * 商品全名.gap, 商品全名.width, 商品全名.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (规格.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[1].Value = Seg_img_to_text(transformed_pic, 规格.x, loc, 规格.width, 规格.height);
                    dataGridView1.Rows[index].Cells[1].Value = Seg_img_to_text(transformed_pic, 规格.x, 规格.y + (i - 1) * 规格.gap, 规格.width, 规格.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (型号.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[2].Value = Seg_img_to_text(transformed_pic, 型号.x, loc, 型号.width, 型号.height);
                    dataGridView1.Rows[index].Cells[2].Value = Seg_img_to_text(transformed_pic, 型号.x, 型号.y + (i - 1) * 型号.gap, 型号.width, 型号.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (单位.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[3].Value = Seg_img_to_text(transformed_pic, 单位.x, loc, 单位.width, 单位.height);
                    dataGridView1.Rows[index].Cells[3].Value = Seg_img_to_text(transformed_pic, 单位.x, 单位.y + (i - 1) * 单位.gap, 单位.width, 单位.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (数量.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[4].Value = Seg_img_to_text(transformed_pic, 数量.x, loc, 数量.width, 数量.height);
                    dataGridView1.Rows[index].Cells[4].Value = Seg_img_to_text(transformed_pic, 数量.x, 数量.y + (i - 1) * 数量.gap, 数量.width, 数量.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (单价.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[5].Value = Seg_img_to_text(transformed_pic, 单价.x, loc, 单价.width, 单价.height);
                    dataGridView1.Rows[index].Cells[5].Value = Seg_img_to_text(transformed_pic, 单价.x, 单价.y + (i - 1) * 单价.gap, 单价.width, 单价.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (金额.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[6].Value = Seg_img_to_text(transformed_pic, 金额.x, loc, 金额.width, 金额.height);
                    dataGridView1.Rows[index].Cells[6].Value = Seg_img_to_text(transformed_pic, 金额.x, 金额.y + (i - 1) * 金额.gap, 金额.width, 金额.height);

                    //loc = (int)(cen_y_left[i] - k[i] * (单据备注.x - (transformed_pic.Width * line_scale)));
                    //dataGridView1.Rows[index].Cells[7].Value = Seg_img_to_text(transformed_pic, 单据备注.x, loc, 单据备注.width, 单据备注.height);
                    dataGridView1.Rows[index].Cells[7].Value = Seg_img_to_text(transformed_pic, 单据备注.x, 单据备注.y + (i - 1) * 单据备注.gap, 单据备注.width, 单据备注.height);

                }
                else if (k[1] == 0)
                {
                    dataGridView1.Rows[index].Cells[0].Value = Seg_img_to_text(transformed_pic, 113, (int)cen_y_left[i], 397, 87);
                }
                else if (k[1] > 0)
                {
                    loc = (int)(- k[i] * (113 + 397 - (transformed_pic.Width * line_scale)) + cen_y_left[i]);
                    dataGridView1.Rows[index].Cells[0].Value = Seg_img_to_text(transformed_pic, 113, loc, 397, 87);
                }

            }
            label1.Text = "识别完成！";
            //int y_loc_1 = (int)y_loc_obj[1];
            //Seg_img_to_text(transformed_pic, 113, (int)y_loc[1], 397, 87);//商品全名
            //dataGridView1.
            //y_loc = Get_items_loc(transformed_pic);//1 开始，length - 3 为止


        }

        /// <summary>
        /// index 从一开始
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string Get_Text(string input, int loc)
        {
            
            if (input == "商品全名")
            { return Seg_img_to_text(transformed_pic, 113, loc, 397, 87);}
            if (input == "规格")
            { return Seg_img_to_text(transformed_pic, 545, loc, 585, 87); }
            if (input == "型号")
            { return Seg_img_to_text(transformed_pic, 1153, loc, 735, 73); }
            if (input == "单位")
            { return Seg_img_to_text(transformed_pic, 1917, loc, 237, 73); }
            if (input == "数量")
            { return Seg_img_to_text(transformed_pic, 2165, loc, 339, 73); }
            if (input == "单价")
            { return Seg_img_to_text(transformed_pic, 2521, loc, 409, 73); }
            if (input == "金额")
            { return Seg_img_to_text(transformed_pic, 2955, loc, 475, 73); }
            if (input == "单据备注")
            { return Seg_img_to_text(transformed_pic, 3349, loc, 793, 73); }
            else return "尴尬，看到我证明出bug了";   
        }

        /// <summary>
        /// 储存到数据库里
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            //存图片
            string ImagePath = @"D:\XD\1-dis\UI2\firebird\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            Cv2.ImWrite(ImagePath, transformed_pic);
            
            //存客户
            using (FbCommand insertData = conn.CreateCommand())
            {
                insertData.CommandText = "insert into Client values(@ClientName, @AddressPhone)";
                insertData.Parameters.Clear();

                insertData.Parameters.Add("@ClientName",   FbDbType.VarChar).Value = TextBox_ClientName.Text;
                insertData.Parameters.Add("@AddressPhone", FbDbType.VarChar).Value = TextBox_AddressNPhonenumber.Text;

                insertData.ExecuteNonQuery();
            }
            //存销售单
            using (FbCommand insertData = conn.CreateCommand())
            {
                insertData.CommandText = "insert into SellingTable values(@SheetID, @ClientName, @Dealer, @Datee, @Money, @Creator, @Picture)";
                insertData.Parameters.Clear();

                insertData.Parameters.Add("@SheetID",     FbDbType.VarChar).Value = TextBox_SheetID.Text;
                insertData.Parameters.Add("@ClientName",  FbDbType.VarChar).Value = TextBox_ClientName.Text;
                insertData.Parameters.Add("@Dealer",      FbDbType.VarChar).Value = TextBox_Dealer.Text;
                insertData.Parameters.Add("@Datee",       FbDbType.VarChar).Value = TextBox_Date.Text;
                insertData.Parameters.Add("@Money",       FbDbType.Float).Value = 136;
                insertData.Parameters.Add("@Creator",     FbDbType.VarChar).Value = "管理员";
                insertData.Parameters.Add("@Picture",     FbDbType.VarChar).Value = ImagePath;

                insertData.ExecuteNonQuery();
            }
            int intA;
            for (int i = 0; i < length; i++)
            {
                //销售单详表
                using (FbCommand insertData = conn.CreateCommand())
                {
                    insertData.CommandText = "insert into SellingTableInfo values(@SheetID, @ItemName, @Number, @Price, @Money, @Note)";
                    insertData.Parameters.Clear();

                    insertData.Parameters.Add("@SheetID", FbDbType.VarChar).Value = TextBox_SheetID.Text;
                    insertData.Parameters.Add("@ItemName",FbDbType.VarChar).Value = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    insertData.Parameters.Add("@Number",  FbDbType.Integer).Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    int.TryParse(dataGridView1.Rows[i].Cells[5].Value.ToString(), out intA);
                    insertData.Parameters.Add("@Price",   FbDbType.Float).Value = intA;
                    int.TryParse(dataGridView1.Rows[i].Cells[6].Value.ToString(), out intA);
                    insertData.Parameters.Add("@Money",   FbDbType.Float).Value = intA;
                    insertData.Parameters.Add("@Note",    FbDbType.VarChar).Value = dataGridView1.Rows[i].Cells[7].Value.ToString();

                    insertData.ExecuteNonQuery();
                }
            }
            label1.Text = "储存完成！";

        }

        private void search_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_SearchTable search_table = new Form_SearchTable();
            search_table.ShowDialog();
        }


        /// <summary>
        /// 得到透视变换后的表单图片
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private Mat Cv_PersprctiveTrans(Mat src)
        {
            Mat img = new Mat();
            Cv2.Resize(src, img, new OpenCvSharp.Size(src.Width * 0.25, src.Height * 0.25));
            if (src.Channels() != 1)
            {
                Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
            }
            Cv2.GaussianBlur(img, img, new OpenCvSharp.Size(5, 5), 0, 0);

            //好蠢的二值化方法，需要优化成自适应算法
            double OTSU;
            using (Mat dst = new Mat())
            {
                OTSU = Cv2.Threshold(img, dst, 200, 255, ThresholdTypes.Otsu);
            }
            Cv2.Threshold(img, img, 0.95 * OTSU, 255, ThresholdTypes.Binary);

            //找最大的轮廓
            Mat element = Cv2.GetStructuringElement(0, new OpenCvSharp.Size(3, 3));
            Cv2.Dilate(img, img, element);
            Mat hierarchly = new Mat();
            Cv2.FindContours(img, out Mat[] f_contours, hierarchly, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, new OpenCvSharp.Point(0, 0));
            double max_area = 0;
            int index = 0;
            for (int i = 0; i < f_contours.Length; i++)
            {
                double tmparea = Math.Abs(Cv2.ContourArea(f_contours[i]));
                if (tmparea > max_area)
                {
                    index = i;
                    max_area = tmparea;
                }
            }
            Mat maxcounter = new Mat(img.Size(), img.Type());
            Cv2.DrawContours(maxcounter, f_contours, index, new Scalar(255, 0, 0), 1);
            //H_ough(maxcounter);
            Point2f[] corners = Cv2.GoodFeaturesToTrack(maxcounter, 4, 0.01, 100, new Mat(), 3, false, 0.04);
            Point2f[] corners_sorted = new Point2f[4];

            //排序
            float tempMax = 0; float tempMin = (corners[0].X + corners[0].Y);
            int Max_index = 0; int Min_index = 0;
            for (int i = 0; i < 4; i++)
            {
                if ((corners[i].X + corners[i].Y) > tempMax)
                { corners_sorted[3] = corners[i]; tempMax = (corners[i].X + corners[i].Y); Max_index = i; }
                else if ((corners[i].X + corners[i].Y) < tempMin)
                { corners_sorted[0] = corners[i]; tempMin = (corners[i].X + corners[i].Y); Min_index = i; }
            }
            float x_max = 0; int xmax_index = 0;
            for (int i = 0; i < 4; i++)
            {
                if ((corners[i].X > x_max) && (i != Max_index) && (i != Min_index))
                { corners_sorted[1] = corners[i]; x_max = corners[i].X; xmax_index = i; }
            }
            for (int i = 0; i < 4; i++)
            {
                if ((i != xmax_index) && (i != Max_index) && (i != Min_index))
                { corners_sorted[2] = corners[i]; }
            }

            //准备变换需要用的点
            //Point2f[] corners_trans = new Point2f[4];
            //corners_trans[0] = new Point2f(0, 0);
            //corners_trans[1] = new Point2f(1098 - 1, 0);
            //corners_trans[2] = new Point2f(0, 596 - 1);
            //corners_trans[3] = new Point2f(1098 - 1, 596 - 1);

            Point2f[] corners_trans = new Point2f[4];
            corners_trans[0] = new Point2f(0, 0);
            corners_trans[1] = new Point2f(img.Width - 1, 0);
            corners_trans[2] = new Point2f(0, img.Height - 1);
            corners_trans[3] = new Point2f(img.Width - 1, img.Height - 1);

            for (int i = 0; i < 4; i++)
            {
                corners_sorted[i] = corners_sorted[i] * 4;
                corners_trans[i] = corners_trans[i] * 4;
            }

            Mat transform = Cv2.GetPerspectiveTransform(corners_sorted, corners_trans);
            Mat final = new Mat();
            Cv2.WarpPerspective(src, final, transform, new OpenCvSharp.Size(4 * img.Width, 4 * img.Height));
            Cv2.ImWrite(@"D:/image/result.png", final);
            return final;
        }


        /// <summary>
        /// 计算两条直线的交点。可能用不上
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private Point2f computeIntersect(Vec4i a, Vec4i b)
        {
            int x1 = a[0], y1 = a[1], x2 = a[2], y2 = a[3];
            int x3 = b[0], y3 = b[1], x4 = b[2], y4 = b[3];
            float d = ((float)(x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4));
            Point2f pt = new Point2f();
            if (d != 0)
            {
                pt.X = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / d;
                pt.Y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / d;
                return pt;
            }

            else
            {
                pt.X = -1;
                pt.Y = -1;
                return pt;
            }

        }


        /// <summary>
        /// 霍夫变换，再反复迭代找出合适的线。效果hin差
        /// </summary>
        /// <param name="input_img"></param>
        private void H_ough(Mat input_img)
        {
            ///<霍夫变换>
            int dianshu_threshold = Cv2.CountNonZero(input_img) / 100;//以白色点数为锚确定霍夫变换threshold
            LineSegmentPoint[] lineSegmentPoint;  //创建接收数组
            while (true)//循环找出合适的阈值
            {
                lineSegmentPoint = Cv2.HoughLinesP(input_img, 1.0, Cv2.PI / 180, dianshu_threshold, 0, 0);//进行霍夫变换直线检测

                //int line_number = lineSegmentPoint.Length;
                if (lineSegmentPoint.Length < 4)
                {
                    dianshu_threshold -= 2;
                }
                else if (lineSegmentPoint.Length > 8)
                {
                    dianshu_threshold += 1;
                }
                else
                {
                    Console.WriteLine("线貌似可以了");
                    Console.WriteLine(lineSegmentPoint.Length);
                    break;
                }
                break;
            }
            using (Mat SeeLines = new Mat("D:/XD/1-dis/pic/1.jpg", ImreadModes.Color))
            {
                Cv2.Resize(SeeLines, SeeLines, new OpenCvSharp.Size(SeeLines.Width * 0.25, SeeLines.Height * 0.25));

                for (int i = 0; i < lineSegmentPoint.Length; i++)
                {
                    Cv2.Line(SeeLines, lineSegmentPoint[i].P1, lineSegmentPoint[i].P2, Scalar.RandomColor(), 3);
                }
                using (new Window("lines", SeeLines)) { Cv2.WaitKey(0); }
            }

        }


        /// <summary>
        /// 从小图片识别文字
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static string ImageToText(string imgPath)
        {
            /*--我装中文的包了吗？？？？？--*/
            using (var engine = new TesseractEngine(@"D:\XD\1-dis\tesseractdemo\tesseractdemo\tessdata", "chi_sim", EngineMode.Default))
            {
                engine.SetVariable("user_defined_dpi", "300");
                using (var img = Pix.LoadFromFile(imgPath))
                {
                    using (var page = engine.Process(img))
                    {
                        return page.GetText();
                    }
                }
            }
        }


        /// <summary>
        /// 把输入的图片变成灰度图，锐化一次会写入一次图片
        /// </summary>
        /// <param name="input_img">原尺寸下分割出的文字</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">向右</param>
        /// <param name="height">向下</param>
        /// <returns></returns>
        public static string Seg_img_to_text(Mat input_img, double x, double y, int width, int height)
        {

            //x *= input_img.Width; y *= input_img.Height; //0.09  0.262
            //width = 71; height = 32;
            /*--缓存，好蠢的方式。但是Pix的LoadFromMemory里的byte是什么鬼--*/
            string imgPath = @"D:\XD\1-dis\pic\cache\seged_image.jpg";
            using (InputArray kernel = InputArray.Create<double>(new double[3, 3] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } }))
            using (Mat dst = new Mat(input_img, new OpenCvSharp.Rect((int)x, (int)y, width, height)))
            {
                //不准的话就画外接矩形
                //https://www.itdaan.com/tw/46038a31b3020fce189dd3f30699ac0e
                if (dst.Channels() != 1)
                {
                    Cv2.CvtColor(dst, dst, ColorConversionCodes.BGR2GRAY);
                }
                //OTSU = Cv2.Threshold(dst, dst, 200, 255, ThresholdTypes.Otsu);
                //Cv2.Resize(dst, dst, new Size(dst.Width * 5, dst.Height * 5), 0, 0, InterpolationFlags.Cubic);
                //Cv2.Filter2D(dst, dst, dst.Depth(), kernel, new Point(-1, -1), 0);
                //Cv2.Threshold(dst, dst, 125, 255, ThresholdTypes.Binary);
                Cv2.ImWrite(imgPath, dst);
            }

            string strResult = ImageToText(imgPath);
            if (string.IsNullOrEmpty(strResult))
            { strResult = "无法识别"; }
            strResult = strResult.Replace(" ", ""); //删除中间的空格。为什么识别中文的时候会有空格？？
            return strResult;

        }


        /// <summary>
        /// 输入透视变换后的原尺寸图片，返回图中横线的斜率和scale部分的y坐标
        /// </summary>
        /// <param name="input_img">这个需要原尺寸的图片</param>
        /// <returns>从小到大，即从上到下。从[1]开始，到[length - 3]为止</returns>
        public static void Get_items_loc(Mat inputt_img, out List<double> cen_y_left, out List<double> k)
        {
            Mat input_img = new Mat();
            Cv2.Resize(inputt_img, input_img, new OpenCvSharp.Size(inputt_img.Width * 0.25, inputt_img.Height * 0.25));
            Mat gray = new Mat(); ;
            if (input_img.Channels() != 1)
            {
                Cv2.CvtColor(input_img, gray, ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                gray = input_img;
            }
            Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(3, 3), 0, 0);
            Mat bw = new Mat();
            Cv2.AdaptiveThreshold(~gray, bw, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 3, -10);
            Mat horizontal = bw.Clone();
            int scale = 40;//使用这个变量来增加/减少待检测的行数
            /*
             * 提取水平线条  
             */
            //指定水平轴上的大小 
            int horizontalsize = horizontal.Cols / scale;

            //结构元素
            Mat horizontalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(horizontalsize, 1));

            // 形态学
            Cv2.Erode(horizontal, horizontal, horizontalStructure, new OpenCvSharp.Point(-1, -1));
            Cv2.Dilate(horizontal, horizontal, horizontalStructure, new OpenCvSharp.Point(-1, -1));
            // Cv2.ImShow("horizontal", horizontal);

            Mat line_left = new Mat(gray.Size(), gray.Type());
            Mat line_right = new Mat(gray.Size(), gray.Type());
            //左面的线
            Cv2.Line(line_left, new OpenCvSharp.Point(line_scale * gray.Width, 20), new OpenCvSharp.Point(line_scale * gray.Width, (gray.Height - 20)), new Scalar(255), 5);
            Cv2.Line(line_right, new OpenCvSharp.Point((1 - line_scale) * gray.Width, 20), new OpenCvSharp.Point((1 - line_scale) * gray.Width, (gray.Height - 20)), new Scalar(255), 5);
            //将两张图片进行与操作
            Mat joints_left = new Mat();
            Mat joints_right = new Mat();
            Cv2.BitwiseAnd(horizontal, line_left, joints_left);
            Cv2.BitwiseAnd(horizontal, line_right, joints_right);

            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            Cv2.Dilate(joints_left, joints_left, kernel, new OpenCvSharp.Point(-1, -1), 9);
            Cv2.Dilate(joints_right, joints_right, kernel, new OpenCvSharp.Point(-1, -1), 9);
            Mat hierarchly = new Mat();
            Cv2.FindContours(joints_left, out Mat[] contours_left, hierarchly, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, new OpenCvSharp.Point(0, 0));
            Cv2.FindContours(joints_right, out Mat[] contours_right, hierarchly, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, new OpenCvSharp.Point(0, 0));
            //ArrayList cen_y = new ArrayList();
            cen_y_left = new List<double>();
            List<double> cen_y_right = new List<double>();
            k = new List<double>();
            double y_left; double y_right;
            //int p = 0;
            for (int i = 0; i < contours_left.Length; i++)
            {
                //Mat tmp = new Mat(contours[i]);
                Moments moment_left = Cv2.Moments(contours_left[i], false);
                Moments moment_right = Cv2.Moments(contours_right[i], false);
                if (moment_left.M00 != 0)//除数不能为0
                {
                    //int x = (int)(moment.M10 / moment.M00);//计算重心横坐标
                    y_left = (moment_left.M01 / moment_left.M00);//计算重心纵坐标
                    y_left *= 4;
                    cen_y_left.Add(y_left);
                    y_right = (moment_right.M01 / moment_right.M00);//计算重心纵坐标
                    y_right *= 4;
                    k.Add((y_right - y_left) / ((2 * line_scale) * (double)gray.Width));

                    //cen_y_right.Add(y_right);
                    //p++;
                    //Cv2.Circle(src, new Point(x, y), 5, Scalar.RandomColor());
                }

            }
            k.Reverse();
            cen_y_left.Sort();//从小到大，即从上到下
            //cen_y_right.Sort();//从小到大，即从上到下
            //return cen_y_left;
        }


        /// <summary>
        /// 得到表头信息。客户，日期，地址电话，经手人，表单ID
        /// </summary>
        /// <param name="input_img"></param>
        /// <returns></returns>
        public static string[] Get_header(Mat input_img)
        {
            string[] out_header = new string[5];
            //string[] in_header = new string[9];
            //这些参数应该还要调
            out_header[0] = Seg_img_to_text(input_img, 410, 644, 1456, 67);//客户
            //out_header[0].Replace(" ", "");//为什么识别汉字的时候会有空格？？？

            out_header[1] = Seg_img_to_text(input_img, 2359, 603, 507, 67);//日期

            out_header[2] = Seg_img_to_text(input_img, 435, 739, 455, 71);//地址，电话

            out_header[3] = Seg_img_to_text(input_img, 2405, 709, 189, 71);//经手人

            out_header[4] = Seg_img_to_text(input_img, 3047, 590, 662, 71);//表单ID

            return out_header;

        }


        /// <summary>
        /// index取值范围一到八
        /// mat输入原尺寸图片
        /// </summary>
        /// <param name="input_img"></param>
        /// <param name="index">
        /// 1  商品全名
        /// 2  规格
        /// 3  型号
        /// 4  单位
        /// 5  数量
        /// 6  单价
        /// 7  金额
        /// 8  单据备注
        /// </param>
        /// <returns></returns>
        public static List<string> Get_items(Mat input_img, int index)
        {
            int cor_x = 0; //int width = 0; int height = 0;
            //有空再改成自适应的。反正表头也要用绝对值来找
            if (index == 1) { cor_x = 137; }//商品全名
            if (index == 2) { cor_x = 547; }//规格
            if (index == 3) { cor_x = 1157; }//型号
            if (index == 4) { cor_x = 1983; }//单位
            if (index == 5) { cor_x = 2273; }//数量
            if (index == 6) { cor_x = 2659; }//单价
            if (index == 7) { cor_x = 3107; }//金额
            if (index == 8) { cor_x = 3733; }//单据备注
            List<int> location = new List<int>();
            //location = Get_items_loc(input_img);
            List<string> result = new List<string>();
            for (int i = 1; i < (location.Count - 3); i++)
            { 
                result.Add(Seg_img_to_text(input_img, cor_x, (int)location[i], 1456, 67));
            }

            return result;
        }

        #region 表头信息的修改，可怕的重复代码。我怎么感觉这段东西没用呢。。。写完了才发现。我是伞兵
        private void TextBox_ClientName_TextChanged(object sender, EventArgs e)
        {
            header[0] = TextBox_ClientName.Text;
        }

        private void TextBox_Date_TextChanged(object sender, EventArgs e)
        {
            header[1] = TextBox_Date.Text;
        }

        private void TextBox_AddressNPhonenumber_TextChanged(object sender, EventArgs e)
        {
            header[2] = TextBox_AddressNPhonenumber.Text;
        }

        private void TextBox_Dealer_TextChanged(object sender, EventArgs e)
        {
            header[3] = TextBox_Dealer.Text;
        }

        private void TextBox_SheetID_TextChanged(object sender, EventArgs e)
        {
            header[4] = TextBox_SheetID.Text;
        }
        #endregion

        /// <summary>
        /// 都在名字里了
        /// </summary>
        /// <returns></returns>
        static string GetConnectionString()
        {

            FbConnectionStringBuilder cs = new FbConnectionStringBuilder();

            cs.Database = @"D:\XD\1-dis\UI2\firebird\AutoSheet.fdb";

            cs.UserID = "SYSDBA";

            cs.Password = "masterkey";

            //cs.Charset = "UTF8"; //不设置任何的字符集，就可以避免出现中文路径不识别问题了          

            cs.ServerType = FbServerType.Embedded; // 设置数据库类型为嵌入式


            return cs.ToString();

        }

        /// <summary>
        /// 创建客户表
        /// </summary>
        /// <param name="conn"></param>
        public static void CreatClient(FbConnection conn)
        {
            using (FbCommand createTable = conn.CreateCommand())

            {
                createTable.CommandText = "create table Client " +
                                                "(" +
                                                "ClientName    varchar(64) not null primary key," +
                                                "AddressPhone  varchar(128))";

                Console.WriteLine(createTable.ExecuteNonQuery());
            }

        }

        /// <summary>
        /// 创建销售单表
        /// </summary>
        /// <param name="conn"></param>
        public static void CreatSellingTable(FbConnection conn)
        {

            using (FbCommand createTable = conn.CreateCommand())

            {

                createTable.CommandText = "create table SellingTable " +
                                                "(" +
                                                "SheetID      varchar(20) not null primary key," +
                                                "ClientName   varchar(64) not null," +
                                                "Dealer       varchar(16) not null," +
                                                "Datee        varchar(20) not null," +
                                                "Money        float," +
                                                "Creator      varchar(10)," +
                                                "Picture      varchar(128))";

                Console.WriteLine(createTable.ExecuteNonQuery());

            }

        }

        /// <summary>
        /// 创建销售单详表
        /// </summary>
        /// <param name="conn"></param>
        public static void CreatSellingTableInfo(FbConnection conn)

        {

            using (FbCommand createTable = conn.CreateCommand())

            {

                createTable.CommandText = "create table SellingTableInfo " +
                                                "(" +
                                                "SheetID      varchar(20) not null," +
                                                "ItemName     varchar(20)," +
                                                "Number       int," +
                                                "Price        float," +
                                                "Money        float," +
                                                "Note         varchar(64))";


                Console.WriteLine(createTable.ExecuteNonQuery());

            }

        }

        private void Button1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.Text = "识别中，请耐心等待. . .";
        }
    }

    class ItemsInfo
    {
        //数据成员：字段
        public int x;
        public int y;
        public int gap;
        public int width;
        public int height;


        //函数成员：方法
        public ItemsInfo(string name)
        {
            gap = 1069 - 947;
            if (name == "商品全名")
            {
                x = 113;
                y = 956;
                width = 397;
                height = 87;
            }
            else if (name == "规格")
            {
                x = 545;
                y = 952;
                width = 585;
                height = 87;
            }
            else if (name == "型号")
            {
                x = 1153;
                y = 943;
                width = 735;
                height = 87;
            }
            else if (name == "单位")
            {
                x = 1917;
                y = 933;
                width = 237;
                height = 87;
            }
            else if (name == "数量")
            {
                x = 2175;
                y = 930;
                width = 323;
                height = 87;
            }
            else if (name == "单价")
            {
                x = 2521;
                y = 920;
                width = 409;
                height = 87;
            }
            else if (name == "金额")
            {
                x = 2955;
                y = 915;
                width = 475;
                height = 87;
            }
            else if (name == "单据备注")
            {
                x = 3349;
                y = 904;
                width = 793;
                height = 87;
            }
            else 
            {
                MessageBox.Show("有bug");
            }
        }
    }

}
