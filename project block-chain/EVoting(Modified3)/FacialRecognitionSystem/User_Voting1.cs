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
using System.IO;


namespace FacialRecognitionSystem
{
    public partial class User_Voting1 : Form
    {

        BaseConnection con = new BaseConnection();
        public static string constituency = "";
        public User_Voting1()
        {
            InitializeComponent();

            fillcombo();

        }
        public void findconstituency()
        {
            string query1 = "select constituency from voters where voterid='" + Program.voterid + "'";
            SqlDataReader dr = con.ret_dr(query1);
            while (dr.Read())
            {
                constituency = dr[0].ToString();

            }
        }
        public void fillcombo()
        {
            findconstituency();
            comboBox1.Items.Clear();
            string query1 = "select eid from elections where status='Election declared' and constituency='" + constituency + "'";
            SqlDataReader dr = con.ret_dr(query1);
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());

            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.eid = comboBox1.Text;
            if (Program.datechecking == "ON")
            {
                string presentdate = "";
                string query1 = "select votingdate from elections where eid='" + comboBox1.SelectedItem.ToString() + "'";
                SqlDataReader dr = con.ret_dr(query1);
                if (dr.Read())
                {
                    presentdate = dr[0].ToString();

                }
                if (Convert.ToDateTime(presentdate) == Convert.ToDateTime(System.DateTime.Now.ToShortDateString()))
                {
                    string query2 = "select Time from Ballet where voterid='" + Program.voterid + "'";
                    SqlDataReader dr1 = con.ret_dr(query2);
                    if (dr1.Read())
                    {
                        if (Convert.ToDateTime(dr1[0].ToString()) == Convert.ToDateTime(System.DateTime.Now.ToShortDateString()))
                        {

                            MessageBox.Show("Already Voted");

                        }
                        else
                        {

                            User_Voting2 obj = new User_Voting2();
                            ActiveForm.Hide();
                            obj.Show();
                        }
                    }
                    else
                    {
                        User_Voting2 obj = new User_Voting2();
                        ActiveForm.Hide();
                        obj.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Date Not Matching");

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        
    }
}
