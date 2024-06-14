/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - Cards class This would hold the CardCombineDTO related details
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
   public class CardCombineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string sourceAccountNumber;
        private OperatorDTO operatorDTO;
        public CardCombineDTO()
        {
            log.LogMethodEntry();
            sourceAccountNumber = null;
            operators = null;
            log.LogMethodExit();
        }
        public string sourceCardNumber { get { return sourceAccountNumber; } set { sourceAccountNumber = value; } }
        public OperatorDTO operators { get { return operatorDTO; } set { operatorDTO = value; } }
    }
}

