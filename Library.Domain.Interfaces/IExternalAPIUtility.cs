using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IExternalAPIUtility
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync<TRequest>(string url, TRequest request);
    }
}
