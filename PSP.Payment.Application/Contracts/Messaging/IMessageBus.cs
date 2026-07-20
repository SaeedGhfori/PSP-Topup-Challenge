using System;
using System.Collections.Generic;
using System.Text;

namespace PSP.Payment.Application.Contracts.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(
            T message,
            CancellationToken cancellationToken = default);
    }
}
