using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;

namespace Semnox.Parafait.Logging
{
    public class SemnoxMemoryAppender : MemoryAppender
    {
        private int count = 0;
        private int limit;
        public SemnoxMemoryAppender(int limit = 10000)
        {
            if(limit <= 0)
            {
                throw new ArgumentException("Limit should be a positive integer");
            }
            this.limit = limit;
        }
        
        protected override void Append(LoggingEvent loggingEvent)
        {
            count++;
            if(count > limit)
            {
                Clear();
                count = 0;
            }
            base.Append(loggingEvent);
        }
    }
}
