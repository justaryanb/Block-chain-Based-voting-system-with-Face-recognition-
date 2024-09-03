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
    public partial class EC_ViewCandidates : Form
    {
        BaseConnection con = new BaseConnection();
        public EC_ViewCandidates()
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
            listView1.Items.Clear();
            imageList1.Images.Clear();
            int count = 0;
            string query = "select name,thumbnail from candidate where eid='" + comboBox1.Text + "'";
            SqlDataReader dr = con.ret_dr(query);
            while(dr.Read())
            {
                Image img = Image.FromFile(Application.StartupPath+dr[1].ToString());
                imageList1.Images.Add(img);
                ListViewItem lst = new ListViewItem(Path.GetFileName(dr[0].ToString()));
                lst.ImageIndex = count;
                listView1.Items.Add(lst);
                count = count + 1;
            }
           
            listView1.Refresh();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0].ToString();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0].ToString();
            }
        }
    }
}
