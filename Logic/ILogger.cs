using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface Ilogger
    {
         bool HasError { get; set; }

        void Clean();

        void LogError(string prefix, string messagge, int line);

        void LogWarning(string prefix, string messagge, int line);

        void Log(string prefix, string messagge, int line);

    }
}
