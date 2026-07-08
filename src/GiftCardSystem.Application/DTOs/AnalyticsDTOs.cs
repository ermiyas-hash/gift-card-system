using System;

namespace GiftCardSystem.Application.DTOs
{
    public class AnalyticsSummaryResponse
    {
        public int TotalGiftCards { get; set; }
        public int ActiveGiftCards { get; set; }
        public int ExpiredGiftCards { get; set; }
        public decimal TotalIssuedAmount { get; set; }
        public decimal TotalRedeemedAmount { get; set; }
        public decimal TotalRemainingBalance { get; set; }
        public int TotalCustomers { get; set; }
        public DateTime ReportDate { get; set; }
    }

    public class ReportResponse
    {
        public Guid Id { get; set; }
        public string ReportType { get; set; }
        public DateTime GeneratedDate { get; set; }
        public object ReportData { get; set; }
    }
}
