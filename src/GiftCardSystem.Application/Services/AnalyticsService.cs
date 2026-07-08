using System;
using System.Linq;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence;

namespace GiftCardSystem.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsSummaryResponse> GetSummaryAsync()
        {
            var giftCards = await _unitOfWork.GiftCards.GetAllAsync();
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            var customers = await _unitOfWork.Customers.GetAllAsync();

            var activeGiftCards = giftCards.Count(g => g.Status == GiftCardStatus.Active);
            var expiredGiftCards = giftCards.Count(g => g.Status == GiftCardStatus.Expired);
            var issuanceTransactions = transactions.Where(t => t.Type == TransactionType.Issuance);
            var redemptionTransactions = transactions.Where(t => t.Type == TransactionType.Redemption);

            return new AnalyticsSummaryResponse
            {
                TotalGiftCards = giftCards.Count,
                ActiveGiftCards = activeGiftCards,
                ExpiredGiftCards = expiredGiftCards,
                TotalIssuedAmount = issuanceTransactions.Sum(t => t.Amount),
                TotalRedeemedAmount = redemptionTransactions.Sum(t => t.Amount),
                TotalRemainingBalance = giftCards.Sum(g => g.CurrentBalance),
                TotalCustomers = customers.Count,
                ReportDate = DateTime.UtcNow
            };
        }

        public async Task<ReportResponse> GenerateReportAsync(string reportType)
        {
            var summary = await GetSummaryAsync();

            return new ReportResponse
            {
                Id = Guid.NewGuid(),
                ReportType = reportType,
                GeneratedDate = DateTime.UtcNow,
                ReportData = summary
            };
        }
    }
}
