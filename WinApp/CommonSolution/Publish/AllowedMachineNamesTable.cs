using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class AllowedMachineNamesTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public AllowedMachineNamesTable(ExecutionContext executionContext) :
            base(executionContext, "AllowedMachineNames", "MachineName")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            string result = @"MERGE INTO AllowedMachineNames tbl 
                USING ( SELECT (SELECT TOP 1 ref.game_id FROM games ref  WHERE ref.MasterEntityId = AllowedMachineNames.Game_id AND ref.site_id = sl.id) Game_id, AllowedMachineNames.MachineName,AllowedMachineNames.IsActive, AllowedMachineNames.CreatedBy, dbo.TimeZoneOffset(AllowedMachineNames.CreationDate, sl.Id) AS CreationDate, AllowedMachineNames.LastUpdatedBy, dbo.TimeZoneOffset(AllowedMachineNames.LastupdateDate, sl.Id) AS LastupdateDate, sl.Id as publishSite_id, AllowedMachineNames.MasterEntityId
                FROM AllowedMachineNames, @pkIdList pkl, @siteIdList sl
                WHERE AllowedMachineNames.AllowedMachineId = pkl.Id) AS src
                ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id
                WHEN MATCHED

                THEN UPDATE SET 
                Game_id = src.Game_id
                , MachineName = src.MachineName
                , IsActive = src.IsActive
                , CreatedBy = src.CreatedBy
                , CreationDate = src.CreationDate
                , LastUpdatedBy = src.LastUpdatedBy
                , LastupdateDate = src.LastupdateDate

                WHEN NOT MATCHED THEN insert (
                Game_id
                , MachineName
                , IsActive
                , CreatedBy
                , CreationDate
                , LastUpdatedBy
                , LastupdateDate
                , site_id
                , MasterEntityId
                ) VALUES ( 
                src.Game_id
                , src.MachineName
                , src.IsActive
                , src.CreatedBy
                , src.CreationDate
                , src.LastUpdatedBy
                , src.LastupdateDate
                , src.publishSite_id
                , src.MasterEntityId
                );";
            result += @"
                        UPDATE m
                        SET m.machine_name = am.MachineName,
                        m.last_updated_user = '" + executionContext.UserId + @"',
                        m.last_updated_date = GETDATE()
                        FROM machines m, AllowedMachineNames am, @pkIdList pkl,@siteIdList sl
                        WHERE m.AllowedMachineId = am.AllowedMachineId
                        AND m.machine_name != am.MachineName
                        AND m.site_id=sl.Id
                        AND am.MasterEntityId = pkl.Id ";

            log.LogMethodExit(result);
            return result;
        }

        
    }
}
