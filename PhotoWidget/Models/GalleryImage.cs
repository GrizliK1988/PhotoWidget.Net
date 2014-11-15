using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhotoWidget.Models
{
    [DataContract]
    public class GalleryImage
    {
        [System.ComponentModel.DefaultValue(null)]
        public string Id { get; set; }

        public uint GalleryId { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        public string MimeType { get; set; }

        public string Extension { get; set; }

        [DataMember(IsRequired = true)]
        public string Source { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}