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
    public partial class User_ViewResults : Form
    {
        BaseConnection con = new BaseConnection();
        public static string constituency = "";
        public User_ViewResults()
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
            fillgrid();
        }
        public void fillgrid()
        {
            try
            {


                string query1 = "select * from ballet where voterid='"+Program.voterid+"' and eid='"+comboBox1.Text+"'";

                DataSet ds1 = con.ret_ds(query1);
                dataGridView2.DataSource = ds1.Tables[0].DefaultView;




            }
            catch (Exception ex)
            {
                MessageBox.Show("exception occured....");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
