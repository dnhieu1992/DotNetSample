namespace ASPNetHandleError
{
    public class ApplicationErrorResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessages { get; set; }
        public string StackTrace { get; set; }
        public string Type { get; set; }
    }
}
