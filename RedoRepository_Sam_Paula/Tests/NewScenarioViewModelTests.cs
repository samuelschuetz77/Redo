using RedoRepository_Sam_Paula.Services;
using RedoRepository_Sam_Paula.ViewModels;
using System.Diagnostics;

namespace RedoRepository_Sam_Paula.Tests
{
    /// <summary>
    /// Unit tests for NewScenarioViewModel - demonstrates PropertyChanged events fire correctly.
    /// These tests can be run by calling RunAllTests() method.
    /// </summary>
    public class NewScenarioViewModelTests
    {
        private readonly ScenarioStore _store = new();

        public void RunAllTests()
        {
            LoanAmountText_WhenSet_RaisesPropertyChangedForOutputs();
            InterestRateText_WhenSet_RaisesPropertyChangedForOutputs();
            LoanTermYearsText_WhenSet_RaisesPropertyChangedForOutputs();
            InvalidInput_RaisesPropertyChangedForErrorProperties();
            ValidInput_ClearsErrorState();
            CanSave_WhenInvalid_ReturnsFalse();
            CanSave_WhenValid_ReturnsTrue();
            Outputs_WhenInvalid_AreZero();
            Debug.WriteLine("All NewScenarioViewModel tests passed!");
        }

        // Test that setting LoanAmountText raises PropertyChanged for output properties
        public void LoanAmountText_WhenSet_RaisesPropertyChangedForOutputs()
        {
            var vm = new NewScenarioViewModel(_store);
            var changedProperties = new List<string>();
            vm.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName!);

            // Set valid inputs
            vm.InterestRateText = "5";
            vm.LoanTermYearsText = "30";
            changedProperties.Clear();

            vm.LoanAmountText = "200000";

            Debug.Assert(changedProperties.Contains("MonthlyPayment"), "MonthlyPayment PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("TotalPaid"), "TotalPaid PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("TotalInterestPaid"), "TotalInterestPaid PropertyChanged not raised");
        }

        // Test that setting InterestRateText raises PropertyChanged for output properties
        public void InterestRateText_WhenSet_RaisesPropertyChangedForOutputs()
        {
            var vm = new NewScenarioViewModel(_store);
            var changedProperties = new List<string>();
            vm.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName!);

            // Set valid inputs
            vm.LoanAmountText = "200000";
            vm.LoanTermYearsText = "30";
            changedProperties.Clear();

            vm.InterestRateText = "6";

            Debug.Assert(changedProperties.Contains("MonthlyPayment"), "MonthlyPayment PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("TotalPaid"), "TotalPaid PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("TotalInterestPaid"), "TotalInterestPaid PropertyChanged not raised");
        }

        // Test that setting LoanTermYearsText raises PropertyChanged for output properties
        public void LoanTermYearsText_WhenSet_RaisesPropertyChangedForOutputs()
        {
            var vm = new NewScenarioViewModel(_store);
            var changedProperties = new List<string>();
            vm.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName!);

            // Set valid inputs
            vm.LoanAmountText = "200000";
            vm.InterestRateText = "5";
            changedProperties.Clear();

            vm.LoanTermYearsText = "30";

            Debug.Assert(changedProperties.Contains("MonthlyPayment"), "MonthlyPayment PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("TotalPaid"), "TotalPaid PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("TotalInterestPaid"), "TotalInterestPaid PropertyChanged not raised");
        }

        // Test that invalid input raises PropertyChanged for HasError and ErrorMessage
        public void InvalidInput_RaisesPropertyChangedForErrorProperties()
        {
            var vm = new NewScenarioViewModel(_store);
            var changedProperties = new List<string>();
            vm.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName!);

            vm.LoanAmountText = "invalid";

            Debug.Assert(changedProperties.Contains("HasError"), "HasError PropertyChanged not raised");
            Debug.Assert(changedProperties.Contains("ErrorMessage"), "ErrorMessage PropertyChanged not raised");
            Debug.Assert(vm.HasError, "HasError should be true");
            Debug.Assert(!string.IsNullOrEmpty(vm.ErrorMessage), "ErrorMessage should not be empty");
        }

        // Test that valid input clears error state
        public void ValidInput_ClearsErrorState()
        {
            var vm = new NewScenarioViewModel(_store);

            // First set invalid
            vm.LoanAmountText = "invalid";
            Debug.Assert(vm.HasError, "HasError should be true after invalid input");

            // Then set valid values
            vm.LoanAmountText = "200000";
            vm.InterestRateText = "5";
            vm.LoanTermYearsText = "30";

            Debug.Assert(!vm.HasError, "HasError should be false after valid input");
            Debug.Assert(string.IsNullOrEmpty(vm.ErrorMessage), "ErrorMessage should be empty after valid input");
        }

        // Test CanSave is false when invalid
        public void CanSave_WhenInvalid_ReturnsFalse()
        {
            var vm = new NewScenarioViewModel(_store);
            vm.LoanAmountText = "invalid";

            Debug.Assert(!vm.CanSave, "CanSave should be false when invalid");
        }

        // Test CanSave is true when valid
        public void CanSave_WhenValid_ReturnsTrue()
        {
            var vm = new NewScenarioViewModel(_store);
            vm.LoanAmountText = "200000";
            vm.InterestRateText = "5";
            vm.LoanTermYearsText = "30";

            Debug.Assert(vm.CanSave, "CanSave should be true when valid");
        }

        // Test outputs go to 0 when invalid
        public void Outputs_WhenInvalid_AreZero()
        {
            var vm = new NewScenarioViewModel(_store);

            // First set valid values
            vm.LoanAmountText = "200000";
            vm.InterestRateText = "5";
            vm.LoanTermYearsText = "30";
            Debug.Assert(vm.MonthlyPayment > 0, "MonthlyPayment should be > 0 for valid inputs");

            // Then make invalid
            vm.LoanAmountText = "invalid";

            Debug.Assert(vm.MonthlyPayment == 0m, "MonthlyPayment should be 0 when invalid");
            Debug.Assert(vm.TotalPaid == 0m, "TotalPaid should be 0 when invalid");
            Debug.Assert(vm.TotalInterestPaid == 0m, "TotalInterestPaid should be 0 when invalid");
        }
    }
}
