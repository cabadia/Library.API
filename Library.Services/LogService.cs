using Library.Domain.Interfaces;
using Library.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LogService : ILogService
    {
        private readonly IExternalAPIUtility _externalAPIUtility;
        public LogService(IExternalAPIUtility externalAPIUtility)
        {
            _externalAPIUtility = externalAPIUtility;
        }

        public void LogException(Exception ex = null, string message = null)
        {
            var task = Task.Run(() => _externalAPIUtility.PostAsync<object>("LogException", new { Location = System.Reflection.Assembly.GetCallingAssembly().GetName().Name, Error = ex?.Message ?? string.Empty, Message = message ?? string.Empty }));
            task.Wait();
        }
    }
}
