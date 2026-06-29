namespace NotificationService.Common
{
    public class ResponseData<T>
    {
        public ResponseData()
        {
            result = 0;
            time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            dataDescription = string.Empty;
            data = default;
            data2nd = null;
            error = new ResponseError();
        }

        public ResponseData(int result, int code, string message)
        {
            this.result = result;
            time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            dataDescription = string.Empty;
            data = default;
            data2nd = null;
            error = new ResponseError(code, message);
        }

        public int result { get; set; }
        public long time { get; set; }
        public string dataDescription { get; set; }
        public T? data { get; set; }
        public object? data2nd { get; set; }
        public ResponseError error { get; set; }
    }

    public class ResponseError
    {
        public ResponseError()
        {
            code = 200;
            message = string.Empty;
        }

        public ResponseError(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public int code { get; set; }
        public string message { get; set; }
    }
}
