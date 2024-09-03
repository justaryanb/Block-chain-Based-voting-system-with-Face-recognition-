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
    public partial class User_voting3 : Form
    {
        BaseConnection con = new BaseConnection();
        public User_voting3()
        {
            InitializeComponent();
            filldetails();
        }
        public void filldetails()
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();
            int count = 0;
            string query = "select name,thumbnail from candidate where eid='" + Program.eid + "'";
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
        public void nodeadd()
        {
            try
            {
                string query = "select isnull(max(nodeid),100)+1 from Blockchain";
                SqlDataReader dr = con.ret_dr(query);
                if (dr.Read())
                {
                    string nodeid = dr[0].ToString();
                    string type = "User has voted";
                    string time = System.DateTime.Now.ToString();
                    string activity = "User with id " + Program.voterid + " added to chain";
                    string query1 = "insert into Blockchain values(" + nodeid.ToString() + ",'" + type + "','" + activity + "','" + time + "')";
                    con.exec(query1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("");
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0].Text.ToString();
                MessageBox.Show("You have voted for " + item.ToString());
                string query = "select * from ballet where eid='" + Program.eid + "' and voterid='" + Program.voterid + "'";
                SqlDataReader dr = con.ret_dr(query);
                if (dr.Read())
                {
                    string query1 = "update ballet set name='" + item.ToString() + "', Time='"+DateTime.Now.ToShortDateString()+"' where eid='" + Program.eid + "' and voterid='" + Program.voterid + "' ";
                    con.exec(query1);
                    string activityfolderpath = Application.StartupPath + "\\EC\\block chain\\activities\\";
                    if (File.Exists(activityfolderpath + "blockchain.txt"))
                    {
                        using (StreamWriter sw = File.AppendText(activityfolderpath + "blockchain.txt"))
                        {
                            string msg = "User with id " + Program.voterid + " has changed their vote";
                            sw.WriteLine(msg);
                            nodeadd();
                        }


                    }
                }
                else
                {
                    string query1 = "insert into ballet values('" + Program.eid + "','" + item.ToString() + "','" + Program.voterid + "','" + System.DateTime.Now.ToShortDateString() + "')";
                    con.exec(query1);
                    string activityfolderpath = Application.StartupPath + "\\EC\\block chain\\activities\\";
                    if (File.Exists(activityfolderpath + "blockchain.txt"))
                    {
                        using (StreamWriter sw = File.AppendText(activityfolderpath + "blockchain.txt"))
                        {
                            string msg = "User with id "+Program.voterid+" has voted";
                            sw.WriteLine(msg);
                            nodeadd();
                        }


                    }
                }
                this.Close();
            }
        }
    }
}
