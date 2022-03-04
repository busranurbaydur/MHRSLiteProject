using MHRSLiteBusiness.Contracts;
using MHRSLiteEntity.Enums;
using MHRSLiteEntity.IdentityModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.CreateDefaultData
{
    public class CreateData
    {
        public static void Create(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IWebHostEnvironment environment) 
 
          private static void CheckRoles(RoleManager<AppRole> roleManager)
            {

                var allRoles = Enum.GetNames(typeof(RoleNames));
                foreach (var item in allRoles)
                {
                    if (!_roleManager.RoleExistsAsync(item).Result)
                    {
                        var result = _roleManager.CreateAsync(new AppRole()
                        {
                            Name = item,
                            Description = item
                        }).Result;

                    }
                }
            }
        }
    }
}
