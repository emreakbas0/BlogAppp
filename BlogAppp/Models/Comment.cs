using System;
using System.ComponentModel.DataAnnotations;
using BlogAppp.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    
    public string AppUserId { get; set; } = string.Empty;
    public AppUser AppUser { get; set; } = null!;

    
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;


}