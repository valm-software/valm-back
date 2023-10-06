namespace Api.Helpers.Errores
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string seriousMessage = null,  string details = null, string funMessage = null) 
                            : base(statusCode, seriousMessage, funMessage)
        {
            Details = details;
        }

        public string Details { get; set; }

    }
}
