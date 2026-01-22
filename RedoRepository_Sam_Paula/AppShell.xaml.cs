using RedoRepository_Sam_Paula.Views;

namespace RedoRepository_Sam_Paula
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewScenarioPage), typeof(NewScenarioPage));
        }
    }
}
