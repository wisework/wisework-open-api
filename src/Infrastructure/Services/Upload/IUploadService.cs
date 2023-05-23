using System.Collections.Generic;
using System.Web;
using IXBI.Net.Helper;

namespace WW.Infrastructure.Services.Upload;
public interface IUploadService
{
    string GetStorageBlobUrl(string fileName, string path = "");
}
