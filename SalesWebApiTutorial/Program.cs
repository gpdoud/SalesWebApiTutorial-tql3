using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebApiTutorial.Data;
namespace SalesWebApiTutorial;

public class Program {

    public static void Main(string[] args) {
    
        var builder = WebApplication.CreateBuilder(args);
    
        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>    
            options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") 
                ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

        builder.Services.AddCors();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
