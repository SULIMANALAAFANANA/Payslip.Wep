using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payslip.Wep.Data;
using Payslip.Wep.Interface;
using Payslip.Wep.Models;
using Payslip.Wep.WepSiteRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.Wep.Implementations
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;


        public DbInitializer(UserManager<IdentityUser>
            userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (!_roleManager.RoleExistsAsync(PayslipRoles.Admin)
                .GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(PayslipRoles.Admin))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(PayslipRoles.Accountant))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(PayslipRoles.Employee))
                    .GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name = "Admin"


                }, "Admin@123").GetAwaiter().GetResult();
                ApplicationUser user = _context.ApplicationUsers
                    .FirstOrDefault(x => x.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, PayslipRoles.Admin).GetAwaiter().GetResult();

            }
            return;

        }
       
    }
}
