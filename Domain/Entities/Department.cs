using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Подразделение в компании
/// </summary>
public class Department : BaseEntity
{
    /// <summary>
    /// Название подразделения
    /// </summary>
    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание подразделения
    /// </summary>
    public string? Description { get; set; }
}