using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace SGoncharovFileSharingService;

public static class ControllerExtension
{
    public static Guid GetUserId(HttpContext context)
    {
        Guid.TryParse(context.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out Guid guidFromClaim);
        return guidFromClaim;
    }
}
