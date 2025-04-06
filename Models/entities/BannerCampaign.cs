using System.ComponentModel.DataAnnotations;
using NewsPage.Enums;

public class BannerCampaign{
    
    [Key]
    public Guid Id {get; set;}

    public Guid AdvertisorId {get; set;}
    public Guid TargetAutidienceId {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public CampaignStatusEnum Status {get; set;}
}