using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Speurzoekers.Common.Helper;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Data.Entities.User;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataSeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting to seed");

                ServiceCollection serviceCollection = new ServiceCollection();
                string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                SpeurzoekersDbConnection con = new SpeurzoekersDbConnection(basePath);

                serviceCollection.AddDbContext<SpeurzoekersDbContext>(con.SetSqlConnection);
                serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>()
                            .AddEntityFrameworkStores<SpeurzoekersDbContext>()
                            .AddDefaultTokenProviders();

                ServiceProvider provider = serviceCollection.BuildServiceProvider();
                SpeurzoekersDbContext SpeurzoekersDbContext = provider.GetService<SpeurzoekersDbContext>();

                con.CheckSqlConnection(SpeurzoekersDbContext);

                SpeurzoekersDbContext.Database.EnsureDeleted();
                SpeurzoekersDbContext.Database.Migrate();

                Console.WriteLine("Removed and migrated database");

                var userManager = provider.GetService<UserManager<ApplicationUser>>();
                var roleManager = provider.GetService<RoleManager<ApplicationRole>>();

                RegionSeeder.SeedRegion(SpeurzoekersDbContext).Wait();
                CollectibleSeeder.SeedAvatars(SpeurzoekersDbContext).Wait();
                CollectibleSeeder.SeedCollectibleTypes(SpeurzoekersDbContext).Wait();
                ActivitySeeder.SeedActivities(SpeurzoekersDbContext).Wait();
                CreateAuthentication(roleManager, userManager).Wait();

                SpeurzoekersDbContext.SaveChanges();
                SpeurzoekersDbContext.Dispose();

                Console.WriteLine("Done seeding data. Press any key to continue.");
            }
            catch (SpeurzoekersDbConnection.SpeurzoekersDbConException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format(Messages.CriticalStartError, ex.Message));
                Console.ResetColor();
            }

            Console.ReadKey();
        }

        private static async Task CreateAuthentication(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "Admin", NormalizedName = "Administrator" });

            await CreateUser(userManager, "Janneke", "admin@test.com", "Test@123", 2, 3, true);
            await CreateUser(userManager, "Jip", "test@test.com", "Test@123", 1, 1);
        }

        private static async Task CreateUser(
            UserManager<ApplicationUser> userManager,
            string displayName,
            string email,
            string password,
            ulong avatarIndex,
            ulong collectedAvatars,
            bool isAdmin = false)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                UserInfo = new UserInfoEntity
                {
                    DisplayName = displayName,
                    ActiveAvatarIndex = (long)avatarIndex,
                    CollectedAvatars = (long)collectedAvatars
                }
            };

            await userManager.CreateAsync(user, password);

            if (isAdmin)
            {
                user = await userManager.Users.FirstAsync(u => u.Email == email);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
