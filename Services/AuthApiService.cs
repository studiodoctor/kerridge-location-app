using System.Text;
using System.Text.Json;
using System.Collections.ObjectModel;

public class AuthApiService
{
    HttpClient _client;
    AccessTokenRes accessToken;
    DateTime expireTime;

    public AuthApiService()
    {
        _client = new HttpClient();
        expireTime = DateTime.Now;

    }

    public Task<ApiConfig> GetStartupConfigAsync()
    {

        var output = new ApiConfig
        {
            ClientId = "94eb850d-448f-48ef-a085-5fee4807606e.apps.kerridgecs.com",
            ClientSecret = "b609f130-2d13-43d4-93df-f8ab9f1cafde",
            KerridgeAuthApi = "https://staging.identity.eos.kerridgecs.online/",
            KerridgeApi = "https://staging.api.eos.kerridgecs.online/location/api/v1/locations/"
        };
        return Task.FromResult(output);
    }

    public async Task<bool> SetAuthTokensAsync(ApiConfig apiConfig)
    {

        if (accessToken is not null && DateTime.Now < expireTime)
            return false;

        var authData = $"{apiConfig.ClientId}:{apiConfig.ClientSecret}";
        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

        _client.DefaultRequestHeaders.Authorization = new("Basic", authHeaderValue);

        StringContent content = new("grant_type=client_credentials&scope=eos.common.fullaccess", Encoding.ASCII, "application/x-www-form-urlencoded");

        HttpResponseMessage res = await _client.PostAsync(apiConfig.KerridgeAuthApi + "connect/token", content);

        if (res.IsSuccessStatusCode)
        {
            var resCont = await res.Content.ReadAsStringAsync();
            accessToken = JsonSerializer.Deserialize<AccessTokenRes>(resCont);
            expireTime = DateTime.Now;
            expireTime = expireTime.AddSeconds(accessToken.expires_in);

            return true;
        }
        else
        {
            throw new Exception("Unable to Authenticate, Sorting Functionality Offline");
        }

    }

    public async Task<ObservableCollection<Locations>> GetLocationUsingInput(string location, ApiConfig apiConfig)
    {
        ObservableCollection<Locations> locationNames = new ObservableCollection<Locations>();

        string query = $"places/?criteria={location}";
        Uri uri = new($"{apiConfig.KerridgeApi}{query}");

        _client.DefaultRequestHeaders.Authorization = new("Bearer", accessToken.access_token);

        HttpResponseMessage res = await _client.GetAsync(uri);

        if (res.IsSuccessStatusCode)
        {
            string content = await res.Content.ReadAsStringAsync();
            var jsonRes = JsonSerializer.Deserialize<Root>(content);

            foreach (var store in jsonRes.data)
            {
                if (!store.placeId.Equals(""))
                {
                    locationNames.Add(new Locations() { mainText = store.mainText, placeId = store.placeId, secondaryText = store.secondaryText, description = store.description });
                }
            }

            if (locationNames.Count == 0)
                throw new Exception("No locations found");

            return locationNames;
        }
        else
        {
            throw new Exception("No locations found");
        }
    }

    public async Task<LocationDetails> GetLocationUsingPlaceId(string placeId, ApiConfig apiConfig)
    {

        string query = $"places/{placeId}";
        Uri uri = new($"{apiConfig.KerridgeApi}{query}");

        _client.DefaultRequestHeaders.Authorization = new("Bearer", accessToken.access_token);

        HttpResponseMessage res = await _client.GetAsync(uri);

        if (res.IsSuccessStatusCode)
        {
            string content = await res.Content.ReadAsStringAsync();
            var jsonRes = JsonSerializer.Deserialize<RootLocationDetail>(content);

            return new LocationDetails
            {
                name = jsonRes.data.name,
                formatted_address = jsonRes.data.formattedAddress,
                vicinity = jsonRes.data.vicinity,
                url = jsonRes.data.url,
                lat_long = jsonRes.data.latitude + "," + jsonRes.data.longitude,
                utc_offset = jsonRes.data.utcOffset.ToString()
            };
        }
        else
        {
            throw new Exception("No locations found");
        }
    }
}