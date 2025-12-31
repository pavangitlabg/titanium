using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static ISSB.Helpers.MailBoxServices;
using ISSB.Helpers;
using ISSB.Hubs;
using jsreport.Client;
using jsreport.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.HttpOverrides;

namespace ISSB
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
           
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
               
            });

            var jsreportService = new ReportingService("http://steelstats.issb.co.uk:8081", "admin", "Letmein2019");
            jsreportService.HttpClientTimeout = TimeSpan.FromMinutes(20);
            
            services.AddJsReport(jsreportService);

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddSessionStateTempDataProvider();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSession();

            services.AddHostedService<ReportRunService>();

            services.AddSingleton<IHostedService, MailBoxService>();
            services.AddSingleton<IHostedService, ImportService>();
            services.AddSingleton<IHostedService, OpenExchangeService>();
            //services.AddCors();
            services.AddSignalR();

            services.AddSingleton<IMessage, Message>();

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "smart";
            })
            .AddPolicyScheme("smart", "Bearer or Jwt", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    var bearerAuth = context.Request.Headers["Authorization"].FirstOrDefault()?.StartsWith("Bearer ") ?? false;
                    // You could also check for the actual path here if that's your requirement:
                    // eg: if (context.HttpContext.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCulture))
                    if (bearerAuth)
                        return JwtBearerDefaults.AuthenticationScheme;
                    else
                        return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                 options.AccessDeniedPath = new PathString("/Auth/Login");
                 options.LoginPath = new PathString("/Auth/Login");
                 options.LogoutPath = new PathString("/Auth/Login");
                 options.Cookie.Name = "CustomerPortal.Identity";
                 options.SlidingExpiration = true;
                 options.ExpireTimeSpan = TimeSpan.FromDays(1);
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            //services.AddSwaggerGen(c => { c.EnableAnnotations(); });
            services.AddSwaggerGen(setup =>
            {
                setup.EnableAnnotations();
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // need to check this for security
            //app.UseCors(builder => builder

            //        .WithOrigins("null")
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials()

            //);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
              
                endpoints.MapHub<MessageHub>("/messageHub");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Uncomment this to run on the server
#if RELEASE

            app.UseFileServer(
                  new FileServerOptions()
                  {
                      FileProvider = new PhysicalFileProvider("/var/www/build/wwwroot")
                  });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

#endif
            app.UseCookiePolicy();

            app.UseAuthentication();
           // app.UseAuthorization();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseStatusCodePages();
        }
    }
}
