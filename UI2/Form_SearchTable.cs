using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using DataAccess;


namespace UI2
{

    public partial class Form_SearchTable : Form
    {
        //public static string picpath = "";
        public static DataTable dt = null;
        public static DataTable dt2 = null;
        string sSQL_1 = "";
        string sSQL_2 = "";
        public static List<string> picpath = new List<string>();
        public static int Rows_index = 0;
        public Form_SearchTable()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {//开启显示图片的窗体
                Rows_index = e.RowIndex;
                Form_SeePicture form_pic = new Form_SeePicture();
                form_pic.ShowDialog();

            }
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            if (TextBox_ClientName.Text != "")
            {
                sSQL_1 = string.Format("select * from SellingTable where ClientName = '{0}'", TextBox_ClientName.Text);
                
            }
            else if (TextBox_Date.Text != "")
            {
                sSQL_1 = string.Format("select * from SellingTable where Datee = '{0}'", TextBox_Date.Text);
            }
            else if (TextBox_Dealer.Text != "")
            {
                sSQL_1 = string.Format("select * from SellingTable where Dealer = '{0}'", TextBox_Dealer.Text);
            }
            else if (TextBox_SheetID.Text != "")
            {
                sSQL_2 = string.Format("select * from SellingTableInfo where SheetID = '{0}'", TextBox_SheetID.Text);
            }

            if (sSQL_1 != "")
            {
                while (dataGridView1.Rows.Count != 0)
                {
                    dataGridView1.Rows.RemoveAt(0);
                }
                dt = FireBirdSQL.ExeQuery(sSQL_1, CommandType.Text, Form_MainTable.conn, null);
                //Console.WriteLine(dt.Rows[0]["ClienttName"]);
                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "表单ID";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].FillWeight = 568;
                dataGridView1.Columns[1].HeaderText = "客户名称";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].FillWeight = 321;
                dataGridView1.Columns[2].HeaderText = "经手人";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].FillWeight = 169;
                dataGridView1.Columns[3].HeaderText = "日期";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].FillWeight = 293;
                dataGridView1.Columns[4].HeaderText = "合计金额";
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].FillWeight = 214;
                dataGridView1.Columns[5].HeaderText = "制单人";
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].FillWeight = 416;
                dataGridView1.Columns[6].HeaderText = "图片";
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].FillWeight = 169;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    picpath.Add(dt.Rows[0]["Picture"].ToString());
                    dataGridView1.Rows[i].Cells[6].Value = "...";
                }
                //dataGridView1.Columns[6].Visible = false;
                sSQL_1 = "";
            }
            if (sSQL_2 != "")
            {
                while (dataGridView2.Rows.Count != 0)
                {
                    dataGridView2.Rows.RemoveAt(0);
                }
                dt2 = FireBirdSQL.ExeQuery(sSQL_2, CommandType.Text, Form_MainTable.conn, null);
                dataGridView2.DataSource = dt2;
                dataGridView2.Columns[0].HeaderText = "销售单号";
                dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[1].HeaderText = "商品名称";
                dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[2].HeaderText = "数量";
                dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[3].HeaderText = "单价";
                dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[4].HeaderText = "金额";
                dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[5].HeaderText = "单据备注";
                dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                sSQL_2 = "";

            }




            TextBox_ClientName.Text = "";
            TextBox_Date.Text = "";
            TextBox_Dealer.Text = "";
            TextBox_SheetID.Text = "";
        }
    }
}
