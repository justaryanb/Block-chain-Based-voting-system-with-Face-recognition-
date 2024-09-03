using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacialRecognitionSystem
{
    internal static class Program
    {
        public static string uid = "";
        public static string uname = "";
        //public static string filepath = "E:\\YUVATECH2024\\PROJECT STUDY\\MCA Mohandas\\FaceRecognitionCar\\VehicleMonitoring\\admin\\rimage\\";
        //public static string sfilepath = "E:\\YUVATECH2024\\PROJECT STUDY\\MCA Mohandas\\FaceRecognitionCar\\VehicleMonitoring\\admin\\suspect\\";
        public static string adminPublickey = "Blockchain voting";
        public static string datechecking = "ON";
        public static string voterid = "101";
        public static string eid = "101";
        public static string ekey = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginPage());
        }
    }
}
