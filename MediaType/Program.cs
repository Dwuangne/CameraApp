using System;

namespace Media
{
    class MediaProgram
    {
        private static List<MediaFile> mediaCollection = new List<MediaFile>();
        private static void AddMediaFile(string filePath, MediaType type)
        {
            MediaFile newOne = new MediaFile(filePath, type, DateTime.Now);
            mediaCollection.Add(newOne);
        }

        static void Main(string[] args)
        {
            AddMediaFile("example.jpg", MediaType.Image);
            AddMediaFile("example.mp4", MediaType.Video);

            foreach (var media in mediaCollection)
            {
                Console.WriteLine($"FilePath: {media.FilePath}, Type: {media.Type}, DateCreated: {media.DateCreated}");
            }
        }

        
    }
}
