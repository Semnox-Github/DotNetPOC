/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - LocalDisplayPanelThemeMapUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    21-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{

    /// <summary>
    /// Implementation of DisplayPanelThemeMap use-cases
    /// </summary>
    public class LocalDisplayPanelThemeMapUseCases:IDisplayPanelThemeMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalDisplayPanelThemeMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<DisplayPanelThemeMapDTO>> GetDisplayPanelThemeMaps(List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<DisplayPanelThemeMapDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);

                DisplayPanelThemeMapListBL displayPanelThemeMapListBL = new DisplayPanelThemeMapListBL(executionContext);
                List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(displayPanelThemeMapDTOList);
                return displayPanelThemeMapDTOList;
            });
        }
        public async Task<string> SaveDisplayPanelThemeMaps(List<DisplayPanelThemeMapDTO> displayPanelThemeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(displayPanelThemeDTOList);
                    if (displayPanelThemeDTOList == null)
                    {
                        throw new ValidationException("displayPanelThemeMapDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DisplayPanelThemeMapListBL panelThemeMap = new DisplayPanelThemeMapListBL(executionContext, displayPanelThemeDTOList);
                            panelThemeMap.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
