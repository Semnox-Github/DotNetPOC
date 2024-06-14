/********************************************************************************************
 * Project Name - Site
 * Description  - DTO of Purge Data
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        08-May-2019   Mushahid Faizan         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    public class PurgeDataDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        DateTime date;
        DateTime cardsDate;
        DateTime gameplayDate;
        DateTime transactionsDate;
        DateTime logsDate;
        bool balance;
        bool manualPurge;


        public PurgeDataDTO()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cardsDate"></param>
        /// <param name="gameplayDate"></param>
        /// <param name="transactionsDate"></param>
        /// <param name="logsDate"></param>
        /// <param name="balance"></param>
        /// <param name="manualPurge"></param>
        public PurgeDataDTO(DateTime date, DateTime cardsDate, DateTime gameplayDate, DateTime transactionsDate, DateTime logsDate, bool balance,
                            bool manualPurge)
        {
            log.LogMethodEntry();
            this.date = date;
            this.cardsDate = cardsDate;
            this.gameplayDate = gameplayDate;
            this.transactionsDate = transactionsDate;
            this.logsDate = logsDate;
            this.balance = balance;
            this.manualPurge = manualPurge;
            log.LogMethodExit();

        }
        /// <summary>
        /// Get/Set method of the ManualPurge field
        /// </summary>
        public bool ManualPurge
        {
            get { return manualPurge; }
            set { manualPurge = value; }
        }
        /// <summary>
        /// Get/Set method of the Balance field
        /// </summary>
        public bool Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        /// <summary>
        /// Get/Set method of the LogsDate field
        /// </summary>
        public DateTime LogsDate
        {
            get { return logsDate; }
            set { logsDate = value; }
        }
        /// <summary>
        /// Get/Set method of the TransactionsDate field
        /// </summary>
        public DateTime TransactionsDate
        {
            get { return transactionsDate; }
            set { transactionsDate = value; }
        }
        /// <summary>
        /// Get/Set method of the Gameplaydate field
        /// </summary>
        public DateTime Gameplaydate
        {
            get { return gameplayDate; }
            set { gameplayDate = value; }
        }
        /// <summary>
        /// Get/Set method of the CardsDate field
        /// </summary>
        public DateTime CardsDate
        {
            get { return cardsDate; }
            set { cardsDate = value; }
        }
        /// <summary>
        /// Get/Set method of the Date field
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
