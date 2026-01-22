namespace RedoRepository_Sam_Paula.Models;

public class LoanScenario
{
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermYears { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalInterestPaid { get; set; }
}
