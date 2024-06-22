using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PayZe.Payment.Application.Dtos.PaymentDtos;
using PayZe.Payment.Domain.Aggregates.OrderAggregate;
using PayZe.Payment.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Payment.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.ApplicationInfrastructure;
using PayZe.Shared.Enums;

namespace PayZe.Payment.Application.Commands.PaymentCommands;

public record CreatePaymentCommand(Guid OrderId, PaymentDto Model) : IRequest<ApplicationResult<PaymentDetailsDto, ApplicationError>>;


public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator(IRepository<Order> orderRepository
        , IRepository<Domain.Aggregates.PaymentAggregate.Payment> paymentRepository)
    {
        RuleFor(x => x.OrderId)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (x, token) =>
            {
                return await orderRepository.Query(ord => ord.UId == x).AnyAsync();
            })
            .WithMessage("this kind of order does not exist")
            .DependentRules(() =>
            {
                RuleFor(x => x.OrderId)
                    .MustAsync(async (x, token) =>
                    {
                        var ord = await orderRepository.Query(ord => ord.UId == x).FirstAsync();
                        return !await paymentRepository.Query(payment => payment.OrderId == ord.Id).AnyAsync();
                    })
                    .WithMessage("payment for this order id already exists")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Model.ExpiryDate)
                            .NotNull()
                            .NotEmpty()
                            .Must(x => x.CompareTo(DateTime.Now) > 0)
                            .WithMessage("card is already expired");
                        RuleFor(x => x.Model.CardNumber)
                            .NotNull()
                            .NotEmpty()
                            .Matches(@"^\d{16}$")
                            .WithMessage("The Card number must be exactly 16 digits long.");
                    });

            });
    }
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ApplicationResult<PaymentDetailsDto, ApplicationError>>
{
    private readonly IRepository<Domain.Aggregates.PaymentAggregate.Payment> _repository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(IRepository<Domain.Aggregates.PaymentAggregate.Payment> repository, IUnitOfWork unitOfWork, IRepository<Order> orderRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<ApplicationResult<PaymentDetailsDto, ApplicationError>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var isOdd = int.Parse(request.Model.CardNumber[^1].ToString()) % 2 == 1;
        var order = await _orderRepository.Query(x => x.UId == request.OrderId).FirstAsync(cancellationToken: cancellationToken);
        var payment = Domain.Aggregates.PaymentAggregate.Payment.CreatePayment(order, request.Model.CardNumber,
            DateOnly.FromDateTime(request.Model.ExpiryDate.DateTime), isOdd ? ProcessingService.ServiceA : ProcessingService.ServiceB, OrderStatus.Processing);
        await _repository.Store(payment);
        await _unitOfWork.SaveAsync();
        return new ApplicationResult<PaymentDetailsDto, ApplicationError>(new PaymentDetailsDto(payment.UId, request.Model.ExpiryDate,
            payment.CardNumber, request.OrderId, OrderStatus.Processing, null));
    }
}