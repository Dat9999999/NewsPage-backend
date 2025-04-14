namespace NewsPage.repositories.interfaces
{
    public interface IAdvertiseMaterialRepository{
        public Task<BannerMaterial> CreateBannerMaterial(Guid CampaignId, string AdUrl, string filePath);
        public Task<BannerMaterial> GetMaterial(Guid campaignId);
    }
}