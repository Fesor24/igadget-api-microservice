using ShoppingCartService.EndpointDefinitions;
using ShoppingCartService.Repositories.Contracts;
using ShoppingCartService.Repositories.Implementation;
using ShoppingCartService.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
{
    var configurationOptions = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")
        ,true);

    return ConnectionMultiplexer.Connect(configurationOptions);
});

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddGrpc();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", pol =>
    {
        pol.AllowAnyMethod().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");

ShoppingCartEndpointDefinition.RegisterEndpoint(app);

app.MapGrpcService<GrpcCartService>();

app.Run();
