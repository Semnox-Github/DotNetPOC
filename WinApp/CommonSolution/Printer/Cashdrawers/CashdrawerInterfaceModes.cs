/********************************************************************************************
 * Project Name - Device                                                                        
 * Description  -CashdrawerInterfaceModes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    
        /// <summary>
        /// Interface Modes
        /// </summary>
        public enum CashdrawerInterfaceModes
        {
            /// <summary>
            /// None
            /// </summary>
            NONE,
            /// <summary>
            /// Single
            /// </summary>
            SINGLE,
            /// <summary>
            /// Multiple
            /// </summary>
            MULTIPLE,

        }

    /// <summary>
    /// Interface Assignment Modes
    /// </summary>
    public enum CashdrawerAssignmentModes
    {
        /// <summary>
        /// Automatic
        /// </summary>
        AUTOMATIC,
        /// <summary>
        /// Manual
        /// </summary>
        MANUAL,
        /// <summary>
        /// NONE
        /// </summary>
        NONE,

    }
    /// <summary>
    /// Converts InterfaceModes from/to string
    /// </summary>
    public class InterfaceModesConverter
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            /// <summary>
            /// Converts InterfaceModes from string
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static CashdrawerInterfaceModes FromString(string value)
            {
                log.LogMethodEntry("value");
                switch (value.ToUpper())
                {
                    case "NONE":
                        {
                            return CashdrawerInterfaceModes.NONE;
                        }
                    case "SINGLE":
                        {
                            return CashdrawerInterfaceModes.SINGLE;
                        }
                    case "MULTIPLE":
                        {
                            return CashdrawerInterfaceModes.MULTIPLE;
                        }
                    default:
                        {
                            log.Error("Error :Not a valid Interface Modes ");
                            log.LogMethodExit(null, "Throwing Exception");
                            throw new ArgumentException("Not a valid Interface Modes");
                        }
                }
            }


            /// <summary>
            /// Converts InterfaceModes to string
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string ToString(CashdrawerInterfaceModes value)
            {
                log.LogMethodEntry("value");
                switch (value)
                {
                    case CashdrawerInterfaceModes.NONE:
                        {
                            return "NONE";
                        }
                    case CashdrawerInterfaceModes.SINGLE:
                        {
                            return "SINGLE";
                        }
                    case CashdrawerInterfaceModes.MULTIPLE:
                        {
                            return "MULTIPLE";
                        }
                    default:
                        {
                            log.Error("Error :Not a valid Interface Modes ");
                            log.LogMethodExit(null, "Throwing Exception");
                            throw new ArgumentException("Not a valid Interface Modes");
                        }
                }
            }
        }

    }
