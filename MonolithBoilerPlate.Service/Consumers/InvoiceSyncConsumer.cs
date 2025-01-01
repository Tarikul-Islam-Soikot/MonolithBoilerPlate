using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Entity.ViewModels;
using MonolithBoilerPlate.Service.Helper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace MonolithBoilerPlate.Service.Consumers
{
    public class InvoiceSyncConsumer: IConsumer<InvoiceSyncConsumerVm>
    {
        readonly ILogger<InvoiceSyncConsumer> _logger;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        public InvoiceSyncConsumer(
            ILogger<InvoiceSyncConsumer> logger,
            IOptions<AppSettings> appSettings,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
        }
        public async Task Consume(ConsumeContext<InvoiceSyncConsumerVm> context)
        {
            try
            {
                _logger.LogInformation("InvoiceSyncConsumer execution starts at {Time}", DateTime.Now);

                _logger.LogInformation("Authentication intialize");
                _httpClient.BaseAddress = new Uri(_appSettings.InvoiceGeneratorHostApi.BaseUrl);
                var accessToken = await _httpClient.GetAccessTokenAsync(_appSettings);
                var accessTokenWithType = $"Bearer {accessToken.AccessToken}";
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(accessTokenWithType);
                _logger.LogInformation("Received token: {Token}", accessToken);

                _logger.LogInformation("Processing InvoiceId: {InvoiceId}", context.Message.InvoiceId);
                var content = new StringContent(context.Message.InvoiceId.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(_appSettings.InvoiceGeneratorHostApi.InvoiceSyncApi, content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Successfully Processed InvoiceId: {InvoiceId}, Response: {Response}", context.Message.InvoiceId, responseBody);
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
