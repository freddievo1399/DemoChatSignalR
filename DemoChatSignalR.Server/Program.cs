
namespace DemoChatSignalR.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DynamicCors", policy =>
                {
                    if (corsOrigins != null && corsOrigins.Length > 0)
                    {
                        foreach (var item in corsOrigins)
                        {
                            Console.WriteLine(item);
                        }
                        policy.WithOrigins(corsOrigins);
                    }

                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<CacheChatService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            app.UseHttpsRedirection();
            app.UseCors("DynamicCors");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
