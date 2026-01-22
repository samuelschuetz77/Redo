using RedoRepository_Sam_Paula.ViewModels;

namespace RedoRepository_Sam_Paula.Views;

public partial class NewScenarioPage : ContentPage
{
    public NewScenarioPage(NewScenarioViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
