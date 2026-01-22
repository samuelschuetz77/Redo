namespace RedoRepository_Sam_Paula.Services;

public class MortgageCalculator
{
    public (decimal MonthlyPayment, decimal TotalPaid, decimal TotalInterest) Calculate(
        decimal principal, decimal annualRate, int years)
    {
        if (principal <= 0 || annualRate <= 0 || years <= 0)
            return (0, 0, 0);

        decimal monthlyRate = annualRate / 100 / 12;
        int months = years * 12;

        double r = (double)monthlyRate;
        double n = months;
        double p = (double)principal;

        double monthlyPayment = p * (r * Math.Pow(1 + r, n)) / (Math.Pow(1 + r, n) - 1);
        decimal monthly = (decimal)monthlyPayment;
        decimal totalPaid = monthly * months;
        decimal totalInterest = totalPaid - principal;

        return (Math.Round(monthly, 2), Math.Round(totalPaid, 2), Math.Round(totalInterest, 2));
    }
}
