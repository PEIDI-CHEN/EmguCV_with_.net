using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_facialrecognition.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        #region FaceName
        private string faceName;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public string FaceName
        {
            get { return faceName; }
            set
            {
                faceName = value.ToUpper();
                RaisePropertyChange("FaceName");
                OnPropertyChanged();
            }
        }
        #endregion
        #region CameraCaptureImage
        private Bitmap cameraCapture;
        public Bitmap CameraCapture
        {
            get { return cameraCapture; }
            set
            {
                cameraCapture = value;
                RaisePropertyChange("CameraCapture");
                OnPropertyChanged();
            }
        }
        #endregion
        #region CameraCaptureFaceImage
        private Bitmap cameraCaptureFace;
        public Bitmap CameraCaptureFace
        {
            get { return cameraCaptureFace; }
            set
            {
                cameraCaptureFace = value;
                RaisePropertyChange("CameraCaptureFace");
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion
        #region command
        bool isCanExec = true;
        LoadedWindowCommand command1 = new LoadedWindowCommand();
        CommandCenter center = new CommandCenter();
        public ICommand LoadedWindowCommand => new MyCommand(command1.LoadedWindowCommand1, MyCanExec);
        public ICommand AboutButton_Click => new MyCommand(center.AboutButton_Click, MyCanExec);
        public ICommand NewFaceButton_Click => new MyCommand(center.NewFaceButton_Click, MyCanExec);
        public ICommand OpenVideoFile_Click => new MyCommand(center.OpenVideoFile_Click, MyCanExec);

        private bool MyCanExec(object parameter)
        {
            return isCanExec;
        }
        #endregion
    }
}
