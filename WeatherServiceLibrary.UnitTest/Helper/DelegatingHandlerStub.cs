using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherServiceLibrary.UnitTests.Helper
{
    public class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _content;

        public DelegatingHandlerStub(HttpStatusCode statusCode,string content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode);
            response.Content = new StringContent(_content);
            return Task.FromResult(response);
        }
    }
}
