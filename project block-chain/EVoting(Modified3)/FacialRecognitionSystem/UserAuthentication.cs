using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FacialRecognitionSystem
{
    public partial class UserAuthentication : Form
    {
        BaseConnection con = new BaseConnection();
        public static string upath = "";
        public static string apath = "";
        public static string dpath = "";
        Bitmap share1;
        Bitmap share2;
        Bitmap result;
        public UserAuthentication()
        {
            InitializeComponent();
        }
        public UserAuthentication(string path)
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                sharepath.Text = openFileDialog1.FileName;
            }
            upath = sharepath.Text;
            pictureBox1.ImageLocation = upath;
            share1 = (Bitmap)Image.FromFile(upath);
        }
        public void reconstruct()
        {

            result = new Bitmap(share1.Width, share1.Height);
            for (int x = 0; x < result.Width - 1; x += 1)
            {
                for (int y = 0; y < result.Height; y += 1)
                {
                    Color c1 = share1.GetPixel(x, y);
                    Color c2 = share2.GetPixel(x, y);

                    if (c1.ToArgb() != Color.Empty.ToArgb() && c2.ToArgb() == Color.Empty.ToArgb())
                    {
                        result.SetPixel(x, y, c1);
                    }
                    else if (c1.ToArgb() == Color.Empty.ToArgb() && c2.ToArgb() != Color.Empty.ToArgb())
                    {
                        result.SetPixel(x, y, c2);
                    }
                    pictureBox3.Image = (Image)result;

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            apath = Application.StartupPath + "\\RegisterShares\\" + Program.voterid + ".png";
            pictureBox2.ImageLocation = Application.StartupPath + "\\RegisterShares\\" + Program.voterid + ".png";
            share2 = (Bitmap)Image.FromFile(Application.StartupPath + "\\RegisterShares\\" + Program.voterid + ".png");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            reconstruct();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == Program.ekey)
            {
                MainRecog obj = new MainRecog();
                ActiveForm.Hide();
                obj.Show();
            }
            else
            {
                MessageBox.Show("Key Not Matching");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
