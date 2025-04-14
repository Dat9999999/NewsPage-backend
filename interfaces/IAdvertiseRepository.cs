using NewsPage.Enums;
using NewsPage.Models;
using NewsPage.Models.entities;

namespace NewsPage.repositories.interfaces
{
    public interface IAdvertiseRepository
    {
        Task<BannerCampaign> CreateBannerCampaign(Guid AdvertisorId, DateTime StartDate, DateTime EndDate, Guid TargetAudienceId);
        Task<BannerMaterial> CreateBannerMaterial(Guid AdvertisorId, string FileUrl);
        Task<BannerAudience> CreateBannerAudience(Gender AudienceSex, AgeRangeEnum AudienceAge, string Topic);
        Task<BannerAudience> GetBannerAudience(Guid AudienceId);
        Task<List<BannerCampaign>> GetCampaigns();
        Task<List<BannerCampaign>> GetCampaignsOfUser(Guid accountId);
        Task<BannerCampaign> GetCampaignsById(Guid CampaignId);
        Task PendingStatus(Guid campaignGuid);
        Task DenyCampaign(Guid CampaginId);
        Task AcceptCampaign(Guid CampaignId);
        Task ActiveCampaign(Guid campaignId);
        Task<List<BannerCampaign>> GetActiveCampaigns();
    }
}
