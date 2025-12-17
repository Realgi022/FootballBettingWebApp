namespace BLL.Exceptions
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
