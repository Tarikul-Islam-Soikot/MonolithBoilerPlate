

namespace MonolithBoilerPlate.Common.Interface
{
    public interface IMessageBrokerHelper
    {
        Task PublishAsync<T>(T message) where T : class;
        Task PublishAsync<T>(object message) where T : class;
    }
}
