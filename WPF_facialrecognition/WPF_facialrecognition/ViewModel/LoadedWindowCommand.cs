using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using WPF_facialrecognition.Model;

namespace WPF_facialrecognition.ViewModel
{
    public class LoadedWindowCommand
    {
        public VideoCapture videoCapture;
        public void LoadedWindowCommand1(object parameter)
        {
            MainWindowModel model1 = new MainWindowModel();
            model1.captureTimer = new Timer()
            {
                Interval = Path.TimerResponseValue

            };
            model1.captureTimer.Elapsed += CaptureTimer_Elapsed;
            Recognition reco = new Recognition();
            reco.GetFacesList();
            videoCapture = new VideoCapture(Path.ActiveCameraIndex);
            model1.captureTimer.Start();
        }
        private void CaptureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Recognition reco = new Recognition();
            reco.ProcessFrame();
        }
    }
}
