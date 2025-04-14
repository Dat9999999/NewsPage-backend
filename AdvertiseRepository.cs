using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Enums;
using NewsPage.repositories.interfaces;

namespace NewsPage.Repositories;

public class AdvertiseRepository : IAdvertiseRepository
{
    private readonly ApplicationDbContext _context;

    public AdvertiseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AcceptCampaign(Guid CampaignId)
    {
        BannerCampaign bc = await GetCampaignsById(CampaignId);
        bc.Status = CampaignStatusEnum.ACCEPTED;
        await _context.SaveChangesAsync();
    }

    public async Task ActiveCampaign(Guid campaignId)
    {
        BannerCampaign bc = await GetCampaignsById(campaignId);
        bc.Status = CampaignStatusEnum.ACTIVE;
        await _context.SaveChangesAsync();
    }

    public async Task<BannerAudience> CreateBannerAudience(Gender AudienceSex, AgeRangeEnum AudienceAge, string Topic)
    {
        // Check if the BannerAudience already exists based on the provided conditions
        var existingBannerAudience = await _context.BannerAudiences
            .FirstOrDefaultAsync(ba => ba.Sex == AudienceSex && ba.AgeRange == AudienceAge && ba.TopicName == Topic);

        if (existingBannerAudience != null)
        {
            // If the BannerAudience already exists, return the existing one
            return existingBannerAudience;
        }

        // Generate a new GUID for the BannerAudienceId
        Guid BannerAudienceId = Guid.NewGuid();

        // Create the new BannerAudience object
        BannerAudience ba = new BannerAudience
        {
            Id = BannerAudienceId,
            Sex = AudienceSex,
            AgeRange = AudienceAge,
            TopicName = Topic
        };

        // Add the new BannerAudience to the context
        await _context.BannerAudiences.AddAsync(ba);
        await _context.SaveChangesAsync();

        return ba;
    }


    public async Task<BannerCampaign> CreateBannerCampaign(Guid AdvertisorId, DateTime StartDate, DateTime EndDate, Guid TargetAudienceId)
    {
        Guid CampaignId = new Guid();
        BannerCampaign bc = new BannerCampaign{Id = CampaignId ,AdvertisorId = AdvertisorId, EndDate = EndDate , StartDate = StartDate, AudienceId  = TargetAudienceId};
        await _context.BannerCampaigns.AddAsync(bc);
        await _context.SaveChangesAsync();
        return bc;
    }

    public async Task<BannerMaterial> CreateBannerMaterial(Guid AdvertisorId, string FileUrl)
    {
        Guid MaterialId = new Guid();
        BannerMaterial bm = new BannerMaterial{Id = MaterialId, CampaignId = AdvertisorId, FileUrl = FileUrl};
        await _context.BannerMaterials.AddAsync(bm);
        await _context.SaveChangesAsync();
        return bm;
    }

    public async Task DenyCampaign(Guid CampaginId)
    {
         BannerCampaign bc = await GetCampaignsById(CampaginId);
        bc.Status = CampaignStatusEnum.DENIED;
        await _context.SaveChangesAsync();
    }

    public async Task<List<BannerCampaign>> GetActiveCampaigns()
    {
        return await _context.BannerCampaigns.Where(c => c.Status == CampaignStatusEnum.ACTIVE).ToListAsync();
    }

    public async Task<BannerAudience> GetBannerAudience(Guid Id)
    {
        return await _context.BannerAudiences.FirstOrDefaultAsync(a => a.Id == Id);
    }

    public async Task<List<BannerCampaign>> GetCampaigns()
    {
        return await _context.BannerCampaigns.ToListAsync();
    }

    public async Task<BannerCampaign> GetCampaignsById(Guid CampaignId)
    {
        return await _context.BannerCampaigns.FirstOrDefaultAsync(bc => bc.Id == CampaignId);
    }

    public async Task<List<BannerCampaign>> GetCampaignsOfUser(Guid userId)
    {
        return await _context.BannerCampaigns.Where(bc => bc.AdvertisorId == userId).ToListAsync();
    }

    public async Task PendingStatus(Guid campaignGuid)
    {
        BannerCampaign bc = await GetCampaignsById(campaignGuid);
        bc.Status = CampaignStatusEnum.PENDING;
        await _context.SaveChangesAsync();
    }
}