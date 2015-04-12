namespace PhotoWidget.Service.Storage.DataMapper.Query
{
    public class SqlQuery<T>
    {
        private readonly SelectQuery<T> _select = new SelectQuery<T>();
        private readonly InsertQuery<T> _insert = new InsertQuery<T>();
        private readonly UpdateQuery<T> _update = new UpdateQuery<T>();
        private readonly DeleteQuery<T> _delete = new DeleteQuery<T>();

        public SelectQuery<T> Select
        {
            get { return _select; }
        }

        public InsertQuery<T> Insert
        {
            get { return _insert; }
        }

        public UpdateQuery<T> Update
        {
            get { return _update; }
        }

        public DeleteQuery<T> Delete
        {
            get { return _delete; }
        }
    }
}