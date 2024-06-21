using MassTransit;
using PayZe.Contracts.Events;
using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Orders.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Orders.Infrastructure.UnitOfWork.Abstractions;

namespace PayZe.Orders.Application.Consumers;

public class CompanyCreatedConsumer : IConsumer<CompanyCreatedEvent>
{
    private readonly IRepository<Company> _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyCreatedConsumer(IRepository<Company> companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<CompanyCreatedEvent> context)
    {
        var company = Company.CreateCompany(context.Message.UId, context.Message.CompanyName);
        await _companyRepository.Store(company);
        await _unitOfWork.SaveAsync();
    }
}