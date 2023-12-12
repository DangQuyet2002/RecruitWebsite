using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Core.Models;
using LuduStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Domain.Models
{
    public class Banner : Entity
    {
        public string FeaturedImage { get; set; }

        public List<MediaListItemVo> Media { get; set; }

        public string LinkURL { get; set; }

        public string TextButton { get; set; }

        public string Description { get; set; }
    }
}
