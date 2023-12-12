using LuduStack.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuduStack.Domain.Models
{
    public class GameComment : Entity
    {
        public Guid? ParentCommentId { get; set; }

        public Guid UserGameId { get; set; }

        public string Text { get; set; }
        public string StarRating { get; set; }

    }
}
