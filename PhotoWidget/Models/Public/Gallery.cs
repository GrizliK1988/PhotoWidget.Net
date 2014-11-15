using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhotoWidget.Models.Public
{
    [DataContract]
    public class Gallery
    {
        [DataMember(Name = "Name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "Images", IsRequired = true)]
        public GalleryImage[] Images { get; set; }
    }
}