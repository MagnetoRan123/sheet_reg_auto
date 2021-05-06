using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI2
{
    
    public partial class Form_ConfigTable : Form
    {
        bool flag = true;
        public static bool IfPic = true;
        public Form_ConfigTable()
        {
            InitializeComponent();
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                label_StartX.Text = e.X.ToString();
                label_StartY.Text = e.Y.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog path = new OpenFileDialog();
            path.ShowDialog();
            folder_path.Text = path.FileName;
            //Properties.Settings.Default.PathOrNot = 1;
            Properties.Settings.Default.Save();
            //展示图片
            Bitmap img;
            try { img = new Bitmap(path.FileName); }
            catch { return; }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = img;
        }

        private void RBtn_Path_Click(object sender, EventArgs e)
        {
            IfPic = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IfPic = false;
        }
    }
}
