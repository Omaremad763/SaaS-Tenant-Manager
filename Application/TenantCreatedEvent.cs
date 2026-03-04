using MediatR;

namespace Application;
public record TenantCreatedEvent(Guid TenantId, string ConnectionString) : INotification;
