/*******************************************************************************************************************************
* Project Name - AdManagement.cs
* Description  - Managing the Ads to be broadcasted
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*******************************************************************************************************************************
*1.00        14-Apr-2008      Iqbal Mohammad Created  
*2.130.0     08-Aug-2021      Mathew Ninan   Added Refresh method and modified for refreshing admanagement object in Machine 
*                                            context 
* *******************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    public class AdManagement
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public class MachineAdShowContext
        {
            public int BroadcastId;
            public int MachineId;
            public string MachineAddress;
            public int AbandonContextRetryCount = 0;
            public int RetryAttempts = 0;
            public string CurrentFile = "";
            public class AdFile
            {
                public string FileName;
                public bool FileFound = true;
                public bool ONAcknowledged = false;
                public bool OFFAcknowledged = false;
                public int FromTime;
                public int EndTime;
            }

            public AdFile[] AdFiles = new AdFile[5];
        }

        public class MachineAdContext
        {
            public int BroadcastId;
            public int MachineId;
            public string MachineAddress;
            public bool ContextExists = false;
            public string FileName;
            public long fileSize;
            public FileStream fileStream;
            public bool ValidContext = false;
            public int SentPacketNumber = 0;
            public int AbandonContextRetryCount = 0;
            public int RetryAttempts = 0;
            public int TotalPackets = 0;
            public bool SendPriceInfo = true;

            Utilities Utilities;
            public MachineAdContext(Utilities inUtilities)
            {
                log.LogMethodEntry(inUtilities);
                Utilities = inUtilities;
                log.LogMethodExit(null);
            }
            private ExecutionContext executionContext;

            public MachineAdContext(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                log.LogMethodExit(null);
            }

            public void updateBeginTime()
            {
                log.LogMethodEntry();
                Utilities.executeNonQuery("update AdBroadcast set BroadcastBeginTime = getdate() " +
                                            "where id = @id",
                                            new SqlParameter("@id", BroadcastId));
                log.LogVariableState("@id", BroadcastId);
                log.LogMethodExit(null);
            }

            public void updateEndTime()
            {
                log.LogMethodEntry();
                Utilities.executeNonQuery("update AdBroadcast set BroadcastEndTime = getdate() " +
                                        "where id = @id",
                                        new SqlParameter("@id", BroadcastId));
                log.LogVariableState("@id", BroadcastId);
                log.LogMethodExit(null);
            }
        }

        public int PACKETSIZE = 22;
        public int MAXRETRYATTEMPTS = 3;

        public MachineAdContext[] BroadcastCollection;
        FileStream[] fileStreams;
        int fileStreamIndex = 0;

        public int machineId = -1;
        public int accessPointId = -1;

        GameServerEnvironment GameServerEnvironment;
        Utilities Utilities;

        private ExecutionContext executionContext;
        public AdManagement(GameServerEnvironment inGameServerEnvironment, Utilities utilities)
        {
            log.LogMethodEntry(inGameServerEnvironment);
            GameServerEnvironment = inGameServerEnvironment;
            executionContext = inGameServerEnvironment.GameEnvExecutionContext;
            Utilities = utilities;
            log.LogMethodExit(null);
        }

        public AdManagement(ExecutionContext executionContext, Utilities utilities)
            : this(new GameServerEnvironment(executionContext), utilities)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        ////public AdManagement(int AccessPointId, GameServerEnvironment gameServerEnvironment, int forMachineId = -1)
        public AdManagement(int AccessPointId, ExecutionContext executionContext, Utilities utilities, int forMachineId = -1)
            : this(new GameServerEnvironment(executionContext), utilities)
        {
            log.LogMethodEntry(AccessPointId, executionContext, forMachineId);
            //this.gameServerEnvironment = gameServerEnvironment;
            machineId = forMachineId;
            accessPointId = AccessPointId;
            MachineAdContext[] machineAdContexts = fetchAdManagementDetails(AccessPointId, forMachineId);

            if (machineAdContexts != null)
            {
                BroadcastCollection = new MachineAdContext[machineAdContexts.Length];
                Array.Copy(machineAdContexts, BroadcastCollection, machineAdContexts.Length);
                log.LogVariableState("MachineADContext array: ", BroadcastCollection);
            }

            //DataTable dt = Utilities.executeDataTable("select m.machine_address, adb.* " +
            //                                        "from AdbroadCast adb, machines m " +
            //                                        "where Id = (select id from ( " +
            //                                        "select top 1 id, machineid, BroadcastFileName, BroadcastBeginTime " +
            //                                         "from AdBroadcast brd, Ads  " +
            //                                         "where BroadcastEndTime is null " +
            //                                         "and machineid = adb.machineid " +
            //                                         "and ads.AdId = brd.AdId " +
            //                                         "and ads.Active = 'Y' " +
            //                                         "order by BroadcastBeginTime desc) v1) " +
            //                                         "and m.machine_id = adb.machineId " +
            //                                         "and (m.machine_id = @machineId or @machineId = -1) " +
            //                                         "and master_id = @master_id",
            //                                        new SqlParameter("@machineId", forMachineId),
            //                                        new SqlParameter("@master_id", AccessPointId));
            //log.LogVariableState("@machineId", forMachineId);
            //log.LogVariableState("@master_id", AccessPointId);
            //if (dt.Rows.Count == 0)
            //{
            //    log.LogMethodExit(null);
            //    return;
            //}

            //BroadcastCollection = new MachineAdContext[dt.Rows.Count];
            //string filePath = Utilities.getParafaitDefaults("AD_IMAGE_DIRECTORY");
            //fileStreams = new FileStream[100];
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    MachineAdContext mac = new MachineAdContext(Utilities);
            //    mac.BroadcastId = Convert.ToInt32(dt.Rows[i]["Id"]);
            //    mac.FileName = dt.Rows[i]["BroadcastFileName"].ToString();
            //    mac.MachineAddress = dt.Rows[i]["Machine_Address"].ToString();
            //    mac.MachineId = Convert.ToInt32(dt.Rows[i]["MachineId"]);
            //    mac.fileStream = getfileStream(filePath + "\\" + mac.FileName);
            //    if (mac.fileStream == null)
            //        mac.ValidContext = false;
            //    else
            //    {
            //        mac.ValidContext = true;
            //        mac.fileSize = mac.fileStream.Length;
            //        mac.TotalPackets = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(mac.fileSize) / (PACKETSIZE))));

            //    }
            //    BroadcastCollection[i] = mac;
            //}
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Method to fetch the ad information 
        /// from Ad Management Entities
        /// </summary>
        /// <param name="AccessPointId">Hub id</param>
        /// <param name="forMachineId">Machine Id requiring ad details</param>
        /// <returns>Machine Ad Context array</returns>
        private MachineAdContext[] fetchAdManagementDetails(int AccessPointId, int forMachineId)
        {
            log.LogMethodEntry(AccessPointId, forMachineId);
            DataTable dt = Utilities.executeDataTable("select m.machine_address, adb.* " +
                                                    "from AdbroadCast adb, machines m " +
                                                    "where Id = (select id from ( " +
                                                    "select top 1 id, machineid, BroadcastFileName, BroadcastBeginTime " +
                                                     "from AdBroadcast brd, Ads  " +
                                                     "where BroadcastEndTime is null " +
                                                     "and machineid = adb.machineid " +
                                                     "and ads.AdId = brd.AdId " +
                                                     "and ads.Active = 'Y' " +
                                                     "order by BroadcastBeginTime desc) v1) " +
                                                     "and m.machine_id = adb.machineId " +
                                                     "and (m.machine_id = @machineId or @machineId = -1) " +
                                                     "and master_id = @master_id",
                                                    new SqlParameter("@machineId", forMachineId),
                                                    new SqlParameter("@master_id", AccessPointId));
            log.LogVariableState("Ad Query output", dt.Rows.Count);
            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit(null);
                return null;
            }

            MachineAdContext[] tempBroadcastCollection = new MachineAdContext[dt.Rows.Count];
            string filePath = Utilities.getParafaitDefaults("AD_IMAGE_DIRECTORY");
            log.LogVariableState("Ad Image directory path", filePath);
            fileStreams = new FileStream[100];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MachineAdContext mac = new MachineAdContext(Utilities);
                mac.BroadcastId = Convert.ToInt32(dt.Rows[i]["Id"]);
                mac.FileName = dt.Rows[i]["BroadcastFileName"].ToString();
                mac.MachineAddress = dt.Rows[i]["Machine_Address"].ToString();
                mac.MachineId = Convert.ToInt32(dt.Rows[i]["MachineId"]);
                mac.fileStream = getfileStream(filePath + "\\" + mac.FileName);
                if (mac.fileStream == null)
                    mac.ValidContext = false;
                else
                {
                    mac.ValidContext = true;
                    mac.fileSize = mac.fileStream.Length;
                    mac.TotalPackets = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(mac.fileSize) / (PACKETSIZE))));

                }
                tempBroadcastCollection[i] = mac;
            }
            log.LogMethodExit(tempBroadcastCollection);
            return tempBroadcastCollection;
        }

        FileStream getfileStream(string fileName)
        {
            log.LogMethodEntry(fileName);
            foreach (FileStream fs in fileStreams)
            {
                if (fs == null)
                    break;
                if (fs.Name == fileName)
                {
                    log.LogMethodExit(fs);
                    return fs;
                }
            }

            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fileStreams[fileStreamIndex++] = fs;
                log.LogMethodExit(fs);
                return fs;
            }
            catch(Exception ex)
            {
                log.Error("Error occured in accessing the File stream",ex);
                log.LogMethodExit(null);
                return null;
            }

        }

        /// <summary>
        /// AttachAdContext
        /// </summary>
        public void AttachAdContext()
        {
            log.LogMethodEntry();
            Machine machine = new Machine(executionContext, machineId);
            attachAdContext(new Machine[] { machine });
            log.LogMethodExit();
        }

        public void attachAdContext(Machine[] Machines)
        {
            log.LogMethodEntry(Machines);
            if (BroadcastCollection != null)
            {
                foreach (Machine m in Machines)
                {
                    foreach (MachineAdContext mac in BroadcastCollection)
                    {
                        if (mac.MachineId == m.MachineId)
                        {
                            m.machineAdContext = mac;
                            if (mac.AbandonContextRetryCount > MAXRETRYATTEMPTS)
                                mac.ValidContext = true;
                        }
                    }
                }
            }
            log.LogMethodExit(null);
        }

        public void Dispose()
        {
            log.LogMethodEntry();
            if (fileStreams != null)
            {
                foreach (FileStream fs in fileStreams)
                {
                    if (fs == null)
                        break;
                    else
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            log.LogMethodExit(null);
        }

        MachineAdShowContext[] AdShowContextCollection;

        /// <summary>
        /// AttachAdShowContext
        /// </summary>
        public void AdShowContext()
        {
            log.LogMethodEntry();
            getAdShowContext(accessPointId, machineId);
            log.LogMethodExit();
        }

        public void getAdShowContext(int AccessPointId, int forMachineId = -1)
        {
            log.LogMethodEntry(AccessPointId, forMachineId);
            DataTable dt = Utilities.executeDataTable(@"select m.machine_address, adb.* 
                                                    from AdbroadCast adb, machines m, Ads 
                                                     where BroadcastEndTime is not null 
                                                     and machine_id = adb.machineid 
                                                     and ads.AdId = adb.AdId 
                                                     and ads.Active = 'Y' 
                                                     and ads.AdType = 'A' 
                                                     and master_id = @master_id
                                                     and (m.machine_id = @machineId or @machineId = -1)
                                                     order by machineId, BroadcastFileName",
                                                     new SqlParameter("@master_id", AccessPointId),
                                                     new SqlParameter("@machineId", forMachineId));
            log.LogVariableState("@master_id", AccessPointId);
            log.LogVariableState("@machineId", forMachineId);

            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit(null);
                return;
            }

            int machineCount = 0;
            int prevMachineId = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int MachineId = Convert.ToInt32(dt.Rows[i]["MachineId"]);
                if (prevMachineId != MachineId)
                    machineCount++;
                prevMachineId = MachineId;
            }

            AdShowContextCollection = new MachineAdShowContext[machineCount];
            prevMachineId = -1;
            int index = -1;
            int fileCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int MachineId = Convert.ToInt32(dt.Rows[i]["MachineId"]);
                if (prevMachineId != MachineId)
                {
                    index++;
                    fileCount = 0;
                    MachineAdShowContext mac = new MachineAdShowContext();
                    mac.BroadcastId = Convert.ToInt32(dt.Rows[i]["Id"]);
                    string CurrentFile = dt.Rows[i]["BroadcastFileName"].ToString();
                    mac.AdFiles[fileCount] = new MachineAdShowContext.AdFile();
                    mac.AdFiles[fileCount].FromTime = GameServerEnvironment.AD_SHOW_WINDOW_START;
                    mac.AdFiles[fileCount].EndTime = GameServerEnvironment.AD_SHOW_WINDOW_END;
                    mac.AdFiles[fileCount++].FileName = CurrentFile;
                    mac.MachineAddress = dt.Rows[i]["Machine_Address"].ToString();
                    mac.MachineId = MachineId;
                    AdShowContextCollection[index] = mac;
                    prevMachineId = MachineId;
                }
                else
                {
                    if (fileCount > 4) // ignore more than 5 files
                        continue;
                    AdShowContextCollection[index].AdFiles[fileCount] = new MachineAdShowContext.AdFile();
                    AdShowContextCollection[index].AdFiles[fileCount].FromTime = GameServerEnvironment.AD_SHOW_WINDOW_START;
                    AdShowContextCollection[index].AdFiles[fileCount].EndTime = GameServerEnvironment.AD_SHOW_WINDOW_END;
                    AdShowContextCollection[index].AdFiles[fileCount++].FileName = dt.Rows[i]["BroadcastFileName"].ToString();
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// AttachAdShowContext
        /// </summary>
        public void AttachAdShowContext()
        {
            log.LogMethodEntry();
            Machine machine = new Machine(executionContext, machineId);
            attachAdShowContext(new Machine[] { machine });
            log.LogMethodExit();
        }

        public void attachAdShowContext(Machine[] Machines)
        {
            log.LogMethodEntry(Machines);
            if (AdShowContextCollection != null)
            {
                foreach (Machine m in Machines)
                {
                    foreach (MachineAdShowContext mac in AdShowContextCollection)
                    {
                        if (mac.MachineId == m.MachineId)
                        {
                            m.machineAdShowContext = mac;
                        }
                    }
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Refresh the ad contents and Machine Ad user context information
        /// </summary>
        /// <param name="AccessPointId">Hub Id</param>
        /// <param name="machineId">Machine Id</param>
        public void AdDataRefresh(int AccessPointId, int machineId)
        {
            log.LogMethodEntry(AccessPointId, machineId);
            Machine machine = new Machine(executionContext, machineId);
            AdContentDataRefresh(AccessPointId, machine);
            log.LogMethodExit();
        }

        /// <summary>
        /// Refresh the ad contents and Machine Ad user context information
        /// </summary>
        /// <param name="AccessPointId">Hub Id</param>
        /// <param name="machine">Machine Object</param>
        public void AdContentDataRefresh(int AccessPointId, Machine machine)
        {
            log.LogMethodEntry(AccessPointId, machine);
            MachineAdContext[] machineAdContexts;
            if (GameServerEnvironment != null)
            {
                machineAdContexts = fetchAdManagementDetails(AccessPointId, machine.MachineId);
                if (machineAdContexts != null)
                {
                    BroadcastCollection = new MachineAdContext[machineAdContexts.Length];
                    Array.Copy(machineAdContexts, BroadcastCollection, machineAdContexts.Length);
                    log.LogVariableState("MachineADContext array: ", BroadcastCollection);
                    attachAdContext(new Machine[] { machine });
                    getAdShowContext(AccessPointId, machine.MachineId);
                    attachAdShowContext(new Machine[] { machine });
                }
            }
            log.LogMethodExit();
        }
    }
}
