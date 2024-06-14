/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - value converter class for Ticketmode
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created  
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    class TicketModeValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected Dictionary<string, string> tickedModeIDList;
        protected Dictionary<string, string> tickedModeNameList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public TicketModeValueConverter()
        {
            log.LogMethodEntry();
            tickedModeIDList = new Dictionary<string, string>();
            tickedModeNameList = new Dictionary<string, string>();
            Dictionary<string, string> values = new Dictionary<string, string>
                        {
                            { "D", "Default" },
                            { "T", "2 Line" },
                            { "E", "E-Ticket" }
                        };
            foreach (var item in values)
            {
                tickedModeIDList.Add(item.Key,item.Value);
                tickedModeNameList.Add(item.Key, item.Value);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts Ticketmode name to id
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            string ticketmode = string.Empty;
            if (tickedModeIDList != null && !string.IsNullOrWhiteSpace(stringValue))
            {
                ticketmode = tickedModeIDList.FirstOrDefault(x => x.Value == stringValue).Key.ToString();
            }
            else
            {
                ticketmode = tickedModeIDList.FirstOrDefault().Key.ToString();
            }
            log.LogMethodExit(ticketmode);
            return ticketmode;
        }
        /// <summary>
        /// Converts Ticketmode id to name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string ticketname = string.Empty;
            if (tickedModeNameList != null && tickedModeNameList.ContainsKey(Convert.ToString(value)))
            {
                ticketname = tickedModeNameList.FirstOrDefault(m =>m.Key == value.ToString()).Value;
            }
            log.LogMethodExit(ticketname);
            return ticketname;
        }
    }
}
