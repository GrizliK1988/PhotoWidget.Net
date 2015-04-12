using PhotoWidget.Service.Storage.DataMapper.Query;

namespace PhotoWidget.Service.Storage.DataMapper.Command
{
    public class DbCommand<T>
    {
        protected readonly SqlQuery<T> query = new SqlQuery<T>();
        private readonly SelectCommand<T> _select;
        private readonly InsertCommand<T> _insert;
        private readonly UpdateCommand<T> _update;
        private readonly DeleteCommand<T> _delete;

        public SelectCommand<T> Select
        {
            get { return _select; }
        }

        public InsertCommand<T> Insert
        {
            get { return _insert; }
        }

        public UpdateCommand<T> Update
        {
            get { return _update; }
        }

        public DeleteCommand<T> Delete
        {
            get { return _delete; }
        }

        public DbCommand()
        {
            _select = new SelectCommand<T>(query);
            _insert = new InsertCommand<T>(query);
            _update = new UpdateCommand<T>(query);
            _delete = new DeleteCommand<T>(query);
        }
    }
}