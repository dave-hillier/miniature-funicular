using System;
using System.Collections.Generic;
using System.Linq;
using HalHelper;
using Properties.Model;

namespace Properties.Controllers
{
    static class ResourceExtensions
    {
        public static Resource ToResource(this Room room)
        {
            return new Resource($"/api/rooms/{room.Id}")
                {
                    State = new
                    {
                        room.Name,
                        room.Description
                    }
                }
                .AddLink("roomType", $"/api/roomTypes/{room.RoomTypeId}");
        }
        
        public static Resource ToResource(this RoomType roomType)
        {
            if (roomType == null)
                throw new ArgumentException("roomType");
            var images = roomType.Images != null ? roomType.Images.Select(l => new Link {  Href = l.Href  }).ToArray() : 
                new Link[]{};            
            var resources = roomType.SubRooms != null ? roomType.SubRooms.Select(ToResource).ToList() : new List<Resource>(); 
            
            return new Resource($"/api/roomTypes/{roomType.Id}") 
                { 
                    State = new
                    {
                        roomType.Name,
                        roomType.Description,
                        roomType.Amenities,
                        Tags = roomType.Tags?.Select(t => t.Tag).ToList(),
                    }
                }
                .AddLinks("images", images)
                .AddEmbedded("subRooms", resources);
        }
        
        public static Resource ToResource(this PropertyVersion propertyVersion)
        {
            var imageLinks = propertyVersion.Images != null ? 
                propertyVersion.Images.Select(l => new Link { Href = l.Href }).ToArray() : new Link[] {};

            return new Resource($"/api/properties/{propertyVersion.Tenant}/{propertyVersion.Version}")
                {
                    State = new
                    {
                        propertyVersion.Name,
                        propertyVersion.Description,
                        propertyVersion.Updated,
                        propertyVersion.Category,
                        propertyVersion.ContactInfos,
                    },
                }
                .AddLinks("images", imageLinks)
                .AddEmbedded("rooms", propertyVersion.Rooms.Select(ToResource).ToList())
                .AddEmbedded("roomsTypes", propertyVersion.RoomTypes.Select(ToResource).ToList());
        }

    }
}