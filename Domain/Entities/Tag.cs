using Domain.Entities.Common;

namespace Domain.Entities;

public class Tag : BaseEntity
{
    /// <summary>
    /// Название тега
    /// </summary>
    public string Name { get; set; }
}