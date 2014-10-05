using System;
using System.Collections.Generic;
using System.Linq;
using PhotoWidget.Models;

namespace PhotoWidget.Service.Repository
{
    public class GalleryRepository : IGalleryRepository<Gallery, uint>
    {
        private static Gallery[] _galleries = {};

        public IEnumerable<Gallery> Get()
        {
            return _galleries.OrderBy(g => g.Id);
        }

        public Gallery Get(uint id)
        {
            return _galleries.FirstOrDefault(g => g.Id == id);
        }

        public Gallery Save(Gallery entity)
        {
            if (entity.Id > 0)
            {
                Delete(entity);
            }
            else
            {
                entity.Id = _galleries.Length > 0 ? _galleries.Select(g => g.Id).Max() + 1 : 1;
                entity.CreatedDate = DateTime.Now;
            }

            var galleriesList = _galleries.ToList();
            galleriesList.Add(entity);
            _galleries = galleriesList.ToArray();

            return entity;
        }

        public void Delete(Gallery entity)
        {
            Delete(entity.Id);
        }

        public void Delete(uint id)
        {
            _galleries = _galleries.Where(g => g.Id != id).ToArray();
        }
    }
}