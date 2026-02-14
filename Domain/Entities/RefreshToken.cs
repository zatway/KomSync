using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Токен обновления сессии.
/// </summary>
public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    /// <summary> Значение токена. </summary>
    [Required, MaxLength(512)]
    public string Token { get; set; } = null!;

    /// <summary> Срок действия. </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary> Флаг отзыва токена. </summary>
    public bool IsRevoked { get; set; }

    /// <summary> Внешний ключ пользователя. </summary>
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}