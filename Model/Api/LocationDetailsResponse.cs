// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AddressComponent
    {
        public string shortName { get; set; }
        public string longName { get; set; }
        public List<int> types { get; set; }
    }

    public class Data
    {
        public List<AddressComponent> addressComponents { get; set; }
        public string formattedAddress { get; set; }
        public string url { get; set; }
        public int utcOffset { get; set; }
        public string vicinity { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
        public List<Photo> photos { get; set; }
        public string placeId { get; set; }
        public List<int> types { get; set; }
    }

    public class MetaLocationDetails
    {
        public string message { get; set; }
    }

    public class Photo
    {
        public string photoId { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class RootLocationDetail
    {
        public Data data { get; set; }
        public MetaLocationDetails meta { get; set; }
    }

