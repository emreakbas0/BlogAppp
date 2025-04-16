using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace BlogAppp.Models;
public class Blog
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }

    public string? ImagePath { get; set; } 

    
    public string AppUserId { get; set; } = null!; 
    public AppUser AppUser { get; set; } = null!;

    public int CategoryId { get; set; } 
    public Category Category { get; set; } = null!;

    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}