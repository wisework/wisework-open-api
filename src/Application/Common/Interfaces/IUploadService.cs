namespace WW.Application.Common.Interfaces;
public interface IUploadService
{
    string GetStorageBlobUrl(string fileName, string path = "");
}
