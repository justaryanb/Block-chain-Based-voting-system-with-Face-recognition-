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

namespace FacialRecognitionSystem
{
    public partial class User_Voting2 : Form
    {
        BaseConnection con = new BaseConnection();
        public static string data = "";
        public User_Voting2()
        {
            InitializeComponent();
            filldata();
        }
        public void filldata()
        {
            string query1 = "select name from voters where voterid='" + Program.voterid + "'";
            SqlDataReader dr = con.ret_dr(query1);
            while (dr.Read())
            {
                data = dr[0].ToString();
                string privatekey = Crypto.EncryptStringAES(data, Program.voterid);
                textBox1.Text = privatekey;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string raw = Crypto.DecryptStringAES(textBox1.Text, Program.voterid);
            if (raw == textBox2.Text)
            {
                MessageBox.Show("Authentication success");
                string query1 = "select * from voters where voterid='" + Program.voterid + "'";
                SqlDataReader dr = con.ret_dr(query1);
                if (dr.Read())
                {
                    pictureBox1.ImageLocation = Application.StartupPath + dr[4].ToString();
                    pictureBox2.ImageLocation = Application.StartupPath + dr[3].ToString();
                    button1.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Authentication failed");
            }
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User_voting3 obj = new User_voting3();
            ActiveForm.Hide();
            obj.Show();
        }
    }
}
