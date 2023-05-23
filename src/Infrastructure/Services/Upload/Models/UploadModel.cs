using System.Web;

namespace WW.Infrastructure.Services.Upload.Models;
public class UploadModel
{
    public string destinationDirectory  // property
    { get; set; } = "";

    // public bool HasProcessCollection() {
    //   return this.httpFilesCollection != null && this.httpFilesCollection.Count != 0;
    // }
    // public bool HasProcessCollectionBase() {
    //   return this.httpFilesCollectionBase != null && this.httpFilesCollectionBase.Count != 0;
    // }
}
