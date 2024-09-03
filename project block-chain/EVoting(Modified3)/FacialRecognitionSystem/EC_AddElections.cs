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
    public partial class EC_AddElections : Form
    {
        BaseConnection con = new BaseConnection();
        public EC_AddElections()
        {
            InitializeComponent();
            electionid();
        }
        public void nodeadd(string id)
        {
            try
            {
                string query = "select isnull(max(nodeid),100)+1 from Blockchain";
                SqlDataReader dr = con.ret_dr(query);
                string nodeid = "";
                if (dr.Read())
                {

                     nodeid = dr[0].ToString();
                    
                }
                string eid = id;
                string type = "Election details added to chain";
                string time = System.DateTime.Now.ToString();
                string activity = "Election  details " + eid + " added to chain";
                string query1 = "insert into Blockchain values(" + nodeid.ToString() + ",'" + type + "','" + activity + "','" + time + "')";
                con.exec(query1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating candidate Id........");
            }
        }
        public void electionid()
        {
            try
            {
                string query = "select isnull(max(eid),100)+1 from elections";
                SqlDataReader dr = con.ret_dr(query);
                if (dr.Read())
                {
                    textBox1.Text = dr[0].ToString();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating election Id........");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string s = "Election declared";
                string query = "insert into elections values(" + textBox1.Text + ",'"+textBox2.Text+"','"+comboBox1.Text+"','"+dateTimePicker1.Text.ToString()+"','"+dateTimePicker2.Text.ToString()+"','"+s+"')";
                if (con.exec1(query) > 0)
                {
                    nodeadd(textBox1.Text);
                    MessageBox.Show("Election details entered ........");
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating patient Id........");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
