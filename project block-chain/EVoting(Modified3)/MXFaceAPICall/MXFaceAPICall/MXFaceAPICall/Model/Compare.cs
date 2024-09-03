using System;
using System.Collections.Generic;
using System.Text;

namespace MXFaceAPICall.Model
{
    public class Compare
    {
        public float confidence { get; set; }
        public FaceDetect image1_face { get; set; }
        public FaceDetect image2_face { get; set; }
    }
}
