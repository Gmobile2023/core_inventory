using System.Runtime.Serialization;
using ServiceStack;

namespace Inventory.Shared.Dtos.CommonDto;

[DataContract]
public class CustomUserSession : AuthUserSession
{
    [DataMember] public string AccountCode { get; set; }
    [DataMember] public string ClientId { get; set; }
    [DataMember] public string PhoneNumberOtp { get; set; }
    [DataMember] public string ParentCode { get; set; }
    [DataMember] public DateTime CreatedDate { get; set; }
    [DataMember] public DateTime VerificationDate { get; set; }
    
}

[Route("/servicestack-identity")]
public class GetIdentity : IReturn<GetIdentityResponse>
{
}

public class GetIdentityResponse
{
    public List<Property> Claims { get; set; }
    public AuthUserSession Session { get; set; }
}