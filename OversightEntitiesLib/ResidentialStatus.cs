namespace Mzeey.Shared;
public class ResidentialStatus
{
    public int Id {get;set;}
    public string? Title {get;set;}

    //Related Entities
    public ICollection<User>? Users {get;set;}
    public ResidentialStatusesFee? ResidentialStatusFee {get;set;}

}
