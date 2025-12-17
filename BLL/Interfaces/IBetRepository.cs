using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAllAsync();
        Task<Bet?> GetByIdAsync(int betId);
        Task AddAsync(Bet bet);
        Task UpdateAsync(Bet bet);
    }
}
