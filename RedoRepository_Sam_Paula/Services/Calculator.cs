using System;
using System.Collections.Generic;
using System.Text;

namespace RedoRepository_Sam_Paula.Services
{
    internal class Calculator
    {
        public static (decimal monthly, decimal totalPaid, decimal totalInterest) Calculate(
        decimal principal,
        decimal annualRatePercent,
        int termYears)
        {
            if (principal <= 0 || annualRatePercent <= 0 || termYears <= 0)
                return (0m, 0m, 0m);

            int n = termYears * 12;
            decimal r = (annualRatePercent / 100m) / 12m;

            decimal pow = (decimal)Math.Pow((double)(1m + r), n);
            decimal monthly = principal * r * pow / (pow - 1m);

            decimal totalPaid = monthly * n;
            decimal totalInterest = totalPaid - principal;

            return (decimal.Round(monthly, 2), decimal.Round(totalPaid, 2), decimal.Round(totalInterest, 2));

        }


    }
}
