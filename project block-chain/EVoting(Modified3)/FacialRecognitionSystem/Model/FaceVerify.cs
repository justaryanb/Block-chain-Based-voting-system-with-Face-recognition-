using System;
using System.Collections.Generic;
using System.Text;

namespace MXFaceAPICall.Model
{
    public class FaceVerify
    {
        public short matchResult { get; set; }
        public FaceDetect image1_face { get; set; }
        public FaceDetect image2_face { get; set; }
    }
}
