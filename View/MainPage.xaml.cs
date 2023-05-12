namespace LocationApp.View;
public partial class MainPage : ContentPage
{
    LocationsViewModel _lvm;
    public MainPage()
    {
        InitializeComponent();
        _lvm = new LocationsViewModel(Navigation);
        BindingContext = _lvm;

        // _lvm.DoSearchQuery("sandton"); 
        HomeLabel.IsVisible = true;
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var locationName = ((SearchBar)sender).Text;
        if (Connectivity.NetworkAccess == NetworkAccess.None)
        {
            DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
        }
        else
        {
            _lvm.DoSearchQuery(locationName);
            HomeLabel.IsVisible = false;
        }

    }
}

