using MassTransit;
using MonolithBoilerPlate.Service.Consumers;
using MonolithBoilerPlate.Common;
using Microsoft.Extensions.Options;

namespace MonolithBoilerPlate.Api.Helper.Extensions
{
    public static class MasstransitConfig
    {
        public static IServiceCollection AddMasstransitConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMassTransit(m =>
            {
                m.AddConsumer<InvoicePdfGeneratorConsumer>();
                m.AddConsumer<InvoiceSyncConsumer>();

                m.UsingRabbitMq((context, cfg) =>
                {
                    var appSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<AppSettings>>().Value;
                    var connection = appSettings.RabbitMq;
                    var queueName = appSettings.MessagingQueueName;

                    cfg.PrefetchCount = connection.PrefetchCount;
                    cfg.Host(connection.Host, connection.VirtualHost, _ =>
                    {
                        _.Username(connection.UserName);
                        _.Password(connection.Password);
                    });


                    cfg.ReceiveEndpoint(queueName.InvoicePdfGeneratorQueue, e =>
                    {
                        e.ConfigureConsumer<InvoicePdfGeneratorConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(queueName.InvoiceSyncQueue, e =>
                    {
                        e.ConfigureConsumer<InvoiceSyncConsumer>(context);
                    });

                });
            });

            return services;
        }
    }
}
