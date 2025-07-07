using System.Text.Json.Serialization;

namespace EcommerceApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2, 
        Shipped = 3,
        Delivered = 4,
    }
}
