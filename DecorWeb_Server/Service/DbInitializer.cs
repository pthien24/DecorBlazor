using Decor_Common;
using Decor_DataAccess.Data;
using DecorWeb_Server.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DecorWeb_Server.Service
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,ApplicationDBContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                }
                else
                {
                    return;
                }
                IdentityUser user = new IdentityUser()
                {
                    UserName = "npthien1409@gmail.com",
                    Email = "npthien1409@gmail.com",
                    EmailConfirmed = true
                };
                _userManager.CreateAsync(user, "Admin123@").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user,SD.Role_Admin).GetAwaiter().GetResult();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
