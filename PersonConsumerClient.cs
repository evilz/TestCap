using DotNetCore.CAP;

namespace Test.CAP;

public interface IPersonConsumerClient
{
    void CheckReceivedMessage(Guid id);
}

public class PersonConsumerClient : IPersonConsumerClient, ICapSubscribe
{
    [CapSubscribe("person.created")]
    public void CheckReceivedMessage(Guid id)
    {
        Console.WriteLine($"----------> Received 'person.created' message with id: {id}");
    }
}