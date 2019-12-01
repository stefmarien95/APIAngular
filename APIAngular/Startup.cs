﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIAngular.Helpers;
using APIAngular.Models;
using APIAngular.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace APIAngular
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
			services.AddCors(o => o.AddPolicy("MyPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));


			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new ApiKeyScheme 
				{ 
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"", 
					Name = "Authorization", 
					In = "header", 
					Type = "apiKey" 
					}); 
					c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> 
					{ { "Bearer", Enumerable.Empty<string>() }, });
			});



				services.AddDbContext<PollContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
				services.AddCors(
					o => o.AddPolicy("MyPolicy", builder =>
					{ builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); 
				}));

				services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
				var appSettingsSection = Configuration.GetSection("AppSettings"); 
				services.Configure<AppSettings>(appSettingsSection);


				var appSettings = appSettingsSection.Get<AppSettings>(); 
				var key = Encoding.ASCII.GetBytes(appSettings.Secret); 
				services.AddAuthentication(x => {
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(x => { x.RequireHttpsMetadata = false; x.SaveToken = true; x.TokenValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateIssuer = false, ValidateAudience = false }; });// configure DI for application services
				services.AddScoped<IUserService, UserService>();
			}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, PollContext context)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poll API v1"); 
			
			});
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseCors("MyPolicy");
			app.UseMvc();
			DBinitializer.Initialize(context);

		}
	}
}
	
