using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BlogAppp.Models;

public class AppUser : IdentityUser
{
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}