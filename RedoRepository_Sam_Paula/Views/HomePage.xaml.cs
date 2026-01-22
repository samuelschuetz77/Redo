using RedoRepository_Sam_Paula.ViewModels;

namespace RedoRepository_Sam_Paula.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}