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
    public partial class Form_SeePicture : Form
    {
        public Form_SeePicture()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //string i = Form_SearchTable.dt.Rows[Form_SearchTable.Rows_index]["PICTURE"].ToString();
            pictureBox1.Load(Form_SearchTable.picpath[Form_SearchTable.Rows_index]);
            
        }
    }
}
