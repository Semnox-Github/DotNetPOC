/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of TransactionProfileContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.130.0      1-Sep-2021    Lakshminarayana         Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class TransactionProfileContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TransactionProfileContainerDTO> transactionProfileContainerDTOList;
        private string hash;

        public TransactionProfileContainerDTOCollection()
        {
            log.LogMethodEntry();
            transactionProfileContainerDTOList = new List<TransactionProfileContainerDTO>();
            log.LogMethodExit();
        }

        public TransactionProfileContainerDTOCollection(List<TransactionProfileContainerDTO> transactionProfileContainerDTOList)
        {
            log.LogMethodEntry(transactionProfileContainerDTOList);
            this.transactionProfileContainerDTOList = transactionProfileContainerDTOList;
            if (this.transactionProfileContainerDTOList == null)
            {
                this.transactionProfileContainerDTOList = new List<TransactionProfileContainerDTO>();
            }
            hash = new DtoListHash(transactionProfileContainerDTOList);
            log.LogMethodExit();
        }

        public List<TransactionProfileContainerDTO> TransactionProfileContainerDTOList
        {
            get
            {
                return transactionProfileContainerDTOList;
            }

            set
            {
                transactionProfileContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
