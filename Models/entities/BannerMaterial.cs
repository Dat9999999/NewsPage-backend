using NewsPage.Enums;

public class BannerMaterial{
    public Guid Id {get; set;}
    public Guid CampaignId {get; set;}
    public string FileUrl {get; set;}
    public FileFormatEnum FileFormat {get; set;}
}