using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Utilities
{
    public class ExternalAPIUtility : IExternalAPIUtility
    {
        private readonly HttpClient _httpClient;
        
        public ExternalAPIUtility(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }
        public async Task<HttpResponseMessage> PostAsync<TRequest>(string url, TRequest request)
        {
            var content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }
    }
}
