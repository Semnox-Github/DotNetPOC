/********************************************************************************************
 * Project Name - Transaction
 * Description  - Represents a combined KDS Terminal
 *
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks
 *********************************************************************************************
 *1.00        10-09-2019      lakshminarayana rao     Created
 ********************************************************************************************/
using System.Collections.Generic;
using System.Drawing;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Represents a delivery alert style
    /// </summary>
    public class DeliveryAlertStyle : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Color color;
        private readonly int interval;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="interval"></param>
        public DeliveryAlertStyle(Color color, int interval)
        {
            log.LogMethodEntry(color,interval);
            this.color = color;
            this.interval = interval;
            log.LogMethodExit();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return color;
            yield return interval;
        }

        /// <summary>
        /// Get method of color field
        /// </summary>
        public Color Color
        {
            get { return color; }
        }

        /// <summary>
        /// Get method of color interval
        /// </summary>
        public int Interval
        {
            get { return interval; }
        }
    }
}
