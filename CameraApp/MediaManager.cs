using Media;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraApp
{
    public class MediaManager
    {
        private List<MediaFile> mediaCollection = new List<MediaFile>();
        private ImageList imageList;
        private ListView listView;

        public MediaManager(ImageList imgList, ListView lstView)
        {
            imageList = imgList;
            listView = lstView;
        }

        public void AddMediaFile(string filePath, MediaType type)
        {
            MediaFile newMediaFile = new MediaFile(filePath, type, DateTime.Now);
            mediaCollection.Add(newMediaFile);
            AddMediaToListView(newMediaFile);
        }

        public void SortMediaByType(MediaType type)
        {
            var sortedMedia = mediaCollection.Where(m => m.Type == type)
                                             .OrderBy(m => m.DateCreated)
                                             .ToList();
            UpdateListView(sortedMedia);
        }

        public void SortAllMedia()
        {
            mediaCollection = mediaCollection.OrderBy(m => m.Type)
                                             .ThenBy(m => m.DateCreated)
                                             .ToList();
            UpdateListView(mediaCollection);
        }

        private void AddMediaToListView(MediaFile mediaFile)
        {
            Bitmap thumbnail = null;

            if (mediaFile.Type == MediaType.Image)
                thumbnail = CreateImageThumbnail(mediaFile.FilePath);
            else if (mediaFile.Type == MediaType.Video)
                thumbnail = ExtractVideoThumbnail(mediaFile.FilePath);

            if (thumbnail != null)
            {
                imageList.Images.Add(mediaFile.FilePath, thumbnail);
                var item = new ListViewItem
                {
                    ImageKey = mediaFile.FilePath,
                    Tag = mediaFile.FilePath,
                    Text = Path.GetFileName(mediaFile.FilePath)
                };
                listView.Items.Add(item);
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

        private void UpdateListView(List<MediaFile> sortedMedia)
        {
            listView.Items.Clear();
            imageList.Images.Clear();
            foreach (var media in sortedMedia)
            {
                AddMediaToListView(media);
            }
        }

    }
}
