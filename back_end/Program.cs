using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Infrastructure.Repositories;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:8080");
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Shopping Cart Dependencies
builder.Services.AddScoped<DeleteUserShoppingCart>();
builder.Services.AddScoped<GetUserShoppingCart>();
builder.Services.AddScoped<IShoppingCartHandler, ShoppingCartHandler>();

builder.Services.AddScoped<GetPendingOrders>();
builder.Services.AddScoped<GetProductsByOrderID>();
builder.Services.AddScoped<ApproveOrder>();
builder.Services.AddScoped<RejectOrder>();
builder.Services.AddScoped<IOrderHandler, OrderHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
