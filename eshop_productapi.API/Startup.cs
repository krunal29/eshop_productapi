using eshop_productapi.Business.Helpers;
using eshop_productapi.Business.ViewModels.Account;
using eshop_productapi.Domain;
using eshop_productapi.Interfaces.Background;
using eshop_productapi.Interfaces.Repositories;
using eshop_productapi.Interfaces.Services;
using eshop_productapi.Mail;
using eshop_productapi.Repositories;
using eshop_productapi.Services;
using eshop_productapi.UoW;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using Microsoft.Extensions.FileProviders;
using FluentMigrator.Runner;
using System.Reflection;
using eshop_productapi.Migrations.DbMigrations;
using AAT.API.Filters;
using eshop_productapi.Domain.Models;

namespace eshop_productapi.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<eshop_productapiContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<eshop_productapiContext>().AddDefaultTokenProviders();
            services.AddDistributedMemoryCache();
            services.AddAutoMapper(c => c.AddProfile<MapperConfiguration>(), typeof(Startup));

            RegisterRequestLocalizationOptions(services);
            RegisterNewtonsoftJson(services);
            RegisterJwt(services);
            RegisterDI(services);
            RegisterHangfire(services);
            RegisterSwagger(services);
            RegisterCors(services);
            RegisterFluentMigration(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "v1/swagger.json", name: "API V1");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (context =>
                {
                    context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                }),
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
                RequestPath = "/uploads",
                ServeUnknownFileTypes = true
            });

            //Enable directory browsing
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
                RequestPath = "/uploads"
            });

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                AppPath = null,
                DashboardTitle = "Hangfire Dashboard",
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
            app.Migrate();
            AppSettings.Initialize(Configuration);
            MailSettings.Initialize(Configuration);
            MemoryCacheHelper.Initialize();
            Jwt.Initialize(Configuration);
        }

        private static void RegisterDI(IServiceCollection services)
        {
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IFileHelper, FileHelper>();
            RegisterServices(services);
            RegisterRepositories(services);
            BackgroundServices(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IRoleModuleService, RoleModuleService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IAccessModuleRepository, AccessModuleRepository>();
            services.AddScoped<IRoleModuleRepository, RoleModuleRepository>();
        }

        private static void BackgroundServices(IServiceCollection services)
        {
            services.AddScoped<IBackgroundService, Services.BackgroundService>();
            services.AddScoped<IBackgroundMailerJobs, BackgroundMailerJobs>();
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "V1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
            });
        }

        private static void RegisterJwt(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = new Jwt().Issuer,
                    ValidAudience = new Jwt().Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new Jwt().Key))
                };
            });
        }

        private void RegisterHangfire(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }).WithJobExpirationTimeout(TimeSpan.FromDays(7)));
            services.AddHangfireServer();
        }

        private void RegisterCors(IServiceCollection services)
        {
            var webUrl = Configuration.GetSection("Urls:FrontEnd").Value;
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder
                    .WithOrigins(webUrl)
                    //.SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    );
            });
        }

        private void RegisterFluentMigration(IServiceCollection services)
        {
            services.AddLogging(c => c.AddFluentMigratorConsole())
                    .AddFluentMigratorCore()
                    .ConfigureRunner(c => c
                        .AddSqlServer2012()
                        .WithGlobalConnectionString(Configuration.GetConnectionString("DefaultConnection"))
                        .ScanIn(Assembly.GetAssembly(typeof(MigrationExtension))));
        }

        private static void RegisterNewtonsoftJson(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            });
        }

        private static void RegisterRequestLocalizationOptions(IServiceCollection services)
        {
            services.AddLocalization(opt => { opt.ResourcesPath = "Resource"; });
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(
                            opt =>
                            {
                                var supportedCulters = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("fr"),
                            };
                                opt.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                                opt.SupportedCultures = supportedCulters;
                                opt.SupportedUICultures = supportedCulters;
                            });
        }
    }
}