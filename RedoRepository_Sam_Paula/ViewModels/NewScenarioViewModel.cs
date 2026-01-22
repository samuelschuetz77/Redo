using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RedoRepository_Sam_Paula.Models;
using RedoRepository_Sam_Paula.Services;

namespace RedoRepository_Sam_Paula.ViewModels;

public class NewScenarioViewModel : INotifyPropertyChanged
{
    private readonly ScenarioStore _store;
    private readonly MortgageCalculator _calculator = new();

    private string _loanAmountText = "";
    private string _interestRateText = "";
    private string _loanTermYearsText = "";
    private string _errorMessage = "";
    private bool _hasError;
    private decimal _monthlyPayment;
    private decimal _totalPaid;
    private decimal _totalInterestPaid;

    public NewScenarioViewModel(ScenarioStore store)
    {
        _store = store;
        SaveCommand = new Command(Save, () => CanSave);
    }

    public string LoanAmountText
    {
        get => _loanAmountText;
        set { if (_loanAmountText == value) return; _loanAmountText = value; OnPropertyChanged(); Recalculate(); }
    }

    public string InterestRateText
    {
        get => _interestRateText;
        set { if (_interestRateText == value) return; _interestRateText = value; OnPropertyChanged(); Recalculate(); }
    }

    public string LoanTermYearsText
    {
        get => _loanTermYearsText;
        set { if (_loanTermYearsText == value) return; _loanTermYearsText = value; OnPropertyChanged(); Recalculate(); }
    }

    public bool HasError
    {
        get => _hasError;
        private set { if (_hasError == value) return; _hasError = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set { if (_errorMessage == value) return; _errorMessage = value; OnPropertyChanged(); }
    }

    public decimal MonthlyPayment
    {
        get => _monthlyPayment;
        private set { if (_monthlyPayment == value) return; _monthlyPayment = value; OnPropertyChanged(); }
    }

    public decimal TotalPaid
    {
        get => _totalPaid;
        private set { if (_totalPaid == value) return; _totalPaid = value; OnPropertyChanged(); }
    }

    public decimal TotalInterestPaid
    {
        get => _totalInterestPaid;
        private set { if (_totalInterestPaid == value) return; _totalInterestPaid = value; OnPropertyChanged(); }
    }

    public bool CanSave => !HasError && MonthlyPayment > 0m;

    public ICommand SaveCommand { get; }

    private void Recalculate()
    {
        HasError = false;
        ErrorMessage = "";

        if (!decimal.TryParse(LoanAmountText, out var principal) || principal <= 0)
        {
            SetInvalid("Loan amount must be a number > 0.");
            return;
        }

        if (!decimal.TryParse(InterestRateText, out var rate) || rate <= 0 || rate > 100)
        {
            SetInvalid("Interest rate must be between 0 and 100.");
            return;
        }

        if (!int.TryParse(LoanTermYearsText, out var years) || years <= 0 || years > 50)
        {
            SetInvalid("Loan term must be a whole number of years (1-50).");
            return;
        }

        var (m, tp, ti) = _calculator.Calculate(principal, rate, years);
        MonthlyPayment = m;
        TotalPaid = tp;
        TotalInterestPaid = ti;

        OnPropertyChanged(nameof(CanSave));
        ((Command)SaveCommand).ChangeCanExecute();
    }

    private void SetInvalid(string message)
    {
        HasError = true;
        ErrorMessage = message;
        MonthlyPayment = 0m;
        TotalPaid = 0m;
        TotalInterestPaid = 0m;

        OnPropertyChanged(nameof(CanSave));
        ((Command)SaveCommand).ChangeCanExecute();
    }

    private void Save()
    {
        decimal.TryParse(LoanAmountText, out var principal);
        decimal.TryParse(InterestRateText, out var rate);
        int.TryParse(LoanTermYearsText, out var years);

        _store.Scenarios.Add(new LoanScenario
        {
            LoanAmount = principal,
            InterestRate = rate,
            TermYears = years,
            MonthlyPayment = MonthlyPayment,
            TotalPaid = TotalPaid,
            TotalInterestPaid = TotalInterestPaid
        });

        Shell.Current.GoToAsync("..");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
