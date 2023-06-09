﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WW.Application.CompanyProfile.Queies.GetCompany;
public record GetCompanyQuery : IRequest<List<Company>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
};

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, List<Company>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public GetCompanyQueryHandler(IApplicationDbContext context, IMapper mapper, IUploadService uploadService)
    {
        _context = context;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<List<Company>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        var companyInfo = (from cf in _context.DbSetCompanies
                           join file in _context.DbSetFile on cf.ProfileImageId equals file.FileId
                           where cf.Status != Status.X.ToString()
                           select new Company
                           {
                               CompanyId = cf.CompanyId,
                               CompanyName = cf.Name,
                               LogoImage = _uploadService.GetStorageBlobUrl(file.FullFileName, ""),
                               Status = cf.Status,
                               CreateBy = cf.CreateBy.ToString(),
                               CreateDate = cf.CreateDate.ToString(),
                               UpdateBy = cf.UpdateBy.ToString(),
                               UpdateDate = cf.UpdateDate.ToString(),
                           }).ToList();

        if (companyInfo.Count == 0 )
        {
            throw new NotFoundException();
        }

        try
        {
            return companyInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

        

       
    }
}
