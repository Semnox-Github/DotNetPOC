using System;
using System.Collections.Generic;
using System.Text;

namespace Semnox.Parafait.Device.Printer
{
    public class FamDefs
    {
        public const byte COMMAND_GET_VERSION = 0x00;
        public const byte COMMAND_SOFTWARE_REBOOT = 0xFF;
        public const byte COMMAND_CHECK_FINGER = 0x4B;
        public const byte COMMAND_CAPTURE_IMAGE = 0x49;
        public const byte COMMAND_PROCESS_IMAGE = 0x50;
        public const byte COMMAND_MATCH_FINGER = 0x52;
        public const byte FLAG_1_1_MATCH = 0;
        public const byte FLAG_1_N_MATCH = 1;
        public const byte COMMAND_STORE_TEMPLATE = 0x41;
        public const byte COMMAND_STORE_SAMPLE = 0x53;
        public const byte COMMAND_DOWNLOAD_SAMPLE = 0x4D;
        public const byte COMMAND_DOWNLOAD_RAW_IMAGE = 0x44;
        //command for upgrade firmware
        public const byte COMMAND_DOWNLOAD_FROM_FLASH = 0x42;
        public const byte COMMAND_UPLOAD_TO_RAM = 0x0D;
        public const byte COMMAND_DOWNLOAD_FROM_RAM = 0x0F;
        public const byte COMMAND_WRITE_TO_FLASH = 0x10;
        public const byte COMMAND_DOWNLOAD_USER_LIST = 0x57;
        public const byte COMMAND_DELETE_ALL_USER = 0x45;
        public const byte COMMAND_DELETE_1_USER = 0x48;
        //Network setting
        public const byte COMMAND_NETWORK_SETTING = 0x61;
        public const byte FLAG_GET_IP_GW = 0x03;
        public const byte FLAG_SET_IP_GW = 0x04;
        public const byte FLAG_GET_MAC_PORT = 0x05;
        public const byte FLAG_SET_MAC_PORT = 0x06;
        public const byte FLAG_GET_SM = 0x09;	//subnet mask
        public const byte FLAG_SET_SM = 0x0A;
        public const byte FLAG_SAVE_SETTING = 0x99;
        //
        public const byte COMMAND_CHANGE_USER_TYPE = 0x47;
        public const byte COMMAND_SECURITY_LEVEL = 0x4A;
        public const byte COMMAND_GET_SPACE = 0x4F;
        public const byte COMMAND_DOWNLOAD_TEMPLATE = 0x54;
        public const byte COMMAND_UPLOAD_TEMPLATE = 0x55;
        //
        public const byte COMMAND_PERIPHERIAL_CONTROL = 0x11;
        //
        //Error code
        public const byte RET_OK = 0x40;
        public const byte RET_NO_IMAGE = 0x41;
        public const byte RET_BAD_QUALITY = 0x42;
        public const byte RET_TOO_LITTLE_POINTS = 0x43;
        public const byte RET_EMPTY_BASE = 0x44;
        public const byte RET_UNKNOWN_USER = 0x45;
        public const byte RET_NO_SPACE = 0x46;
        public const byte RET_BAD_ARGUMENT = 0x47;
        public const byte RET_CRC_ERROR = 0x49;
        public const byte RET_RXD_TIMEOUT = 0x4A;
        public const byte RET_USER_ID_IS_ABSENT = 0x4D;
        public const byte RET_USER_ID_IS_USED = 0x4E;
        public const byte RET_VERY_SIMILAR_SAMPLE = 0x4F;
        public const byte RET_USER_SUSPENDED = 0x54;
        public const byte RET_UNKNOWN_COMMAND = 0x55;
        public const byte RET_INVALID_STOP_BYTE = 0x57;
        public const byte RET_HARDWARE_ERROR = 0x58;
        public const byte RET_BAD_TEST_OBJECT = 0x59;
        public const byte RET_BAD_FLASH = 0x5A;
        public const byte RET_TOO_MANY_VIP = 0x5B;
        //
        public const byte RET_WINSOCK_ERROR = 0x30;
        public const byte RET_CONNECT_TIMEOUT = 0x31;
        public const byte RET_FLAG_ZERO = 0x32;
        //
        public const byte FAM_BAUDRATE_115200 = 5;
        public const byte FAM_BAUDRATE_230400 = 6;
        public const byte FAM_BAUDRATE_460800 = 7;
        public const byte FAM_BAUDRATE_921600 = 8;
    }
}
