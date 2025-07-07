using EcommerceApp.Data;
using EcommerceApp.Services;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // This will use the property names as defined in the C# model
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection")));
            
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<ShoppingCartService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<PaymentService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddHostedService<PendingPaymentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
