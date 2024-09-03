using System;
using System.Threading.Tasks;

namespace MXFaceAPICall
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string subscriptionKey = "SNnMRggxjWI429k3e5-94bxI8106j2472";//your subscription key
            MXFaceAPI mxfaceAPI = new MXFaceAPI("https://faceapi.mxface.ai", subscriptionKey);
            Console.WriteLine("Calling Face Detect API *******************");
            await mxfaceAPI.Detect();
            Console.WriteLine("Calling Face Landmark API *******************");
            await mxfaceAPI.Landmark();
            Console.WriteLine("Calling Face Attribute/Analytics API *******************");
            await mxfaceAPI.Attribute();
            Console.WriteLine("Calling Face Compare API *******************");
            await mxfaceAPI.Compare();
            Console.WriteLine("Calling Face Quality API *******************");
            await mxfaceAPI.Quality();
            Console.ReadLine();
        }
    }
}
