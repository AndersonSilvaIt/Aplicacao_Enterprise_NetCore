﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
	public static class JwtConfig
	{
		public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var appSettingsSection = configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(bearesOptions =>
			{
				bearesOptions.RequireHttpsMetadata = true;
				bearesOptions.SaveToken = true;
				bearesOptions.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("x")),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = appSettings.ValidoEm,
					ValidIssuer = appSettings.Emissor
				};
			});
		}

		public static void UseAuthConfiguration(this IApplicationBuilder app)
		{
			app.UseAuthentication();
			app.UseAuthorization();
		}
	}
}
