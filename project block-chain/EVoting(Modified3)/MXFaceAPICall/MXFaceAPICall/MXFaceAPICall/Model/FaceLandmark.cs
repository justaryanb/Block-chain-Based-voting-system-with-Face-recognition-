using System;
using System.Collections.Generic;
using System.Text;

namespace MXFaceAPICall.Model
{
    public class FaceLandmark
    {
        public Location MouthLeft { get; set; }
        public Location MouthRight { get; set; }
        public Location Nose { get; set; }
        public Location eye_left { get; set; }
        public Location eye_right { get; set; }
    }
}
