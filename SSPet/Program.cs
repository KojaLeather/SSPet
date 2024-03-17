using SSPet.Services;

namespace SSPet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors();

            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IRtmpService, RtmpService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(options =>
            {
                options.WithOrigins("https://127.0.0.1:4200/").AllowAnyMethod().AllowAnyHeader();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthorization();


            app.MapRazorPages();

            app.Run(); //chgecking
        }
    }
}