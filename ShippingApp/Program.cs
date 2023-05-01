using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShippingApp.Data;
using ShippingApp.Hubs;
using ShippingApp.RabbitMQ;
using ShippingApp.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//add logger service
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//adding authentication feature in swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    //ErrorEventHandler errorEventHandler;
});
//adding entity framework dbcontext to app
builder.Services.AddDbContext<ShippingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

//authentication and authorization scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            // for socket authentication and authorization
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (string.IsNullOrEmpty(accessToken) == false)
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

//signalr socket added
builder.Services.AddSignalR();

//enable cors policy
builder.Services.AddCors(options => options.AddPolicy(name: "CorsPolicy",
    policy =>
    {
        policy.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    }
    ));

// service dependencies to be resolved here for dependency injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUploadPicService, UploadPicService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAPIGatewayService, APIGatewayService>();

//rabbitmq
builder.Services.AddScoped<IMessageProducer, MQProducer>();
builder.Services.AddScoped<IMQConsumer, MQConsumer>();

//add background hosted service
builder.Services.AddHostedService<BackgroundServiceConsumer>();

//authorization middleware
builder.Services.AddTransient<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandlerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//create folder for file/image storage
try
{
    string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets");
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    app.UseStaticFiles(new StaticFileOptions
    {
        //File assests = new File()
        FileProvider = new PhysicalFileProvider(
               Path.Combine(builder.Environment.ContentRootPath, "Assets")),
        RequestPath = "/Assets"
    });
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.UseCors("CorsPolicy");
app.UseAuthentication();

app.UseAuthorization();

//socket hub mapping 
app.MapHub<ShippingHub>("/shippingHub");

app.MapControllers();

app.Run();
