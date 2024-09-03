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
    public partial class EC_ViewVoters : Form
    {
        BaseConnection con = new BaseConnection();

        public EC_ViewVoters()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();
            int count = 0;
            string query = "select name,photo from voters where constituency='" + comboBox1.Text + "'";
            SqlDataReader dr = con.ret_dr(query);
            while (dr.Read())
            {
                Image img = Image.FromFile(Application.StartupPath + dr[1].ToString());
                imageList1.Images.Add(img);
                ListViewItem lst = new ListViewItem(Path.GetFileName(dr[0].ToString()));
                lst.ImageIndex = count;
                listView1.Items.Add(lst);
                count = count + 1;
            }

            listView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
