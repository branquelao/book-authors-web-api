namespace WebSQLCRUD.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(String msg)
        : base (msg)
        { 
        }
    }
}
