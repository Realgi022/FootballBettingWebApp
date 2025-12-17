using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Interfaces;

namespace BLL.Tests.BetTesting
{
    public class FakeBetRepository : IBetRepository
    {
        private readonly List<Bet> _bets = new();

        public Task AddAsync(Bet bet)
        {
            _bets.Add(bet);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Bet>> GetAllAsync()
        {
            return Task.FromResult(_bets.AsEnumerable());
        }

        public Task<Bet?> GetByIdAsync(int betId)
        {
            return Task.FromResult(_bets.FirstOrDefault(b => b.BetId == betId));
        }

        public Task UpdateAsync(Bet bet)
        {
            var index = _bets.FindIndex(b => b.BetId == bet.BetId);
            if (index >= 0)
                _bets[index] = bet;

            return Task.CompletedTask;
        }
    }
}
