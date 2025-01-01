using InvoiceGenerator.Common;
using InvoiceGenerator.Entity.ViewModels;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Text;

namespace InvoiceGenerator.Consumer.Consumers
{
    public class InvoicePdfGeneratorConsumer : IConsumer<InvoicePdfConsumerVm>
    {
        readonly ILogger<InvoicePdfGeneratorConsumer> _logger;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        public InvoicePdfGeneratorConsumer(
            ILogger<InvoicePdfGeneratorConsumer> logger,
            IOptions<AppSettings> appSettings,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
        }
        public async Task Consume(ConsumeContext<InvoicePdfConsumerVm> context)
        {
            try
            {
                _logger.LogInformation("Processing InvoiceId: {InvoiceId}", context.Message.InvoiceId);
                var content = new StringContent(context.Message.InvoiceId.ToString(), Encoding.UTF8, "application/json");
                _httpClient.BaseAddress = new Uri(_appSettings.InvoiceGeneratorHostApi.BaseUrl);
                HttpResponseMessage response = await _httpClient.PostAsync(_appSettings.InvoiceGeneratorHostApi.InvoicePdfSaverApi, content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Successfully processed InvoiceId: {InvoiceId}, Response: {Response}", context.Message.InvoiceId, responseBody);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while processing InvoiceId: {InvoiceId}", context.Message.InvoiceId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing InvoiceId: {InvoiceId}", context.Message.InvoiceId);
                throw;
            }
        }
    }
}
