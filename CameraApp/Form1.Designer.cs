namespace CameraApp
{
    partial class formCamera
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboCamera;
        private Label label1;
        private PictureBox pictureBox;
        private Button btnStart;
        private Button btnTakePhoto;
        private SaveFileDialog saveFileDialog1;
        private Button btnStop;
        private Button btnStartRecording;
        private Button btnStopRecording;
        private SaveFileDialog saveFileDialog2;
        private ToolTip toolTip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Button btnImageSort;
        private Button btnVideoSort;
        private Button btnPathLink;
        private Button btnSelectPath;
        private ListView listView1;
        private ImageList imageList;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cboCamera = new ComboBox();
            label1 = new Label();
            pictureBox = new PictureBox();
            btnStart = new Button();
            btnTakePhoto = new Button();
            saveFileDialog1 = new SaveFileDialog();
            btnStop = new Button();
            btnStartRecording = new Button();
            btnStopRecording = new Button();
            saveFileDialog2 = new SaveFileDialog();
            toolTip = new ToolTip(components);
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            btnImageSort = new Button();
            btnVideoSort = new Button();
            btnPathLink = new Button();
            btnSelectPath = new Button();
            listView1 = new ListView();
            imageList = new ImageList(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // cboCamera
            // 
            cboCamera.FormattingEnabled = true;
            cboCamera.Location = new Point(113, 21);
            cboCamera.Name = "cboCamera";
            cboCamera.Size = new Size(264, 28);
            cboCamera.TabIndex = 0;
            toolTip.SetToolTip(cboCamera, "Select the camera to use");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(40, 24);
            label1.Name = "label1";
            label1.Size = new Size(63, 20);
            label1.TabIndex = 1;
            label1.Text = "Camera:";
            // 
            // pictureBox
            // 
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(27, 63);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(797, 374);
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(27, 457);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(109, 34);
            btnStart.TabIndex = 3;
            btnStart.Text = "&Start";
            toolTip.SetToolTip(btnStart, "Start the camera");
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnTakePhoto
            // 
            btnTakePhoto.Location = new Point(361, 457);
            btnTakePhoto.Name = "btnTakePhoto";
            btnTakePhoto.Size = new Size(108, 34);
            btnTakePhoto.TabIndex = 4;
            btnTakePhoto.Text = "Take Photo";
            toolTip.SetToolTip(btnTakePhoto, "Take a photo");
            btnTakePhoto.UseVisualStyleBackColor = true;
            btnTakePhoto.Click += btnTakePhoto_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Image Files|*.jpg";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(717, 457);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(108, 34);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            toolTip.SetToolTip(btnStop, "Stop the camera");
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnStartRecording
            // 
            btnStartRecording.Location = new Point(195, 457);
            btnStartRecording.Name = "btnStartRecording";
            btnStartRecording.Size = new Size(124, 34);
            btnStartRecording.TabIndex = 6;
            btnStartRecording.Text = "Start Recording";
            toolTip.SetToolTip(btnStartRecording, "Start recording video");
            btnStartRecording.UseVisualStyleBackColor = true;
            btnStartRecording.Click += btnStartRecording_Click;
            // 
            // btnStopRecording
            // 
            btnStopRecording.Location = new Point(539, 457);
            btnStopRecording.Name = "btnStopRecording";
            btnStopRecording.Size = new Size(122, 34);
            btnStopRecording.TabIndex = 7;
            btnStopRecording.Text = "Stop Recording";
            toolTip.SetToolTip(btnStopRecording, "Stop recording video");
            btnStopRecording.UseVisualStyleBackColor = true;
            btnStopRecording.Click += btnStopRecording_Click;
            // 
            // saveFileDialog2
            // 
            saveFileDialog2.Filter = "AVI Files|*.avi";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 513);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1158, 22);
            statusStrip.TabIndex = 8;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 16);
            // 
            // btnImageSort
            // 
            btnImageSort.Location = new Point(822, 9);
            btnImageSort.Margin = new Padding(3, 2, 3, 2);
            btnImageSort.Name = "btnImageSort";
            btnImageSort.Size = new Size(73, 33);
            btnImageSort.TabIndex = 9;
            btnImageSort.Text = "Image";
            btnImageSort.UseVisualStyleBackColor = true;
            btnImageSort.Click += btnImageSort_Click;
            // 
            // btnVideoSort
            // 
            btnVideoSort.Location = new Point(906, 9);
            btnVideoSort.Margin = new Padding(3, 2, 3, 2);
            btnVideoSort.Name = "btnVideoSort";
            btnVideoSort.Size = new Size(73, 33);
            btnVideoSort.TabIndex = 10;
            btnVideoSort.Text = "Video";
            btnVideoSort.UseVisualStyleBackColor = true;
            btnVideoSort.Click += btnVideoSort_Click;
            // 
            // btnPathLink
            // 
            btnPathLink.Location = new Point(1068, 9);
            btnPathLink.Margin = new Padding(3, 2, 3, 2);
            btnPathLink.Name = "btnPathLink";
            btnPathLink.Size = new Size(73, 33);
            btnPathLink.TabIndex = 11;
            btnPathLink.Text = "Path";
            btnPathLink.UseVisualStyleBackColor = true;
            btnPathLink.Click += btnPathLink_Click;
            // 
            // btnSelectPath
            // 
            btnSelectPath.Location = new Point(0, 0);
            btnSelectPath.Name = "btnSelectPath";
            btnSelectPath.Size = new Size(75, 23);
            btnSelectPath.TabIndex = 0;
            // 
            // listView1
            // 
            listView1.LargeImageList = imageList;
            listView1.Location = new Point(844, 62);
            listView1.Margin = new Padding(3, 2, 3, 2);
            listView1.Name = "listView1";
            listView1.Size = new Size(286, 412);
            listView1.TabIndex = 12;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.DoubleClick += listView1_DoubleClick;
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(64, 64);
            imageList.TransparentColor = Color.Transparent;
            // 
            // formCamera
            // 
            ClientSize = new Size(1158, 535);
            Controls.Add(listView1);
            Controls.Add(btnPathLink);
            Controls.Add(btnVideoSort);
            Controls.Add(btnImageSort);
            Controls.Add(statusStrip);
            Controls.Add(btnStopRecording);
            Controls.Add(btnStartRecording);
            Controls.Add(btnStop);
            Controls.Add(btnTakePhoto);
            Controls.Add(btnStart);
            Controls.Add(pictureBox);
            Controls.Add(label1);
            Controls.Add(cboCamera);
            Name = "formCamera";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Camera App";
            Load += formCamera_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
