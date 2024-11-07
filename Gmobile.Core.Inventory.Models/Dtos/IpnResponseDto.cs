using System.Runtime.Serialization;

namespace Gmobile.Core.Inventory.Models.Dtos;

[DataContract]
public class IpnResponseDto
{
    [DataMember(Order = 1, Name = "status")]
    public string Status { get; set; }

    [DataMember(Order = 2, Name = "message")]
    public string Message { get; set; }
}