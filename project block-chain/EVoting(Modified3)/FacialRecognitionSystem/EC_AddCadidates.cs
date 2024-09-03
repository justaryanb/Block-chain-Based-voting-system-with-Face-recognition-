using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace FacialRecognitionSystem
{
    public partial class EC_AddCadidates : Form
    {
        float imageResolution = 72;
        long compressionLevel = 80L;
        BaseConnection con = new BaseConnection();
        public static string cid = "";
        public EC_AddCadidates()
        {
            InitializeComponent();
            candidateid();
            fillcombo();
        }
        public void fillcombo()
        {
            comboBox1.Items.Clear();
            string query1 = "select eid from elections where status='Election declared'";
            SqlDataReader dr = con.ret_dr(query1);
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());

            }


        }
        public void candidateid()
        {
            try
            {
                string query = "select isnull(max(cid),100)+1 from candidate";
                SqlDataReader dr = con.ret_dr(query);
                if (dr.Read())
                {
                    cid = dr[0].ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating candidate Id........");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Logo Files",

                CheckFileExists = true,
                CheckPathExists = true,

              
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                pictureBox1.ImageLocation = textBox1.Text;
            }
        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //----------        Getting the Image File
            System.Drawing.Image img = System.Drawing.Image.FromFile(textBox1.Text);

            //----------        Getting Size of Original Image
            double imgHeight = img.Size.Height;
            double imgWidth = img.Size.Width;

            //----------        Getting Decreased Size
            double x = imgWidth / 200;
            int newWidth = Convert.ToInt32(imgWidth / x);
            int newHeight = Convert.ToInt32(imgHeight / x);

            //----------        Creating Small Image
            System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            System.Drawing.Image myThumbnail = img.GetThumbnailImage(newWidth, newHeight, myCallback, IntPtr.Zero);

            //----------        Saving Image
            myThumbnail.Save(Application.StartupPath+"\\thumb\\"+cid.ToString()+".jpeg");
            System.Drawing.Image mylogo = Image.FromFile(textBox1.Text);
            mylogo.Save(Application.StartupPath + "\\logo\\" + cid.ToString() + ".jpeg");

            string thumbpath = "\\thumb\\" + cid.ToString() + ".jpeg";
            string logopath = "\\logo\\" + cid.ToString() + ".jpeg";


            string query = "insert into candidate values(" + comboBox1.Text + "," + cid.ToString() + ",'" + textBox2.Text + "','" + logopath + "','" + thumbpath + "')";
            if (con.exec1(query) > 0)
            {
                MessageBox.Show("Candidate details entered ........");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
