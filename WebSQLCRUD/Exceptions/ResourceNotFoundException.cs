namespace WebSQLCRUD.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(object id)
            : base($"Resource with ID '{id}' not found.")
        {
        }
    }
}
