using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoWidget.Models
{
    public class Gallery
    {
        [System.ComponentModel.DefaultValue(0)]
        public uint Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}