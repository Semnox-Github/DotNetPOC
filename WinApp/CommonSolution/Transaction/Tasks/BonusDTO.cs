/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Balance Transfer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.1     12-May-2021   Fiona                   Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public class BonusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int accountId;
        private BonusTypeEnum bonusType;
        private decimal bonusValue;
        private string remarks;
        private int managerId;
        private int gamePlayId;
        private int trxId;
        
        
        /// <summary>
        /// 
        /// </summary>
        public enum BonusTypeEnum
        {
            ///<summary>
            ///NONE
            ///</summary>
            NONE = -1,

            ///<summary>
            ///TICKETS
            ///</summary>
            CARD_BALANCE = 1,

            ///<summary>
            ///CREDITS
            ///</summary>
            LOYALTY_POINT = 2,

            /// <summary>
            /// BONUS
            /// </summary>
            GAME_PLAY_CREDIT = 3,

            /// <summary>
            /// TIME
            /// </summary>
            GAME_PLAY_BONUS = 4
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public BonusDTO()
        {
            log.LogMethodEntry();
            this.accountId = -1;
            this.bonusType = BonusTypeEnum.NONE;
            this.bonusValue = 0;
            this.remarks = string.Empty;
            this.managerId = -1;
            this.gamePlayId = -1;
            this.trxId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="bonusType"></param>
        /// <param name="bonusValue"></param>
        /// <param name="remarks"></param>
        /// <param name="managerId"></param>
        /// <param name="gamePlayId"></param>
        /// <param name="trxId"></param>
        public BonusDTO(int accountId, BonusTypeEnum bonusType, decimal bonusValue, string remarks, int managerId, int gamePlayId, int trxId)
        {
            log.LogMethodEntry();
            this.accountId = accountId;
            this.bonusType = bonusType;
            this.bonusValue = bonusValue;
            this.remarks = remarks;
            this.managerId = managerId;
            this.gamePlayId = gamePlayId;
            this.trxId = trxId;
            log.LogMethodExit();
        }


        /// <summary>
        /// 
        /// </summary>
        public int AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        /// <summary>
        /// BonusValue
        /// </summary>
        public BonusTypeEnum BonusType
        {
            get { return bonusType; }
            set { bonusType = value; }
        }
        /// <summary>
        /// BonusValue
        /// </summary>
        public decimal BonusValue
        {
            get { return bonusValue; }
            set { bonusValue = value; }
        }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        /// <summary>
        /// ManagerId
        /// </summary>
        public int ManagerId
        {
            get { return managerId; }
            set { managerId = value; }
        }
        /// <summary>
        /// GamePlayId
        /// </summary>
        public int GamePlayId
        {
            get { return gamePlayId; }
            set { gamePlayId = value; }
        }
        /// <summary>
        /// TrxId
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; }
        }
    }
}
