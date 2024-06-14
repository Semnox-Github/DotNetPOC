/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSPrinterContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.120.0        18- Jun- 2021   Prajwal         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class POSPrinterContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int posPrinterId;
        private int printerId;
        private int printTemplateId;
        private int posTypeId;
        private int posMachineId;
        private int orderTypeGroupId;
        private int secondaryPrinterId;
        private int printerTypeId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSPrinterContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public POSPrinterContainerDTO(int posPrinterId, int posMachineId, int printerId, int posTypeId, int secondaryPrinterId,
                             int orderTypeGroupId, int printTemplateId, int printerTypeId)
            : this()
        {
            log.LogMethodEntry(posPrinterId, posMachineId, printerId, posTypeId, secondaryPrinterId, orderTypeGroupId,
                            printTemplateId,  printerTypeId);
            this.posPrinterId = posPrinterId;
            this.printerId = printerId;
            this.posMachineId = posMachineId;
            this.posTypeId = posTypeId;
            this.secondaryPrinterId = secondaryPrinterId;
            this.orderTypeGroupId = orderTypeGroupId;
            this.printTemplateId = printTemplateId;
            this.printerTypeId = printerTypeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the POSPrinterId field
        /// </summary>
        [DisplayName("POSPrinterId")]
        public int POSPrinterId
        {
            get
            {
                return posPrinterId;
            }
            set
            {
                posPrinterId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterId field
        /// </summary>
        [DisplayName("PrinterId")]
        public int PrinterId
        {
            get
            {
                return printerId;
            }
            set
            {
                printerId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the SecondaryPrinterId field
        /// </summary>
        [DisplayName("SecondaryPrinterId")]
        public int SecondaryPrinterId
        {
            get
            {
                return secondaryPrinterId;
            }
            set
            {
                secondaryPrinterId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the OrderTypeGroupId field
        /// </summary>
        [DisplayName("OrderTypeGroupId")]
        public int OrderTypeGroupId
        {
            get
            {
                return orderTypeGroupId;
            }
            set
            {
                orderTypeGroupId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POSMachineId")]
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }
            set
            {
                posMachineId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterId field
        /// </summary>
        [DisplayName("POSTypeId")]
        public int POSTypeId
        {
            get
            {
                return posTypeId;
            }
            set
            {
                posTypeId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the TemplateId field
        /// </summary>
        [DisplayName("PrintTemplateId")]
        public int PrintTemplateId
        {
            get
            {
                return printTemplateId;
            }
            set
            {
                printTemplateId = value;
              
            }
        }

        /// <summary>
        /// Get/Set method of the SecondaryPrinterId field
        /// </summary>
        [DisplayName("PrinterTypeId")]
        public int PrinterTypeId
        {
            get
            {
                return printerTypeId;
            }
            set
            {
                printerTypeId = value;
              
            }
        }
    }
}
