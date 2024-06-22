using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayZe.Payment.Application.Services.Interfaces;
using PayZe.Payment.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Payment.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared;
using PayZe.Shared.Enums;

namespace PayZe.Payment.Application.Services;

public class ServiceA : IPaymentProcessor
{
    private readonly ILogger<ServiceA> _logger;
    private readonly IRepository<Domain.Aggregates.PaymentAggregate.Payment> _repository;
    private readonly IUnitOfWork _unitOfWork;
    public ServiceA(ILogger<ServiceA> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task ComputeOrder(ComputeOrderDto computeOrderDto, CancellationToken cancellationToken)
    {
        //todo change to 2000
        await Task.Delay(2, cancellationToken);
        _logger.LogInformation("Processed in ServiceA");
        var payment = await _repository.Query(x => x.UId == computeOrderDto.PaymentId)
            .Include(x => x.Order)
            .FirstAsync(cancellationToken: cancellationToken);
        if (Random.Shared.Next(100) > 50)
        {
            payment.UpdatePaymentStatus(OrderStatus.Completed, null, SystemDate.Now);
        }
        else
        {
            payment.UpdatePaymentStatus(OrderStatus.Failed, "some error occured something is wrong", SystemDate.Now);
        }

        await _unitOfWork.SaveAsync();
    }
}