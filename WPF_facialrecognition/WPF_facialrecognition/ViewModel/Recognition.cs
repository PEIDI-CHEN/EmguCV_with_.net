using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_facialrecognition.Model;

namespace WPF_facialrecognition.ViewModel
{
    public class Recognition
    {
        public CascadeClassifier? haarCascade;
        public List<MainWindowModel> faceList = new List<MainWindowModel>();
        public VectorOfMat imageList = new VectorOfMat();
        public List<string> nameList = new List<string>();
        public VectorOfInt labelList = new VectorOfInt();
        public EigenFaceRecognizer recognizer;
        public VideoCapture videoCapture;
        public Image<Bgr, Byte> bgrFrame = null;
        public Image<Gray, Byte> detectedFace = null;
        #region Method
        public void GetFacesList()
        {
            //haar cascade classifier
            if (!File.Exists(Path.HaarCascadePath))
            {
                string text = "Cannot find Haar cascade data file:\n\n";
                text += Path.HaarCascadePath;
                MessageBoxResult result = MessageBox.Show(text, "Error",
                       MessageBoxButton.OK, MessageBoxImage.Error);
            }

            haarCascade = new CascadeClassifier(Path.HaarCascadePath);
            faceList.Clear();
            string line;
            MainWindowModel faceInstance = null;

            // Create empty directory / file for face data if it doesn't exist
            if (!Directory.Exists(Path.FacePhotosPath))
            {
                Directory.CreateDirectory(Path.FacePhotosPath);
            }

            if (!File.Exists(Path.FaceListTextFile))
            {
                string text = "Cannot find face data file:\n\n";
                text += Path.FaceListTextFile + "\n\n";
                text += "If this is your first time running the app, an empty file will be created for you.";
                MessageBoxResult result = MessageBox.Show(text, "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        String dirName = System.IO.Path.GetDirectoryName(Path.FaceListTextFile);
                        Directory.CreateDirectory(dirName);
                        File.Create(Path.FaceListTextFile).Close();
                        break;
                }
            }

            StreamReader reader = new StreamReader(Path.FaceListTextFile);
            int i = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] lineParts = line.Split(':');
                faceInstance = new MainWindowModel();
                faceInstance.FaceImage = new Image<Gray, byte>(Path.FacePhotosPath + lineParts[0] + Path.ImageFileExtension);
                faceInstance.PersonName = lineParts[1];
                faceList.Add(faceInstance);
            }
            foreach (var face in faceList)
            {
                imageList.Push(face.FaceImage.Mat);
                nameList.Add(face.PersonName);
                labelList.Push(new[] { i++ });
            }
            reader.Close();

            // Train recogniser
            if (imageList.Size > 0)
            {
                recognizer = new EigenFaceRecognizer(imageList.Size);
                recognizer.Train(imageList, labelList);
            }

        }
        public void ProcessFrame()
        {
            bgrFrame = videoCapture.QueryFrame().ToImage<Bgr, Byte>();

            if (bgrFrame != null)
            {
                try
                {//for emgu cv bug
                    Image<Gray, byte> grayframe = bgrFrame.Convert<Gray, byte>();

                    Rectangle[] faces = haarCascade.DetectMultiScale(grayframe, 1.2, 10, new System.Drawing.Size(50, 50), new System.Drawing.Size(200, 200));

                    //detect face
                    MainWindowViewModel viewModel = new MainWindowViewModel();
                    viewModel.FaceName = "No face detected";
                    foreach (var face in faces)
                    {
                        bgrFrame.Draw(face, new Bgr(255, 255, 0), 2);
                        detectedFace = bgrFrame.Copy(face).Convert<Gray, byte>();
                        FaceRecognition();
                        break;
                    }
                    viewModel.CameraCapture = bgrFrame.ToBitmap();
                }
                catch (Exception ex)
                {

                    //todo log
                }

            }
        }
        private void FaceRecognition()
        {
            if (imageList.Size != 0)
            {
                MainWindowViewModel viewModel = new MainWindowViewModel();
                //Eigen Face Algorithm
                FaceRecognizer.PredictionResult result = recognizer.Predict(detectedFace.Resize(100, 100, Inter.Cubic));
                viewModel.FaceName = nameList[result.Label];
                viewModel.CameraCaptureFace = detectedFace.ToBitmap();
            }
            else
            {
                MainWindowViewModel viewModel = new MainWindowViewModel();
                viewModel.FaceName = "Please Add Face";
            }
        }
        /// <summary>
        /// Convert bitmap to bitmap image for image control
        /// </summary>
        /// <param name="bitmap">Bitmap image</param>
        /// <returns>Image Source</returns>
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        #endregion
    }
}
