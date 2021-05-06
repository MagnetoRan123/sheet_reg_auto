
namespace UI2
{
    partial class Form_ConfigTable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RBtn_Path = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_StartX = new System.Windows.Forms.Label();
            this.label_StartY = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_width = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_length = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.folder_path = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RBtn_Path
            // 
            this.RBtn_Path.AutoSize = true;
            this.RBtn_Path.Location = new System.Drawing.Point(34, 24);
            this.RBtn_Path.Name = "RBtn_Path";
            this.RBtn_Path.Size = new System.Drawing.Size(88, 19);
            this.RBtn_Path.TabIndex = 0;
            this.RBtn_Path.TabStop = true;
            this.RBtn_Path.Text = "选择路径";
            this.RBtn_Path.UseVisualStyleBackColor = true;
            this.RBtn_Path.Click += new System.EventHandler(this.RBtn_Path_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(34, 63);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(97, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "摄像头...";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RBtn_Path);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(45, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(155, 102);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图片源";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(45, 139);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(119, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "查看识别过程";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(270, 133);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(805, 437);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(267, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "X:";
            // 
            // label_StartX
            // 
            this.label_StartX.AutoSize = true;
            this.label_StartX.Location = new System.Drawing.Point(296, 115);
            this.label_StartX.Name = "label_StartX";
            this.label_StartX.Size = new System.Drawing.Size(15, 15);
            this.label_StartX.TabIndex = 6;
            this.label_StartX.Text = " ";
            // 
            // label_StartY
            // 
            this.label_StartY.AutoSize = true;
            this.label_StartY.Location = new System.Drawing.Point(362, 115);
            this.label_StartY.Name = "label_StartY";
            this.label_StartY.Size = new System.Drawing.Size(15, 15);
            this.label_StartY.TabIndex = 8;
            this.label_StartY.Text = " ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(333, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Y:";
            // 
            // label_width
            // 
            this.label_width.AutoSize = true;
            this.label_width.Location = new System.Drawing.Point(636, 115);
            this.label_width.Name = "label_width";
            this.label_width.Size = new System.Drawing.Size(15, 15);
            this.label_width.TabIndex = 12;
            this.label_width.Text = " ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(607, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "宽:";
            // 
            // label_length
            // 
            this.label_length.AutoSize = true;
            this.label_length.Location = new System.Drawing.Point(540, 115);
            this.label_length.Name = "label_length";
            this.label_length.Size = new System.Drawing.Size(15, 15);
            this.label_length.TabIndex = 10;
            this.label_length.Text = " ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(511, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "长:";
            // 
            // folder_path
            // 
            this.folder_path.Location = new System.Drawing.Point(270, 49);
            this.folder_path.Name = "folder_path";
            this.folder_path.Size = new System.Drawing.Size(658, 25);
            this.folder_path.TabIndex = 67;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(934, 44);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(92, 30);
            this.button4.TabIndex = 66;
            this.button4.Text = ". . .";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form_ConfigTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 582);
            this.Controls.Add(this.folder_path);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label_width);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_length);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label_StartY);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_StartX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_ConfigTable";
            this.Text = "设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton RBtn_Path;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_StartX;
        private System.Windows.Forms.Label label_StartY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_width;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_length;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox folder_path;
        private System.Windows.Forms.Button button4;
    }
}