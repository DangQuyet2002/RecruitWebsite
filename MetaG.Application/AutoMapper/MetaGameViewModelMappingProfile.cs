using AutoMapper;
using LuduStack.Domain.Models;
using MetaG.Application.ViewModels;
using MetaG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Application.AutoMapper
{
	public class MetaGameViewModelMappingProfile : Profile
	{
		public MetaGameViewModelMappingProfile() {

			CreateMap<GameCategory, GameCategoryViewModel>();
            CreateMap<GameCategoryViewModel, GameCategory>();
            CreateMap<GameFeeViewModel, GameFee>();
            CreateMap<GameFee,GameFeeViewModel>();
            CreateMap<BannerViewModel, Banner>();
            CreateMap<Banner,BannerViewModel>();
            CreateMap<Game, GameFeeViewModel>();
            CreateMap<GameFeeViewModel,Game>();






        }
    }
}
