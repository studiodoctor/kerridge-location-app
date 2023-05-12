namespace LocationApp.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


public partial class LocationsViewModel : INotifyPropertyChanged
{

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }

    private LocationDetails _locationDetails;

    public LocationDetails locationDetails
    {
        get { return _locationDetails; }
        set
        {
            _locationDetails = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public INavigation Navigation { get; set; }
    AuthApiService _authapis;
    ObservableCollection<Locations> locations;
    public ObservableCollection<Locations> locationNames { get; } = new();
    string locationSearched;

    public Locations SelectedItem { get; set; }
    public ICommand ViewDetailsCommand { get; }

    public LocationsViewModel(INavigation navigation)
    {
        _authapis = new AuthApiService();
        this.Navigation = navigation;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public ICommand SelectionChangedCommand => new Command<Object>((Object e) =>
    {
        Navigation.PushAsync(new DescriptionPage(SelectedItem));
    });

    public async void DoSearchQuery(string locationName)
    {
        IsBusy = true;
        ApiConfig apiconfig = await _authapis.GetStartupConfigAsync();
        var done = await _authapis.SetAuthTokensAsync(apiconfig);

        try
        {
            locations = await _authapis.GetLocationUsingInput(locationName, apiconfig);

            if (locationNames.Count > 0)
                locationNames.Clear();

            foreach (var location in locations)
            {
                locationNames.Add(location);
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Nothing Found", "Please try searching for other location", "Ok");
            return;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async void getLocationDetailsUsingPlaceId(string placeId)
    {
        ApiConfig apiconfig = await _authapis.GetStartupConfigAsync();
        var done = await _authapis.SetAuthTokensAsync(apiconfig);

        try
        {
            locationDetails = await _authapis.GetLocationUsingPlaceId(placeId, apiconfig);
            _locationDetails = locationDetails;
            // Android.Util.Log.Debug("locationDetails",locationDetails.name);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Nothing Found", "Location details not found, please try again", "Ok");
            return;
        }
    }
}
