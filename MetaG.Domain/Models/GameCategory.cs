using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Core.Models;

namespace MetaG.Domain.Models
{
	public class GameCategory : Entity
	{
		public Guid ParentId { get; set; }
		public GameGenre Genre { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public string? CoverImageUrl { get; set; }
		public string? LargeThumbImageUrl { get; set; }
		public string? IconUrl { get; set; }
		public bool IsHidden { get; set; }
		public int SortOrder { get; set; }
	}
}