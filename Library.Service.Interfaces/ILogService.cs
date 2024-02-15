using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Interfaces
{
    public interface ILogService
    {
        void LogException(Exception ex = null, string message = null);
    }
}
