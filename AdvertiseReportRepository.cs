using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Enums;
using NewsPage.Models.Entities;
using NewsPage.repositories.interfaces;

namespace NewsPage.Repositories;

public class AdvertiseReportRepository : IAdvertiseBannerReport
{
    private readonly ApplicationDbContext _context;

    public AdvertiseReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BannerReport> CreateBannerReport(Guid CampaignId)
    {
        BannerReport rp = new BannerReport{
            Id = new Guid(),
            Impressions = 0,
            Clicks = 0,
            CampaignId = CampaignId,
            CreateDate = DateTime.UtcNow
        };
        await _context.BannerReports.AddAsync(rp);
        await _context.SaveChangesAsync();
        return rp;
    }

    public async Task<decimal> GetViewsByCampaignId(Guid id)
    {
        BannerReport rp = await _context.BannerReports.FirstOrDefaultAsync(rp => rp.CampaignId == id);
        return rp.Impressions;
    }

    public async Task IncrementAdClickCount(Guid id)
    {
        BannerReport rp = await _context.BannerReports.FirstOrDefaultAsync(rp => rp.CampaignId == id);
        rp.Clicks += 1;
        await _context.SaveChangesAsync();
    }

    public async Task IncrementAdViewCount(Guid id)
    {
        BannerReport rp = await _context.BannerReports.FirstOrDefaultAsync(rp => rp.CampaignId == id);
        rp.Impressions += 1;
        await _context.SaveChangesAsync();
    }
}