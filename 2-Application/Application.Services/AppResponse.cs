namespace Application
{
    public abstract class AppResponse
    {
        public AppResponse()
        {
        }

        public AppResponse(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
