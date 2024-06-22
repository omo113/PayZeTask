using PayZe.Shared.Enums;
using System.Threading.Channels;

namespace PayZe.Payment.Application.Services;


public record PaymentToProcess(string Currency, decimal Amount, string CardNumber, DateOnly ExpiryDate, Guid PaymentId, ProcessingService ProcessingService);
public class PaymentToProcessChannel
{
    private readonly Channel<PaymentToProcess> _jobChannel;
    public ChannelReader<PaymentToProcess> Reader => _jobChannel.Reader;

    public PaymentToProcessChannel()
    {
        _jobChannel = Channel.CreateUnbounded<PaymentToProcess>();
    }

    public async Task EnqueueJobAsync(PaymentToProcess item)
    {
        await _jobChannel.Writer.WriteAsync(item);
    }
}
