using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using FunctionalService;

namespace DataService
{
    public static class DbContextInitializer
    {
        public static async Task Initialize(DataProtectionKeysContext dataProtectionKeysContext, 
            ApplicationDbContext applicationDbContext,
            IFunctionalSvc functionalSvc)
        {
            // Check if db DataProtectionKeyContext and ApplicationDbContext is created
            await dataProtectionKeysContext.Database.EnsureCreatedAsync();
            await applicationDbContext.Database.EnsureCreatedAsync();

            // Check if db contains any user.  If not empty : db has been already seeded
            if (applicationDbContext.ApplicationUsers.Any())
            {
                return;
            }
            // If empty : create AdminUser and App User
            else
            {
                await functionalSvc.CreateDefaultAdminUser();
                await functionalSvc.CreateDefaultAppUser();
            }

            

        }
    }
}
