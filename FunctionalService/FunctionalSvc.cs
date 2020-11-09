using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using ModelService;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Serilog;
using System.Threading.Tasks;

namespace FunctionalService
{
    class FunctionalSvc : IFunctionalSvc
    {
        private readonly AdminUserOptions _adminUserOptions;
        private readonly AppUserOptions _appUserOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public FunctionalSvc(IOptions<AppUserOptions> appUserOptions, 
            IOptions<AdminUserOptions> adminUserOptions, 
            UserManager<ApplicationUser> userManager)
        {
            _adminUserOptions = adminUserOptions.Value;
            _appUserOptions = appUserOptions.Value;
            _userManager = userManager;
        }

        public async Task CreateDefaultAdminUser()
        {
            try
            {
                var adminUser = new ApplicationUser
                {
                    Email = _adminUserOptions.Email,
                    UserName = _adminUserOptions.Username,
                    EmailConfirmed = true,
                    ProfilePic = GetDefaultProfilePic(), // TODO
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true,
                    Firstname = _adminUserOptions.Firstname,
                    Lastname = _adminUserOptions.Lastname,
                    UserRole = "Administrator",
                    IsActive = true,
                    UserAddress = new List<AddressModel>
                    {
                        new AddressModel {Country = _adminUserOptions.Country, Type = "Shipping"},
                        new AddressModel { Country = _adminUserOptions.Country, Type = "Billing" }

                    }
                };

                var result = await _userManager.CreateAsync(adminUser, _adminUserOptions.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Administrator");
                    Log.Information("Admin user is created {UserName}", adminUser.UserName);
                } else
                {
                    var errorString = String.Join(',', result.Errors);
                    Log.Error("Error while creating user {errors}", errorString);
                }
            } catch (Exception ex)
            {
                Log.Error("Error while creating admin user {Error} {StackTrace} {InnerException} {Source}", 
                    ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
            }
        }

        public async Task CreateDefaultAppUser() {
            try
            {
                var appUser = new ApplicationUser
                {
                    Email = _appUserOptions.Email,
                    UserName = _appUserOptions.Username,
                    EmailConfirmed = true,
                    ProfilePic = GetDefaultProfilePic(), // TODO
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true,
                    Firstname = _appUserOptions.Firstname,
                    Lastname = _appUserOptions.Lastname,
                    UserRole = "Customer",
                    IsActive = true,
                    UserAddress = new List<AddressModel>
                    {
                        new AddressModel {Country = _appUserOptions.Country, Type = "Shipping"},
                        new AddressModel { Country = _appUserOptions.Country, Type = "Billing" }

                    }
                };

                var result = await _userManager.CreateAsync(appUser, _appUserOptions.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Customer");
                    Log.Information("Admin user is created {UserName}", appUser.UserName);
                }
                else
                {
                    var errorString = String.Join(',', result.Errors);
                    Log.Error("Error while creating user {errors}", errorString);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while creating admin user {Error} {StackTrace} {InnerException} {Source}",
                    ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
            }
        }

        private string GetDefaultProfilePic()
        {
            return string.Empty;
        }
    }
}
