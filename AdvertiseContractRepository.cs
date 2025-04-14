using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Models.entities;
using NewsPage.Repositories;
namespace NewsPage.Repositories;

public class AdvertiseContractRepository : IAdvertiseContractRepository
{
    private readonly ApplicationDbContext _context;
    public AdvertiseContractRepository(ApplicationDbContext applicationDbContext){
        _context =applicationDbContext;
    }

    public async Task CancelContract(Guid id)
    {
        AdvertiseContract contract = await _context.AdvertiseContracts.FirstOrDefaultAsync(c => c.Id == id);
        contract.PaymentStatus = Enums.PaymentStatusEnum.Cancelled;
        await _context.SaveChangesAsync();
    }

    public async Task ConfirmContract(Guid contractId, AdvertiseContract advertiseContract)
    {
        advertiseContract.ConfirmContract = Enums.ConfirmContractEnum.Confirmed;
        await _context.SaveChangesAsync();
    }

    public async Task<AdvertiseContract> CreateContract(decimal totalCost, Guid CampaginId, Guid postionId, Guid BaseId)
    {
        AdvertiseContract ac = new AdvertiseContract {
            Id = new Guid(),
            CampaignId = CampaginId,
            PositionId = postionId,
            BasePriceId = BaseId,
            TotalCost = totalCost,
            // Thiết lập thời hạn thanh toán 2 ngày sau khi xác nhận
            PaymentDueDate = DateTime.Now.AddDays(2)
        };
        await _context.AdvertiseContracts.AddAsync(ac);
        await _context.SaveChangesAsync();
        return ac;
    }

    public async Task<List<AdvertiseContract>> GetAllContracts()
    {
        return await _context.AdvertiseContracts.ToListAsync();
    }

    public async Task<AdvertiseContract> GetById(Guid contractId)
    {
        return await _context.AdvertiseContracts.FirstOrDefaultAsync(c => c.Id == contractId);
    }

    public async Task<List<AdvertiseContract>> GetContractsByAdvertiserId(Guid AdvertiserId)
    {
        HashSet<Guid> campaigns = (await _context.BannerCampaigns
        .Where(cp => cp.AdvertisorId == AdvertiserId)
        .Select(cp => cp.Id)
        .ToListAsync())
        .ToHashSet();
        List<AdvertiseContract> contracts = await _context.AdvertiseContracts.Where(ac => campaigns.Contains(ac.CampaignId)).ToListAsync();
        return contracts;
    }

    public async Task PaidSuccess(Guid contractId)
    {
        AdvertiseContract contract = await _context.AdvertiseContracts.FirstOrDefaultAsync(c => c.Id == contractId);
        contract.PaymentStatus = Enums.PaymentStatusEnum.Paid;
        await _context.SaveChangesAsync();
    }
}