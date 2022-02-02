using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task.app.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace task.app.Data
{
    public class Seed
    {
        /*
        readonly UserManager<User> userManager;
        readonly RoleManager<Role> roleManager;
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager){
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        */
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
                if(!userManager.Users.Any()){
                    var userDate = System.IO.File.ReadAllText("Data/UserSeedData.json");
                    var users = JsonConvert.DeserializeObject<List<User>>(userDate);
                
                    var roles = new List<Role>{
                        new Role{Name = "Admin"},
                        new Role{Name = "Member"},
                    };
                    foreach(var role in roles){
                        roleManager.CreateAsync(role).Wait();
                    }
                    foreach(var user in users){
                        user.Photos.SingleOrDefault().IsApproved = true;
                        userManager.CreateAsync(user, "password").Wait();
                        userManager.AddToRoleAsync(user, "Member").Wait();
                    }
                    var adminUser = new User{
                        UserName = "Admin"
                    };
                    IdentityResult identityResult = userManager.CreateAsync(adminUser, "password").Result;
                    if(identityResult.Succeeded){
                        var admin = userManager.FindByNameAsync("admin").Result;
                        userManager.AddToRoleAsync(user: admin, "Admin").Wait();
                    }
                }
        }
    }
}