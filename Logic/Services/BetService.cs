using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Models;
using BLL.Mappers;
using BLL.Exceptions;

namespace BLL.Services
{
    public class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;

        public BetService(
            IBetRepository betRepository,
            IMatchRepository matchRepository,
            IUserRepository userRepository,
            IWalletRepository walletRepository)
        {
            _betRepository = betRepository;
            _matchRepository = matchRepository;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

        // ---------------------------------------------------------
        // GET ALL BETS FOR USER
        // ---------------------------------------------------------
        public async Task<IEnumerable<BetDto>> GetAllBetsAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new InvalidBetException("Username cannot be empty.");

                var user = await _userRepository.GetUserByUsernameAsync(username)
                    ?? throw new BetNotFoundException("User not found.");

                var bets = await _betRepository.GetAllAsync();
                var matches = await _matchRepository.GetAllAsync();

                var result = from bet in bets
                             where bet.UserId == user.UserId
                             join match in matches on bet.MatchId equals match.MatchId
                             select BetMapper.ToDto(bet, username, match);

                return result;
            }
            catch (InvalidBetException)
            {
                throw;
            }
            catch (BetNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Unexpected error while retrieving bets.", ex);
            }
        }

        // ---------------------------------------------------------
        // ADD BET
        // ---------------------------------------------------------
        public async Task AddBetAsync(BetDto betDto, string username)
        {
            if (betDto == null)
                throw new InvalidBetException("Bet cannot be null.");

            var user = await _userRepository.GetUserByUsernameAsync(username)
                ?? throw new BetNotFoundException("User not found.");

            var wallet = await _walletRepository.GetWalletByUserIdAsync(user.UserId)
                ?? throw new BetNotFoundException("Wallet not found.");

            if (wallet.Balance < betDto.Stake)
                throw new InvalidBetException("Insufficient balance.");

            wallet.Balance -= betDto.Stake;
            await _walletRepository.UpdateWalletAsync(wallet);

            var bet = BetMapper.ToEntity(betDto, user.UserId);
            await _betRepository.AddAsync(bet); 
        }


        // ---------------------------------------------------------
        // RESOLVE BETS
        // ---------------------------------------------------------
        public async Task ResolveBetsAsync(MatchDto match)
        {
            try
            {
                if (match == null)
                    throw new InvalidBetException("Match cannot be null.");

                var allBets = await _betRepository.GetAllAsync();
                var matchBets = allBets.Where(b => b.MatchId == match.MatchId && b.Status == "Pending");

                foreach (var bet in matchBets)
                {
                    bet.Status = (bet.BetType == match.Result) ? "Won" : "Lost";
                    await _betRepository.UpdateAsync(bet);
                }
            }
            catch (InvalidBetException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Unexpected error while resolving bets.", ex);
            }
        }

        // ---------------------------------------------------------
        // UPDATE BET
        // ---------------------------------------------------------
        public async Task UpdateBetAsync(BetDto betDto)
        {
            try
            {
                if (betDto == null)
                    throw new InvalidBetException("Bet cannot be null.");

                var bet = await _betRepository.GetByIdAsync(betDto.BetId)
                    ?? throw new BetNotFoundException("Bet not found.");

                bet.Status = betDto.Status;
                await _betRepository.UpdateAsync(bet);
            }
            catch (InvalidBetException)
            {
                throw;
            }
            catch (BetNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Unexpected error while updating bet.", ex);
            }
        }
    }
}
