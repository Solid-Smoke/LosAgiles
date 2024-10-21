using back_end.Application;
using back_end.Repositories;
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
builder.Services.AddScoped<SqlConnection>(a => new SqlConnection(builder.Configuration.GetConnectionString("ARQContext")));
builder.Services.AddSingleton<IFactoryProductSearchFilter, FactoryProductSearchFilter>();
builder.Services.AddScoped<IProductHandler, ProductHandler>();
builder.Services.AddScoped<IProductSearchHttpRequestParameterValidator, ProductSearchHttpRequestParameterValidator>();
builder.Services.AddScoped<IProductSearchLogic, ProductSearchLogic>();
builder.Services.AddScoped<IProductSearchHttpLogic, ProductSearchHttpLogic>();

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

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
