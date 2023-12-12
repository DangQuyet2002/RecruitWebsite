using LuduStack.Application.Interfaces;
using MetaG.Application.Interfaces;
using MetaG.Application.Services;
using MetaG.Data.Repository;
using MetaG.Data.Repository.Game;
using MetaG.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MetaG.Infra.CrossCutting.IoC
{
	public class MetaGInjectorBootStrapper
	{
		public static void RegisterServices(IServiceCollection services)
		{

			#region Game

			services.AddScoped<IGameCategoryRepository, MetaGameCategoryRepository>();
			services.AddScoped<IMetaGameAppService, MetaGameAppService>();
			services.AddScoped<IGameFeeRepository, GameFeeRepository>();
            services.AddScoped<IGameAppService, MetaGameAppService>();

            #endregion Game

            #region Banner

            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IBannerAppService, BannerAppService>();

            #endregion Banner
        }
    }
}