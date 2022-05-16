using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_facialrecognition.Model;
using WPF_facialrecognition.View;

namespace WPF_facialrecognition.ViewModel
{
    public class CommandCenter
    {
        public void AboutButton_Click(object parameter)
        {
            AboutMe wfAbout = new AboutMe();
            wfAbout.ShowDialog();
        }
        public void NewFaceButton_Click(object parameter)
        {
            Recognition reco=new Recognition();
            if (reco.detectedFace == null)
            {
                MessageBox.Show("No face detected.");
                return;
            }
            //Save detected face
            reco.detectedFace = reco.detectedFace.Resize(100, 100, Inter.Cubic);
            reco.detectedFace.Save(Path.FacePhotosPath + "face" + (reco.faceList.Count + 1) + Path.ImageFileExtension);
            StreamWriter writer = new StreamWriter(Path.FaceListTextFile, true);
            string personName = Microsoft.VisualBasic.Interaction.InputBox("Your Name");
            writer.WriteLine(String.Format("face{0}:{1}", (reco.faceList.Count + 1), personName));
            writer.Close();
            reco.GetFacesList();
            MessageBox.Show("Successful.");
        }
        public void OpenVideoFile_Click(object parameter)
        {
            MainWindowModel mainWindowModel = new MainWindowModel();
            Recognition reco = new Recognition();
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog().Value == true)
            {
                mainWindowModel.captureTimer.Stop();
                reco.videoCapture.Dispose();

                reco.videoCapture = new VideoCapture(openDialog.FileName);
                mainWindowModel.captureTimer.Start();
                return;
            }
        }
    }
}
