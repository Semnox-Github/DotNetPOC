using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LogManager
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<string, Semnox.Parafait.logging.Logger> repository = new ConcurrentDictionary<string, Parafait.logging.Logger>();
        public static Semnox.Parafait.logging.Logger GetLogger(ExecutionContext executionContext, Type type)
        {
            log.LogMethodEntry(executionContext, "type");
            string key = type.AssemblyQualifiedName;
            Semnox.Parafait.logging.Logger result;
            result = repository.GetOrAdd(key, (k) => new Semnox.Parafait.logging.Logger(type));
            log.LogMethodExit();
            return result;
        }

        public static Semnox.Parafait.logging.Logger GetLogger(Type type)
        {
            return GetLogger(null, type);
        }


    }
}
