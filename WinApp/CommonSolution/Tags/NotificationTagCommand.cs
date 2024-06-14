using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// Enum for commands to communicate with Radian devices
    /// </summary>
    public enum RadianDeviceCommand
    {
        NONE = 0,
        HANDSHAKE = 1,
        SYNC = 2,
        AUTHENTICATE_WB = 3,
        PING_GW = 4,
        GET_WB_INFO = 5,
        SOFT_WB_REMOVE = 6,
        GET_ALL_WB_INFO = 7,
        PING_WB_WILDCARD = 8,
        PING_WB = 9,
        DEPLOY_WB = 10,
        SET_PATTERN_WB = 11,
        STOP_PATTERN_WB = 12,
        RETURN_WB = 13,
        STORAGE_WB = 14,
        SCAN_WB = 15,
        GET_RF_CHANNEL = 16,
        SET_RF_CHANNEL = 17,
        STOP_AUTOMATIC_PING = 18,
        RESTORE_AUTOMATIC_PING = 19,
        //UPDATE_ALL_WB_INFO = 20,
        JUMP_TO_BOOT_STAGE = 21,
        SET_FILTER_ADDRESS = 0x1A,
        GET_FILTER_ADDRESS = 0x1B,
        HARD_WB_REMOVE = 0x1F,
        TEST_WB = 100,
        TEST_CUSTOM_PATTERNS = 101
    }

    public enum RadianDeviceSyncStatus
    {
        SUCCESS = 0,
        FAIL = 1
    }

    public enum RadianDeviceConnectionMode
    {
        RefreshFromGWCache,
        ReAuthenticate,
        AuthenticateSameChannelGW,
        AuthenticateDiffChannelGW,
        Scan,
        MaxAttempts
    }

    /// <summary>
    /// Converts RadianDeviceCommand from/to string
    /// </summary>
    public class RadianDeviceCommandConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts RadianDeviceCommand from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RadianDeviceCommand FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "NONE":
                    {
                        return RadianDeviceCommand.NONE;
                    }
                case "AUTHENTICATE_WB":
                    {
                        return RadianDeviceCommand.AUTHENTICATE_WB;
                    }
                case "DEPLOY_WB":
                    {
                        return RadianDeviceCommand.DEPLOY_WB;
                    }
                case "GET_ALL_WB_INFO":
                    {
                        return RadianDeviceCommand.GET_ALL_WB_INFO;
                    }
                case "GET_RF_CHANNEL":
                    {
                        return RadianDeviceCommand.GET_RF_CHANNEL;
                    }
                case "GET_WB_INFO":
                    {
                        return RadianDeviceCommand.GET_WB_INFO;
                    }
                case "HANDSHAKE":
                    {
                        return RadianDeviceCommand.HANDSHAKE;
                    }
                case "JUMP_TO_BOOT_STAGE":
                    {
                        return RadianDeviceCommand.JUMP_TO_BOOT_STAGE;
                    }
                case "PING_GW":
                    {
                        return RadianDeviceCommand.PING_GW;
                    }
                case "PING_WB":
                    {
                        return RadianDeviceCommand.PING_WB;
                    }
                case "PING_WB_WILDCARD":
                    {
                        return RadianDeviceCommand.PING_WB_WILDCARD;
                    }
                case "HARD_WB_REMOVE":
                    {
                        return RadianDeviceCommand.HARD_WB_REMOVE;
                    }
                case "SOFT_WB_REMOVE":
                    {
                        return RadianDeviceCommand.SOFT_WB_REMOVE;
                    }
                case "RESTORE_AUTOMATIC_PING":
                    {
                        return RadianDeviceCommand.RESTORE_AUTOMATIC_PING;
                    }
                case "RETURN_WB":
                    {
                        return RadianDeviceCommand.RETURN_WB;
                    }
                case "SCAN_WB":
                    {
                        return RadianDeviceCommand.SCAN_WB;
                    }
                case "SET_PATTERN_WB":
                    {
                        return RadianDeviceCommand.SET_PATTERN_WB;
                    }
                case "SET_RF_CHANNEL":
                    {
                        return RadianDeviceCommand.SET_RF_CHANNEL;
                    }
                case "SET_FILTER_ADDRESS":
                    {
                        return RadianDeviceCommand.SET_FILTER_ADDRESS;
                    }
                case "GET_FILTER_ADDRESS":
                    {
                        return RadianDeviceCommand.GET_FILTER_ADDRESS;
                    }
                case "STOP_AUTOMATIC_PING":
                    {
                        return RadianDeviceCommand.STOP_AUTOMATIC_PING;
                    }
                case "STOP_PATTERN_WB":
                    {
                        return RadianDeviceCommand.STOP_PATTERN_WB;
                    }
                case "STORAGE_WB":
                    {
                        return RadianDeviceCommand.STORAGE_WB;
                    }
                case "SYNC":
                    {
                        return RadianDeviceCommand.SYNC;
                    }
                case "TEST_WB":
                    {
                        return RadianDeviceCommand.TEST_WB;
                    }
                case "TEST_CUSTOM_PATTERNS":
                    {
                        return RadianDeviceCommand.TEST_CUSTOM_PATTERNS;
                    }
                default:
                    {
                        log.Error("Error :Not a valid Radian Device Command type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Radian Device Command type");
                    }
            }
        }
        /// <summary>
        /// Converts RadianDeviceCommand to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(RadianDeviceCommand value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case RadianDeviceCommand.NONE:
                    {
                        return "NONE";
                    }
                case RadianDeviceCommand.AUTHENTICATE_WB:
                    {
                        return "AUTHENTICATE_WB";
                    }
                case RadianDeviceCommand.DEPLOY_WB:
                    {
                        return "DEPLOY_WB";
                    }
                case RadianDeviceCommand.GET_ALL_WB_INFO:
                    {
                        return "GET_ALL_WB_INFO";
                    }
                case RadianDeviceCommand.GET_RF_CHANNEL:
                    {
                        return "GET_RF_CHANNEL";
                    }
                case RadianDeviceCommand.GET_WB_INFO:
                    {
                        return "GET_WB_INFO";
                    }
                case RadianDeviceCommand.HANDSHAKE:
                    {
                        return "HANDSHAKE";
                    }
                case RadianDeviceCommand.JUMP_TO_BOOT_STAGE:
                    {
                        return "JUMP_TO_BOOT_STAGE";
                    }
                case RadianDeviceCommand.PING_GW:
                    {
                        return "PING_GW";
                    }
                case RadianDeviceCommand.PING_WB:
                    {
                        return "PING_WB";
                    }
                case RadianDeviceCommand.PING_WB_WILDCARD:
                    {
                        return "PING_WB_WILDCARD";
                    }
                case RadianDeviceCommand.HARD_WB_REMOVE:
                    {
                        return "HARD_WB_REMOVE";
                    }
                case RadianDeviceCommand.SOFT_WB_REMOVE:
                    {
                        return "SOFT_WB_REMOVE";
                    }
                case RadianDeviceCommand.RESTORE_AUTOMATIC_PING:
                    {
                        return "RESTORE_AUTOMATIC_PING";
                    }
                case RadianDeviceCommand.RETURN_WB:
                    {
                        return "RETURN_WB";
                    }
                case RadianDeviceCommand.SCAN_WB:
                    {
                        return "SCAN_WB";
                    }
                case RadianDeviceCommand.SET_PATTERN_WB:
                    {
                        return "SET_PATTERN_WB";
                    }
                case RadianDeviceCommand.SET_RF_CHANNEL:
                    {
                        return "SET_RF_CHANNEL";
                    }
                case RadianDeviceCommand.SET_FILTER_ADDRESS:
                    {
                        return "SET_FILTER_ADDRESS";
                    }
                case RadianDeviceCommand.GET_FILTER_ADDRESS:
                    {
                        return "GET_FILTER_ADDRESS";
                    }
                case RadianDeviceCommand.STOP_AUTOMATIC_PING:
                    {
                        return "STOP_AUTOMATIC_PING";
                    }
                case RadianDeviceCommand.STOP_PATTERN_WB:
                    {
                        return "STOP_PATTERN_WB";
                    }
                case RadianDeviceCommand.STORAGE_WB:
                    {
                        return "STORAGE_WB";
                    }
                case RadianDeviceCommand.SYNC:
                    {
                        return "SYNC";
                    }
                case RadianDeviceCommand.TEST_WB:
                    {
                        return "TEST_WB";
                    }
                case RadianDeviceCommand.TEST_CUSTOM_PATTERNS:
                    {
                        return "TEST_CUSTOM_PATTERNS";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Command type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Command");
                    }
            }
        }
    }

    /// <summary>
    /// Enum for Radian Device Response values
    /// </summary>
    public enum RadianDeviceResponse
    {
        NONE = 0,
        HANDSHAKE = 1,
        ACK = 2,
        NACK = 3,
        SYNC = 4,
        //SYNC_MISMATCH = 5,
        AUTHENTICATION_RESULTS = 6,
        NOT_ENOUGH_SPACE = 7,
        PING = 8,
        WB_INFO = 9,
        WB_NOT_FOUND = 10,
        REMOVE_WB_RESULTS = 11,
        WB_RESPONSE_TIMEOUT = 12,
        WB_DEPLOY_OK = 13,
        WB_DEPLOY_NOK = 14,
        SET_PATTERN_OK = 15,
        STOP_PATTERN_OK = 16,
        WB_RETURN_OK = 17,
        WB_STORAGE_OK = 18,
        WB_STORAGE_NOK = 19,
        RF_CHANNEL = 20,
        SET_RF_CHANNEL_OK = 21,
        SET_RF_CHANNEL_NOK = 22,
        SCAN_WB_RESULTS = 23,
        STOP_AUTOMATIC_PING_OK = 24,
        RESTORE_AUTOMATIC_PING_OK = 25,
        UPDATE_ALL_WB_INFO_OK = 26,
        JUMP_TO_BOOT_OK = 0x1B,
        FILTER_ADDRESS = 0x1D,
        RF_TRANSACTION_FAIL = 0x21,
        INVALID_COMMAND_SEQUENCE = 99,
        ERROR = 100
    }

    /// <summary>
    /// Converts RadianDeviceResponse from/to string
    /// </summary>
    public class RadianDeviceResponseConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts RadianDeviceResponse from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RadianDeviceResponse FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "NONE":
                    {
                        return RadianDeviceResponse.NONE;
                    }
                case "HANDSHAKE":
                    {
                        return RadianDeviceResponse.HANDSHAKE;
                    }
                case "ACK":
                    {
                        return RadianDeviceResponse.ACK;
                    }
                case "NACK":
                    {
                        return RadianDeviceResponse.NACK;
                    }
                //case "SYNC_OK":
                //    {
                //        return RadianDeviceResponse.SYNC_OK;
                //    }
                //case "SYNC_MISMATCH":
                //    {
                //        return RadianDeviceResponse.SYNC_MISMATCH;
                //    }
                case "NOT_ENOUGH_SPACE":
                    {
                        return RadianDeviceResponse.NOT_ENOUGH_SPACE;
                    }
                case "AUTHENTICATION_RESULTS":
                    {
                        return RadianDeviceResponse.AUTHENTICATION_RESULTS;
                    }
                case "PING":
                    {
                        return RadianDeviceResponse.PING;
                    }
                case "WB_INFO":
                    {
                        return RadianDeviceResponse.WB_INFO;
                    }
                case "WB_NOT_FOUND":
                    {
                        return RadianDeviceResponse.WB_NOT_FOUND;
                    }
                case "REMOVE_WB_RESULTS":
                    {
                        return RadianDeviceResponse.REMOVE_WB_RESULTS;
                    }
                case "WB_RESPONSE_TIMEOUT":
                    {
                        return RadianDeviceResponse.WB_RESPONSE_TIMEOUT;
                    }
                case "WB_DEPLOY_OK":
                    {
                        return RadianDeviceResponse.WB_DEPLOY_OK;
                    }
                case "WB_DEPLOY_NOK":
                    {
                        return RadianDeviceResponse.WB_DEPLOY_NOK;
                    }
                case "SET_PATTERN_OK":
                    {
                        return RadianDeviceResponse.SET_PATTERN_OK;
                    }
                case "STOP_PATTERN_OK":
                    {
                        return RadianDeviceResponse.STOP_PATTERN_OK;
                    }
                case "WB_RETURN_OK":
                    {
                        return RadianDeviceResponse.WB_RETURN_OK;
                    }
                case "WB_STORAGE_OK":
                    {
                        return RadianDeviceResponse.WB_STORAGE_OK;
                    }
                case "WB_STORAGE_NOK":
                    {
                        return RadianDeviceResponse.WB_STORAGE_NOK;
                    }
                case "RF_CHANNEL":
                    {
                        return RadianDeviceResponse.RF_CHANNEL;
                    }
                case "SET_RF_CHANNEL_OK":
                    {
                        return RadianDeviceResponse.SET_RF_CHANNEL_OK;
                    }
                case "SET_RF_CHANNEL_NOK":
                    {
                        return RadianDeviceResponse.SET_RF_CHANNEL_NOK;
                    }
                case "SCAN_WB_RESULTS":
                    {
                        return RadianDeviceResponse.SCAN_WB_RESULTS;
                    }
                case "STOP_AUTOMATIC_PING_OK":
                    {
                        return RadianDeviceResponse.STOP_AUTOMATIC_PING_OK;
                    }
                case "RESTORE_AUTOMATIC_PING_OK":
                    {
                        return RadianDeviceResponse.RESTORE_AUTOMATIC_PING_OK;
                    }
                case "UPDATE_ALL_WB_INFO_OK":
                    {
                        return RadianDeviceResponse.UPDATE_ALL_WB_INFO_OK;
                    }
                case "JUMP_TO_BOOT_OK":
                    {
                        return RadianDeviceResponse.JUMP_TO_BOOT_OK;
                    }
                case "INVALID_COMMAND_SEQUENCE":
                    {
                        return RadianDeviceResponse.INVALID_COMMAND_SEQUENCE;
                    }
                case "ERROR":
                    {
                        return RadianDeviceResponse.ERROR;
                    }
                default:
                    {
                        log.Error("Error :Not a valid Radian Device Response type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Radian Device Response type");
                    }
            }
        }
        /// <summary>
        /// Converts RadianDeviceResponse to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(RadianDeviceResponse value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case RadianDeviceResponse.NONE:
                    {
                        return "NONE";
                    }
                case RadianDeviceResponse.HANDSHAKE:
                    {
                        return "HANDSHAKE";
                    }
                case RadianDeviceResponse.ACK:
                    {
                        return "ACK";
                    }
                case RadianDeviceResponse.NACK:
                    {
                        return "NACK";
                    }
                //case RadianDeviceResponse.SYNC_OK:
                //    {
                //        return "SYNC_OK";
                //    }
                //case RadianDeviceResponse.SYNC_MISMATCH:
                //    {
                //        return "SYNC_MISMATCH";
                //    }
                case RadianDeviceResponse.NOT_ENOUGH_SPACE:
                    {
                        return "NOT_ENOUGH_SPACE";
                    }
                case RadianDeviceResponse.AUTHENTICATION_RESULTS:
                    {
                        return "AUTHENTICATION_RESULTS";
                    }
                case RadianDeviceResponse.PING:
                    {
                        return "PING";
                    }
                case RadianDeviceResponse.WB_INFO:
                    {
                        return "WB_INFO";
                    }
                case RadianDeviceResponse.WB_NOT_FOUND:
                    {
                        return "WB_NOT_FOUND";
                    }
                case RadianDeviceResponse.REMOVE_WB_RESULTS:
                    {
                        return "REMOVE_WB_RESULTS";
                    }
                case RadianDeviceResponse.WB_RESPONSE_TIMEOUT:
                    {
                        return "WB_RESPONSE_TIMEOUT";
                    }
                case RadianDeviceResponse.WB_DEPLOY_OK:
                    {
                        return "WB_DEPLOY_OK";
                    }
                case RadianDeviceResponse.WB_DEPLOY_NOK:
                    {
                        return "WB_DEPLOY_NOK";
                    }
                case RadianDeviceResponse.SET_PATTERN_OK:
                    {
                        return "SET_PATTERN_OK";
                    }
                case RadianDeviceResponse.STOP_PATTERN_OK:
                    {
                        return "STOP_PATTERN_OK";
                    }
                case RadianDeviceResponse.WB_RETURN_OK:
                    {
                        return "WB_RETURN_OK";
                    }
                case RadianDeviceResponse.WB_STORAGE_OK:
                    {
                        return "WB_STORAGE_OK";
                    }
                case RadianDeviceResponse.WB_STORAGE_NOK:
                    {
                        return "WB_STORAGE_NOK";
                    }
                case RadianDeviceResponse.RF_CHANNEL:
                    {
                        return "RF_CHANNEL";
                    }
                case RadianDeviceResponse.SET_RF_CHANNEL_OK:
                    {
                        return "SET_RF_CHANNEL_OK";
                    }
                case RadianDeviceResponse.SET_RF_CHANNEL_NOK:
                    {
                        return "SET_RF_CHANNEL_NOK";
                    }
                case RadianDeviceResponse.SCAN_WB_RESULTS:
                    {
                        return "SCAN_WB_RESULTS";
                    }
                case RadianDeviceResponse.STOP_AUTOMATIC_PING_OK:
                    {
                        return "STOP_AUTOMATIC_PING_OK";
                    }
                case RadianDeviceResponse.RESTORE_AUTOMATIC_PING_OK:
                    {
                        return "RESTORE_AUTOMATIC_PING_OK";
                    }
                case RadianDeviceResponse.UPDATE_ALL_WB_INFO_OK:
                    {
                        return "UPDATE_ALL_WB_INFO_OK";
                    }
                case RadianDeviceResponse.JUMP_TO_BOOT_OK:
                    {
                        return "JUMP_TO_BOOT_OK";
                    }
                case RadianDeviceResponse.INVALID_COMMAND_SEQUENCE:
                    {
                        return "INVALID_COMMAND_SEQUENCE";
                    }
                case RadianDeviceResponse.ERROR:
                    {
                        return "ERROR";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Response type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Response");
                    }
            }
        }
    }
}
