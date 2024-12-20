using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models
{
    public class CameraManager : IModel
    {
        private VideoCapture _capture;
        private Mat _frame;
        public CameraManager()
        {
            _capture = new VideoCapture();
            _frame = new Mat();
        }

        public void InitCamera()
        {
            try
            {
                if (!_capture.IsOpened())
                {
                    _capture.Open(1); // 사용자 지정 카메라 사용
                    Debug.WriteLine("Camera Opened");
                    return;
                }
                Debug.WriteLine("Camera Open Failed");
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }

        public void ReleaseCamera()
        {
            if (_capture.IsOpened())
            {
                _capture.Release();
            }
        }

        // 카메라 활성화 여부 확인
        public bool IsOpened()
        {
            return _capture.IsOpened();
        }

        public BitmapSource CaptureFrame()
        {
            // Debug.WriteLine("isOpened = " + _capture.IsOpened());
            if (_capture.IsOpened())
            {
                _capture.Read(_frame);
                return BitmapSourceConverter.ToBitmapSource(_frame);
            }
            return null;
        }
    }
}
