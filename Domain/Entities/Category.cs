using Domain.Entities.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    /// <summary>
    /// Название категории
    /// </summary>
    public string Name { get; set; }
}