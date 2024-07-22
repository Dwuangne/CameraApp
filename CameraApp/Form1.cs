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
        private FilterInfoCollection cameras;
        private VideoCaptureDevice cam;
        private VideoWriter writer;
        private bool isRecording = false;
        private OpenCvSharp.Size frameSize;
        private List<MediaFile> mediaCollection = new List<MediaFile>();

        public formCamera()
        {
            InitializeComponent();
            FFmpeg.SetExecutablesPath(@"C:\path\to\ffmpeg\bin"); // Ensure this path is correct
            LoadCameras();
        }

        private void LoadCameras()
        {
            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in cameras)
            {
                cboCamera.Items.Add(info.Name);
            }
            cboCamera.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cam != null && cam.IsRunning)
            {
                MessageBox.Show("Camera is already running.");
                return;
            }

            StartCamera();
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

        private void StartCamera()
        {
            try
            {
                cam = new VideoCaptureDevice(cameras[cboCamera.SelectedIndex].MonikerString);
                cam.NewFrame += Cam_NewFrame;
                cam.Start();
                frameSize = GetFrameSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting camera: {ex.Message}");
            }
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
            StartCamera();
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
                    AddMediaFile(saveFileDialog1.FileName, MediaType.Image);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}");
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void StopCamera()
        {
            try
            {
                if (cam != null && cam.IsRunning)
                {
                    cam.SignalToStop();
                    cam.WaitForStop();
                    cam = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping camera: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopCamera();
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
                    writer = new VideoWriter(saveFileDialog2.FileName, fourcc, 25, frameSize);
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
            AddMediaFile(saveFileDialog2.FileName, MediaType.Video);
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
                            AddMediaFile(file, MediaType.Image);
                        }
                        else if (extension == ".avi" || extension == ".mp4" || extension == ".mkv")
                        {
                            AddMediaFile(file, MediaType.Video);
                        }
                    }
                }
            }
        }

        private void AddMediaFile(string filePath, MediaType type)
        {
            MediaFile newMediaFile = new MediaFile(filePath, type, DateTime.Now);
            mediaCollection.Add(newMediaFile);
            AddMediaToListView(newMediaFile);
        }

        private void AddMediaToListView(MediaFile mediaFile)
        {
            Bitmap thumbnail = null;

            if (mediaFile.Type == MediaType.Image)
            {
                thumbnail = CreateImageThumbnail(mediaFile.FilePath);
            }
            else if (mediaFile.Type == MediaType.Video)
            {
                thumbnail = ExtractVideoThumbnail(mediaFile.FilePath);
            }

            if (thumbnail != null)
            {
                imageList.Images.Add(mediaFile.FilePath, thumbnail);
                ListViewItem item = new ListViewItem
                {
                    ImageKey = mediaFile.FilePath,
                    Tag = mediaFile.FilePath,
                    Text = Path.GetFileName(mediaFile.FilePath)
                };
                listView1.Items.Add(item);
            }
            else
            {
                MessageBox.Show($"Error: Could not create thumbnail for {mediaFile.FilePath}");
            }
        }

        private Bitmap CreateImageThumbnail(string imagePath)
        {
            try
            {
                using (var mat = Cv2.ImRead(imagePath))
                {
                    if (mat.Empty())
                        return null;

                    Cv2.Resize(mat, mat, new OpenCvSharp.Size(100, 100));
                    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating image thumbnail: {ex.Message}");
                return null;
            }
        }

        private Bitmap ExtractVideoThumbnail(string videoPath)
        {
            try
            {
                using (var capture = new VideoCapture(videoPath))
                {
                    if (!capture.IsOpened())
                        return null;

                    using (var frame = new Mat())
                    {
                        capture.Read(frame);
                        if (frame.Empty())
                            return null;

                        Cv2.Resize(frame, frame, new OpenCvSharp.Size(100, 100));
                        return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error extracting video thumbnail: {ex.Message}");
                return null;
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

        //private void PreviewMedia(string mediaPath)
        //{
        //    var extension = Path.GetExtension(mediaPath).ToLower();
        //    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
        //    {
        //        pictureBox.Image = Image.FromFile(mediaPath);
        //    }
        //    else if (extension == ".avi" || extension == ".mp4" || extension == ".mkv")
        //    {
        //        // Implement video preview logic here
        //        MessageBox.Show("Video preview not implemented yet.");
        //    }
        //}

        private void SortMediaCollection()
        {
            mediaCollection = mediaCollection.OrderBy(m => m.Type).ThenBy(m => m.DateCreated).ToList();
        }

        private void btnImageSort_Click(object sender, EventArgs e)
        {
            var sortedImages = mediaCollection.Where(m => m.Type == MediaType.Image)
                                               .OrderBy(m => m.DateCreated)
                                               .ToList();
            UpdateListView(sortedImages);
        }

        private void btnVideoSort_Click(object sender, EventArgs e)
        {
            var sortedVideos = mediaCollection.Where(m => m.Type == MediaType.Video)
                                              .OrderBy(m => m.DateCreated)
                                              .ToList();
            UpdateListView(sortedVideos);
        }

        private void UpdateListView(List<MediaFile> sortedMedia)
        {
            listView1.Items.Clear();
            imageList.Images.Clear();
            foreach (var media in sortedMedia)
            {
                AddMediaToListView(media);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedMediaPath = listView1.SelectedItems[0].Tag.ToString();
                OpenFileWithDefaultApplication(selectedMediaPath);
            }
        }

        private void OpenFileWithDefaultApplication(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // This ensures the file is opened with the default associated application
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}");
            }
        }
    }
}
