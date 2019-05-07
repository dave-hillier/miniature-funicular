using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    
    [Owned]
    public class ImageLink 
    {       
        public int SortValue { get; set; }
        public string Href { get; set; }
    }
    
    [Owned]
    public class ImageLink1
    {       
        public int SortValue { get; set; }
        public string Href { get; set; }
    }
   
}