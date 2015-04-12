using System;
using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;
using PhotoWidget.Attributes;

namespace PhotoWidget.Models
{
    [Table("galleries.galleries")]
    public class Gallery
    {
        [Column("id", NpgsqlDbType.Integer, IsSetByStorage = true, IsPrimaryKey = true)]
        [System.ComponentModel.DefaultValue(0)]
        public int Id { get; set; }

        [Column("name", NpgsqlDbType.Varchar)]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Column("description", NpgsqlDbType.Varchar)]
        [MaxLength(255, ErrorMessage = "Description can't be longer than 255 characters")]
        public string Description { get; set; }

        [Column("created_date", NpgsqlDbType.TimestampTZ, IsSetByStorage = true)]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date", NpgsqlDbType.TimestampTZ, IsSetByStorage = true)]
        public DateTime UpdatedDate { get; set; }
    }

    public class GallerySettings
    {
        public ImageSize ImagesSize { get; set; }
    }
}