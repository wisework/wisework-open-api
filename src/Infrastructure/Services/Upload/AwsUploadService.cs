using System;
using Amazon.S3;
using Amazon.S3.Model;
using WW.Infrastructure.Services.Upload.Infrastructures;
using WW.Infrastructure.Services.Upload.Constants;
using Microsoft.Extensions.Configuration;

namespace WW.Infrastructure.Services.Upload;
public class AwsUploadService : IUploadService
{

    private AmazonS3Client _sdk;
    private IConfiguration configuration = ConfigurationFactory.GetConfigApp();
    public AwsUploadService()
    {
        this._sdk = new AmazonS3Client(
            this.configuration.GetValue<String>("Storage:AWSAccessKey"),
            this.configuration.GetValue<String>("Storage:AWSSecretKey"),
            Amazon.RegionEndpoint.APSoutheast1
        );
    }

    public string GetStorageBlobUrl(string fileName, string path = "")
    {
        try
        {
            string BucketName = this.configuration.GetValue<String>("Storage:AWSBuckets");
            GetObjectRequest reqObject = new GetObjectRequest { BucketName = BucketName, Key = fileName };
            GetPreSignedUrlRequest reqPreSigned = new GetPreSignedUrlRequest { BucketName = BucketName, Key = fileName };
            reqPreSigned.BucketName = BucketName;
            reqPreSigned.Key = fileName;
            reqPreSigned.Expires = (DateTime.Now.Add(new TimeSpan(7, 0, 0, 0)));
            string url = this._sdk.GetPreSignedURL(reqPreSigned);
            return url;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetTypeUpload()
    {
        return TypeUpload.AWS.ToString();
    }
}
