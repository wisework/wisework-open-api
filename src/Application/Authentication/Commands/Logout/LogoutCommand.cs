using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WW.Application.Common.Interfaces;

namespace WW.Application.Authentication.Commands.Logout;

public record LogoutCommand : IRequest<int>
{
    public string? visitorId { get; init; }
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, int>
{
    private readonly IApplicationDbContext _context;

    public LogoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(100);

        return 0;
    }
}