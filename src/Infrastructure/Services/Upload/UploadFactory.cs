using System;
using Microsoft.Extensions.Configuration;
using WW.Infrastructure.Services.Upload.Constants;

namespace WW.Infrastructure.Services.Upload;
public static class UploadFactory
{
    private static IUploadService _uploadService;
    public static IUploadService Create(IConfiguration configuration)
    {
        if (UploadFactory._uploadService == null)
        {
            string type = configuration.GetValue<String>("Storage:Provider");
            if (type.ToUpper() == TypeUpload.AWS.ToString()) UploadFactory._uploadService = new AwsUploadService();
            else if (type.ToUpper() == TypeUpload.AZURE.ToString()) UploadFactory._uploadService = new AzureUploadService();
            else { UploadFactory._uploadService = new ServerUploadService(); }
        }
        return UploadFactory.GetService();
    }
    public static IUploadService GetService()
    {
        return UploadFactory._uploadService;
    }
}
