namespace UserService.Common
{
    public class ResponseData<T>
    {
        public ResponseData()
        {
            result = 0;
            time = Utilities.CurrentTimeSeconds();
            dataDescription = string.Empty;
            data = default;
            data2nd = null;
            error = new error();
        }
        public ResponseData(int result, int code, string message)
        {
            this.result = result;
            time = Utilities.CurrentTimeSeconds();
            dataDescription = string.Empty;
            data = default;
            data2nd = null;
            error = new error(code, message);
        }
        public int result { get; set; } // 0:fail | 1:success
        public long time { get; set; }
        public string dataDescription { get; set; }
        public T data { get; set; }
        public dynamic data2nd { get; set; }
        public error error { get; set; }
    }
    public class error
    {
        public error()
        {
            code = 200;
            message = string.Empty;
        }
        public error(int _code, string _messege)
        {
            code = _code;
            message = _messege;
        }
        public int code { get; set; }
        public string message { get; set; }
    }
}
