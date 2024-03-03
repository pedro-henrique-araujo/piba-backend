namespace Piba.Data.Entities
{
    public class Log
    {
        public Log(string message)
        {
            Message = message;
            Created = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Message { get; private set; }
        public DateTime Created { get; private set; }
    }
}
