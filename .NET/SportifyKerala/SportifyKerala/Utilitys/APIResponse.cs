namespace SportifyKerala.Utilitys
{
    public class APIResponse<T>
    {
        public T data { get; set; }
        public string message { get; set; }
        public bool status { get; set; }

        public APIResponse(T data, string message, bool status)
        {
            this.data = data;
            this.message = message;
            this.status = status;
        }

        public static APIResponse<T> Error(string message)
        {
            return new APIResponse<T>( default, message,false);
        }

        public static APIResponse<T> Success(T data,string message)
        {
            return new APIResponse<T>(data,message,true);
        }
       

        
    }
}
