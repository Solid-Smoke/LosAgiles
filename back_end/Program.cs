using back_end.Application.Commands;
using back_end.Application.interfaces;
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Application.Reports;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
using System.Data.SqlClient;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://agile-buy.netlify.app", "http://localhost:8080")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
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
builder.Services.AddScoped<SqlConnection>(auxiliarVariable => new SqlConnection(builder.Configuration.GetConnectionString("ClientsContext")));
builder.Services.AddScoped<IBusinessHandler, BusinessHandler>();
builder.Services.AddScoped<IProductDeleteHandler, ProductDeleteHandler>();
builder.Services.AddScoped<IProductDelete, ProductDelete>();
builder.Services.AddScoped<IBusinessDeleteHandler, BusinessDeleteHandler>();
builder.Services.AddScoped<IBusinessDelete, BusinessDelete>();

builder.Services.AddScoped<GetMonthlyRevenueByBusinessID>();
builder.Services.AddScoped<GetOrdersInProgressByBusinessID>();

//Shopping Cart Dependencies
builder.Services.AddScoped<GetBusinessByEmployeeID>();
builder.Services.AddScoped<GetBusinessAddressByBusinessID>();
builder.Services.AddScoped<GetAllBusiness>();
builder.Services.AddScoped<InsertNewBusiness>();
builder.Services.AddScoped<IShoppingCartHandler, ShoppingCartHandler>();

//Orders dependencies
builder.Services.AddScoped<GetPendingOrders>();
builder.Services.AddScoped<GetApprovedOrders>();
builder.Services.AddScoped<GetOrdersByClientID>();
builder.Services.AddScoped<GetProductsByOrderID>();
builder.Services.AddScoped<ApproveOrder>();
builder.Services.AddScoped<RejectOrder>();
builder.Services.AddScoped<IOrderHandler, OrderHandler>();
builder.Services.AddScoped<GetOrdersExcludingCompleted>();
builder.Services.AddScoped<GetLastTenPurchased>();
builder.Services.AddScoped<ISubmitOrder, SubmitOrder>();
builder.Services.AddScoped<SqlConnection>(auxiliarVariable => new SqlConnection(builder.Configuration.GetConnectionString("ClientsContext")));

//Reports Dependencies
builder.Services.AddScoped<GenerateCompletedOrdersReport>();
builder.Services.AddScoped<GenerateCompletedOrderReportPDF>();
builder.Services.AddScoped<GenerateAllCancelledOrdersReport>();
builder.Services.AddScoped<GenerateAllCancelledOrderReportPDF>();
builder.Services.AddScoped<GenerateAllPendingOrdersReport>();
builder.Services.AddScoped<GenerateAllPendingOrderReportPDF>();
builder.Services.AddScoped<GenerateAllCompletedOrdersReport>();
builder.Services.AddScoped<GenerateAllCompletedOrderReportPDF>();
builder.Services.AddScoped<GenerateAdminEarningsReport>();
builder.Services.AddScoped<GenerateEarningsReportPDF>();
builder.Services.AddScoped<IReportHandler, ReportHandler>();
builder.Services.AddScoped<OrderReportTemplate<ReportCompletedOrderData>, CompletedOrderReport>();
builder.Services.AddScoped<AllCancelledOrderReport>();
builder.Services.AddScoped<AllPendingOrderReport>();
builder.Services.AddScoped<AllCompletedOrderReport>();
builder.Services.AddScoped<OrderReportTemplate<AdminReportOrderData>>(provider => {
    return provider.GetRequiredService<AllCancelledOrderReport>();
});

// Admin dependencies
builder.Services.AddScoped<IAdminHandler, AdminHandler>();
builder.Services.AddScoped<GetMonthlyRevenueQuery>();
builder.Services.AddScoped<GetMonthlyShippingExpensesQuery>();
builder.Services.AddScoped<GetOrdersInProgressQuery>();
builder.Services.AddScoped<EarningsReportTemplate<ReportEarningsFilters>, AdminEarningsReport>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
