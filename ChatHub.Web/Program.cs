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
    .AllowAnyMethod()           
    .AllowAnyHeader()           
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHubService>("/chat");
});

app.MapControllers();

app.Run();
