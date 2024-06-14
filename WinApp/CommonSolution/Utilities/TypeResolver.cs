/********************************************************************************************
* Project Name - Utilities
* Description  - utility class to get the types based on name.  
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     2-Feb-2021   Lakshminarayana         Created 
********************************************************************************************/
using System;
using System.Collections.Concurrent;

namespace Semnox.Core.Utilities
{
    class TypeResolver
    {
        
        private static readonly ConcurrentDictionary<string, Type> typeCache = new ConcurrentDictionary<string, Type>();
        public static Type GetType(string typeName, Type defaltValue)
        {
            Type result;
            if (typeCache.TryGetValue(typeName, out result))
            {
                return result;
            }
            result = GetTypeImpl(typeName);
            if(result == null)
            {
                result = defaltValue;
            }
            typeCache[typeName] = result;
            return result;
        }

        private static Type GetTypeImpl(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
                return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
