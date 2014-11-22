using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhotoWidget.Models.Public
{
    public class GalleryImage
    {
        [DataMember(IsRequired = true)]
        public string SourceUrl { get; set; }
    }
}