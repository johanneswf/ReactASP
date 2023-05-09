using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ReactASP
{
    public class AddSubTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            var claimType = "sub";
            if (!principal.HasClaim(claim => claim.Type == claimType))
            {
                var id = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (id is not null)
                    claimsIdentity.AddClaim(new Claim(claimType, id));
            }

            principal.AddIdentity(claimsIdentity);
            return Task.FromResult(principal);
        }
    }
}
