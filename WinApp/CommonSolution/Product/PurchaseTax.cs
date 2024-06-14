using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class PurchaseTaxTemp
    {
        PurchaseTaxDTOTemp purchaseTaxDTO;
        public PurchaseTaxDTOTemp getPurchaseTaxDTO { get { return purchaseTaxDTO; } }
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PurchaseTaxTemp()
        {
            log.LogMethodEntry();
            purchaseTaxDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="purchaseTaxDTO">Parameter of the type PurchaseTaxDTO</param>
        public PurchaseTaxTemp(PurchaseTaxDTOTemp purchaseTaxDTO)
        {
            log.LogMethodEntry(purchaseTaxDTO);
            this.purchaseTaxDTO = purchaseTaxDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the PurchaseTax id as the parameter
        /// Would fetch the PurchaseTax object based on the ID passed. 
        /// </summary>
        /// <param name="PurchaseTaxId">PurchaseTax Id</param>
        public PurchaseTaxTemp(int PurchaseTaxId)
            : this()
        {
            log.LogMethodEntry(PurchaseTaxId);
            PurchaseTaxDataHandlerTemp purchaseTaxDataHandler = new PurchaseTaxDataHandlerTemp();
            purchaseTaxDTO = purchaseTaxDataHandler.GetPurchaseTax(PurchaseTaxId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Return the tax amount for the passed amount and the loaded tax DTO  
        /// </summary>
        /// <param name="amount">Cost of the product</param>
        /// <param name="IsInclusive">Inclusive or not</param>
        /// <returns></returns>
        public double GetTaxAmount(double amount,bool IsInclusive)
        {
            log.LogMethodEntry(amount, IsInclusive);
            double taxAmount = 0.0;
            if (purchaseTaxDTO != null)
            {
                if (IsInclusive)
                {
                    taxAmount = (purchaseTaxDTO.TaxPercentage / 100 * amount / (1 + purchaseTaxDTO.TaxPercentage / 100));
                }
                else
                {
                    taxAmount = amount * purchaseTaxDTO.TaxPercentage / 100;
                }
            }            
            log.LogMethodExit(taxAmount);
            return taxAmount;
        }

        /// <summary>
        /// Saves the PurchaseTax
        /// UOM will be inserted if PurchaseTaxId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            ExecutionContext UserContext = ExecutionContext.GetExecutionContext();
            PurchaseTaxDataHandlerTemp purchaseTaxDataHandler = new PurchaseTaxDataHandlerTemp();
            if (purchaseTaxDTO.TaxId < 0)
            {
                int TaxId = purchaseTaxDataHandler.InsertPurchaseTax(purchaseTaxDTO, UserContext.GetSiteId(), null);
                purchaseTaxDTO.TaxId = TaxId;
            }
            else
            {
                if (purchaseTaxDTO.IsChanged == true)
                {
                    purchaseTaxDataHandler.UpdatePurchaseTax(purchaseTaxDTO, UserContext.GetSiteId(), null);
                    purchaseTaxDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    public class PurchaseTaxListTemp
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the vendor list
        /// </summary>
        public List<PurchaseTaxDTOTemp> GetAllPurchaseTaxes(List<KeyValuePair<PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            PurchaseTaxDataHandlerTemp purchaseTaxDataHandler = new PurchaseTaxDataHandlerTemp();
            log.LogMethodExit(purchaseTaxDataHandler.GetPurchaseTaxList(searchParameters));
            return purchaseTaxDataHandler.GetPurchaseTaxList(searchParameters);
        }
        /// <summary>
        /// Retriving PurchaseTax by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the vendor</param>
        /// <returns> List of PurchaseTaxDTO </returns>
        public List<PurchaseTaxDTOTemp> GetPurchaseTaxList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            PurchaseTaxDataHandlerTemp purchaseTaxDataHandler = new PurchaseTaxDataHandlerTemp();
            log.LogMethodExit(purchaseTaxDataHandler.GetPurchaseTaxList(sqlQuery));
            return purchaseTaxDataHandler.GetPurchaseTaxList(sqlQuery);
        }
        /// <summary>
        /// Returns the coulumns name list of uom table.
        /// </summary>
        /// <returns></returns>
        public DataTable GetPurchaseTaxColumnsName()
        {
            PurchaseTaxDataHandlerTemp purchaseTaxDataHandler = new PurchaseTaxDataHandlerTemp();
            return purchaseTaxDataHandler.GetPurchaseTaxColumns();
        }
    }
}
