using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using FaceRecognition;
using System.IO;
using Emgu.CV.CvEnum;
using System.Xml.Linq;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System.Data.SqlClient;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace FacialRecognitionSystem
{
    public partial class RegisterUser : Form
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

        public static bool isTrained = false;

        private List<string> Names = new List<string>();

        private EigenFaceRecognizer eigenFaceRecognizer;
        public string uid = "";
        private Size IMAGE_SIZE = new Size(437, 106);
        private const int GENERATE_IMAGE_COUNT = 2;
        private Bitmap[] m_EncryptedImages;
        public static string filename = "";
        public string fname = "";
        private bool capture = false;

        BaseConnection con = new BaseConnection();


        public static string vid = "";
        BaseConnection ob = new BaseConnection();
        public RegisterUser()
        {
            InitializeComponent();
            if (!Directory.Exists(Environment.CurrentDirectory + "\\rimage"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\rimage");
            }
            votersid();
        }
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
        public void votersid()
        {
            try
            {
                string query = "select isnull(max(voterid),100)+1 from voters";
                SqlDataReader dr = con.ret_dr(query);
                if (dr.Read())
                {
                    vid = dr[0].ToString();

                }
                textBox1.Text = vid.ToString();
                string privatekey = Crypto.EncryptStringAES(vid, Program.adminPublickey);
                textBox5.Text = privatekey;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating candidate Id........");
            }
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
                    string type = "User added to chain";
                    string time = System.DateTime.Now.ToString();
                    string activity = "User with " + vid + " added to chain";
                    string query1 = "insert into Blockchain values(" + nodeid.ToString() + ",'" + type + "','" + activity + "','" + time + "')";
                    con.exec(query1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating candidate Id........");
            }
        }
        private Bitmap[] GenerateImage(string inputText)
        {
            Bitmap finalImage = new Bitmap(IMAGE_SIZE.Width, IMAGE_SIZE.Height);
            Bitmap tempImage = new Bitmap(IMAGE_SIZE.Width / 2, IMAGE_SIZE.Height);
            Bitmap[] image = new Bitmap[GENERATE_IMAGE_COUNT];

            Random rand = new Random();
            SolidBrush brush = new SolidBrush(Color.Black);
            //  Brush brush = new Brush(Color.Black);
            Point mid = new Point(IMAGE_SIZE.Width / 2, IMAGE_SIZE.Height / 2);

            Graphics g = Graphics.FromImage(finalImage);
            Graphics gtemp = Graphics.FromImage(tempImage);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font font = new Font("Times New Roman", 64);
            Color fontColor;

            g.DrawString(inputText, font, brush, mid, sf);
            gtemp.DrawImage(finalImage, 0, 0, tempImage.Width, tempImage.Height);


            for (int i = 0; i < image.Length; i++)
            {
                image[i] = new Bitmap(IMAGE_SIZE.Width, IMAGE_SIZE.Height);
            }


            int index = -1;
            int width = tempImage.Width;
            int height = tempImage.Height;
            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    fontColor = tempImage.GetPixel(x, y);
                    index = rand.Next(image.Length);
                    if (fontColor.Name == Color.Empty.Name)
                    {
                        for (int i = 0; i < image.Length; i++)
                        {
                            if (index == 0)
                            {
                                image[i].SetPixel(x * 2, y, Color.Black);
                                image[i].SetPixel(x * 2 + 1, y, Color.Empty);
                            }
                            else
                            {
                                image[i].SetPixel(x * 2, y, Color.Empty);
                                image[i].SetPixel(x * 2 + 1, y, Color.Black);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < image.Length; i++)
                        {
                            if ((index + i) % image.Length == 0)
                            {
                                image[i].SetPixel(x * 2, y, Color.Black);
                                image[i].SetPixel(x * 2 + 1, y, Color.Empty);
                            }
                            else
                            {
                                image[i].SetPixel(x * 2, y, Color.Empty);
                                image[i].SetPixel(x * 2 + 1, y, Color.Black);
                            }
                        }
                    }
                }
            }
            if (Directory.Exists(Application.StartupPath + "\\RegisterShares"))
            {
                int fileCount = Directory.GetFiles(Application.StartupPath + "\\RegisterShares").Length;
                fileCount++;
                image[0].Save(Application.StartupPath + "\\RegisterShares\\" + vid + ".png");
                image[1].Save("C:\\data\\" + vid + ".png");
                // filename = fileCount.ToString() + ".png";

            }
            //SaveFileDialog save = new SaveFileDialog();
            //if (save.ShowDialog() == DialogResult.OK)
            //{
            //    image[1].Save("D:\\" + name.Text + ".png");

            //}
            brush.Dispose();
            tempImage.Dispose();
            finalImage.Dispose();

            return image;
        }
        public void getPersonName(Control control)
        {
            Timer timer = new Timer();
            timer.Tick += timer_getPersonName_Tick;
            timer.Interval = 100;
            timer.Start();
            void timer_getPersonName_Tick(object sender, EventArgs e)
            {
                control.Text = setPersonName;

            }
        }
        public void openCamera(PictureBox pictureBox_Camera)
        {
            PictureBox_Frame = pictureBox_Camera;
           // PictureBox_smallFrame = pictureBox_Trained;
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
                    if (isEnable_SaveImage == true)
                    {
                       // SaveImage2(rectangle);
                        SaveImage(rectangle);
                    }
                   
                    image.ROI = rectangle;

                    trainedIamge();
                    checkName(image, rectangle);
                }
            }
            else
            {
                setPersonName = "";
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
                        CvInvoke.PutText(Frame, setPersonName, new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.LimeGreen).MCvScalar);
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

        private void button4_Click(object sender, EventArgs e)
        {
            openCamera(pictureBox1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void RegisterUser_Load(object sender, EventArgs e)
        {
            string Rand1 = RandomString(6);
            textBox7.Text = Rand1.ToString();
            nodeadd();

        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        private void SaveImage2(Rectangle face)
        {
            string im = "123";

            // string imgpath1 = @"E\suspect\" + imgid + ".jpg";
            Image<Bgr, byte> image = Frame.Convert<Bgr, byte>();
            image.ROI = face;
            image.Resize(100, 100, Inter.Cubic).Save(System.Windows.Forms.Application.StartupPath + "\\rimage\\" + im + ".jpg");
            


        }
        private void button2_Click(object sender, EventArgs e)
        {
         

            if (textBox2.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Name");
            }
            else if (textBox3.Text == String.Empty)
            {
                MessageBox.Show("Please Upload ID Proof");
            }
            else if (textBox6.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Aaadhaar Number");

            }
            else if (textBox4.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Address");

            }
            else
            {
                Save_IMAGE(textBox1.Text);

                if (m_EncryptedImages != null)
                {
                    for (int i = m_EncryptedImages.Length - 1; i > 0; i--)
                    {
                        m_EncryptedImages[i].Dispose();
                    }
                    Array.Clear(m_EncryptedImages, 0, m_EncryptedImages.Length);
                }
             
                m_EncryptedImages = GenerateImage(textBox7.Text);

                panelCanvas.Invalidate();
               

                //----------        Getting the Image File
                System.Drawing.Image img = System.Drawing.Image.FromFile(textBox3.Text);

                //----------        Getting Size of Original Image
                double imgHeight = img.Size.Height;
                double imgWidth = img.Size.Width;

                //----------        Getting Decreased Size
                double x = imgWidth / 200;
                int newWidth = Convert.ToInt32(imgWidth / x);
                int newHeight = Convert.ToInt32(imgHeight / x);

                //----------        Creating Small Image
                System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                System.Drawing.Image myThumbnail = img.GetThumbnailImage(newWidth, newHeight, myCallback, IntPtr.Zero);

               
                System.Drawing.Image mylogo = Image.FromFile(textBox3.Text);
                mylogo.Save(Application.StartupPath + "\\idcard\\" + vid.ToString() + ".jpg");

                string thumbpath = "\\photo\\" + vid.ToString() + ".jpeg";
                string logopath = "\\idcard\\" + vid.ToString() + ".jpg";



                string folderpath = Application.StartupPath + "\\EC\\block chain\\users\\";
                if (!File.Exists(folderpath + vid + ".txt"))
                {
                    StreamWriter strm = File.CreateText(folderpath + vid + ".txt");
                    strm.Flush();
                    strm.Close();
                }
                if (File.Exists(folderpath + vid + ".txt"))
                {
                    using (StreamWriter sw = File.AppendText(folderpath + vid + ".txt"))
                    {
                        sw.WriteLine(textBox5.Text);

                    }


                }
                string activityfolderpath = Application.StartupPath + "\\EC\\block chain\\activities\\";
                if (File.Exists(activityfolderpath + "blockchain.txt"))
                {
                    using (StreamWriter sw = File.AppendText(activityfolderpath + "blockchain.txt"))
                    {
                        string msg = "User with id xxx has joined our chain";
                        sw.WriteLine(msg);
                        nodeadd();
                    }


                }


                string query = "insert into voters values(" + vid.ToString() + ",'" + textBox2.Text + "','" + comboBox1.Text + "','" + logopath + "','" + thumbpath + "','" + textBox4.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + textBox8.Text + "')";
                if (con.exec1(query) > 0)
                {
                    MessageBox.Show("Voter's details entered ........");
                    MessageBox.Show("Your Username will be " + vid.ToString() + " and password will be your Aadhaar Number");
                    

                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,


            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;
                pictureBox2.ImageLocation = textBox3.Text;
            }
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (m_EncryptedImages != null)
            {
                Graphics g = e.Graphics;
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                for (int i = 0; i < m_EncryptedImages.Length; i++)
                {
                    rect.Size = m_EncryptedImages[i].Size;
                    g.DrawImage(m_EncryptedImages[i], rect);
                    rect.Y += m_EncryptedImages[i].Height + 5;
                }

                g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), rect.Location, new Point(rect.Width, rect.Y));
                rect.Y += 5;

               
            }
        }

        private void panelCanvas_Click(object sender, EventArgs e)
        {

        }
    }
}
