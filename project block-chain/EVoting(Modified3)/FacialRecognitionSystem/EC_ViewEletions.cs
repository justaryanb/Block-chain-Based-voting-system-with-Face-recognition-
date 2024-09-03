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
    public partial class EC_ViewEletions : Form
    {
        BaseConnection con = new BaseConnection();
        public EC_ViewEletions()
        {
            InitializeComponent();
            fillgrid();
        }
        public void fillgrid()
        {
            try
            {


                string query1 = "select election,constituency,votingdate,resultsdate,status from elections";

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
