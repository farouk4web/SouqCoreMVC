namespace Souq.Services.Files;
public interface IFileService
{
    string UploadPicture(IFormFile picture, string folderName, string fileName);

    bool RemovePicture(string src);
}