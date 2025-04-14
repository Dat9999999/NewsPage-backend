using NewsPage.Models.entities;

namespace NewsPage.repositories.interfaces
{
    public interface IBasePriceRepository{
        Task<List<BasePrice>> GetPrices();
        Task<BasePrice> GetById(Guid BasePriceId);
        Task<BasePrice> Create(BasePriceCreateDto dto);
        Task<BasePrice?> Update(Guid id, BasePriceUpdateDto dto);
        Task<bool> Delete(Guid id);
    }
}