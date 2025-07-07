using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.PaymentDTOs;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Services
{
    public class PendingPaymentService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5); // Adjust as needed
        public PendingPaymentService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task<ApiResponse<ConfirmationResponseDTO>> ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        // Retrieve payments with status "Pending"
                        var pendingPayments = await context.Payments
                            .Include(p => p.Order)
                            .Where(p => p.Status == PaymentStatus.Pending && p.PaymentMethod.ToUpper() != "COD")
                            .ToListAsync(stoppingToken);

                        // List to track orders for which we need to send confirmation emails.
                        var ordersToEmail = new List<int>();
                        foreach (var payment in pendingPayments)
                        {
                            // Simulate checking payment status
                            string updatedStatus = SimulatePaymentGatewayResponse();
                            if (updatedStatus == "Completed")
                            {
                                payment.Status = PaymentStatus.Completed;
                                payment.Order.OrderStatus = OrderStatus.Processing;
                                ordersToEmail.Add(payment.Order.Id);
                            }
                            else if (updatedStatus == "Failed")
                            {
                                payment.Status = PaymentStatus.Failed;
                            }
                            // If "Pending", no change
                            context.Payments.Update(payment);
                            context.Orders.Update(payment.Order);
                        }
                        await context.SaveChangesAsync(stoppingToken);

                        // If there are any orders that have been updated to Processing, send order confirmation emails.
                        if (ordersToEmail.Any())
                        {
                            // Retrieve the PaymentService which has our email sending method.
                            var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();
                            foreach (var orderId in ordersToEmail)
                            {
                                // Send the order confirmation email.
                                await paymentService.SendOrderConfirmationEmailAsync(orderId);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
            return new ApiResponse<ConfirmationResponseDTO>(200, "Service stopped gracefully.");
        }

        private string SimulatePaymentGatewayResponse()
        {
            // Simulate payment gateway response:
            Random rnd = new Random();
            int chance = rnd.Next(1, 101); // 1 to 100
            if (chance <= 50)
                return "Completed";
            else if (chance <= 80)
                return "Failed";
            else
                return "Pending";
        }
    }
}
