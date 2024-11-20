using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.DependencyResolver;
using ChatHub.Application.Mappings;
using ChatHub.Domain.Entity.setup;
using ChatHub.Infrastructure;
using ChatHub.Web;
using ChatHub.Web.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();
app.UseRouting();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyMethod()           // Allow any HTTP methods like GET, POST, etc.
    .AllowAnyHeader()           // Allow any headers, including Authorization
    .SetIsOriginAllowed(origin => true) // Allow any origin (can be adjusted for specific origins)
    .AllowCredentials());





app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHubService>("/chat");
});

app.MapControllers();

app.Run();
