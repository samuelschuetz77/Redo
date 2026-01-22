using RedoRepository_Sam_Paula.Services;
using System.Diagnostics;

namespace RedoRepository_Sam_Paula.Tests
{
    /// <summary>
    /// Unit tests for MortgageCalculator - demonstrates calculations change with different inputs.
    /// These tests can be run by calling RunAllTests() method.
    /// </summary>
    public class MortgageCalculatorTests
    {
        private readonly MortgageCalculator _calculator = new();

        public void RunAllTests()
        {
            Calculate_KnownValues_ReturnsCorrectResults();
            Calculate_ChangingPrincipal_ChangesMonthlyPayment();
            Calculate_ChangingRate_ChangesMonthlyPayment();
            Calculate_ChangingYears_ChangesMonthlyPayment();
            Calculate_InvalidInputs_ReturnsZeros();
            Debug.WriteLine("All MortgageCalculator tests passed!");
        }

        // Test known scenario outputs
        public void Calculate_KnownValues_ReturnsCorrectResults()
        {
            // $200,000 loan at 6% for 30 years
            var (monthly, totalPaid, totalInterest) = _calculator.Calculate(200000m, 6m, 30);

            // Monthly payment should be approximately $1199.10
            Debug.Assert(monthly > 1199m && monthly < 1200m, $"Monthly payment was {monthly}");
            Debug.Assert(totalPaid > 430000m, $"Total paid was {totalPaid}");
            Debug.Assert(totalInterest > 230000m, $"Total interest was {totalInterest}");
        }

        // Test that changing principal changes monthly payment
        public void Calculate_ChangingPrincipal_ChangesMonthlyPayment()
        {
            var (monthly1, _, _) = _calculator.Calculate(100000m, 5m, 30);
            var (monthly2, _, _) = _calculator.Calculate(200000m, 5m, 30);

            Debug.Assert(monthly1 != monthly2, "Monthly payments should differ");
            Debug.Assert(monthly2 > monthly1, "Higher principal should result in higher payment");
        }

        // Test that changing rate changes monthly payment
        public void Calculate_ChangingRate_ChangesMonthlyPayment()
        {
            var (monthly1, _, _) = _calculator.Calculate(200000m, 4m, 30);
            var (monthly2, _, _) = _calculator.Calculate(200000m, 6m, 30);

            Debug.Assert(monthly1 != monthly2, "Monthly payments should differ");
            Debug.Assert(monthly2 > monthly1, "Higher rate should result in higher payment");
        }

        // Test that changing years changes monthly payment
        public void Calculate_ChangingYears_ChangesMonthlyPayment()
        {
            var (monthly1, _, _) = _calculator.Calculate(200000m, 5m, 30);
            var (monthly2, _, _) = _calculator.Calculate(200000m, 5m, 15);

            Debug.Assert(monthly1 != monthly2, "Monthly payments should differ");
            Debug.Assert(monthly2 > monthly1, "Shorter term should result in higher payment");
        }

        // Test invalid inputs return zeros
        public void Calculate_InvalidInputs_ReturnsZeros()
        {
            var (m1, _, _) = _calculator.Calculate(0m, 5m, 30);
            var (m2, _, _) = _calculator.Calculate(200000m, 0m, 30);
            var (m3, _, _) = _calculator.Calculate(200000m, 5m, 0);

            Debug.Assert(m1 == 0m, "Zero principal should return 0");
            Debug.Assert(m2 == 0m, "Zero rate should return 0");
            Debug.Assert(m3 == 0m, "Zero years should return 0");
        }
    }
}
