using AForge.Video;
using AForge.Video.DirectShow;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;

namespace CameraApp
{
    public partial class formCamera : Form
    {
        private FilterInfoCollection cameras;
        private VideoCaptureDevice cam;
        private VideoWriter writer;
        private bool isRecording = false;
        private OpenCvSharp.Size frameSize;

        public formCamera()
        {
            InitializeComponent();
            LoadCameras();
        }

        private void LoadCameras() //Load camera mặc định khi bắt đầu phần mềm
        {
            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);//tìm những thiết bị ghi hình kết nối với máy
            foreach (FilterInfo info in cameras) //add thiết bị vô cboCamera để lựa chọn
            {
                cboCamera.Items.Add(info.Name);
            }
            cboCamera.SelectedIndex = 0; //luôn luôn chọn camera mặc định ở vị trí bằng 0
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cam != null && cam.IsRunning) //Nếu dã có cam chạy rồi thì không cần khởi động lại cam
            {
                MessageBox.Show("Camera is already running.");
                return;
            }

            StartCamera(); //nếu cam chưa chạy thì chạy cam
        }

        private OpenCvSharp.Size GetFrameSize()  //căn chỉnh kích thước của màn hình
        {
            if (cam != null && cam.VideoCapabilities.Length > 0) //nếu có cam thì lấy kích thước của cam
            {
                var resolution = cam.VideoCapabilities[0];
                return new OpenCvSharp.Size(resolution.FrameSize.Width, resolution.FrameSize.Height);
            }
            // Nếu không lấy được kích thước từ camera, sử dụng kích thước mặc định
            return new OpenCvSharp.Size(640, 480);
        }
        private void StartCamera()
        {
            try
            {
                cam = new VideoCaptureDevice(cameras[cboCamera.SelectedIndex].MonikerString); //chọn camera trong cboCamera
                cam.NewFrame += Cam_NewFrame; // tạo New Frame
                cam.Start(); //chạy camera

                // Lấy kích thước frame từ camera
                frameSize = GetFrameSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting camera: {ex.Message}");
            }
        }

        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Tạo một bản sao của frame hiện tại từ camera
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            // Sử dụng đối tượng Graphics để vẽ lên bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Tạo font cho văn bản: Arial, cỡ 20, in đậm, màu đỏ
                Font font = new Font("Arial", 20, FontStyle.Bold);
                Brush brush = new SolidBrush(Color.Red);
                // Vẽ chuỗi thời gian hiện tại lên bitmap ở vị trí (10, 10)
                g.DrawString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), font, brush, new PointF(10, 10));
            }

            // Kiểm tra xem có đang ghi video không và writer đã được khởi tạo chưa
            if (isRecording && writer != null)
            {
                try
                {
                    // Chuyển đổi bitmap sang Mat (định dạng của OpenCV)
                    using (Mat mat = bitmap.ToMat())
                    {
                        // Ghi frame vào file video
                        writer.Write(mat);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error writing frame: {ex.Message}");
                }
            }
            // Giải phóng bộ nhớ của ảnh cũ trong pictureBox (nếu có)
            pictureBox.Image?.Dispose();
            // Hiển thị frame mới lên pictureBox
            pictureBox.Image = bitmap;
        }

        private void formCamera_Load(object sender, EventArgs e)
        {
            StartCamera(); //Load camera khi bắt đầu
        }

        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            //kiểm tra xem có image không
            if (pictureBox.Image == null)
            {
                MessageBox.Show("No image to save.");
                return;
            }

            //địa chỉ lưu file
            saveFileDialog1.InitialDirectory = "D:\\Materials_FPTU\\Season_5\\PRN212_PlatformApplicationProgramming\\Code_PRN212\\Project_PRN\\File";
            // Hiển thị hộp thoại lưu file và kiểm tra xem người dùng đã chọn vị trí lưu chưa
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Lưu hình ảnh từ pictureBox vào file được chọn
                    pictureBox.Image.Save(saveFileDialog1.FileName);
                    MessageBox.Show("Image saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}");
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera(); //dừng camera
        }

        private void StopCamera()
        {
            try
            {
                //nếu đang có cam thì dừng lại
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
            //ấn nút tắt 
            base.OnFormClosing(e);
            StopCamera();
            StopRecording();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            //Nếu đang trong trạng thái record
            if (isRecording)
            {
                MessageBox.Show("Already recording.");
                return;
            }
            //thiết lập kiểu file muốn lưu(có thể không cần vì đã cài trươc)
            saveFileDialog2.Filter = "AVI Files|*.avi";
            //địa chỉ lưu file
            saveFileDialog2.InitialDirectory = "D:\\Materials_FPTU\\Season_5\\PRN212_PlatformApplicationProgramming\\Code_PRN212\\Project_PRN\\File";
            //mở folder cần lưu file
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Tạo mã FourCC cho codec video XVID
                    int fourcc = VideoWriter.FourCC('X', 'V', 'I', 'D');

                    // Khởi tạo đối tượng VideoWriter để ghi video
                    // Tham số:
                    // - saveFileDialog2.FileName: Đường dẫn và tên file video sẽ được lưu
                    // - fourcc: Mã codec video
                    // - 25: Số khung hình trên giây (fps)
                    // - frameSize: Kích thước khung hình
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
        }

        private void StopRecording()
        {
            //nếu đang record thì dừng lại
            if (isRecording)
            {
                isRecording = false;
                //để giải phóng các tài nguyên liên quan đến video, như đóng file video đang ghi.
                writer?.Release();
                //đảm bảo hệ thống được giải phóng đúng cách
                writer?.Dispose();
                writer = null;
                MessageBox.Show("Recording stopped.");
            }
        }
    }
}