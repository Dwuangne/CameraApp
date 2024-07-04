namespace CameraApp
{
    partial class formCamera
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                if (cam != null && cam.IsRunning)
                {
                    cam.SignalToStop();
                    cam.WaitForStop();
                }

                if (writer != null)
                {
                    writer.Release();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // cboCamera
            // 
            cboCamera.FormattingEnabled = true;
            cboCamera.Location = new Point(113, 21);
            cboCamera.Name = "cboCamera";
            cboCamera.Size = new Size(264, 23);
            cboCamera.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(40, 24);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 1;
            label1.Text = "Camera:";
            // 
            // pictureBox
            // 
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(27, 63);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(797, 346);
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(27, 440);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(109, 34);
            btnStart.TabIndex = 3;
            btnStart.Text = "&Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnTakePhoto
            // 
            btnTakePhoto.Location = new Point(361, 440);
            btnTakePhoto.Name = "btnTakePhoto";
            btnTakePhoto.Size = new Size(108, 34);
            btnTakePhoto.TabIndex = 4;
            btnTakePhoto.Text = "Take Photo";
            btnTakePhoto.UseVisualStyleBackColor = true;
            btnTakePhoto.Click += btnTakePhoto_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Imagename|*.jpg";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(716, 440);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(108, 34);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnStartRecording
            // 
            btnStartRecording.Location = new Point(195, 440);
            btnStartRecording.Name = "btnStartRecording";
            btnStartRecording.Size = new Size(108, 34);
            btnStartRecording.TabIndex = 6;
            btnStartRecording.Text = "Start Recording";
            btnStartRecording.UseVisualStyleBackColor = true;
            btnStartRecording.Click += btnStartRecording_Click;
            // 
            // btnStopRecording
            // 
            btnStopRecording.Location = new Point(539, 440);
            btnStopRecording.Name = "btnStopRecording";
            btnStopRecording.Size = new Size(108, 34);
            btnStopRecording.TabIndex = 7;
            btnStopRecording.Text = "Stop Recording";
            btnStopRecording.UseVisualStyleBackColor = true;
            btnStopRecording.Click += btnStopRecording_Click;
            // 
            // saveFileDialog2
            // 
            saveFileDialog2.Filter = "AVI Files|*.avi";
            // 
            // formCamera
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(836, 486);
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
            Text = "Camera";
            Load += formCamera_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
    }
}
