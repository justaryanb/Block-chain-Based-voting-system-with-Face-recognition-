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
    public partial class LoginPage : Form
    {
        BaseConnection con = new BaseConnection();
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }

        }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "pwd")
            {
                EC_Home obj = new EC_Home();
                ActiveForm.Hide();
                obj.Show();
            }
            else
            {
                string q = "select * from voters where voterid='" + textBox1.Text + "' and aadhaar='" + textBox2.Text + "'";
                SqlDataReader dr = con.ret_dr(q);
                if (dr.Read())
                {
                    UserAuthentication ob = new UserAuthentication();
                    //User_Home ob = new User_Home();
                    Program.voterid = textBox1.Text;
                    Program.uname= textBox1.Text;
                    Program.ekey = dr[7].ToString();
                    ActiveForm.Hide();
                    ob.Show();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegisterUser obj = new RegisterUser();
            obj.Show();
        }
    }
}
