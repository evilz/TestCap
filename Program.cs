using DotNetCore.CAP;
using Savorboard.CAP.InMemoryMessageQueue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.CAP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PersonContext>(x =>
    x.UseSqlite("Data Source=./person.db"));

builder.Services.AddTransient<IPersonConsumerClient,PersonConsumerClient>();
builder.Services.AddCap(x =>
{
    x.FailedRetryInterval = 3;
    x.FailedRetryCount = 5;
    x.SucceedMessageExpiredAfter = 10;
    x.CollectorCleaningInterval = 5;
    
    //x.UseInMemoryStorage();
    x.UseInMemoryMessageQueue();

    x.UseEntityFramework<PersonContext>();
    
    // un comment to make it failed
    //.UseKafka(opt=>{});
   
    // Register Dashboard
    x.UseDashboard(x=>x.StatsPollingInterval = 1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("publishevent", ([FromServices] ICapPublisher capBus, [FromServices] PersonContext dbContext ) =>
{
    var id = Guid.Empty;
    using (var trans = dbContext.Database.BeginTransaction(capBus, autoCommit: false))
    {
        var person = dbContext.People.Add(new Person()
        {
            Firstname = "ef.transaction", Lastname = "test"
        });

        capBus.Publish("person.created", person.Entity.Id);

        dbContext.SaveChanges();
        trans.Commit();
        id = person.Entity.Id;
    }
    return new{id};
});

// Clean sqlite db
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<PersonContext>();
    dbContext!.Database.EnsureDeleted();
    dbContext!.Database.EnsureCreated();
}
app.Run();