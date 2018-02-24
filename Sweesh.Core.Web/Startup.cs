using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Sweesh.Core.Web
{
    using Configuration.Jwt;
    using Microsoft.Extensions.FileProviders;
    using StructureMap;
    using Swashbuckle.AspNetCore.Swagger;
    using Sweesh.Core.Configuration;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //Moved configruation and queries to seperate libraries for better modularity.
            //The get automatically placed in the application base directory instead of the Contant director.
            var fp = new PhysicalFileProvider(AppContext.BaseDirectory);

            //Build configuration from the base physical path
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(fp, "appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = JwtSecurityKey.Create(Configuration["Tokens:Key"])
                    };
                });

            services.AddMvc();

            //Include SwashBuckler
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Sweesh.Api",
                    Description = "The API layout for Sweesh"
                });
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            var sm = new Setup(Configuration)
                .Build();
            //Populate our services into structure map
            sm.Populate(services);
            return sm.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sweeh.Api");
                });

                app.UseSwagger();
            }

            //Setup default routes
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
        }
    }
}
