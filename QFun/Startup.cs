using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using QFun.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QFun.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebPWrecover.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;
using QFun.Data.DbTables;

namespace QFun
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ContributionServices>();
            services.AddScoped<ChallengeServices>();
            services.AddScoped<VoteServices>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(5));

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddRazorPages();


            services.AddAuthentication() //Google auth
               .AddGoogle(options =>
               {
                   IConfigurationSection googleAuthNSection =
                   Configuration.GetSection("Authentication:Google");

                   options.ClientId = googleAuthNSection["ClientId"];
                   options.ClientSecret = googleAuthNSection["ClientSecret"];
                   options.CallbackPath = "/signin-google";
               });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.";
                options.User.RequireUniqueEmail = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                     name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id1?}/{id2?}");
                endpoints.MapRazorPages();
            });
        }

        public class CustomEmailConfirmationTokenProvider<TUser>
                                       : DataProtectorTokenProvider<TUser> where TUser : class
        {
            public CustomEmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
                IOptions<EmailConfirmationTokenProviderOptions> options,
                ILogger<DataProtectorTokenProvider<TUser>> logger)
                                                  : base(dataProtectionProvider, options, logger)
            {

            }
        }
        public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
        {
            public EmailConfirmationTokenProviderOptions()
            {
                Name = "EmailDataProtectorTokenProvider";
                TokenLifespan = TimeSpan.FromHours(4);
            }
        }
    }
}
