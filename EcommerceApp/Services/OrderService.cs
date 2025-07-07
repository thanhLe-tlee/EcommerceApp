﻿using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.OrderDTOs;
using EcommerceApp.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        // Allowed order status transitions for validating status changes.
        private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedStatusTransitions = new()
{
{ OrderStatus.Pending, new List<OrderStatus>() },
{ OrderStatus.Processing, new List<OrderStatus>() },
{ OrderStatus.Shipped, new List<OrderStatus>() },
{ OrderStatus.Delivered, new List<OrderStatus>() },
};
        // Inject the ApplicationDbContext.
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Creates a new order from the provided OrderCreateDTO.
        // This method validates customer and address data, checks product stock,
        // calculates financial totals, deducts product stock, and marks any active cart as checked out.
        public async Task<ApiResponse<OrderResponseDTO>> CreateOrderAsync(OrderCreateDTO orderDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(orderDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<OrderResponseDTO>(404, "Customer does not exist.");
                }
                var addressId = await _context.Addresses.FindAsync(orderDto.CustomerId);
                if (addressId == null || addressId.CustomerId != orderDto.CustomerId)
                {
                    return new ApiResponse<OrderResponseDTO>(404, "Address is not valid or does not belong to the customer.");
                }
                // Initialize financial tracking.
                decimal totalBaseAmount = 0;
                decimal totalDiscountAmount = 0;
                decimal totalAmount = 0;
                // Generate a unique order number.
                string orderNumber = GenerateOrderNumber();
                // List to hold order items.
                var orderItems = new List<OrderItem>();
                // Process each order item from the DTO.
                foreach (var itemDto in orderDto.OrderItems)
                {
                    // Check if the product exists.
                    var product = await _context.Products.FindAsync(itemDto.ProductId);
                    if (product == null)
                    {
                        return new ApiResponse<OrderResponseDTO>(404, $"Product with ID {itemDto.ProductId} does not exist.");
                    }
                    // Check if sufficient stock is available.
                    if (product.StockQuantity < itemDto.Quantity)
                    {
                        return new ApiResponse<OrderResponseDTO>(400, $"Insufficient stock for product {product.Name}.");
                    }
                    // Calculate base price, discount, and total price for the order item.
                    decimal basePrice = itemDto.Quantity * product.Price;
                    decimal discount = (product.DiscountPercent / 100.0m) * basePrice;
                    decimal totalPrice = basePrice - discount;
                    // Create a new OrderItem.
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = itemDto.Quantity,
                        UnitPrice = product.Price,
                        Discount = discount,
                        TotalPrice = totalPrice
                    };
                    // Add the order item to the list.
                    orderItems.Add(orderItem);
                    // Update the running totals.
                    totalBaseAmount += basePrice;
                    totalDiscountAmount += discount;
                    // Deduct the purchased quantity from the product’s stock.
                    product.StockQuantity -= itemDto.Quantity;
                    _context.Products.Update(product);
                }
                // Calculate the final total amount.
                totalAmount = totalBaseAmount - totalDiscountAmount;
                // Manually map from DTO to Order model.
                var order = new Order
                {
                    OrderNumber = orderNumber,
                    CustomerId = orderDto.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    TotalBaseAmount = totalBaseAmount,
                    TotalDiscountAmount = totalDiscountAmount,
                    TotalAmount = totalAmount,
                    OrderStatus = OrderStatus.Pending,
                    OrderItems = orderItems,
                    AddressId = orderDto.AddressId,
                };
                // Add the order to the database.
                _context.Orders.Add(order);
                // Mark the customer's active cart as checked out (if it exists).
                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == orderDto.CustomerId && !c.IsCheckedOut);
                if (cart != null)
                {
                    cart.IsCheckedOut = true;
                    cart.UpdatedAt = DateTime.UtcNow;
                    _context.Carts.Update(cart);
                }
                // Save all changes.
                await _context.SaveChangesAsync();
                // Map the saved order to OrderResponseDTO.
                var orderResponse = MapOrderToDTO(order, customer, addressId);
                return new ApiResponse<OrderResponseDTO>(200, orderResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<OrderResponseDTO>> GetOrderByIdAsync(int orderId)
        {
            try
            {
                // Retrieve the order 
                var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .Include(o => o.Address1)
                .FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null)
                {
                    return new ApiResponse<OrderResponseDTO>(404, "Order not found.");
                }
                var orderResponse = MapOrderToDTO(order, order.Customer, order.Address1);
                return new ApiResponse<OrderResponseDTO>(200, orderResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        // Updates the status of an existing order.
        // Validates allowed status transitions before applying the update.
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateOrderStatusAsync(OrderStatusUpdateDTO statusDto)
        {
            try
            {
                // Retrieve the order.
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == statusDto.OrderId);
                if (order == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Order not found.");
                }
                var currentStatus = order.OrderStatus;
                var newStatus = statusDto.OrderStatus;
                // Validate the status transition.
                if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(500, "Current order status is invalid.");
                }
                if (!allowedStatuses.Contains(newStatus))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, $"Cannot change order status from {currentStatus} to {newStatus}.");
                }
                // Update the order status.
                order.OrderStatus = newStatus;
                await _context.SaveChangesAsync();
                // Prepare a confirmation message.
                var confirmation = new ConfirmationResponseDTO
                {
                    Message = $"Order Status with Id {statusDto.OrderId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        // Retrieves all orders in the system.
        public async Task<ApiResponse<List<OrderResponseDTO>>> GetAllOrdersAsync()
        {
            try
            {
                // Retrieve all orders 
                var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .AsNoTracking()
                .ToListAsync();
                var orderList = orders.Select(o => MapOrderToDTO(o, o.Customer, o.Address1)).ToList();
                return new ApiResponse<List<OrderResponseDTO>>(200, orderList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<OrderResponseDTO>>> GetOrdersByCustomerAsync(int customerId)
        {
            try
            {
                // Retrieve the customer
                var customer = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(c => c.Addresses)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    return new ApiResponse<List<OrderResponseDTO>>(404, "Customer not found.");
                }
                var orders = customer.Orders.Select(o => MapOrderToDTO(o, customer, o.Address1)).ToList();
                return new ApiResponse<List<OrderResponseDTO>>(200, orders);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        #region Helper Methods
        private OrderResponseDTO MapOrderToDTO(Order order, Customer customer, Address address)
        {
            var orderItemsDto = order.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Discount = oi.Discount,
                TotalPrice = oi.TotalPrice
            }).ToList();
            // Create and return the DTO.
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                TotalBaseAmount = order.TotalBaseAmount,
                TotalDiscountAmount = order.TotalDiscountAmount,
                TotalAmount = Math.Round(order.TotalAmount, 2),
                OrderStatus = order.OrderStatus,
                OrderItems = orderItemsDto,
                AddressId = order.AddressId
            };
        }
        // Format: ORD-yyyyMMdd-HHmmss-XXXX
        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}-{RandomNumber(1000, 9999)}";
        }
        // Generates random number
        private int RandomNumber(int min, int max)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                return Math.Abs(BitConverter.ToInt32(bytes, 0) % (max - min + 1)) + min;
            }
        }
        #endregion
    }
}
