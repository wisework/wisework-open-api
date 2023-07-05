using IXBI.Net.Helper;
using WW.Application.Common.Models;

namespace WW.Application.Common.Interfaces;
public interface IUploadService
{
    string GetStorageBlobUrl(string fileName, string path = "");
    //List<FileUpload> UploadStorageBlob(UploadModel input);
}
