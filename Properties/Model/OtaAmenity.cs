namespace Properties.Model
{
    class OtaAmenity 
    {
        public int Id { get; set; } // Matches the OTA value
        
        public int? Value { get; set; }
        
        public string ShortCode { get; set; } // As a lookup for localization
    }
}