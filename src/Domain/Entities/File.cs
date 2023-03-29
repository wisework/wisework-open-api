using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class File
{
    public int FileId { get; set; }
    public long CompanyId { get; set; }
    public int? FileCategoryId { get; set; }
    public int FileTypeId { get; set; }
    public string Status { get; set; } = null!;
    public int CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public int UpdateBy { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public string? FullFileName { get; set; }
    public string? ThumbFileName { get; set; }
    public string? OriginalFileName { get; set; }
    public int? FileSize { get; set; }
}
