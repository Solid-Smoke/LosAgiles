using back_end.Application.Commands;
using back_end.Application.interfaces;
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Infrastructure.Repositories;
using System.Data.SqlClient;

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
builder.Services.AddScoped<SqlConnection>(a =>
    new SqlConnection(builder.Configuration.GetConnectionString("ClientsContext")));
builder.Services.AddScoped<IProductHandler, ProductHandler>();
builder.Services.AddScoped<IProductQuery, ProductQuery>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Business Dependencies
builder.Services.AddScoped<DeleteInvalidProductsFromUserCart>();
builder.Services.AddScoped<GetShoppingCartInvalidItems>();
builder.Services.AddScoped<AddItemToShoppingCart>();
builder.Services.AddScoped<DeleteUserShoppingCart>();
builder.Services.AddScoped<GetUserShoppingCart>();
builder.Services.AddScoped<IBusinessHandler, BusinessHandler>();

//Shopping Cart Dependencies
builder.Services.AddScoped<GetBusinessByEmployeeID>();
builder.Services.AddScoped<GetBusinessAddressByBusinessID>();
builder.Services.AddScoped<GetAllBusiness>();
builder.Services.AddScoped<InsertNewBusiness>();
builder.Services.AddScoped<IShoppingCartHandler, ShoppingCartHandler>();

//Orders dependencies
builder.Services.AddScoped<GetPendingOrders>();
builder.Services.AddScoped<GetOrdersByClientID>();
builder.Services.AddScoped<GetProductsByOrderID>();
builder.Services.AddScoped<ApproveOrder>();
builder.Services.AddScoped<RejectOrder>();
builder.Services.AddScoped<IOrderHandler, OrderHandler>();
builder.Services.AddScoped<ISubmitOrder, SubmitOrder>();
builder.Services.AddScoped<SqlConnection>(auxiliarVariable => new SqlConnection(builder.Configuration.GetConnectionString("ClientsContext")));

//Reports Dependencies
builder.Services.AddScoped<GenerateCompletedOrdersReport>();
builder.Services.AddScoped<IReportHandler, ReportHandler>();

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
