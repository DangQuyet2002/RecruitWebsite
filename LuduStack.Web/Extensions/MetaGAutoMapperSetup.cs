using LuduStack.Application.AutoMapper;
using MetaG.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LuduStack.Web.Extensions
{
	public static class MetaGAutoMapperSetup
	{
		public static void AddMetaGAutoMapperSetup(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}
			services.AddAutoMapper(typeof(MetaGameViewModelMappingProfile));

			// Registering Mappings automatically only works if the
			// Automapper Profile classes are in ASP.NET project
			MetaGAutoMapperConfig.RegisterMappings();
		}
	}
}