using LuduStack.Application.Interfaces;
using LuduStack.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Application.ViewModels
{
    public class BannerViewModel : UserGeneratedBaseViewModel
    {
        public DateTime PublishDate { get; set; }

        [Display(Name = "Featured Image")]
        public string FeaturedImage { get; set; }
        public string FeaturedImageResponsive { get; set; }
        public string FeaturedImageLquip { get; set; }

        [StringLength(128)]
        [Display(Name = "TextButton")]
        public string TextButton { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "LinkURL")]
        public string LinkURL { get; set; }
        public bool HasFeaturedImage { get; set; }

        public bool IsComplex => HasFeaturedImage;
    }
}
