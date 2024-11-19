using System.Security.Claims;
using ChatHub.Application.Common.Interfaces;

namespace ChatHub.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? Id
    {
        get
        {
            var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            return int.TryParse(idClaim, out var id) ? id : (int?)null;
        }
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
   

    public string? BranchText
    {
        get
        {
                var branchClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("branch");
                return branchClaim;
            
        }
    }
}
