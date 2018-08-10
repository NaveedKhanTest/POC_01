using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace POC.API.Tests.IntegrationTests
{
    public class ApiResponseMessage<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public Uri Location { get; set; }
        public T Item { get; set; }
        public List<T> ListItems { get; set; }


    }
}
