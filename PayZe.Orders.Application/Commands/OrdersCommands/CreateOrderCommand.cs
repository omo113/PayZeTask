using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PayZe.Orders.Application.Dtos.OrdersDtos;
using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Orders.Domain.Aggregates.OrderAggregate;
using PayZe.Orders.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Orders.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared;
using PayZe.Shared.ApplicationInfrastructure;

namespace PayZe.Orders.Application.Commands.OrdersCommands;

public record CreateOrderCommand(OrderDto Model) : IRequest<ApplicationResult<OderDetailsDto, ApplicationError>>;


public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(IRepository<Company> companyRepository, CurrencyDictionary currencyDictionary)
    {
        RuleFor(x => x.Model.Currency)
            .NotEmpty()
            .NotNull()
            .Must(x => currencyDictionary.Currencies.Keys.Contains(x))
            .WithMessage("not a valid iso format currency");

        RuleFor(x => x.Model.Amount)
            .NotNull()
            .NotEmpty()
            .Must(x => x > 0)
            .WithMessage("amount must be more than 0");

        RuleFor(x => x.Model.CompanyId)
            .NotEmpty()
            .NotNull()
            .MustAsync(async (compId, token) =>
            {
                return await companyRepository.Query(x => x.UId == compId).AnyAsync();
            })
            .WithMessage("this kind of company does not exist");
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApplicationResult<OderDetailsDto, ApplicationError>>
{
    private readonly IRepository<Order> _repository;
    private readonly IRepository<Company> _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IRepository<Order> repository, IRepository<Company> companyRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ApplicationResult<OderDetailsDto, ApplicationError>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.Query(x => x.UId == request.Model.CompanyId).FirstAsync(cancellationToken: cancellationToken);
        var order = Order.CreateOrder(company, request.Model.Amount, request.Model.Currency);
        await _repository.Store(order);
        await _unitOfWork.SaveAsync();
        return new ApplicationResult<OderDetailsDto, ApplicationError>(
            new OderDetailsDto(order.UId, company.UId, order.Currency, order.Amount, order.CreateDate));
    }
}