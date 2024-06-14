/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu object types that can be assigned to a button
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      05-Aug-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product menu object types that can be assigned to a button
    /// </summary>
    public class ProductMenuObjectTypes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string PRODUCT = "PRODUCTS";
        public const string PRODUCT_MENU_PANEL = "PRODUCT_MENU_PANEL";

        public static bool IsValidObjectType(string objectType)
        {
            log.LogMethodEntry(objectType);
            bool result = objectType == PRODUCT || objectType == PRODUCT_MENU_PANEL;
            log.LogMethodExit(result);
            return result;
        }
    }
}
