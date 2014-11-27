using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Nest;
using PhotoWidget.Models;

namespace PhotoWidget.Service.Repository
{
    public class GalleryRepository : IGalleryRepository<Gallery, uint>
    {
        private readonly ElasticClient _client;

        private const string ElasticIndex = "galleries";

        public GalleryRepository()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node, ElasticIndex);
            _client = new ElasticClient(settings);
        }

        public IEnumerable<Gallery> Get()
        {
            return _client.Search<Gallery>(
                s => s
                    .Query(q => q.MatchAll())
                    .SortAscending(o => o.CreatedDate)
                ).Documents;
        }

        public Gallery Get(uint id)
        {
            return _client.Search<Gallery>(
                s => s.Query(q => q.Term(f => f.Id, id))
                ).Documents.FirstOrDefault();
        }

        public Gallery Save(Gallery entity)
        {
            if (entity.Id > 0)
            {
                //Delete(entity);
            }
            else
            {
                var result = _client.Search<Gallery>(
                    s => s.Aggregations(a => a.Max("id_max", m => m.Field(p => p.Id)))
                    );
                var maxId = result.Aggs.Max("id_max");
                entity.Id = maxId != null && maxId.Value != null ? (uint)maxId.Value + 1 : 1;
                entity.CreatedDate = DateTime.Now;
            }

            entity.UpdatedDate = DateTime.Now;
            var resultSave = _client.Index(entity);

            if (resultSave.ServerError != null)
            {
                throw new Exception(resultSave.ServerError.Error);
            }

            return entity;
        }

        public void Delete(Gallery entity)
        {
            Delete(entity.Id);
        }

        public void Delete(uint id)
        {
            _client.Delete<Gallery>(id);
        }
    }
}