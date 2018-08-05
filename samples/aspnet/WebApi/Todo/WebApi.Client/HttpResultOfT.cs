using System.Collections.Generic;

namespace WebApi.Client
{
    public class HttpResult<T> : HttpResult
    {
        public HttpResult()
            : base()
        {
        }

        protected HttpResult(IList<string> errors)
            : base(errors)
        {
        }

        public HttpResult(T content)
            : base()
        {
            this.Content = content;
        }

        public T Content { get; set; }

        public static new HttpResult<T> Failure(string error)
        {
            return new HttpResult<T>(new string[] { error });
        }

        public static new HttpResult<T> Failure(IList<string> errors)
        {
            return new HttpResult<T>(errors);
        }
    }
}
