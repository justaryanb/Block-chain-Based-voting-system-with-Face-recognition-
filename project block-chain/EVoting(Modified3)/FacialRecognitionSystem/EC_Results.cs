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
    public partial class EC_Results : Form
    {
        BaseConnection con = new BaseConnection();
        public EC_Results()
        {
            InitializeComponent();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string c = "0";
            listView1.Items.Clear();
            imageList1.Images.Clear();
            int count = 0;
            string query = "select name,thumbnail from candidate where eid='" + comboBox1.Text + "'";
            SqlDataReader dr = con.ret_dr(query);
            while (dr.Read())
            {
                Image img = Image.FromFile(Application.StartupPath + dr[1].ToString());
                imageList1.Images.Add(img);
                
                string query1 = "select count(name) from ballet where eid='" + comboBox1.Text + "' and name='"+ dr[0].ToString() + "'";
                SqlDataReader dr1 = con.ret_dr(query1);
                if (dr1.Read())
                {
                    c=dr1[0].ToString();

                }
                ListViewItem lst = new ListViewItem(dr[0].ToString() +"="+ c);
                lst.ImageIndex = count;
                listView1.Items.Add(lst);
                count = count + 1;
            }

            listView1.Refresh();
        }
    }
}
