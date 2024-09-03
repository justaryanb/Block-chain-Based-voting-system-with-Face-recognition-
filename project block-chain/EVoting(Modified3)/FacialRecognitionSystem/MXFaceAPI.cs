using MXFaceAPICall.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using FacialRecognitionSystem;
using System.Drawing;

namespace MXFaceAPICall
{
    public class MXFaceAPI
    {
        private string _apiUrl;
        private string _subscripptionKey;
        public static string res;
        BaseConnection con = new BaseConnection();
        public MXFaceAPI(string apiUrl, string subscripptionKey)
        {
            _apiUrl = apiUrl;
            _subscripptionKey = subscripptionKey;
        }

        
        public async Task Detect()
        {
            using (var httpClient = new HttpClient())
            {
                APIRequest request = new APIRequest
                {
                    encoded_image = Convert.ToBase64String(System.IO.File.ReadAllBytes(@"Leonardo.jpg"))
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscripptionKey);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/v3/face/detect", httpContent);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FaceDetectResponse detectFaces = JsonConvert.DeserializeObject<FaceDetectResponse>(apiResponse);
                    foreach (var face in detectFaces.Faces)
                    {
                        Console.WriteLine("face.Quality: {0}, face.Rectangle: {1},{2},{3},{4}", face.quality, face.FaceRectangle.x, face.FaceRectangle.y, face.FaceRectangle.width, face.FaceRectangle.height);
                    }
                }
                else
                {
                    Console.WriteLine("Error {0}, {1}", response.StatusCode, apiResponse);
                }
            }
        }

        public async Task Landmark()
        {
            using (var httpClient = new HttpClient())
            {
                APIRequest request = new APIRequest
                {
                    encoded_image = Convert.ToBase64String(System.IO.File.ReadAllBytes(@"Leonardo.jpg"))
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscripptionKey);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/v3/face/landmark", httpContent);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FaceLandmarkResponse landmarkFaces = JsonConvert.DeserializeObject<FaceLandmarkResponse>(apiResponse);
                    foreach (var face in landmarkFaces.Faces)
                    {
                        Console.WriteLine("face.Quality: {0}, face.Rectangle: {1},{2},{3},{4}", face.quality, face.FaceRectangle.x, face.FaceRectangle.y, face.FaceRectangle.width, face.FaceRectangle.height);
                        var landmarkJson = JsonConvert.SerializeObject(face, Formatting.Indented);
                        Console.WriteLine(landmarkJson);
                    }
                }   
                else
                {
                    Console.WriteLine("Error {0}, {1}", response.StatusCode, apiResponse);
                }
            }
        }
        public async Task Attribute()
        {
            using (var httpClient = new HttpClient())
            {
                APIRequest request = new APIRequest
                {
                    encoded_image = Convert.ToBase64String(System.IO.File.ReadAllBytes(@"Leonardo.jpg"))
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscripptionKey);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/v3/face/analytics", httpContent);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FaceAttributeResponse detectFaces = JsonConvert.DeserializeObject<FaceAttributeResponse>(apiResponse);
                    foreach (var face in detectFaces.Faces)
                    {
                        Console.WriteLine("face.Quality: {0}, face.Rectangle: {1},{2},{3},{4}", face.quality, face.FaceRectangle.x, face.FaceRectangle.y, face.FaceRectangle.width, face.FaceRectangle.height);
                        var analyticsJson = JsonConvert.SerializeObject(face, Formatting.Indented);
                        Console.WriteLine(analyticsJson);
                    }
                }
                else
                {
                    Console.WriteLine("Error {0}, {1}", response.StatusCode, apiResponse);
                }
            }
        }
        public async Task Compare(string a, string b)
        {
           // string res = "";
            using (var httpClient = new HttpClient())
            {
                APICompareRequest request = new APICompareRequest
                {
                    encoded_image1 = Convert.ToBase64String(System.IO.File.ReadAllBytes(System.Windows.Forms.Application.StartupPath+"\\rimage\\"+a)),
                    encoded_image2 = Convert.ToBase64String(System.IO.File.ReadAllBytes(System.Windows.Forms.Application.StartupPath+"\\rcapture\\" + b)),
                    //QualityThreshold =56
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscripptionKey);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/v3/face/verify", httpContent);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MatchedFaceResponse compareFaces = JsonConvert.DeserializeObject<MatchedFaceResponse>(apiResponse);
                    foreach(var item in compareFaces.MatchedFaces)
                    {
                        Console.WriteLine("Confidence of match {0}", item.matchResult);
                        res = item.matchResult.ToString();
                        
                        //  MessageBox.Show(item.matchResult.ToString());
                    }
                    //var compareJson = JsonConvert.SerializeObject(compareFaces, Formatting.Indented);
                    //Console.WriteLine(compareJson);
                }
                else
                {
                    res = "2";
                    Console.WriteLine("Error {0}, {1}", response.StatusCode, apiResponse);
                }
                string str12 = "update fdata set status='" + res + "'";
                con.exec(str12);
            }
           
        }
        public async Task<string> Compare1()
        {
            string stat = "";
            using (var httpClient = new HttpClient())
            {
                APICompareRequest request = new APICompareRequest
                {
                    encoded_image1 = Convert.ToBase64String(System.IO.File.ReadAllBytes("E:\\YUVATECH2024\\PROJECT\\MCA_Mohandas\\FaceRecognitionCar(Modified)\\VehicleMonitoring\\VehicleSecurity\\FacialRecognitionSystem\\bin\\Debug\\rimage\\anoop123.jpg")),
                    encoded_image2 = Convert.ToBase64String(System.IO.File.ReadAllBytes("E:\\YUVATECH2024\\PROJECT\\MCA_Mohandas\\FaceRecognitionCar(Modified)\\VehicleMonitoring\\VehicleSecurity\\FacialRecognitionSystem\\bin\\Debug\\Image1\\AAAAA.jpg")),
                    //QualityThreshold =56
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscripptionKey);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/v3/face/verify", httpContent);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MatchedFaceResponse compareFaces = JsonConvert.DeserializeObject<MatchedFaceResponse>(apiResponse);
                    foreach (var item in compareFaces.MatchedFaces)
                    {
                        Console.WriteLine("Confidence of match {0}", item.matchResult);
                        stat = item.matchResult.ToString();
                    }
                    //var compareJson = JsonConvert.SerializeObject(compareFaces, Formatting.Indented);
                    //Console.WriteLine(compareJson);
                }
                else
                {
                    Console.WriteLine("Error {0}, {1}", response.StatusCode, apiResponse);
                }
            }
            return stat;
        }

        public async Task Quality()
        {
            using (var httpClient = new HttpClient())
            {
                APIRequest request = new APIRequest
                {
                    encoded_image = Convert.ToBase64String(System.IO.File.ReadAllBytes(@"Leonardo.jpg"))
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscripptionKey);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/v3/face/quality", httpContent);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FaceQuality quality = JsonConvert.DeserializeObject<FaceQuality>(apiResponse);
                    Console.WriteLine("face.Quality: {0}", quality.Quality);
                    
                }
                else
                {
                    Console.WriteLine("Error {0}, {1}", response.StatusCode, apiResponse);
                }
            }
        }
    }
}
