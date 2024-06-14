/********************************************************************************************
 * Project Name - Pass Tech Locker Lock DTO
 * Description  - Data object of Pass tech locker lock DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Apr-2017   Raghuveera    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Data object of Passtech Locker Lock
    /// </summary>
    public class PassTechLockCardHandlerDTO
    {
        /// <summary>
        ///  User Card Serial No
        /// </summary>
        public byte[] UsercardSerialNo = new byte[7];
        /// <summary>
        /// No of times user card issued
        /// </summary>
        public short IssueCount = -1;
        /// <summary>
        /// Locker Multi User Count In case of LockerMaxCount: 0~5 
        /// </summary>
        public short nTCount = -1;
        /// <summary>
        /// User Card Type    0x08 : Free Selection, 0x09:Assigned Mode
        /// </summary>
        public byte bCardType = new byte();
        /// <summary>
        ///  Locker IDList(must 35bytes)   
        /// </summary>
        public byte[] LockerIdList = new byte[35];
        /// <summary>
        /// Valid Term(Start Date/Time). In case of Date/ime : “YYYYMMDDhhnnss” (must 14bytes) 
        /// </summary>
        public byte[] ValidFrom = new byte[14];
        /// <summary>
        /// Valid Term(End Date/Time). In case of Date/ime : “YYYYMMDDhhnnss” (must 14bytes) 
        /// </summary>
        public byte[] ValidTo = new byte[14];
        /// <summary>
        ///  Last access Lock ID(5Bytes / without Sub ID) 
        /// </summary>
        public byte[] LastAccessLockerID = new byte[5];
        /// <summary>
        ///  Last access Lock Battery Status(1Byte) 0x30 : Normal   0x43 : Low 
        /// </summary>
        public byte BatteryStatus = new byte();
        /// <summary>
        /// Parameter for door Open/Close after Limt Time duration (1Byte) 0x30 : Close 0x31 : Open
        /// </summary>
        public byte DoorOpenCloseAfterTime = new byte();
        /// <summary>
        /// LockerTime Limit duration(must 1Byte) 0x00: No Time Limit 1 ~ 99 : Time Limit duration (hours) 
        /// </summary>
        public byte LockerTimeLimit = new byte();
    }
}
