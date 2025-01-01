using MonolithBoilerPlate.Common.Interface;
using MassTransit;


namespace MonolithBoilerPlate.Common.Helpers
{
    public class MessageBrokerHelper : IMessageBrokerHelper
    {
        private readonly IBus _publishEndpoint;

        public MessageBrokerHelper(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message) where T : class
            => await _publishEndpoint.Publish<T>(message);

        public async Task PublishAsync<T>(object message) where T : class
            => await _publishEndpoint.Publish<T>(message);
    }
}
