using AngleSharp.Css;
using LuduStack.Domain.Core.Enums;
using LuduStack.Infra.CrossCutting.Identity.Model;
using LuduStack.Infra.Data.MongoDb.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuduStack.Web.Extensions
{
	internal static class DbInitializerExtension
	{
		public static IApplicationBuilder UseMetaGSeedInitData(this IApplicationBuilder app)
		{
			ArgumentNullException.ThrowIfNull(app, nameof(app));

			using var scope = app.ApplicationServices.CreateScope();
			var services = scope.ServiceProvider;
			try
			{
				CreateUserRoles(services);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return app;
		}


		static async Task CreateUserRoles(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
			var roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();

			foreach (Roles role in roles)
			{				
				try
				{					
					var roleName = role.ToString();
					bool roleCheck = roleManager.Roles.FirstOrDefault(x => x.Name == roleName) != null;
					if (!roleCheck)
					{
						roleManager.CreateAsync(new Role(roleName)).Wait();
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}

			}
		}
	}
}
