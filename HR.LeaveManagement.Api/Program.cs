using HR.LeaveManagement.Application;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddCors(options =>
    options.AddPolicy("all", corsPolicyBuilder 
        => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/LeaveTypes", async (IMediator mediator) =>
    {
        var result = new List<LeaveTypeDto>();

        var data = await mediator.Send(new GetLeaveTypesQuery());
        result.AddRange(data);
        
        return result;
    })
    .WithName("GetLeaveTypes")
    .WithOpenApi();

app.Run();
