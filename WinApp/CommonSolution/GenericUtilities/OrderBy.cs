/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - OrderBy 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Order by condition
    /// </summary>
    public class OrderBy
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ColumnProvider columnProvider;
        private Enum columnIdentifier;
        private OrderByType orderByType;
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="columnIdentifier"></param>
        /// <param name="orderByType"></param>
        public OrderBy(ColumnProvider columnProvider, Enum columnIdentifier, OrderByType orderByType)
        {
            log.LogMethodEntry(columnProvider, columnIdentifier, orderByType);
            this.columnProvider = columnProvider;
            this.columnIdentifier = columnIdentifier;
            this.orderByType = orderByType;
            log.LogMethodExit();
        }

        /// <summary>
        /// string represention of the order by clause
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            string orderByString = " ";
            orderByString += columnProvider.GetColumn(columnIdentifier).Name;
            if (orderByType == OrderByType.ASC)
            {
                orderByString += " ASC";
            }
            else
            {
                orderByString += " DESC";
            }
            log.LogMethodExit(orderByString);
            return orderByString;
        }

        /// <summary>
        /// returns the column identifier
        /// </summary>
        public Enum ColumnIdentifier
        {
            get
            {
                return columnIdentifier;
            }
        }
    }
}
