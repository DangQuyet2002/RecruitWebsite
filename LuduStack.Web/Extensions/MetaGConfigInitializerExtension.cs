using AngleSharp;
using LuduStack.Application;
using LuduStack.Application.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LuduStack.Web.Extensions
{
	internal static class MetaGConfigInitializerExtension
	{
		public static ConfigurationManager UseMetaGConfiguration(this ConfigurationManager configuration)
		{
			ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

			try
			{
				var myOptions = new ConfigOptions();
				myOptions.FacebookAppId = configuration["Authentication:Facebook:AppId"];
				myOptions.ReCaptchaSiteKey = configuration["ReCaptcha:SiteKey"];
				myOptions.CloudinaryUrl = configuration["CLOUDINARY_URL"];

				ConfigHelper.SetConfigOptions(myOptions);
			}
			catch (Exception ex)
			{

			}

			return configuration;
		}
	}
}
