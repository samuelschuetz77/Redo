using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RedoRepository_Sam_Paula.Models;
using RedoRepository_Sam_Paula.Services;
using RedoRepository_Sam_Paula.Views;

namespace RedoRepository_Sam_Paula.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ScenarioStore _store;

        public HomeViewModel(ScenarioStore store)
        {
            _store = store;
            NewScenarioCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(NewScenarioPage)));
            ClearCommand = new Command(() => _store.Scenarios.Clear());
        }

        public ObservableCollection<LoanScenario> Scenarios => _store.Scenarios;

        public ICommand NewScenarioCommand { get; }

        public ICommand ClearCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
