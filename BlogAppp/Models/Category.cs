using System.ComponentModel.DataAnnotations;

namespace BlogAppp.Models{
    public class Category
    {
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
}