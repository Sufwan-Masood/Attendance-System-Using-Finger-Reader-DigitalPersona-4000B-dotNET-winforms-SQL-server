using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atendance_System
{
    public partial class Enroll_Form : Form
    {
        private Att_Enterance parent_Enterance;
        public string storedInDb;
        public Enroll_Form(Att_Enterance parent_Enterance)
        {
            InitializeComponent();
            this.parent_Enterance = parent_Enterance;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            parent_Enterance.reader.Dispose();
            parent_Enterance.Show();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Name is mandatory field");
                textBox1.Focus();
                pictureBox1.Visible = false;

            }
            else
            {
                pictureBox1.Visible = true;
                errorProvider1.Clear();
            }
        }
        public void _displayCapture(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                Console.WriteLine(bitmap.GetPixel(100, 100));
            }
            else
            {
                MessageBox.Show("Empty Bitmap");
            }
        }

        private void Enroll_Form_Load(object sender, EventArgs e)
        {
            timer1.Start();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                pictureBox1.Visible = false;
            }

        }
        public string getName()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Name not given!");

            }
            return (textBox1.Text);
        }
        public void closeForm()
        {
            if (storedInDb == "Stored")
            {
                this.Close();
                Console.WriteLine("Enroll Form is closed");
                parent_Enterance.reader.Dispose();
                parent_Enterance.Show();

            }
            //else
            //{
            //    Console.WriteLine("Enroll Form is not closed");
            //}
        }
        public void closeMessage(string closeMessage)
        {
            storedInDb = closeMessage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            closeForm();
        }
    }
}
