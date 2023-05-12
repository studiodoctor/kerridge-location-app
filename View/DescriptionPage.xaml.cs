namespace LocationApp.View;

using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

public partial class DescriptionPage : ContentPage
{
    private readonly LocationsViewModel _lvm;
    public Locations SelectedItem { get; }
    public DescriptionPage(Locations selectedItem)
    {
        InitializeComponent();
        _lvm = new LocationsViewModel(Navigation);
        SelectedItem = selectedItem;
        BindingContext = _lvm;

        if (Connectivity.NetworkAccess == NetworkAccess.None)
        {
            DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
        }
        else
        {
            _lvm.getLocationDetailsUsingPlaceId(SelectedItem.placeId);
        }
    }
}