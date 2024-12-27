using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using OpenCvSharp;
using OpenCvSharp.Internal;
using OpenCvSharp.WpfExtensions;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models
{
    public class CameraManager : IModel
    {
        //{
        //    private VideoCapture _capture;
        //    private Mat _frame;
        //    public CameraManager()
        //    {
        //        _capture = new VideoCapture();
        //        _frame = new Mat();
        //    }

        //    public void InitCamera()
        //    {
        //        try
        //        {
        //            if (!_capture.IsOpened())
        //            {
        //                _capture.Open(1); // 사용자 지정 카메라 사용
        //                Debug.WriteLine("Camera Opened");
        //                return;
        //            }
        //            Debug.WriteLine("Camera Open Failed");
        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Write(this, ex.Message.ToString());
        //        }
        //    }

        //    public void ReleaseCamera()
        //    {
        //        if (_capture.IsOpened())
        //        {
        //            _capture.Release();
        //        }
        //    }

        //    // 카메라 활성화 여부 확인
        //    public bool IsOpened()
        //    {
        //        return _capture.IsOpened();
        //    }

        //    public BitmapSource CaptureFrame()
        //    {
        //        // Debug.WriteLine("isOpened = " + _capture.IsOpened());
        //        if (_capture.IsOpened())
        //        {
        //            _capture.Read(_frame);
        //            return BitmapSourceConverter.ToBitmapSource(_frame);
        //        }
        //        return null;
        //    }
        //}

        //public class CrevisManager : IModel
        //{
        //    VirtualFG40Library _virtualFG40 = new VirtualFG40Library();
        //    Int32 _hDevice = 0;
        //    Int32 _width = 0;
        //    Int32 _height = 0;
        //    Int32 _bufferSize = 0;
        //    Boolean _isOpen = false;
        //    IntPtr _pImage = new IntPtr();
        //    IntPtr _cvtImage = new IntPtr();
        //    public void InitCamera()
        //    {
        //        Int32 status = VirtualFG40Library.MCAM_ERR_SUCCESS;
        //        UInt32 camNum = 0;

        //        try
        //        {
        //            // Update Device List
        //            status = _virtualFG40.UpdateDevice();
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                _virtualFG40.FreeSystem();
        //                throw new Exception(String.Format("Update Device list failed : {0}", status));
        //            }

        //            status = _virtualFG40.GetAvailableCameraNum(ref camNum);
        //            if (camNum <= 0)
        //            {
        //                _virtualFG40.FreeSystem();
        //                throw new Exception("The camera can not be connected.");
        //            }
        //            // camera open
        //            status = _virtualFG40.OpenDevice(0, ref _hDevice);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                _virtualFG40.FreeSystem();
        //                throw new Exception(String.Format("Open device failed : {0}", status));
        //            }

        //            _isOpen = true;

        //            // Call Set Feature
        //            SetFeature();

        //            // Get Width
        //            status = _virtualFG40.GetIntReg(_hDevice, VirtualFG40Library.MCAM_WIDTH, ref _width);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Read Register failed : {0}", status));
        //            }

        //            // Get Height
        //            status = _virtualFG40.GetIntReg(_hDevice, VirtualFG40Library.MCAM_HEIGHT, ref _height);

        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Read Register failed : {0}", status));
        //            }

        //            // Image buffer allocation
        //            _bufferSize = _width * _height;
        //            _pImage = Marshal.AllocHGlobal(_bufferSize);

        //            // Display Image buffer allocation
        //            _cvtImage = Marshal.AllocHGlobal(_bufferSize * 3);

        //            // pictureBox_Display.Image = new Bitmap(_width, _height, PixelFormat.Format24bppRgb);
        //        }
        //        catch (Exception ex)
        //        {
        //            AP.System.InterlockMsgEvent(ex.Message.ToString());
        //            Logger.Write(this, ex.Message.ToString());
        //        }
        //    }

        //    public void ReleaseCamera()
        //    {
        //        try
        //        {
        //            if (_isOpen == true)
        //            {
        //                if (_pImage != IntPtr.Zero)
        //                {
        //                    Marshal.FreeHGlobal(_pImage);
        //                    _pImage = IntPtr.Zero;
        //                }
        //                if (_cvtImage != IntPtr.Zero)
        //                {
        //                    Marshal.FreeHGlobal(_cvtImage);
        //                    _cvtImage = IntPtr.Zero;
        //                }

        //                // Close Device
        //                _virtualFG40.CloseDevice(_hDevice);

        //                _isOpen = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AP.System.InterlockMsgEvent(ex.Message.ToString());
        //            Logger.Write(this, ex.Message.ToString());
        //        }
        //    }

        //    public void StreamStart()
        //    {
        //        // 1. Change Acquisition Mode : Continuous
        //        // 2. Excute Acqusition Start Command	
        //        // 3. Acqusution Loop Function (Timer)	
        //        //	-> Grab Image using GrabImage
        //        //	-> Image Display

        //        Int32 status = VirtualFG40Library.MCAM_ERR_SUCCESS;

        //        try
        //        {
        //            // Change Acqusition Mode
        //            status = _virtualFG40.SetEnumReg(_hDevice, VirtualFG40Library.MCAM_ACQUISITION_MODE, VirtualFG40Library.ACQUISITION_MODE_CONTINUOUS);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Write Register failed : {0}", status));
        //            }

        //            // Acqusition Start
        //            status = _virtualFG40.AcqStart(_hDevice);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Acqusition Start failed : {0}", status));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AP.System.InterlockMsgEvent(ex.Message.ToString());
        //            Logger.Write(this, ex.Message.ToString());
        //        }
        //    }

        //    public void StreamEnd()
        //    {
        //        // 1. Thread suspend or Timer Stop
        //        // 2. Excute Acqusition Stop Command 

        //        Int32 status = VirtualFG40Library.MCAM_ERR_SUCCESS;

        //        try
        //        {
        //            // Acqusition Stop
        //            status = _virtualFG40.AcqStop(_hDevice);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Acqusition Start failed : {0}", status));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AP.System.InterlockMsgEvent(ex.Message.ToString());
        //            Logger.Write(this, ex.Message.ToString());
        //        }
        //    }

        //    public BitmapSource CaptureFrame()
        //    {
        //        // Debug.WriteLine("isOpened = " + _capture.IsOpened());
        //        if (_isOpen)
        //        {
        //            // Grab Function
        //            _virtualFG40.GrabImage(_hDevice, _pImage, (UInt32)_bufferSize);
        //            _virtualFG40.CvtColor(_pImage, _cvtImage, _width, _height, VirtualFG40Library.CV_BayerRG2RGB);

        //            return BitmapSourceConverter.ToBitmapSource(IntPrtToBitmap());
        //        }
        //        return null;
        //    }

        //    private Bitmap IntPrtToBitmap()
        //    {
        //        Int32 bitsPerPixel = 0;
        //        Int32 stride = 0;
        //        Bitmap bitmap;
        //        PixelFormat pixelFormat = PixelFormat.Format24bppRgb;

        //        //color
        //        bitsPerPixel = 24;
        //        stride = (Int32)((_width * bitsPerPixel + 7) / 8);
        //        bitmap = new Bitmap(_width, _height, stride, pixelFormat, _cvtImage);

        //        return bitmap;
        //    }

        //    private void SetFeature()
        //    {
        //        Int32 status = VirtualFG40Library.MCAM_ERR_SUCCESS;

        //        try
        //        {
        //            // Set Trigger Mode
        //            status = _virtualFG40.SetEnumReg(_hDevice, VirtualFG40Library.MCAM_TRIGGER_MODE, VirtualFG40Library.TRIGGER_MODE_OFF);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Write Register failed : {0}", status));
        //            }

        //            // Set PixelFormat
        //            status = _virtualFG40.SetEnumReg(_hDevice, VirtualFG40Library.MCAM_PIXEL_FORMAT, VirtualFG40Library.PIXEL_FORMAT_BAYERRG8);
        //            if (status != VirtualFG40Library.MCAM_ERR_SUCCESS)
        //            {
        //                throw new Exception(String.Format("Write Register failed : {0}", status));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AP.System.InterlockMsgEvent(ex.Message.ToString());
        //            Logger.Write(this, ex.Message.ToString());
        //        }
        //    }
        //}
    }
}
