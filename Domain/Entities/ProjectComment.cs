using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class ProjectComment : BaseEntity
{
    /// <summary>
    /// Проект
    /// </summary>
    [Required]
    public Guid ProjectId { get; set; }     
    
    [ForeignKey(nameof(ProjectId))] 
    public Project Project { get; set; } = null!;
    
    /// <summary>
    /// Текст комментария.
    /// </summary>
    [Required, MaxLength(200)]
    public string Content { get; set; }         
    
    /// <summary>
    /// Родительский комментраий
    /// </summary>
    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public ProjectComment? Parent { get; set; }

    public ICollection<ProjectComment> Children { get; set; } = new List<ProjectComment>();
    
    /// <summary>
    /// Автор
    /// </summary>
    public Guid AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))] 
    public User Author { get; set; } = null!;
}