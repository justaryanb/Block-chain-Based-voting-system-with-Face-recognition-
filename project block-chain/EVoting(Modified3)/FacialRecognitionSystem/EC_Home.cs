using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacialRecognitionSystem
{
    public partial class EC_Home : Form
    {
        public EC_Home()
        {
            InitializeComponent();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            EC_AddElections obj = new EC_AddElections();
            obj.Show();
        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            EC_ViewEletions obj = new EC_ViewEletions();
            obj.Show();
        }

        private void toolStripLabel7_Click(object sender, EventArgs e)
        {
            EC_AddCadidates OBJ = new EC_AddCadidates();
            OBJ.Show();
        }

        private void toolStripLabel11_Click(object sender, EventArgs e)
        {
            EC_ViewCandidates obj = new EC_ViewCandidates();
            obj.Show();
        }

        private void toolStripLabel14_Click(object sender, EventArgs e)
        {
            EC_ViewVoters obj = new EC_ViewVoters();
            obj.Show();
        }

        private void toolStripLabel18_Click(object sender, EventArgs e)
        {
            EC_BlockchainActivities obj = new EC_BlockchainActivities();
            obj.Show();
        }

        private void toolStripLabel20_Click(object sender, EventArgs e)
        {
            EC_Results obj = new EC_Results();
            obj.Show();
        }

        private void toolStripLabel16_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
