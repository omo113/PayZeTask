using MassTransit;
using PayZe.Contracts.Events;

namespace PayZe.Identity.Application.Consumers;



public class CompanyCreatedConsumer : IConsumer<CompanyCreatedEvent>
{
    public Task Consume(ConsumeContext<CompanyCreatedEvent> context)
    {
        Console.WriteLine();
        return Task.CompletedTask; ;
    }
}