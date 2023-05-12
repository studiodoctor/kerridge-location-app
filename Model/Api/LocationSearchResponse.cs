// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string placeId { get; set; }
        public string description { get; set; }
        public string mainText { get; set; }
        public string secondaryText { get; set; }
        public List<Term> terms { get; set; }
        public List<int> types { get; set; }
    }

    public class Meta
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; }
        public string message { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }

    public class Term
    {
        public string value { get; set; }
        public string offset { get; set; }
    }

