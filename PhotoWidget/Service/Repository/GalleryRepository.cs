using System.Collections.Generic;
using System.Linq;
using Npgsql;
using PhotoWidget.Models;
using PhotoWidget.Service.Exception;
using PhotoWidget.Service.Helper.Attributes;
using PhotoWidget.Service.Storage;
using PhotoWidget.Service.Storage.DataMapper;
using PhotoWidget.Service.Storage.DataMapper.Command;

namespace PhotoWidget.Service.Repository
{
    public class GalleryRepository : IGalleryRepository<Gallery, int>
    {
        private readonly IConnectionManager _connManager;
        private readonly DbCommand<Gallery> _dbCommand = new DbCommand<Gallery>();

        public GalleryRepository(IConnectionManager connectionManager)
        {
            _connManager = connectionManager;
        }

        public IEnumerable<Gallery> Get()
        {
            var galleries = new List<Gallery>();

            var command = _dbCommand.Select.All();
            command.Connection = _connManager.GetConnection();
            command.Prepare();

            var result = command.ExecuteReader();
            if (!result.HasRows) {
                return galleries;
            }

            while (result.Read()) {
                galleries.Add(Mapper<Gallery>.MapResultToEntity(result));
            }
            result.Close();
            return galleries;
        }

        public Gallery Get(int id)
        {
            var pkProperty = ColumnHelper.GetColumnProperties<Gallery>().First(x => x.column.IsPrimaryKey);

            var command = _dbCommand.Select.ByProperty(pkProperty.property, id);
            command.Connection = _connManager.GetConnection();
            command.Prepare();

            var result = command.ExecuteReader();
            if (!result.HasRows) {
                return null;
            }

            result.Read();
            var gallery = Mapper<Gallery>.MapResultToEntity(result);
            result.Close();

            return gallery;
        }

        public void Save(ref Gallery entity)
        {
            NpgsqlCommand command;
            if (entity.Id == 0) {
                command = _dbCommand.Insert.Single(entity);
            } else {
                command = _dbCommand.Update.ById(entity);
            }

            command.Connection = _connManager.GetConnection();
            command.Prepare();

            var result = command.ExecuteReader();
            if (!result.HasRows)
            {
                throw new RepositoryException("Operation is failed");
            }

            result.Read();
            Mapper<Gallery>.AggregateEntityFromReturningResult(ref entity, result);
            result.Close();
        }

        public void Delete(Gallery entity)
        {
            Delete(entity.Id);
        }

        public void Delete(int id)
        {
            var command = _dbCommand.Delete.ById(id);
            command.Connection = _connManager.GetConnection();
            command.Prepare();

            command.ExecuteNonQuery();
        }
    }
}