using System.Web;

namespace WW.Application.Common.Models;
public class UploadModel
{
    public string destinationDirectory { get; set; } = ""; // property
    //public HttpFileCollection httpFilesCollection { get; set; }
    //public HttpFileCollectionBase httpFilesCollectionBase { get; set; }

    // public bool HasProcessCollection() {
    //   return this.httpFilesCollection != null && this.httpFilesCollection.Count != 0;
    // }
    // public bool HasProcessCollectionBase() {
    //   return this.httpFilesCollectionBase != null && this.httpFilesCollectionBase.Count != 0;
    // }
}
