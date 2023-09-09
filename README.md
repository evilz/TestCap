# Test project of CAP

### Cap

CAP is a library based on .net standard, which is a solution to deal with distributed transactions, also has the function of EventBus, it is lightweight, easy to use, and efficient

https://cap.dotnetcore.xyz/

### Detail

This is .NET 8.0 project.
You can run with dotnet run

It use Entity Framework core on SQLite local database.
It use Inmemory queue.

You can use Swagger to push a message in the memory queue
what will be consume by the PersonConsumerClient.

Change queue system or consumer to see effect of the Inbox/outbox pattern
