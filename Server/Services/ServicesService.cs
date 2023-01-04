using InternalAPI;
using Grpc.Net.Client;
using ByteString = Google.Protobuf.ByteString;
using UberClient.Services;

namespace Services.ServicesService
{
    public class ServicesService : InternalAPI.Services.ServicesClient , IServicesService , IHostedService
    {
        private readonly InternalAPI.Services.ServicesClient _services;
        private readonly ILogger<ServicesService> _logger;

        public ServicesService(InternalAPI.Services.ServicesClient services, ILogger<ServicesService> logger)
        {
            _services = services;
            _logger = logger;
        }

        private async Task RegisterService(string id, string name, IEnumerable<ServiceFeatures> features)
        {
            var request = new RegisterServiceRequest
            {
                Id = ByteString.CopyFrom(Guid.Parse(id).ToByteArray()),
                Name = name,
                ClientName = "Uber",
            };

            foreach (var feature in features)
            {
                request.Features.Add(ServiceFeatures.ProfessionalDriver);
            }

            _logger.LogDebug($"[UberClient::ServicesService::RegisterServiceRequest] Registering [{name}] service...");
            await _services.RegisterServiceAsync(request);
        }

        public async Task RegisterServiceRequest()
        {
            _logger.LogInformation("[UberClient::ServicesService::RegisterServiceRequest] Registering services...");

            RegisterService("d4abaae7-f4d6-4152-91cc-77523e8165a4", "UberBlack", new ({
                ServiceFeatures.ProfessionalDriver
            }));

            RegisterService("26546650-e557-4a7b-86e7-6a3942445247", "UberPOOL", new ({
                ServiceFeatures.Shared
            }));

            RegisterService("2d1d002b-d4d0-4411-98e1-673b244878b2", "UberX", new ({
                ServiceFeatures.ProfessionalDriver
            }));

            _logger.LogInformation("[UberClient::ServicesService::RegisterServiceRequest] Services Registeration complete.");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RegisterServiceRequest();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
