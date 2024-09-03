using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacialRecognitionSystem
{
    public partial class LoginForm : Form
    {
        BaseConnection con = new BaseConnection();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string q = "select * from register where username='" + textBox1.Text + "' and pwd='" + textBox2.Text + "'";
            SqlDataReader dr = con.ret_dr(q);
            if (dr.Read())
            {
                Program.uid = dr[0].ToString();
                Program.uname = textBox1.Text;

               

            }
            else
            {
                MessageBox.Show("invalid user");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegisterUser obj = new RegisterUser();
            obj.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
