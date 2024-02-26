using System;

using Arrow.DeveloperTest.Common.Enums;

namespace Arrow.DeveloperTest.Models
{
    public class MakePaymentRequest
    {
        public string CreditorAccountNumber { get; set; }

        public string DebtorAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public PaymentScheme PaymentScheme { get; set; }
    }
}
