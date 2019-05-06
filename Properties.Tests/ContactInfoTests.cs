using System.Collections.Generic;
using Newtonsoft.Json;
using Properties.Model;
using Xunit;

namespace Properties.Tests
{
    public class ContactInfoTests
    {
        [Fact]
        public void Serialize()
        {
            var contactInfos = new ContactInfo
            {
                Address = new Address
                {
                    Lines = new List<AddressLine>
                    {
                        new AddressLine {Content = "Line0", LineNo = 0},
                        new AddressLine {Content = "Line1", LineNo = 1}
                    },
                    CityName = "City",
                    CountryName = "Country",
                    PostalCode = "Postcode"
                },
                Email = "email@what.com",
                Name = "Name name",
                Type = "Type",
                PhoneInfos = new List<PhoneInfo>
                {
                    new PhoneInfo {Number = "+441234567890", Type = "Reception"}
                }
            };

            var json = JsonConvert.SerializeObject(contactInfos);
            
            Assert.Equal("{\"Type\":\"Type\",\"Name\":\"Name name\",\"Email\":\"email@what.com\",\"Address\":{\"Lines\":[\"Line0\",\"Line1\"],\"CityName\":\"City\",\"PostalCode\":\"Postcode\",\"CountryName\":\"Country\"},\"PhoneInfos\":[{\"Type\":\"Reception\",\"Number\":\"+441234567890\"}]}", json);
        }

        [Fact]
        public void Deserialize()
        {
            var json = "{\"Type\":\"Type\",\"Name\":\"Name name\",\"Email\":\"email@what.com\",\"Address\":{\"Lines\":[\"Line0\",\"Line1\"],\"CityName\":\"City\",\"PostalCode\":\"Postcode\",\"CountryName\":\"Country\"},\"PhoneInfos\":[{\"Type\":\"Reception\",\"Number\":\"+441234567890\"}]}";
            var contactInfo = JsonConvert.DeserializeObject<ContactInfo>(json);
            
            Assert.Equal("Name name", contactInfo.Name);
        }
    }
}