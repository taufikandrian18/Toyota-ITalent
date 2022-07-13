using System;
using System.Net;

namespace TAM.Talent.Commons.Core.Models
{
    public class S3Response
    {
        public HttpStatusCode Status { get; set; }

        public string Message { get; set; }
    }
}
