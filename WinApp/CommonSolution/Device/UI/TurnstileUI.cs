/********************************************************************************************
 * Project Name - Device.Turnstile UI
 * Description  - Class for  of TurnstileUI      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TurnstileUI
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public class TurnstileObjectClass
        {
            public TurnstileDTO DTO;
            public TurnstileClass Device;
            public string GameProfileName;
            public string Type;
            public string Make;
            public string Model;
            public string Status;
        }

        public frmShowTurnstiles UIForm;
        List<TurnstileObjectClass> lstTurnstiles = new List<TurnstileObjectClass>();

        public TurnstileUI()
        {
            log.LogMethodEntry();
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            TurnstilesList turnstileList = new TurnstilesList(executionContext);
            TurnstileSearchParams searchParams = new TurnstileSearchParams();
            searchParams.Active = true;
            List<TurnstileDTO> turnstiles = turnstileList.GetAllTurnstilesList(searchParams);

            foreach (TurnstileDTO tDto in turnstiles)
            {
                TurnstileObjectClass item = new TurnstileObjectClass();
                
                item.Type = getTurnstileType(Convert.ToInt32(tDto.Type));
                item.Make = getTurnstileMake(Convert.ToInt32(tDto.Make));
                item.Model = getTurnstileModel(Convert.ToInt32(tDto.Model));
                item.GameProfileName = getProfileName(tDto.GameProfileId);

                TurnstileClass device = null;
                if (TurnstileClass.TunstileMake.TISO.ToString().Equals(item.Make))
                {
                    device = new TISO();
                    if (tDto.PortNumber <= 0)
                        tDto.PortNumber = 9761;
                }

                item.DTO = tDto;
                item.Device = device;

                lstTurnstiles.Add(item);

                if (device != null)
                    device.SetParameters(tDto.IPAddress, (tDto.PortNumber == null ? -1 : Convert.ToInt32(tDto.PortNumber)));
            }

            UIForm = new frmShowTurnstiles(lstTurnstiles);
            log.LogMethodExit();
        }

        string getTurnstileType(int Type)
        {
            log.LogMethodEntry(Type);
            if (Type == -1)
                return null;

            DataAccessHandler dataHandler = new DataAccessHandler();
            log.LogVariableState("QueryParam", Type);
            string lookupValue = dataHandler.executeSelectQuery(@"select LookupValue
                                                from LookupView
                                                where LookupName = 'TURNSTILE_TYPE'
                                                and LookupValueId = @id",
                                                new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", Type) },
                                                null).Rows[0][0].ToString();
            log.LogMethodExit(lookupValue);
            return lookupValue;
            
        }

        string getTurnstileMake(int Make)
        {
            log.LogMethodEntry(Make);
            if (Make == -1)
                return null;

            DataAccessHandler dataHandler = new DataAccessHandler();
            log.LogVariableState("QueryParam", Make);
            string lookupValue = dataHandler.executeSelectQuery(@"select LookupValue
                                                from LookupView
                                                where LookupName = 'TURNSTILE_MAKE'
                                                and LookupValueId = @id",
                                                new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", Make) },
                                                null).Rows[0][0].ToString();
            log.LogMethodExit(lookupValue);
            return lookupValue;
        }

        string getTurnstileModel(int Model)
        {
            log.LogMethodEntry(Model);
            if (Model == -1)
                return null;

            DataAccessHandler dataHandler = new DataAccessHandler();
            string lookupValue = dataHandler.executeSelectQuery(@"select LookupValue
                                                from LookupView
                                                where LookupName = 'TURNSTILE_MODEL'
                                                and LookupValueId = @id",
                                                new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", Model) },
                                                null).Rows[0][0].ToString();
            log.LogMethodExit(lookupValue);
            return lookupValue;
        }

        string getProfileName(int profileId)
        {
            log.LogMethodEntry(profileId);
            if (profileId == -1)
                return null;

            DataAccessHandler dataHandler = new DataAccessHandler();
            string profileName = dataHandler.executeSelectQuery(@"select profile_name 
                                                from game_profile 
                                                where game_profile_id = @id",
                                                new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", profileId) },
                                                null).Rows[0][0].ToString();
            log.LogMethodExit(profileName);
            return profileName;
        }

        public void Show()
        {
            log.LogMethodEntry();
            if (UIForm.IsDisposed || UIForm.Disposing)
            {
                UIForm = new frmShowTurnstiles(lstTurnstiles);
                UIForm.Show();
            }
            else if (!UIForm.Visible)
            {
                UIForm.Show();
            }
            else
            {
                UIForm.Activate();
            }
            log.LogMethodExit();
        }

        public void Close()
        {
            log.LogMethodEntry();
            if (!UIForm.IsDisposed && !UIForm.Disposing)
            {
                UIForm.Close();
            }
            log.LogMethodExit();
        }
    }
}
