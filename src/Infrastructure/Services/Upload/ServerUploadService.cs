using IXBI.Net.Helper;
using WW.Application.Common.Models;
using WW.Infrastructure.Services.Upload.Constants;

namespace WW.Application.Common.Interfaces;
public class ServerUploadService : IUploadService
{

    string IUploadService.GetStorageBlobUrl(string fileName, string path)
    {
        return string.Format("{0}{1}", path, fileName);
    }

    public string GetTypeUpload()
    {
        return TypeUpload.SERVER.ToString();
    }

    //public List<FileUpload> UploadStorageBlob(UploadModel input)
    //{
    //    throw new NotImplementedException();
    //}
}
