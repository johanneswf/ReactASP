﻿using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using ReactASP.Models;
using System.Security.Claims;

namespace ReactASP.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> mUserManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            mUserManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ApplicationUser user = await mUserManager.GetUserAsync(context.Subject);

            IList<string> roles = await mUserManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            context.IssuedClaims.AddRange(roleClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }

}
