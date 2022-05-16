using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WPF_facialrecognition.Model
{
    public class MainWindowModel
    {
        public string? PersonName { get; set; }
        public Image<Gray, byte>? FaceImage { get; set; }
        public DateTime CreateDate { get; set; }
        public Timer captureTimer { get; set; }

    }
}
