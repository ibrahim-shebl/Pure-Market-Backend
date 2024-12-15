using test.Models;

namespace test.Services
{
    public interface IBannerProduct
    {
        Task<IEnumerable<Banner>> GetAll();
        Task<Banner> GetById(int id);
        Task<Banner> Add(Banner banner);
        Banner Update(Banner banner);
        Banner Delete(Banner banner);
    }
}
