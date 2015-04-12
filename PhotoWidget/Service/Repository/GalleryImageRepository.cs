using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Nest;
using PhotoWidget.Models;

namespace PhotoWidget.Service.Repository
{
    public class GalleryImageRepository : IGalleryImageRepository<GalleryImage, string>
    {
        private readonly ElasticClient _client;

        private const string ElasticIndex = "galleries";

        public GalleryImageRepository()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node, ElasticIndex);
            _client = new ElasticClient(settings);
        }

        public IEnumerable<GalleryImage> Get()
        {
            return _client.Search<GalleryImage>(
                s => s
                    .Query(q => q.MatchAll())
                    .SortAscending(o => o.CreatedDate)
                ).Documents;
        }

        public IEnumerable<GalleryImage> GetForGallery(uint galleryId)
        {
            return _client.Search<GalleryImage>(
                s => s
                    .Query(q => q.Term("galleryId", galleryId))
                    .SortAscending(o => o.CreatedDate)
                ).Documents;
        }

        public GalleryImage Get(string id)
        {
            var response = _client.Get<GalleryImage>(id);
            return response.Source;
        }

        public void Save(ref GalleryImage entity)
        {
            if (entity.Id != null)
            {
                Delete(entity);
            }
            else
            {
                entity.CreatedDate = DateTime.Now;
            }

            var response = _client.Index(entity);
            entity.Id = response.Id;
        }

        public void Delete(GalleryImage entity)
        {
            Delete(entity.Id);
        }

        public void Delete(string id)
        {
            _client.Delete<GalleryImage>(id);
        }
    }
}