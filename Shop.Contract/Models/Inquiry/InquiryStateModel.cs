using System.Text.Json.Serialization;

namespace Tranquiliza.Shop.Contract.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InquiryStateModel
    {
        AddingToCart = 0,
        Placed = 1,
        InquriyConfirmed = 2,
        Dispatched = 3,
    }
}
