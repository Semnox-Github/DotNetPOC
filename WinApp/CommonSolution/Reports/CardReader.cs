/********************************************************************************************
 * Project Name - CardReader Programs 
 * Description  - Data object of the CardReader class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *1.00        01-October-2017   Rakshith           Updated 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
 

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// CardReader class
    /// </summary>
    public static class CardReader
    {
        // used to trigger code in calling program to receive serial port data
        // set calling program's function to setReceiveAction
        // set RequiredByOthers = true in order to trigger calling program's proc when serial port receives any data

        /// <summary>
        /// CardSwiped property
        /// </summary>
        public static bool CardSwiped = false;

        /// <summary>
        /// CardSwiped CardNumber
        /// </summary>
        public static string CardNumber = "";


        /// <summary>
        /// CardSwiped RequiredByOthers
        /// </summary>
        public static bool RequiredByOthers = false;


        /// <summary>
        /// InvokeHandle delegate
        /// </summary>
        public delegate void InvokeHandle();

        static InvokeHandle receiveAction;


        /// <summary>
        /// OthersTasks method
        /// </summary>
        public static void OthersTasks()
        {
            if (receiveAction != null)
                receiveAction.Invoke();
        }

        /// <summary>
        /// setReceiveAction 
        /// </summary>
        public static InvokeHandle setReceiveAction
        {
            get
            {
                return receiveAction;
            }
            set
            {
                receiveAction = value;
            }
        }
    }
}
