using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Media
{
    public class MediaFile
    {
        public string FilePath { get; set; }
        public MediaType Type { get; set; }
        public DateTime DateCreated { get; set; }
        public Image Thumbnail { get; set; }

        public MediaFile(string filePath, MediaType type, DateTime dateCreated)
        {
            FilePath = filePath;
            Type = type;
            DateCreated = dateCreated;
            Thumbnail = Task.Run(() => CreateThumbnail(filePath, type)).Result;
        }

        private async Task<Image> CreateThumbnail(string filePath, MediaType type)
        {
            try
            {
                if (type == MediaType.Image)
                {
                    return Image.FromFile(filePath).GetThumbnailImage(50, 50, null, IntPtr.Zero);
                }
                else if (type == MediaType.Video)
                {
                    return await CreateVideoThumbnail(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating thumbnail: {ex.Message}");
            }
            return null;
        }

        private async Task<Image> CreateVideoThumbnail(string filePath)
        {
            try
            {
                var outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

                // Create a snapshot from the video file
                await FFmpeg.Conversions.New()
                    .AddParameter($"-i \"{filePath}\" -ss 00:00:01 -vframes 1 \"{outputPath}\"")
                    .Start();

                if (File.Exists(outputPath))
                {
                    var thumbnail = Image.FromFile(outputPath).GetThumbnailImage(50, 50, null, IntPtr.Zero);
                    File.Delete(outputPath); // Clean up the temporary thumbnail file
                    return thumbnail;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating video thumbnail: {ex.Message}");
            }
            return null;
        }
    }

    public enum MediaType
    {
        Image,
        Video
    }
}
