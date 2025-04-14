using NewsPage.Models.Entities;

namespace NewsPage.Repositories;

public interface IAdvertiseBannerReport{
    Task<BannerReport> CreateBannerReport(Guid CampaignId);
    Task<decimal> GetViewsByCampaignId(Guid id);
    Task IncrementAdClickCount(Guid campaignId);
    Task IncrementAdViewCount(Guid id);
}