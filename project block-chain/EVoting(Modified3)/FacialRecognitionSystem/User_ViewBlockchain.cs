using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FacialRecognitionSystem
{
    public partial class User_ViewBlockchain : Form
    {
        public User_ViewBlockchain()
        {
            InitializeComponent();
            readfile();
        }
        public void readfile()
        {
            string path = Application.StartupPath + "\\EC\\block chain\\activities\\blockchain.txt";
            string readText = File.ReadAllText(path);
            richTextBox1.Text = readText;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
