using AForge.Video;
using AForge.Video.DirectShow;
using Media;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xabe.FFmpeg;

namespace CameraApp
{
    public partial class formCamera : Form
    {
        //private FilterInfoCollection cameras;
        //private VideoCaptureDevice cam;
        private VideoWriter writer;
        private bool isRecording = false;
        //private OpenCvSharp.Size frameSize;
        //private List<MediaFile> mediaCollection = new List<MediaFile>();
        private MediaManager mediaManager;
        private CameraManager cameraManager;

        public formCamera()
        {
            InitializeComponent();
            FFmpeg.SetExecutablesPath(@"C:\path\to\ffmpeg\bin"); // Ensure this path is correct
            //LoadCameras();
            mediaManager = new MediaManager(imageList, listView1);
            cameraManager = new CameraManager(cboCamera);

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            cameraManager.StartCamera(cboCamera.SelectedIndex, Cam_NewFrame);
        }

        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Font font = new Font("Arial", 20, FontStyle.Bold);
                Brush brush = new SolidBrush(Color.Red);
                g.DrawString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), font, brush, new PointF(10, 10));
            }

            if (isRecording && writer != null)
            {
                try
                {
                    using (Mat mat = bitmap.ToMat())
                    {
                        writer.Write(mat);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error writing frame: {ex.Message}");
                }
            }

            pictureBox.Image?.Dispose();
            pictureBox.Image = bitmap;
        }

        private void formCamera_Load(object sender, EventArgs e)
        {
            cameraManager.StartCamera(cboCamera.SelectedIndex, Cam_NewFrame);
        }

        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null)
            {
                MessageBox.Show("No image to save.");
                return;
            }

            saveFileDialog1.InitialDirectory = "D:\\Materials_FPTU\\Season_5\\PRN212_PlatformApplicationProgramming\\Code_PRN212\\Project_PRN\\File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox.Image.Save(saveFileDialog1.FileName);
                    MessageBox.Show("Image saved successfully.");
                    mediaManager.AddMediaFile(saveFileDialog1.FileName, MediaType.Image);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}");
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cameraManager.StopCamera();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            cameraManager.StopCamera();
            StopRecording();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            if (isRecording)
            {
                MessageBox.Show("Already recording.");
                return;
            }

            saveFileDialog2.Filter = "AVI Files|*.avi";
            saveFileDialog2.InitialDirectory = "D:\\Materials_FPTU\\Season_5\\PRN212_PlatformApplicationProgramming\\Code_PRN212\\Project_PRN\\File";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int fourcc = VideoWriter.FourCC('X', 'V', 'I', 'D');
                    writer = new VideoWriter(saveFileDialog2.FileName, fourcc, 25, cameraManager.FrameSize);
                    isRecording = true;
                    MessageBox.Show("Recording started.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error starting recording: {ex.Message}");
                }
            }
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            StopRecording();
            mediaManager.AddMediaFile(saveFileDialog2.FileName, MediaType.Video);
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

        private void btnPathLink_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    var files = Directory.GetFiles(selectedPath);
                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file).ToLower();
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                        {
                            mediaManager.AddMediaFile(file, MediaType.Image);
                        }
                        else if (extension == ".avi" || extension == ".mp4" || extension == ".mkv")
                        {
                            mediaManager.AddMediaFile(file, MediaType.Video);
                        }
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedMediaPath = listView1.SelectedItems[0].Tag.ToString();
                //PreviewMedia(selectedMediaPath);
            }
        }

        private void btnImageSort_Click(object sender, EventArgs e)
        {
            mediaManager.SortMediaByType(MediaType.Image);
        }

        private void btnVideoSort_Click(object sender, EventArgs e)
        {
            mediaManager.SortMediaByType(MediaType.Video);
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedMediaPath = listView1.SelectedItems[0].Tag.ToString();
                FileManager.OpenFile(selectedMediaPath);
            }
        }
    }
}
