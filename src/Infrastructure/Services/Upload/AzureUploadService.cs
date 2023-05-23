using System;
using IXBI.Net.Helper;
using Microsoft.Extensions.Configuration;
using WW.Infrastructure.Services.Upload.Constants;
using WW.Infrastructure.Services.Upload.Infrastructures;

namespace WW.Infrastructure.Services.Upload;
public class AzureUploadService : IUploadService
{
    public void DeleteStorageBlob(string fileName)
    {
        throw new NotImplementedException();
    }

    public string GetStorageBlobUrl(string fileName, string path)
    {
        IConfiguration configuration = ConfigurationFactory.GetConfigApp();
        var isosuiteAzureStorageConnectionString = configuration.GetValue<String>("Storage:AzureStorageConnectionString");
        var isosuiteAzureStorageContainer = configuration.GetValue<String>("Storage:AzureStorageContainer");
        return UploadHelper.GetStorageBlobUrl(fileName, isosuiteAzureStorageConnectionString, isosuiteAzureStorageContainer);
    }

    public string GetTypeUpload()
    {
        return TypeUpload.AZURE.ToString();
    }
}
