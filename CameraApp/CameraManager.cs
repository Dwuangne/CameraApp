using AForge.Video;
using AForge.Video.DirectShow;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraApp
{
    public class CameraManager
    {
        private VideoCaptureDevice cam;
        private FilterInfoCollection cameras;
        private VideoWriter writer;
        private OpenCvSharp.Size frameSize;
        public OpenCvSharp.Size FrameSize => frameSize;

        public event EventHandler<NewFrameEventArgs> OnNewFrame;
        private bool isRecording = false;


        public CameraManager(ComboBox cboCamera)
        {
            LoadCameras(cboCamera);
        }

        private void LoadCameras(ComboBox cboCamera)
        {
            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in cameras)
            {
                cboCamera.Items.Add(info.Name);
            }
            cboCamera.SelectedIndex = 0;
        }

        public void StartCamera(int selectedIndex, NewFrameEventHandler frameHandler)
        {
            if (cam != null && cam.IsRunning)
            {
                MessageBox.Show("Camera is already running.");
                return;
            }
            cam = new VideoCaptureDevice(cameras[selectedIndex].MonikerString);
            cam.NewFrame += frameHandler;
            cam.Start();
            frameSize = GetFrameSize();
        }

        private OpenCvSharp.Size GetFrameSize()
        {
            if (cam != null && cam.VideoCapabilities.Length > 0)
            {
                var resolution = cam.VideoCapabilities[0];
                return new OpenCvSharp.Size(resolution.FrameSize.Width, resolution.FrameSize.Height);
            }
            return new OpenCvSharp.Size(640, 480);
        }

        public void StopCamera()
        {
            if (cam != null && cam.IsRunning)
            {
                cam.SignalToStop();
                cam.WaitForStop();
                cam = null;
            }
        }
        private void StopRecording()
        {
            if (isRecording)
            {
                isRecording = false;
                writer?.Release();
                writer?.Dispose();
                writer = null;
                MessageBox.Show("Recording stopped.");
            }
        }

    }
}
