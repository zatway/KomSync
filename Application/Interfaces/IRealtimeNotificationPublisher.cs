namespace Application.Interfaces;



/// <summary>Отправка событий конкретному пользователю (SignalR группа user_{id}).</summary>

public interface IRealtimeNotificationPublisher

{

    Task PublishToUserAsync(Guid userId, string topic, object payload, CancellationToken cancellationToken = default);

}

