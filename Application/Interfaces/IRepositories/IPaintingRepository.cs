using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IPaintingRepository : IGenericRepository<Painting>
    {
        public Task UpdNumPaintings(int paintingId, int numToBuy);
    }
}