/********************************************************************************************
 * Project Name - CustomerGamePlayLevelResultBL                                                                          
 * Description  - Business logic  class to manipulate game machine results level details
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     09-Feb-2021   Fiona              Created : Virtual Arcade changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// VirtualArcadeBL
    /// </summary>
    public class VirtualArcadeBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public VirtualArcadeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetGameMachineImages
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public List<string> GetGameMachineImages(string machineName, string fileName)
        {
            log.LogMethodEntry(machineName);

            List<string> result = new List<string>();
            string sharedFolderPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_PLAY_UPLOAD_DIRECTORY");
            string siteUrl = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_ARCADE_IMAGE_SITE_URL");
            string filePath = sharedFolderPath + "\\" + machineName + "\\";
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(filePath);
                FileInfo[] files = dInfo.GetFiles().Where(f => f.Extension == ".png" || f.Extension == ".jpg").ToArray();
                string str = string.Empty;
                if(files == null || files.Count() == 0)
                {
                    log.Debug("No files found for this machine");
                    log.LogMethodExit(result);
                    return result;
                }
                foreach (FileInfo file in files)
                {
                    str = siteUrl + machineName + "/" + file.Name;
                    result.Add(str);
                }
                if (string.IsNullOrEmpty(fileName) == false)
                {
                    result = result.Where(x => x.EndsWith(fileName)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(result);
                return result;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetGameMachineTranslations
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public List<string> GetGameMachineTranslations(string machineName, string fileName)
        {
            log.LogMethodEntry(machineName, fileName);
            List<string> result = new List<string>();
            try
            {
                string sharedFolderPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_PLAY_UPLOAD_DIRECTORY");
                string siteUrl = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_ARCADE_IMAGE_SITE_URL");
                string filePath = sharedFolderPath + "\\" + machineName + "\\";
                DirectoryInfo dInfo = new DirectoryInfo(filePath);
                FileInfo[] Files = dInfo.GetFiles("*.json");
                if (Files == null || Files.Count() == 0)
                {
                    log.Debug("No files found for this machine");
                    log.LogMethodExit(result);
                    return result;
                }
                string str = "";
                foreach (FileInfo file in Files)
                {
                    str = siteUrl + machineName + "/" + file.Name;
                    result.Add(str);
                }
                if (string.IsNullOrEmpty(fileName) == false)
                {
                    result = result.Where(x => x.EndsWith(fileName)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(result);
                return result;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetMachineFile
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetMachineFile(string machineName, string fileName)
        {
            log.LogMethodEntry(machineName, fileName);
            string sharedFolderPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_PLAY_UPLOAD_DIRECTORY");
            string filePath = sharedFolderPath + "\\" + machineName + "\\" + fileName;
            if (File.Exists(filePath))
            {
                if (fileName.Contains(".json"))
                {
                    string text = File.ReadAllText(filePath);
                    string details = JsonConvert.DeserializeObject(text).ToString();
                    log.LogMethodExit(details);
                    return details;
                }
                else if (fileName.Contains(".png") || fileName.Contains(".jpg"))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    log.LogMethodExit(base64ImageRepresentation);
                    return base64ImageRepresentation;
                }
            }
            return null;
        }

    }
}
