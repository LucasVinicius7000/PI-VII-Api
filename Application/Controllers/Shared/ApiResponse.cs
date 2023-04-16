namespace LocalStore.Application.Controllers.Shared
{
    public class ApiResponse<T> where T : class?
    {
        public T? Data { get; set; }
        public bool? IsSucessful { get; set; }
        public string ClientMessage { get; set; } = string.Empty;
        public string TechnicalMessage { get; set; } = string.Empty;
        public object Errors { get; set; }

        public ApiResponse<T> SucessResponse(T Data, string ClientMessage)
        {
            this.Data = Data;
            this.ClientMessage = ClientMessage;
            IsSucessful = true;

            return this;
        }

        public ApiResponse<T> FailureResponse(string ClientMessage, string? TechnicalMessage, object? Errors)
        {
            this.Data = null;
            this.ClientMessage = ClientMessage;
            this.TechnicalMessage = TechnicalMessage;
            IsSucessful = false;
            this.Errors = Errors;

            return this;
        }

    }
}
