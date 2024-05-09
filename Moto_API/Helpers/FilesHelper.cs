namespace Moto_API.Helpers
{
    public class FilesHelper
    {
        public static bool UploadPhoto(MemoryStream memoryStream, string folderName, string fileName)
        {
            try
            {
                memoryStream.Position = 0;
                var path = Path.Combine(folderName, fileName);
                File.WriteAllBytes(path, memoryStream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
