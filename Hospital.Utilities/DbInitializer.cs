using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace Hospital.Utilities
{
    public class DbInitializer: IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
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
                if(_context.Database.GetPendingMigrations().Count()>0)
                {
                    _context.Database.Migrate();
                }
            }catch (Exception ex)
            {
                throw ex;
            }

            if (!_roleManager.RoleExistsAsync(WebSiteRoles.Website_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Doctor)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Durjoy",
                    Email = "durjoy6405@gmail.com"
                }, "durjoy6405").GetAwaiter().GetResult();

                var Appuser = _context.ApplicationUsers.FirstOrDefault(x=>x.Email == "durjoy6405@gmail.com");
                if (Appuser != null)
                {
                    _userManager.AddToRolesAsync(Appuser, WebSite_Admin).GetAwaite().GetResult();
                }
            }
        }
    }
}
