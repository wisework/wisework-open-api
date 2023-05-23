using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using IXBI.Net.Helper;
using WW.Infrastructure.Services.Upload.Constants;
using WW.Infrastructure.Services.Upload.Models;

namespace WW.Infrastructure.Services.Upload;
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

    public List<FileUpload> UploadStorageBlob(UploadModel input)
    {
        throw new NotImplementedException();
    }
}
