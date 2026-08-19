using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi;
using A1Template.Data;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
        });

        // Makes sure the program accesses the db in the project folder instead of the executable forlder
        /*
        string executableFolder = AppContext.BaseDirectory;
        string projectFolder = Path.GetFullPath(Path.Combine(executableFolder, "..", "..", ".."));
        string dbPath = Path.Combine(projectFolder, "A1Database.sqlite");
        string connectionString = $"Data Source={dbPath}";
        builder.Services.AddDbContext<A1DbContext>(options =>
            options.UseSqlite(connectionString));
        */


        builder.Services.AddDbContext<A1DbContext>(options => options.UseSqlite(builder.Configuration["P1DBConnection"]));

        builder.Services.AddScoped<IA1Repo, A1Repo>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
