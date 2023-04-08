namespace LocalStore.Controllers.Shared
{
    public class ApiResponse<T> where T : class?
    {
        public T? Data { get; set; }
        public bool? IsSucesful { get; set; }
        public string ClientMessage { get; set; } = string.Empty;
        public string TechnicalMessage { get; set; } = string.Empty;
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public ApiResponse<T> SucessResponse(T Data, string ClientMessage)
        {
            this.Data = Data;
            this.ClientMessage = ClientMessage;
            IsSucesful = true;

            return this;
        }

        public ApiResponse<T> FailureResponse(T? Data, string ClientMessage, string TechnicalMessage, IEnumerable<string> Errors)
        {
            this.Data = Data;
            this.ClientMessage = ClientMessage;
            this.TechnicalMessage = TechnicalMessage;
            IsSucesful = false;
            this.Errors = Errors;

            return this;
        }

    }
}
