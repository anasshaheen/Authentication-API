namespace ApiAuth.API.ViewModels
{
    public class ResponseViewModel
    {
        public ResponseViewModel(dynamic data, string message = null)
        {
            Data = data;
            Message = message;
        }

        public ResponseViewModel(string message)
        {
            Message = message;
        }

        public dynamic Data { get; set; }
        public string Message { get; set; }
    }
}
