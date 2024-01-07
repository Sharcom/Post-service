using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RabbitMQ_Messenger_Lib.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Post_service.AuthConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PostContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PostDatabase"), b =>
    {
        b.MigrationsAssembly("Post-service");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Auth0 Services
string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:current_user", policy => policy.Requirements.Add(new Post_service.AuthConfig.HasScopeRequirement("read:current_user", domain)));
});

builder.Services.AddSingleton<IAuthorizationHandler, Post_service.AuthConfig.HasScopeHandler>();
builder.Services.AddSingleton(new ManagementAPIConfig() { Audience = builder.Configuration["Auth0:Audience"], Domain = builder.Configuration["Auth0:Domain"], ClientID = builder.Configuration["Auth0:ManagementAPI:ClientID"], ClientSecret = builder.Configuration["Auth0:ManagementAPI:ClientSecret"] });

MessengerConfig messengerConfig = new MessengerConfig() { HostName = builder.Configuration["MessageBus:Host"], Exchange = builder.Configuration["MessageBus:Exchange"]};
builder.Services.AddSingleton(messengerConfig);

var app = builder.Build();

// EF migration
try
{
    using (IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        DbContext context = serviceScope.ServiceProvider.GetRequiredService<PostContext>();
        context.Database.Migrate();
    }
}
catch (Exception e)
{
    Console.WriteLine("An error occured during EF Migration, migration aborted");
    Console.WriteLine(e.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseExceptionHandler("/Error");
}

app.UseAuthorization();

app.MapControllers();

app.Run();