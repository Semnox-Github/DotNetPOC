using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Semnox.Parafait.Publish
{
    public class ScreenZoneContentMapTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ScreenZoneContentMapTable(ExecutionContext executionContext) :
            base(executionContext, "ScreenZoneContentMap")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            Columns.Add(new ForeignKeyColumn("ContentGuid", "Ticker", "TickerID"));
            Columns.Add(new ForeignKeyColumn("ContentGuid", "DSLookup", "DSLookupID"));
            Columns.Add(new ForeignKeyColumn("ContentGuid", "SignagePattern", "SignagePatternId"));
            Columns.Add(new ForeignKeyColumn("ContentGuid", "Media", "MediaID"));
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish, referredEntity);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"ScreenZoneContentMap tbl 
                            USING ( SELECT (SELECT TOP 1 ref.ZoneID FROM ScreenZoneDefSetup ref  WHERE ref.MasterEntityId = ScreenZoneContentMap.ZoneID AND ref.site_id = sl.id) ZoneID, 
                            (SELECT TOP 1 ref.LookupValueId FROM LookupValues ref  WHERE ref.MasterEntityId = ScreenZoneContentMap.ContentTypeID AND ref.site_id = sl.id) ContentTypeID, 
                            ScreenZoneContentMap.ContentType, ScreenZoneContentMap.ContentID, ScreenZoneContentMap.BackImage, ScreenZoneContentMap.BackColor, ScreenZoneContentMap.BorderSize, ScreenZoneContentMap.BorderColor, ScreenZoneContentMap.ImgSize, ScreenZoneContentMap.ImgAlignment, ScreenZoneContentMap.ImgRefreshSecs, ScreenZoneContentMap.VideoRepeat, ScreenZoneContentMap.LookupRefreshSecs, ScreenZoneContentMap.LookupHeaderDisplay, ScreenZoneContentMap.TickerScrollDirection, ScreenZoneContentMap.TickerSpeed, ScreenZoneContentMap.TickerRefreshSecs, sl.Id as publishSite_id, ScreenZoneContentMap.last_updated_user, ScreenZoneContentMap.last_updated_date, ScreenZoneContentMap.DisplayOrder, ScreenZoneContentMap.Creationdate, ScreenZoneContentMap.CreatedUser, ScreenZoneContentMap.Active_Flag, ScreenZoneContentMap.MasterEntityId, 
                            (CASE WHEN ScreenZoneContentMap.ContentType = 'TICKER' THEN(SELECT TOP 1 ref.guid FROM Ticker ref WHERE ref.MasterEntityId = (SELECT TickerId from Ticker Where guid = ScreenZoneContentMap.ContentGuid) AND ref.site_id = sl.id) 
                                  WHEN ScreenZoneContentMap.ContentType = 'PATTERN' THEN(SELECT TOP 1 ref.guid FROM SignagePattern ref WHERE ref.MasterEntityId = (SELECT SignagePatternId from SignagePattern Where guid = ScreenZoneContentMap.ContentGuid) AND ref.site_id = sl.id)
	                              WHEN ScreenZoneContentMap.ContentType = 'LOOKUP' THEN(SELECT TOP 1 ref.guid FROM DSlookup ref WHERE ref.MasterEntityId = (SELECT DSlookupId from DSlookup Where guid = ScreenZoneContentMap.ContentGuid) AND ref.site_id = sl.id) 
	                              ELSE (SELECT TOP 1 ref.guid FROM Media ref WHERE ref.MasterEntityId = (SELECT MediaId from Media Where guid = ScreenZoneContentMap.ContentGuid) AND ref.site_id = sl.id) END) ContentGuid
                            FROM ScreenZoneContentMap, @pkIdList pkl, @siteIdList sl
                            WHERE ScreenZoneContentMap.ScreenContentID = pkl.Id) AS src
                            ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id ";
            if (referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND 
                         ( EXISTS(SELECT tbl.ZoneID EXCEPT SELECT src.ZoneID)
                         OR  EXISTS(SELECT tbl.ContentTypeID EXCEPT SELECT src.ContentTypeID)
                         OR  EXISTS(SELECT tbl.ContentType EXCEPT SELECT src.ContentType)
                         OR  EXISTS(SELECT tbl.ContentID EXCEPT SELECT src.ContentID)
                         OR  EXISTS(SELECT tbl.BackImage EXCEPT SELECT src.BackImage)
                         OR  EXISTS(SELECT tbl.BackColor EXCEPT SELECT src.BackColor)
                         OR  EXISTS(SELECT tbl.BorderSize EXCEPT SELECT src.BorderSize)
                         OR  EXISTS(SELECT tbl.BorderColor EXCEPT SELECT src.BorderColor)
                         OR  EXISTS(SELECT tbl.ImgSize EXCEPT SELECT src.ImgSize)
                         OR  EXISTS(SELECT tbl.ImgAlignment EXCEPT SELECT src.ImgAlignment)
                         OR  EXISTS(SELECT tbl.ImgRefreshSecs EXCEPT SELECT src.ImgRefreshSecs)
                         OR  EXISTS(SELECT tbl.VideoRepeat EXCEPT SELECT src.VideoRepeat)
                         OR  EXISTS(SELECT tbl.LookupRefreshSecs EXCEPT SELECT src.LookupRefreshSecs)
                         OR  EXISTS(SELECT tbl.LookupHeaderDisplay EXCEPT SELECT src.LookupHeaderDisplay)
                         OR  EXISTS(SELECT tbl.TickerScrollDirection EXCEPT SELECT src.TickerScrollDirection)
                         OR  EXISTS(SELECT tbl.TickerSpeed EXCEPT SELECT src.TickerSpeed)
                         OR  EXISTS(SELECT tbl.TickerRefreshSecs EXCEPT SELECT src.TickerRefreshSecs)
                         OR  EXISTS(SELECT tbl.last_updated_user EXCEPT SELECT src.last_updated_user)
                         OR  EXISTS(SELECT tbl.last_updated_date EXCEPT SELECT src.last_updated_date)
                         OR  EXISTS(SELECT tbl.DisplayOrder EXCEPT SELECT src.DisplayOrder)
                         OR  EXISTS(SELECT tbl.Creationdate EXCEPT SELECT src.Creationdate)
                         OR  EXISTS(SELECT tbl.CreatedUser EXCEPT SELECT src.CreatedUser)
                         OR  EXISTS(SELECT tbl.Active_Flag EXCEPT SELECT src.Active_Flag)
                         OR  EXISTS(SELECT tbl.ContentGuid EXCEPT SELECT src.ContentGuid)
                        )";
                }


                query +=
                      @"
                    THEN UPDATE SET 
                    ZoneID = src.ZoneID
                    , ContentTypeID = src.ContentTypeID
                    , ContentType = src.ContentType
                    , ContentID = src.ContentID
                    , BackImage = src.BackImage
                    , BackColor = src.BackColor
                    , BorderSize = src.BorderSize
                    , BorderColor = src.BorderColor
                    , ImgSize = src.ImgSize
                    , ImgAlignment = src.ImgAlignment
                    , ImgRefreshSecs = src.ImgRefreshSecs
                    , VideoRepeat = src.VideoRepeat
                    , LookupRefreshSecs = src.LookupRefreshSecs
                    , LookupHeaderDisplay = src.LookupHeaderDisplay
                    , TickerScrollDirection = src.TickerScrollDirection
                    , TickerSpeed = src.TickerSpeed
                    , TickerRefreshSecs = src.TickerRefreshSecs
                    , last_updated_user = src.last_updated_user
                    , last_updated_date = src.last_updated_date
                    , DisplayOrder = src.DisplayOrder
                    , Creationdate = src.Creationdate
                    , CreatedUser = src.CreatedUser
                    , Active_Flag = src.Active_Flag
                    , ContentGuid = src.ContentGuid";
            }

            query +=
              @" WHEN NOT MATCHED THEN insert (
                    ZoneID
                    , ContentTypeID
                    , ContentType
                    , ContentID
                    , BackImage
                    , BackColor
                    , BorderSize
                    , BorderColor
                    , ImgSize
                    , ImgAlignment
                    , ImgRefreshSecs
                    , VideoRepeat
                    , LookupRefreshSecs
                    , LookupHeaderDisplay
                    , TickerScrollDirection
                    , TickerSpeed
                    , TickerRefreshSecs
                    , site_id
                    , last_updated_user
                    , last_updated_date
                    , DisplayOrder
                    , Creationdate
                    , CreatedUser
                    , Active_Flag
                    , MasterEntityId
                    , ContentGuid
                    ) VALUES ( 
                    src.ZoneID
                    , src.ContentTypeID
                    , src.ContentType
                    , src.ContentID
                    , src.BackImage
                    , src.BackColor
                    , src.BorderSize
                    , src.BorderColor
                    , src.ImgSize
                    , src.ImgAlignment
                    , src.ImgRefreshSecs
                    , src.VideoRepeat
                    , src.LookupRefreshSecs
                    , src.LookupHeaderDisplay
                    , src.TickerScrollDirection
                    , src.TickerSpeed
                    , src.TickerRefreshSecs
                    , src.publishSite_id
                    , src.last_updated_user
                    , src.last_updated_date
                    , src.DisplayOrder
                    , src.Creationdate
                    , src.CreatedUser
                    , src.Active_Flag
                    , src.MasterEntityId
                    , src.ContentGuid
                    ) ";
            sb.Append(query);
            if (enableAuditLog)
            {
                sb.Append(Environment.NewLine);
                sb.Append(GetOutputQuery());
            }
            sb.Append(";");
            if (enableAuditLog)
            {
                sb.Append(Environment.NewLine);
                sb.Append(GetDBAuditQuery());
            }
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public override string GetParentPrimaryKeyListQuery(ForeignKeyColumn foreignKeyColumn)
        {
            log.LogMethodEntry(foreignKeyColumn);
            string result = string.Empty;
            if(foreignKeyColumn.ReferencedTableName.ToLower() == "ticker")
            {
                result = @"select Ticker.TickerID Id
                            from ScreenZoneContentMap, Ticker, @pkIdList pkl
                            WHERE  ScreenZoneContentMap.ContentGuid = Ticker.Guid AND ContentType = 'TICKER'
                            AND pkl.Id = ScreenZoneContentMap.ScreenContentID";
            }
            else if (foreignKeyColumn.ReferencedTableName.ToLower() == "dslookup")
            {
                result = @"select DSLookup.DSLookupID Id
                            from ScreenZoneContentMap, DSLookup, @pkIdList pkl
                            WHERE  ScreenZoneContentMap.ContentGuid = DSLookup.Guid AND ContentType = 'LOOKUP'
                            AND pkl.Id = ScreenZoneContentMap.ScreenContentID";
            }
            else if (foreignKeyColumn.ReferencedTableName.ToLower() == "signagepattern")
            {
                result = @"select SignagePattern.SignagePatternId Id
                            from ScreenZoneContentMap, SignagePattern, @pkIdList pkl
                            WHERE  ScreenZoneContentMap.ContentGuid = SignagePattern.Guid AND ContentType = 'PATTERN'
                            AND pkl.Id = ScreenZoneContentMap.ScreenContentID";
            }
            else if (foreignKeyColumn.ReferencedTableName.ToLower() == "media")
            {
                result = @"select Media.MediaID Id
                            from ScreenZoneContentMap, Media, @pkIdList pkl
                            WHERE  ScreenZoneContentMap.ContentGuid = Media.Guid AND ContentType NOT IN ('TICKER','PATTERN', 'LOOKUP') 
                            AND pkl.Id = ScreenZoneContentMap.ScreenContentID";
            }
            else if (foreignKeyColumn.ReferencedTableName.ToLower() == "screenzonedefsetup")
            {
                result = base.GetParentPrimaryKeyListQuery(foreignKeyColumn);
            }
            else
            {
                string message = "ScreenZoneContentMap for " + foreignKeyColumn.Name + " is not implemented";
                log.LogMethodExit(null, message);
                throw new NotImplementedException(message);
            }
            log.LogMethodExit(result);
            return result;
        }
       
    }
}
