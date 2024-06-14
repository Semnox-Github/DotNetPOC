/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - ColumnProvider 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Column provider which returns the column based on the enum
    /// </summary>
    public abstract class ColumnProvider
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// column container dictionary
        /// </summary>
        protected Dictionary<Enum, Column> columnDictionary;

        /// <summary>
        /// return list of all columns
        /// </summary>
        /// <returns>columnDictionary</returns>
        public Dictionary<Enum, Column> GetAllColumns()
        {
            log.LogMethodEntry();
            log.LogMethodExit(columnDictionary);
            return columnDictionary;
        }

        /// <summary>
        /// returns the column based on the columnidentifier enum
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <returns></returns>
        public Column GetColumn(Enum columnIdentifier)
        {
            log.LogMethodEntry(columnIdentifier);
            Column column = null;
            if (columnDictionary.ContainsKey(columnIdentifier))
            {
                column = columnDictionary[columnIdentifier];
            }
            else
            {
                throw new Exception("Invalid column");
            }
            log.LogMethodExit(column);
            return column;
        }
    }
}
