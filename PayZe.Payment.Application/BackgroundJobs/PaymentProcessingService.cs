using Microsoft.Extensions.Hosting;
using PayZe.Payment.Application.Services;
using PayZe.Payment.Application.Services.Interfaces;
using PayZe.Shared.Enums;

namespace PayZe.Payment.Application.BackgroundJobs;

public class PaymentProcessingService : BackgroundService
{
    private readonly PaymentToProcessChannel _paymentToProcessChannel;
    private readonly IServiceProvider _serviceProvider;

    public PaymentProcessingService(PaymentToProcessChannel paymentToProcessChannel, IServiceProvider serviceProvider)
    {
        _paymentToProcessChannel = paymentToProcessChannel;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in _paymentToProcessChannel.Reader.ReadAllAsync(stoppingToken))
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var paymentProcessor = item.ProcessingService switch
                {
                    ProcessingService.ServiceA => scope.ServiceProvider.GetKeyedService<IPaymentProcessor>("ServiceA"),
                    ProcessingService.ServiceB => scope.ServiceProvider.GetKeyedService<IPaymentProcessor>("ServiceB"),
                    _ => throw new ArgumentOutOfRangeException()
                };
                await paymentProcessor!.ComputeOrder(new ComputeOrderDto(item.PaymentId, item.Amount, item.CardNumber, item.Currency, item.ExpiryDate), stoppingToken);
            }, stoppingToken);
        }
    }
}