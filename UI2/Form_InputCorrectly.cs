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
    public partial class Form_InputCorrectly : Form
    {
        public event setTextValue setFormTextValue;
        public Form_InputCorrectly()
        {
            InitializeComponent();
        }
        private void Change(object sender, EventArgs e)
        {
            //Check form = new Check();
            //form.listBox9.Items[form.listBox9.SelectedIndex] = textBox1.Text;
            setFormTextValue(this.textBox1.Text);
            this.Close();
        }
        
    }
    public delegate void setTextValue(string textValue);
}
