namespace WebSQLCRUD.Models
{
    public class ResponseModel<T>
    {
        public T? data { get; set; }
        public string message { get; set; } = string.Empty;
        public bool status { get; set; } = true;
    }
}
