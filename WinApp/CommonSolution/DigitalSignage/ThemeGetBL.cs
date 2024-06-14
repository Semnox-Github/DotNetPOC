/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - ThemeGetBL class is used to call the business methods - Only  Get operation
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Data.SqlClient;

namespace Semnox.Parafait.DigitalSignage
{
    public class ThemeGetBL
    {
        private ThemeDTO themeDTO;
        private ThemeDataHandler themeDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;


        private ThemeGetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public ThemeGetBL(ExecutionContext executionContext, ThemeDTO themeDTO)
      : this(executionContext)
        {
            log.LogMethodEntry(executionContext, themeDTO);
            this.themeDTO = themeDTO;
            log.LogMethodExit();
        }
        public ThemeGetBL(ExecutionContext executionContext ,int themeId)
            : this(executionContext)
        {
            log.LogMethodEntry(themeId);
            this.themeDTO = themeDataHandler.GetThemeDTO(themeId);
            log.LogMethodExit(themeDTO);
        }
        public ThemeDTO GetThemeDTO
        {
            get { return themeDTO; }
        }
    }
}
