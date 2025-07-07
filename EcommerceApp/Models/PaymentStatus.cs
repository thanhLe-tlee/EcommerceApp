using System.Text.Json.Serialization;

namespace EcommerceApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 5,
        Failed = 6,
    }
}
