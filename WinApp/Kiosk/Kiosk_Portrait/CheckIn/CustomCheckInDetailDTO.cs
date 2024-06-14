/********************************************************************************************
 * Project Name - CustomCheckInDetailDTO
 * Description  - Data object of CustomCheckInDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By            Remarks          
 *********************************************************************************************
 *2.140.0     18-Oct-2021     Sathyavathi            Created 
 *2.150.0.0   18-Sep-2022     Sathyavathi            CheckIn CheckOut Phase 2 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the CustomCheckInDetailDTO data object class. This acts as data holder for the CustomCheckInDetails business object
    /// </summary>
    public class CustomCheckInDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        private CheckInDetailDTO checkInDetailDTO = new CheckInDetailDTO();
        private int customerId;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomCheckInDetailDTO()
        {
            log.LogMethodEntry();
            this.CheckInDetailId = -1;
            this.CheckInId = -1;
            this.Name = string.Empty;
            this.CardId = -1;
            this.VehicleNumber = string.Empty;
            this.VehicleModel = string.Empty;
            this.VehicleColor = string.Empty;
            this.DateOfBirth = null;
            this.Age = -1;
            this.SpecialNeeds = string.Empty;
            this.Allergies = string.Empty;
            this.Remarks = string.Empty;
            this.CheckOutTime = string.Empty;
            this.CheckOutTrxId = -1;
            this.TrxLineId = checkInDetailDTO.TrxLineId;
            this.CheckInTrxId = -1;
            this.CheckInTrxLineId = -1;
            this.CheckInTime = null;
            this.Status = CheckInStatus.PENDING;
            this.CustomerId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CheckInDetailId field
        /// </summary>
        public int CheckInDetailId
        {
            get { return checkInDetailDTO.CheckInDetailId; }
            set { checkInDetailDTO.CheckInDetailId = value; }
        }
        /// <summary>
        /// Get/Set method of the CheckInId field
        /// </summary>
        public int CheckInId
        {
            get { return checkInDetailDTO.CheckInId; }
            set { checkInDetailDTO.CheckInId = value; }
        }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get { return checkInDetailDTO.Name; }
            set { checkInDetailDTO.Name = value; }
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId
        {
            get { return checkInDetailDTO.CardId; }
            set { checkInDetailDTO.CardId = value; }
        }
        /// <summary>
        /// Get/Set method of the VehicleNumber field
        /// </summary>
        [DisplayName("Vehicle Number")]
        public string VehicleNumber
        {
            get { return checkInDetailDTO.VehicleNumber; }
            set { checkInDetailDTO.VehicleNumber = value; }
        }
        /// <summary>
        /// Get/Set method of the VehicleModel field
        /// </summary>
        [DisplayName("Vehicle Model")]
        public string VehicleModel
        {
            get { return checkInDetailDTO.VehicleModel; }
            set { checkInDetailDTO.VehicleModel = value; }
        }
        /// <summary>
        /// Get/Set method of the VehicleColor field
        /// </summary>
        [DisplayName("Vehicle Color")]
        public string VehicleColor
        {
            get { return checkInDetailDTO.VehicleColor; }
            set { checkInDetailDTO.VehicleColor = value; }
        }
        /// <summary>
        /// Get/Set method of the DateOfBirth field
        /// </summary>
        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth
        {
            get
            {
                return checkInDetailDTO.DateOfBirth;
            }
            set
            {
                checkInDetailDTO.DateOfBirth = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Age field
        /// </summary>
        public decimal Age
        {
            get
            {
                if (checkInDetailDTO.Age < 0 && checkInDetailDTO.DateOfBirth.HasValue)
                {
                    string dateOfBirth = checkInDetailDTO.DateOfBirth.ToString();
                    decimal age = KioskHelper.GetAge(dateOfBirth);
                    checkInDetailDTO.Age = age;
                }

                return checkInDetailDTO.Age;
            }
            set { checkInDetailDTO.Age = value; }
        }
        /// <summary>
        /// Get/Set method of the SpecialNeeds field
        /// </summary>
        [DisplayName("Special Needs")]
        public string SpecialNeeds
        {
            get { return checkInDetailDTO.SpecialNeeds; }
            set { checkInDetailDTO.SpecialNeeds = value; }
        }
        /// <summary>
        /// Get/Set method of the Allergies field
        /// </summary>
        [DisplayName("Allergies")]
        public string Allergies
        {
            get { return checkInDetailDTO.Allergies; }
            set { checkInDetailDTO.Allergies = value; }
        }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get { return checkInDetailDTO.Remarks; }
            set { checkInDetailDTO.Remarks = value; }
        }

        /// <summary>
        /// Get/Set method of the CheckOutTime field
        /// </summary>
        public string CheckOutTime
        {
            get { return checkInDetailDTO.CheckOutTime.ToString(); }
            //set { checkInDetailDTO.CheckOutTime = value; }
            set
            {
                if (value != null)
                {
                    try
                    {
                        checkInDetailDTO.CheckOutTime = Convert.ToDateTime(value);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit();
                    }
                }
            }
        }
        /// <summary>
        /// Get/Set method of the CheckOutTrxId field
        /// </summary>
        public int CheckOutTrxId
        {
            get { return checkInDetailDTO.CheckOutTrxId; }
            set { checkInDetailDTO.CheckOutTrxId = value; }
        }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }
        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        public int? TrxLineId
        {
            get { return checkInDetailDTO.TrxLineId; }
            set { checkInDetailDTO.TrxLineId = value; }
        }

        /// <summary>
        /// Get method of the Detail field
        /// </summary>
        public string Detail
        {
            get { return checkInDetailDTO.Detail; }
            set { checkInDetailDTO.Detail = value; }
        }

        /// <summary>
        /// Get method of the AccountDTO field
        /// </summary>
        public string AccountNumber
        {
            get { return checkInDetailDTO.AccountNumber; }
            set { checkInDetailDTO.AccountNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the CheckInTrxId field
        /// </summary>
        public int CheckInTrxId
        {
            get { return checkInDetailDTO.CheckInTrxId; }
            set { checkInDetailDTO.CheckInTrxId = value; }
        }
        /// <summary>
        /// Get/Set method of the CheckInTrxLineId field
        /// </summary>
        public int CheckInTrxLineId
        {
            get { return checkInDetailDTO.CheckInTrxLineId; }
            set { checkInDetailDTO.CheckInTrxLineId = value; }
        }
        /// <summary>
        /// Get/Set method of the CheckInTime field
        /// </summary>
        public DateTime? CheckInTime
        {
            get { return checkInDetailDTO.CheckInTime; }
            set { checkInDetailDTO.CheckInTime = value; }
        }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public CheckInStatus Status
        {
            get { return checkInDetailDTO.Status; }
            set { checkInDetailDTO.Status = value; }
        }

        public CheckInDetailDTO CheckInDetailDTO
        {
            get { return checkInDetailDTO; }
            set { checkInDetailDTO = value; }
        }

    }
}
