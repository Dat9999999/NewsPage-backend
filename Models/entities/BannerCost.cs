using NewsPage.Enums;

public class BannerCost{
    public Guid Id {get; set;}
    public Guid CampaignId {get; set;}
    public Guid PositionId {get; set;}
    public Guid SizePriceId {get; set;}
    public Decimal TotalCost {get; set;}
}