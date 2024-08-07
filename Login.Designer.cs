namespace Atendance_System
{
    partial class Login_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login_form));
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            textBox2 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft YaHei Light", 24F, FontStyle.Italic, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(218, 63, 61);
            label1.Location = new Point(26, 28);
            label1.Name = "label1";
            label1.Size = new Size(174, 62);
            label1.TabIndex = 1;
            label1.Text = "LOGIN";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft PhagsPa", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(218, 63, 61);
            label2.Location = new Point(119, 165);
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.No;
            label2.Size = new Size(150, 61);
            label2.TabIndex = 2;
            label2.Text = "Email:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(343, 183);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Enter your email here";
            textBox1.Size = new Size(342, 33);
            textBox1.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Microsoft PhagsPa", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(218, 63, 61);
            label3.Location = new Point(80, 260);
            label3.Name = "label3";
            label3.Size = new Size(236, 61);
            label3.TabIndex = 4;
            label3.Text = "Password:";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(343, 278);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Enter your password here";
            textBox2.Size = new Size(342, 33);
            textBox2.TabIndex = 5;
            textBox2.UseSystemPasswordChar = true;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(43, 168, 178);
            button1.Font = new Font("Microsoft Himalaya", 22F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.Gainsboro;
            button1.Location = new Point(541, 386);
            button1.Name = "button1";
            button1.Size = new Size(144, 42);
            button1.TabIndex = 6;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Login_form
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Login_form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private Label label3;
        private TextBox textBox2;
        private Button button1;
    }
}