using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.repositories.interfaces;

namespace NewsPage.Repositories;
public class AdvertiseMaterialRepository : IAdvertiseMaterialRepository
{
    private readonly ApplicationDbContext _context;

    public AdvertiseMaterialRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BannerMaterial> CreateBannerMaterial(Guid CampaignId, string AdUrl, string filePath)
    {
        BannerMaterial bm = new BannerMaterial {
            Id = new Guid(),
            CampaignId = CampaignId,
            FileUrl = filePath,
            AdvertiseUrl = AdUrl
        };
        await _context.BannerMaterials.AddAsync(bm);
        await _context.SaveChangesAsync();
        return bm;
    }

    public async Task<BannerMaterial> GetMaterial(Guid campaignId)
    {
        return await _context.BannerMaterials.FirstOrDefaultAsync(m => m.CampaignId == campaignId);
    }
}