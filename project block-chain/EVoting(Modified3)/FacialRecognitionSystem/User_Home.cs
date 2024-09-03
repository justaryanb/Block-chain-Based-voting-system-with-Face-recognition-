using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using AForge.Video;
//using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FacialRecognitionSystem
{
    public partial class User_Home : Form
    {

        private double distance = 1E+19;

        private CascadeClassifier CascadeClassifier = new CascadeClassifier(Environment.CurrentDirectory + "/Haarcascade/haarcascade_frontalface_alt.xml");

        private Image<Bgr, byte> Frame = null;

        private Capture camera;

        private Mat mat = new Mat();

        private List<Image<Gray, byte>> trainedFaces = new List<Image<Gray, byte>>();

        private List<int> PersonLabs = new List<int>();

        private bool isEnable_SaveImage = false;

        private string ImageName;

        private PictureBox PictureBox_Frame;

        private PictureBox PictureBox_smallFrame;

        private string setPersonName;

        public static bool isTrained = false;
        DateTime start;

        private List<string> Names = new List<string>();

        private EigenFaceRecognizer eigenFaceRecognizer;
        public static int pcount = 0;
        public User_Home()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            User_Voting1 obj = new User_Voting1();
         
            obj.Show();
        }

        private void toolStripLabel9_Click(object sender, EventArgs e)
        {
            User_ViewResults obj = new User_ViewResults();
            obj.Show();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripLabel10_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripLabel12_Click(object sender, EventArgs e)
        {
            User_ViewBlockchain obj = new User_ViewBlockchain();
            obj.Show();
        }
        public void openCamera(PictureBox pictureBox_Camera)
        {
            PictureBox_Frame = pictureBox_Camera;
            // PictureBox_smallFrame = pictureBox_Trained;
            camera = new Capture();
            camera.ImageGrabbed += Camera_ImageGrabbed;
            camera.Start();
        }

        private void User_Home_Load(object sender, EventArgs e)
        {
            start = DateTime.UtcNow;
            openCamera(pictureBoxCamara);
        }
        private void Camera_ImageGrabbed(object sender, EventArgs e)
        {
            camera.Retrieve(mat);
            Frame = mat.ToImage<Bgr, byte>().Resize(PictureBox_Frame.Width, PictureBox_Frame.Height, Inter.Cubic);
            detectFace();
            PictureBox_Frame.Image = Frame.Bitmap;
            textBox1.Text = pcount.ToString();
            if(pcount>=2)
            {
                Application.Exit();
            }
            int tcount = Convert.ToInt32(label1.Text);
            if (tcount >= 60)
            {
                Application.Exit();
            }

        }
        private void detectFace()
        {
            Image<Bgr, byte> image = Frame.Convert<Bgr, byte>();
            Mat mat = new Mat();
            CvInvoke.CvtColor(Frame, mat, ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(mat, mat);
            Rectangle[] array = CascadeClassifier.DetectMultiScale(mat, 1.1, 4);
            if (array.Length != 0)
            {
                Rectangle[] array2 = array;
                foreach (Rectangle rectangle in array2)
                {
                    CvInvoke.Rectangle(Frame, rectangle, new Bgr(Color.LimeGreen).MCvScalar, 2);
                    //SaveImage(rectangle);
                    image.ROI = rectangle;
                  //  trainedIamge();
                   // checkName(image, rectangle);
                }
            }
            else
            {
                setPersonName = "";
            }
            pcount = array.Length;
        }

        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime end = DateTime.UtcNow;
            TimeSpan timeDiff = end - start;
            label1.Text = Math.Round(timeDiff.TotalSeconds).ToString();
        }

        
    }
}
