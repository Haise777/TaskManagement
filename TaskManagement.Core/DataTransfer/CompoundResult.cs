using TaskManagement.API.DataTransfer;

namespace TaskManagement.API.Response
{
    // Serves the purpose of allowing services to send back more dynamic responses
    public class CompoundResult<T>
    {
        public T Value { get; set; }

        public bool HasError { get; set; }
        public ErrorResponse Error { get; set; }
    }
}
