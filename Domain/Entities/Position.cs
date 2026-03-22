using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Должность в компании
/// </summary>
public class Position : BaseEntity
{
    /// <summary>
    /// Название должность
    /// </summary>
    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание отдела
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Подразделение
    /// </summary>
    public Guid DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))] 
    public Department Department { get; set; } = null!;
    
    /// <summary>
    /// Сотрудники с должностью
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}