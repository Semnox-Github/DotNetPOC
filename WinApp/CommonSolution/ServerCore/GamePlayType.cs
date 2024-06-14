/********************************************************************************************
 * Project Name - ServerCore
 * Description  - Class for  of GamePlayType      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2      03-Mar-2022   Abhishek       Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.ServerCore
{
    /// <summary>
    /// Game Play Type
    /// </summary>
    public enum GamePlayType
    {
        /// <summary>
        /// REGULAR
        /// </summary>
        REGULAR,
        /// <summary>
        /// MIFARE
        /// </summary>
        MIFARE,
        /// <summary>
        /// COIN
        /// </summary>
        COIN,
        /// <summary>
        /// FREE
        /// </summary>
        FREE,
    }

    /// <summary>
    /// Converts CreditPlusType from/to string
    /// </summary>
    public class GamePlayTypeConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly ConcurrentDictionary<string, string> attributeDictionary = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Converts GamePlayType from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static GamePlayType FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "R":
                    {
                        return GamePlayType.REGULAR;
                    }
                case "M":
                    {
                        return GamePlayType.MIFARE;
                    }
                case "C":
                    {
                        return GamePlayType.COIN;
                    }
                case "F":
                    {
                        return GamePlayType.FREE;
                    }
                default:
                    {
                        log.Error("Error :Not a valid game play type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid game play type");
                    }
            }
        }


        /// <summary>
        /// Converts GamePlayType to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(GamePlayType value)
        {
            log.LogMethodEntry("value" + value);
            switch (value)
            {
                case GamePlayType.REGULAR:
                    {
                        return "R";
                    }
                case GamePlayType.MIFARE:
                    {
                        return "M";
                    }
                case GamePlayType.COIN:
                    {
                        return "C";
                    }
                case GamePlayType.FREE:
                    {
                        return "F";
                    }
                default:
                    {
                        log.Error("Error :Not a valid game play type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid game play type");
                    }
            }
        }

        /// <summary>
        /// Converts GamePlayType to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(string value)
        {
            log.LogMethodEntry("value" + value);
            switch (value)
            {
                case "R":
                    {
                        return "REGULAR";
                    }
                case "M":
                    {
                        return "MIFARE";
                    }
                case "C":
                    {
                        return "COIN";
                    }
                case "F":
                    {
                        return "FREE";
                    }
                default:
                    {
                        log.Error("Error :Not a valid game play type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid game play type");

                    }
            }
        }
    }
}
