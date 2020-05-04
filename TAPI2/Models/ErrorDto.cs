using TAPI2.Exceptions;

namespace TAPI2.Models
{
    public class ErrorDto
    {
        public ExceptionType ErrorType  { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ErrorClassType { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }

        public ErrorDto()
        { }

        public ErrorDto(ExceptionType errorType, string title, string message, int status, string traceId, string errorClassType)
        {
            ErrorType = errorType;
            Title = title;
            Message = message;
            Status = status;
            TraceId = traceId;
            ErrorClassType = errorClassType;
        }
    }
}