namespace Application
{
    public abstract class AppResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
