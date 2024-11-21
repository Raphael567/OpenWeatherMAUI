using OpenWeatherAPP.ViewModels;

namespace OpenWeatherAPP.Views;

public partial class OpenWeatherView : ContentPage
{
	public OpenWeatherView()
	{
		InitializeComponent();
		BindingContext = new OpenWeatherViewModel();
	}
}