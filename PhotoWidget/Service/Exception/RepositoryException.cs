namespace PhotoWidget.Service.Exception
{
    public class RepositoryException : System.Exception
    {
        public RepositoryException(string error) : base(error)
        {}
    }
}