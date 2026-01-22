using System.Collections.ObjectModel;
using RedoRepository_Sam_Paula.Models;

namespace RedoRepository_Sam_Paula.Services;

public class ScenarioStore
{
    public ObservableCollection<LoanScenario> Scenarios { get; } = new();
}
