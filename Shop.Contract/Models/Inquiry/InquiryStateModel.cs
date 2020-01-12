using System.Text.Json.Serialization;

namespace Tranquiliza.Shop.Contract.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InquiryStateModel
    {
        AddingToCart = 0,
        Placed = 1,
        PaymentExpected = 2,
        PaymentReceived = 3,
        Dispatched = 4,
    }
}
