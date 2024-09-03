using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV;
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
using Emgu.CV.CvEnum;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using MXFaceAPICall;

namespace FacialRecognitionSystem
{
    public partial class MainRecog : Form
    {
        private double distance = 1E+19;

        private CascadeClassifier CascadeClassifier = new CascadeClassifier(Environment.CurrentDirectory + "/Haarcascade/haarcascade_frontalface_alt.xml");

        private Image<Bgr, byte> Frame = null;

        private Emgu.CV.Capture camera;

        private Mat mat = new Mat();

        private List<Image<Gray, byte>> trainedFaces = new List<Image<Gray, byte>>();

        private List<int> PersonLabs = new List<int>();

        private bool isEnable_SaveImage = false;

        private string ImageName;

        private PictureBox PictureBox_Frame;

        private PictureBox PictureBox_smallFrame;

        private string setPersonName;

        public static bool isTrained = true;
        public static int pcount = 0;
        private bool capture = false;

        private List<string> Names = new List<string>();

        private EigenFaceRecognizer eigenFaceRecognizer;
        BaseConnection con = new BaseConnection();
        public static int pos, neg = 0;
        public static string imgid = "";
        public MainRecog()
        {
            InitializeComponent();
            if (!Directory.Exists(Environment.CurrentDirectory + "\\rimage"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\rimage");
            }
          //  Program.uname = "101";
            CheckForIllegalCrossThreadCalls = false;
        }
        public void getPersonName(Control control)
        {
           
            

        }
        public void openCamera(PictureBox pictureBox_Camera, PictureBox pictureBox_Trained)
        {
            PictureBox_Frame = pictureBox_Camera;
            PictureBox_smallFrame = pictureBox_Trained;
            camera = new Emgu.CV.Capture();
            camera.ImageGrabbed += Camera_ImageGrabbed;
            camera.Start();
        }

        public void Save_IMAGE(string imageName)
        {
            ImageName = imageName;
            isEnable_SaveImage = true;
        }

        private void Camera_ImageGrabbed(object sender, EventArgs e)
        {
            camera.Retrieve(mat);
            Frame = mat.ToImage<Bgr, byte>().Resize(PictureBox_Frame.Width, PictureBox_Frame.Height, Inter.Cubic);
            detectFace();
            textBox2.Text = pcount.ToString();
            PictureBox_Frame.Image = Frame.Bitmap;
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
                    SaveImage(rectangle);
                    image.ROI = rectangle;
                    if (capture == true)
                    {
                        SaveImage2(rectangle);
                    }
                    trainedIamge();
                    checkName(image, rectangle);
                }
            }
           
            else
            {
                setPersonName = "";
            }

            pcount = array.Length;
        }
        private void SaveImage2(Rectangle face)
        {
            string im = "123";

            // string imgpath1 = @"E\suspect\" + imgid + ".jpg";
            Image<Bgr, byte> image = Frame.Convert<Bgr, byte>();
            image.ROI = face;
            image.Resize(100, 100, Inter.Cubic).Save(System.Windows.Forms.Application.StartupPath + "\\rcapture\\" + im + ".jpg");
            string imgpath1 = System.Windows.Forms.Application.StartupPath + "\\rcapture\\" + im + ".jpg";
            pictureBox3.Image = System.Drawing.Image.FromFile(imgpath1);
            capture = false;


        }
        private void SaveImage(Rectangle face)
        {
            if (isEnable_SaveImage)
            {
                Image<Bgr, byte> image = Frame.Convert<Bgr, byte>();
                image.ROI = face;
                image.Resize(100, 100, Inter.Cubic).Save(Environment.CurrentDirectory + "\\rimage\\" + ImageName + ".jpg");
                isEnable_SaveImage = false;
                trainedIamge();
            }
        }
        private void SaveImage1(Rectangle face)
        {
                getimgid();
              // string imgpath1 = @"E\suspect\" + imgid + ".jpg";
                Image<Bgr, byte> image = Frame.Convert<Bgr, byte>();
                image.ROI = face;
            image.Resize(100, 100, Inter.Cubic).Save(Application.StartupPath + "\\suspect\\" + imgid + ".jpg");
            string imgpath1 = Application.StartupPath + "\\suspect\\" + imgid + ".jpg";
            string simgpath= "suspect/"+imgid +".jpg";
            // image.Resize(100, 100, Inter.Cubic).Save(imgpath1);
            string query = "insert into sdata values(" + imgid + ",'" + Program.uname + "','" + DateTime.Now.ToString() + "','"+ simgpath + "',0)";
               con.exec1(query);
            


        }
        public void getimgid()
        {
            try
            {
                string query = "select isnull(max(imgid),200)+1 from sdata";
               
                SqlDataReader dr = con.ret_dr(query);
                if (dr.Read())
                {
                    imgid= dr[0].ToString();
                   

                }
              

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating Jser Id........");
            }
        }
        private void trainedIamge()
        {
            try
            {
                int num = 0;
                trainedFaces.Clear();
                PersonLabs.Clear();
                Names.Clear();
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\rimage", "*.jpg", SearchOption.AllDirectories);
                string[] array = files;
                foreach (string text in array)
                {
                    Image<Gray, byte> item = new Image<Gray, byte>(text);
                    trainedFaces.Add(item);
                    PersonLabs.Add(num);
                    Names.Add(text);
                    num++;
                }

                eigenFaceRecognizer = new EigenFaceRecognizer(num, distance);
                eigenFaceRecognizer.Train(trainedFaces.ToArray(), PersonLabs.ToArray());
            }
            catch
            {
            }
        }

        private void checkName(Image<Bgr, byte> resultImage, Rectangle face)
        {
            try
            {
                if (isTrained)
                {
                    Image<Gray, byte> image = resultImage.Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                    CvInvoke.EqualizeHist(image, image);
                    FaceRecognizer.PredictionResult predictionResult = eigenFaceRecognizer.Predict(image);
                    // label1.Text = predictionResult.Distance.ToString();
                    //MessageBox.Show(predictionResult.Distance.ToString());
                    if (predictionResult.Label != -1 && predictionResult.Distance < distance)
                    {
                        PictureBox_smallFrame.Image = trainedFaces[predictionResult.Label].Bitmap;
                        setPersonName = Names[predictionResult.Label].Replace(Environment.CurrentDirectory + "\\rimage\\", "").Replace(".jpg", "");
                        if (setPersonName == Program.uname)
                        {
                            textBox1.Text = setPersonName;
                            
                            // listBox1.Items.Add("Login Details Matches");
                            //  MessageBox.Show("Mactches Successfully");
                        }
                        else
                        {
                            textBox1.Text = "invalid";

                           
                            //listBox1.Items.Add("Login faild");
                            //  MessageBox.Show("invalid");
                        }
                       // CvInvoke.PutText(Frame, setPersonName, new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.LimeGreen).MCvScalar);
                    }
                    
                    else
                    {
                        CvInvoke.PutText(Frame, "Unknown", new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.OrangeRed).MCvScalar);
                    }
                }
            }
            catch
            {
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
           // openCamera(pictureBox1, pictureBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MainRecog.isTrained = true;
            //getPersonName(lblName);
            string uid = "";
            string query = "select status from fdata";
            SqlDataReader dr = con.ret_dr(query);
            if (dr.Read())
            {
                uid = dr[0].ToString();

            }
            if (uid == "1")
            {
                MessageBox.Show("Matches Successfully");

                if (textBox1.Text == Program.uname)
                {
                    User_Home ob = new User_Home();
                    ActiveForm.Hide();
                    ob.Show();

                }
            }
            else 
            {
                MessageBox.Show("Face Match Failed");
                Application.Exit();
            }
        }

        private void MainRecog_Load(object sender, EventArgs e)
        {
            pictureBox4.Image = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\rimage\\" + Program.uname + ".jpg");
            openCamera(pictureBox1, pictureBox2);
            
          
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            capture = true;
            string subscriptionKey = "SNnMRggxjWI429k3e5-94bxI8106j2472";//your subscription key
            MXFaceAPI mxfaceAPI = new MXFaceAPI("https://faceapi.mxface.ai", subscriptionKey);
            string a = Program.uname + ".jpg";
            string b = "123.jpg";
            var task = mxfaceAPI.Compare(a, b);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(textBox1.Text==Program.uname)
            {
                User_Home ob = new User_Home();
                ActiveForm.Hide();
                ob.Show();

            }
            
        }
    }
}
