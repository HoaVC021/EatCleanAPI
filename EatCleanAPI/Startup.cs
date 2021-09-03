using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EatCleanAPI.Catalog.Services;
using EatCleanAPI.Handlers;
using EatCleanAPI.Models;
using EatCleanAPI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using FluentValidation.AspNetCore;
using EatCleanAPI.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EatCleanAPI
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
            // Adds services for controllers
            services.AddControllers();

            // Add NewJson Service
            // Add service cho các method trong controller. 
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //setting up authentication services 
            services.AddAuthentication("Authentication")
                    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);

            // Declare DI
            services.AddTransient<IUserService, UserService>();

            // Adds services for controllers
            services.AddControllers()
              .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());



            services.AddDbContext<VegafoodBotContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("Vegafood")));
            //Add service send mail
            services.AddTransient<IEmailSender, EmailSenderService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<IMailService, SendGridMailService>();

            var jwtSection = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtSection);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Vegafood API", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
            });


            //to validate the token which has been sent by clients
            var appSettings = jwtSection.Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                    //,ClockSkew = TimeSpan.Zero
                };
            });


            services.AddCors(options =>
            {
                options.AddPolicy("ExpensesPolicy",
                    builder =>
                    {
                        builder.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
             .AllowAnyMethod()
             .AllowAnyHeader()
             .SetIsOriginAllowed(origin => true) // allow any origin
             .AllowCredentials()); // allow credentials


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(ui =>
            {
                ui.SwaggerEndpoint("/swagger/v1.0/swagger.json", "EatClean API Endpoint");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


//using EatCleanAPI.Catalog.Services;
//using EatCleanAPI.Handlers;
//using EatCleanAPI.Models;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EatCleanAPI
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllers();

//            services.AddMvc(option => option.EnableEndpointRouting = false)
//               .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
//               .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

//            //setting up authentication services 
//            services.AddAuthentication("Authentication")
//                    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);

//            // Declare DI
//            services.AddTransient<IUserService, UserService>();

//            // Adds services for controllers
//            services.AddControllers()
//              .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());


//            services.AddDbContext<VegafoodBotContext>(options =>
//                     options.UseSqlServer(Configuration.GetConnectionString("Vegafood")));

//            var jwtSection = Configuration.GetSection("JWTSettings");
//            services.Configure<JWTSettings>(jwtSection);

//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Vegafood API", Version = "v1.0" });

//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
//                      Enter 'Bearer' [space] and then your token in the text input below.
//                      \r\n\r\nExample: 'Bearer 12345abcdef'",
//                    Name = "Authorization",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.ApiKey,
//                    Scheme = "Bearer"
//                });

//                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
//                  {
//                    {
//                      new OpenApiSecurityScheme
//                      {
//                        Reference = new OpenApiReference
//                          {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                          },
//                          Scheme = "oauth2",
//                          Name = "Bearer",
//                          In = ParameterLocation.Header,
//                        },
//                        new List<string>()
//                      }
//                    });
//            });

//            //to validate the token which has been sent by clients
//            var appSettings = jwtSection.Get<JWTSettings>();
//            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

//            services.AddAuthentication(x =>
//            {
//                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(x =>
//            {
//                x.RequireHttpsMetadata = true;
//                x.SaveToken = true;
//                x.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false
//                    //,ClockSkew = TimeSpan.Zero
//                };
//            });


//            services.AddCors(options =>
//            {
//                options.AddPolicy("ExpensesPolicy",
//                    builder =>
//                    {
//                        builder.WithOrigins("*")
//                        .AllowAnyHeader()
//                        .AllowAnyMethod();
//                    });
//            });


//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

//            app.UseHttpsRedirection();

//            app.UseRouting();

//            app.UseCors(x => x
//             .AllowAnyMethod()
//             .AllowAnyHeader()
//             .SetIsOriginAllowed(origin => true) // allow any origin
//             .AllowCredentials()); // allow credentials


//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseSwagger();

//            app.UseSwaggerUI(ui =>
//            {
//                ui.SwaggerEndpoint("/swagger/v1.0/swagger.json", "EatClean API Endpoint");
//            });

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }
//}
