/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - SCTOMO-Structure.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;
using System.Runtime.InteropServices;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    /// <summary>
    /// Class to hold all structures for Sankyo Card Dispenser
    /// </summary>
    public class SankyoCommandStructure
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public STRUCTCOMMAND commandStruct;

        public byte commandTag;
        public byte commandCode;
        public byte parameterCode;
        public UInt32 dataSize;
        public IntPtr dataBody;

        public enum REPLYTYPE
        {
            PositiveReply,
            NegativeReply,
            ReplyReceivingFailure,
            CommandCancellation,
            ReplyTimeout,
        }
         
        /// <summary>
        /// Structure for Command to be passed to Sankyo Card Dispenser
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCTCOMMAND
        {
            public byte commandTag;
            public byte commandCode;
            public byte parameterCode;
            public UInt32 dataSize;
            public IntPtr dataBody;
       }

        
        /// <summary>
        /// Structure for Positive Response received from Sankyo Card Dispenser
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct STRUCTPOSITIVEREPLY
        {
            public byte replyTag;
            public byte commandCode;
            public byte parameterCode;
            public byte bStatusCode0;
            public byte bStatusCode1;
            public byte bStatusCode2;
            public UInt32 responseDataSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] responseData;
         }

        /// <summary>
        /// Structure for Negative Response received from Sankyo Card Dispenser
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct STRUCTNEGATIVEREPLY
        {
            public byte replyTag;
            public byte commandCode;
            public byte parameterCode;
            public byte bErrorCode0;
            public byte bErrorCode1;
            public UInt32 responseDataSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] responseData;
        }

        /// <summary>
        /// Structure for reply received from Sankyo Card Dispenser
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct STRUCTREPLY
        {
            // [FieldOffset(0)]
            public REPLYTYPE replyType;
            public STRUCTPOSITIVEREPLY positiveReply;
            public STRUCTNEGATIVEREPLY negativeReply;
        }

        /// <summary>
        /// Assignment of properties to set the Command Struct
        /// </summary>
        public void SetCommandStructure()
        {
            log.LogMethodEntry();
            commandStruct.commandTag = commandTag;
            commandStruct.commandCode = commandCode;
            commandStruct.parameterCode = parameterCode;
            commandStruct.dataSize = dataSize;
            commandStruct.dataBody = dataBody;
            log.LogMethodExit();
        }
    }
}
