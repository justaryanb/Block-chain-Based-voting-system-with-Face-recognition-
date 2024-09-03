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
    public partial class EC_BlockchainActivities : Form
    {
        public EC_BlockchainActivities()
        {
            InitializeComponent();
            fillgrid();
        }
        BaseConnection con = new BaseConnection();
        public void fillgrid()
        {
            try
            {


                string query1 = "select * from blockchain";

                DataSet ds1 = con.ret_ds(query1);
                dataGridView2.DataSource = ds1.Tables[0].DefaultView;

                string query2 = "select count(voterid) from voters";
                SqlDataReader dr1 = con.ret_dr(query2);
                if (dr1.Read())
                {
                    label3.Text = dr1[0].ToString();
                }

                string query3 = "select count(eid) from elections";
                SqlDataReader dr2 = con.ret_dr(query3);
                if (dr2.Read())
                {
                    label4.Text = dr2[0].ToString();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("exception occured....");
            }
        }
    }
}
