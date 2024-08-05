using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SGoncharovFileSharingService;

public static class ControllerExtension
{
    public static Guid GetUserId(this ControllerBase controller)
    {
        Guid.TryParse(controller.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out Guid guidFromClaim);
        return guidFromClaim;
    }
}
