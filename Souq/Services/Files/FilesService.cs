using Souq.Settings;

namespace Souq.Services.Files;
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }


    public string UploadPicture(IFormFile picture, string folderName, string fileName)
    {
        try
        {
            //  Upload To Server method
            FileInfo info = new(picture.FileName);
            var src = $"/Uploads/{folderName}/{fileName}.png";

            var path = Path.Combine(_env.WebRootPath + src);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                picture.CopyTo(stream);
            }

            return src;
        }

        catch (Exception)
        {
            string placeholderImage = folderName.Remove(folderName.Length - 1) + ".png";
            return $"/Uploads/{folderName}/{placeholderImage}";
        }

    }

    public bool RemovePicture(string src)
    {
        // remove picture 
        var path = Path.Combine(_env.WebRootPath + src);

        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }

        return false;
    }
}