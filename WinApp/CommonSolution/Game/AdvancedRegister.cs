/********************************************************************************************
 * Project Name - AdvancedRegister
 * Description  - Read all configure register details in Hub Configure for the SP1ML and set register the Configuration for Hub
 * 
 **************
 **Version Log
 **************
 *Version     Date                    Modified By                       Remarks          
 *********************************************************************************************
 *2.60        22-Feb-2019           Indrajeet Kumar                     Created 
 *2.60        25-Mar-2019           Mushahid Faizan                     Modified- Author Version
 *********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    public enum Registers
    {
        BAUD_RATE,
        FREQUENCY,
        DATA_RATE,
        MODULATION,
        OUTPUT_POWER,
        FREQ_DEVIATION,
        RX_FILTER,
        CS_MODE,
        RSSI_THRESHOLD,
        PREAMBLE_LEN,
        SYNC_LENGTH,
        SYNC_VALUE,
        CRC_MODE,
        WHITENING,
        FEC,
        SOURCE_ADDR,
        DESTINATION_ADDR,
        MULTICAST_ADDR,
        BROADCAST_ADDR,
        FILTER_CRC,
        FILTER_SOURCE,
        FILTER_DESTINATION,
        FILTER_MULTICAST,
        FILTER_BROADCAST,
        TXRX_LED,
        ESCAPE_SEQ,
        SOURCE_FILT_MASK,
        PAYLOAD_SIZE
    };
    public static class AdvancedRegister
    {
        public static readonly Dictionary<Registers, string> RegisterMap = new Dictionary<Registers, string>()
            {
                {Registers.BAUD_RATE,"S00"},
                {Registers.FREQUENCY,"S01"},
                {Registers.DATA_RATE, "S02"},
                {Registers.MODULATION, "S03"},
                {Registers.OUTPUT_POWER, "S04"},
                {Registers.FREQ_DEVIATION, "S05"},
                {Registers.RX_FILTER, "S06"},
                {Registers.CS_MODE, "S07"},
                {Registers.RSSI_THRESHOLD, "S08"},
                {Registers.PREAMBLE_LEN, "S09"},
                {Registers.SYNC_LENGTH, "S10"},
                {Registers.SYNC_VALUE, "S11"},
                {Registers.CRC_MODE, "S12"},
                {Registers.WHITENING, "S13"},
                {Registers.FEC, "S14"},
                {Registers.SOURCE_ADDR, "S15"},
                {Registers.DESTINATION_ADDR, "S16"},
                {Registers.MULTICAST_ADDR, "S17"},
                {Registers.BROADCAST_ADDR, "S18"},
                {Registers.FILTER_CRC, "S19"},
                {Registers.FILTER_SOURCE, "S20"},
                {Registers.FILTER_DESTINATION, "S21"},
                {Registers.FILTER_MULTICAST, "S22"},
                {Registers.FILTER_BROADCAST, "S23"},
                {Registers.TXRX_LED, "S24"},
                {Registers.ESCAPE_SEQ, "S26"},
                {Registers.SOURCE_FILT_MASK, "S27"},
                {Registers.PAYLOAD_SIZE, "S28"}
            };
        public static readonly Dictionary<Registers, string> SystemDefaultValues = new Dictionary<Registers, string>()
                {
                {Registers.MODULATION, "1"},
                {Registers.FEC, "1"},
                {Registers.FILTER_SOURCE, "1"},
                {Registers.FILTER_DESTINATION, "1"},
                {Registers.TXRX_LED, "1"}
                };
        public static readonly Dictionary<Registers, string> UserSettableValues = new Dictionary<Registers, string>()
                {
                {Registers.FREQUENCY,"868000000"},
                {Registers.DATA_RATE, "10000"},
                {Registers.SOURCE_ADDR, "0x01"},
                {Registers.DESTINATION_ADDR, "0x01"}
                };
    }
    public static class Commands
    {
        public static string SemnoxCommandModeEnter = "Commode_Enter";
        public static string SemnoxCommandModeExit = "Commode_Exit";
        public static string SemnoxConfigRead = "ConfigR_AT";
        public static string SemnoxConfigWrite = "ConfigW_AT";
        public static string CommandMode = "+++";
        public static string ATCommand = "AT";
        public static string OperatingMode = "O";
        public static string Version = "/V";
        public static string ReadInfoRegister = "I";
        public static string ReadConfigRegister = "?";
        public static string WriteConfigRegister = "=";
        public static string ReadAllConfigRegisters = "/S";
        public static string StoreConfig = "/C";
        public static string ResetConfig = "R";
        public static string Restart = "Z";
    }
}
