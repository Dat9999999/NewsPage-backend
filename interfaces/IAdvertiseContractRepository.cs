using NewsPage.Models.entities;

namespace NewsPage.Repositories;
public interface IAdvertiseContractRepository{
    Task ConfirmContract(Guid contractId, AdvertiseContract contract);
    Task<AdvertiseContract> CreateContract(Decimal totalCost,Guid CampaginId, Guid postionId, Guid BaseId);
    Task<AdvertiseContract> GetById(Guid contractId);
    Task<List<AdvertiseContract>> GetAllContracts();
    Task<List<AdvertiseContract>> GetContractsByAdvertiserId(Guid AdvertiserId);
    Task CancelContract(Guid id);
    Task PaidSuccess(Guid guid);
}