using System.Linq;
using HalHelper;
using Properties.Model;

namespace Properties.Controllers
{
    static class ResourceExtensions
    {
        public static Resource ToResourceX(this Room room)
        {
            return new Resource($"/api/rooms/{room.Id}")
            {
                State = room
            }.AddLink("roomType", $"/api/roomTypes/{room.RoomTypeId}");
        }
        
        public static Resource ToResource(this RoomType roomType)
        {
            var images = roomType.Images.Select(l => new Link {  Href = l.Href  }).ToList();
            var resources = roomType.SubRooms.Select(ToResource).ToList(); // TODO: Actually query for them
            
            return new Resource($"/api/roomTypes/{roomType.Id}") 
                { 
                    State = new
                    {
                        roomType.Name,
                        roomType.Description,
                        roomType.Amenities,
                        Tags = roomType.Tags.Select(t => t.Tag).ToList(),
                    }
                }
                .AddLinks("images", images.ToArray())
                .AddEmbedded("subRooms", resources);
        }

    }
}