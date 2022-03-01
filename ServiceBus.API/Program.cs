using SendGrid.Extensions.DependencyInjection;
using SerivceBus.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EmailNotificationMessageConsumer>();
builder.Services.AddSendGrid(options=>options.ApiKey = builder.Configuration.GetSection("Notification:SendGridAPIKey").Value);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var bus = app.Services.GetService<EmailNotificationMessageConsumer>();
bus?.RegisterHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
