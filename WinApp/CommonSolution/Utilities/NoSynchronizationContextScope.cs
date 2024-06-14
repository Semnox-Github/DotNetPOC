/********************************************************************************************
 * Project Name - UploadServerService
 * Description  - Object clears the thread synchronization context  so asynch task can be waited without deadlock
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        7-Jul-2020   Lakshminarayana         Created 
 ********************************************************************************************/
using System;
using System.Threading;

namespace Semnox.Core.Utilities
{
    public static class NoSynchronizationContextScope
    {
        public static Disposable Enter()
        {
            SynchronizationContext context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new Disposable(context);
        }

        public struct Disposable : IDisposable
        {
            private readonly SynchronizationContext synchronizationContext;

            public Disposable(SynchronizationContext synchronizationContext)
            {
                this.synchronizationContext = synchronizationContext;
            }

            public void Dispose() {
                SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            }
        }
    }
}
