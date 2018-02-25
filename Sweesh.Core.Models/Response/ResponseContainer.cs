using System;
namespace Sweesh.Core.Models.Response
{
    public class ResponseContainer<T>
    {
        public T Data { get; set; }
        public bool Worked { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public ResponseContainer(T data)
        {
            Data = data;
            Worked = true;
            Message = "Success";
            StatusCode = 200;
        }

        public ResponseContainer(string message, int status)
        {
            Data = default(T);
            Worked = false;
            Message = message;
            StatusCode = status;
        }
    }
}
