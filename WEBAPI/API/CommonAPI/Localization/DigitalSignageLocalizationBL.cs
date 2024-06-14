/********************************************************************************************
 * Project Name - DigitalSignage Module
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        26-Dec-2018   Jagan Mohana Rao          Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Localization
{
    public class DigitalSignageLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for DigitalSignage Locallization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public DigitalSignageLocalizationBL(ExecutionContext executionContext, string entityName)
        {
            log.LogMethodEntry(executionContext, entityName);
            this.executionContext = executionContext;
            this.entityName = entityName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Getting lable messageno and headers
        /// </summary>
        /// <returns>json</returns>

        private void GetLiteralsAndMessages(string entity, List<string> literalsOrMessageList, ref Dictionary<string, string> listHeadersList)
        {
            string localizedValue = string.Empty;
            foreach (string literalsOrMessages in literalsOrMessageList)
            {
                localizedValue = MessageContainerList.GetMessage(executionContext, literalsOrMessages);
                if (!listHeadersList.ContainsKey(literalsOrMessages))
                {
                    listHeadersList.Add(literalsOrMessages, localizedValue);
                }
            }
        }

        public string GetLocalizedLabelsAndHeaders()
        {
            log.LogMethodEntry();            
            string literalsMessagesList = string.Empty;
            if (!string.IsNullOrEmpty(entityName))
            {                
                List<string> literalsOrMessage = GetLiterals(entityName);
                GetLiteralsAndMessages(entityName, literalsOrMessage, ref listHeadersList);
                if (listHeadersList.Count != 0)
                {
                    literalsMessagesList = JsonConvert.SerializeObject(listHeadersList, Formatting.Indented);
                }
            }
            log.LogMethodExit(literalsMessagesList);
            return literalsMessagesList;
        }

        private List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            List<string> literals = new List<string>();
            switch (entityName.ToUpper().ToString())
            {
                case "CONTENTMEDIA":
                    literals.Add("Media Id");
                    literals.Add("Type");
                    literals.Add("FileName");
                    literals.Add("Media Details");
                    literals.Add("Media Name");
                    literals.Add("Browse");
                    literals.Add("Previous");
                    literals.Add("Next");
                    literals.Add("Media");
                    literals.Add("Enter URL here");
                    break;
                case "CONTENTLIST":
                    literals.Add("Move Data(Secs)");
                    literals.Add("Offset - X Axis(in pixels)");
                    literals.Add("Offset - Y Axis(in pixels)");
                    literals.Add("HDR 1 & HDR 2");
                    literals.Add("HDR 2 & HDR 3");
                    literals.Add("HDR 3 & HDR 4");
                    literals.Add("HDR 4 & HDR 5");
                    literals.Add("Header 1");
                    literals.Add("Header 2");
                    literals.Add("Header 3");
                    literals.Add("Header 4");
                    literals.Add("Header 5");
                    literals.Add("Hdr-1 Text Color");
                    literals.Add("Hdr-2 Text Color");
                    literals.Add("Hdr-3 Text Color");
                    literals.Add("Hdr-4 Text Color");
                    literals.Add("Hdr-5 Text Color");
                    literals.Add("Hdr-1 Font");
                    literals.Add("Hdr-2 Font");
                    literals.Add("Hdr-3 Font");
                    literals.Add("Hdr-4 Font");
                    literals.Add("Hdr-5 Font");
                    literals.Add("Hdr-1 Alignment");
                    literals.Add("Hdr-2 Alignment");
                    literals.Add("Hdr-3 Alignment");
                    literals.Add("Hdr-4 Alignment");
                    literals.Add("Hdr-5 Alignment");
                    literals.Add("Hdr-1 Back Color");
                    literals.Add("Hdr-2 Back Color");
                    literals.Add("Hdr-3 Back Color");
                    literals.Add("Hdr-4 Back Color");
                    literals.Add("Hdr-5 Back Color");
                    literals.Add("Spacing in Pixels");
                    literals.Add("List");
                    literals.Add("Lookup Values");
                    literals.Add("Lookup List");
                    literals.Add("Dynamic List Query");
                    literals.Add("ID");
                    literals.Add("Name");
                    literals.Add("Dynamic Flag");
                    literals.Add("Font");
                    literals.Add("Select Font Style");
                    literals.Add("Select Font Size");
                    literals.Add("OK");
                    break;
                case "CONTENTTICKER":
                    literals.Add("Tickers");
                    literals.Add("Ticker Name");
                    literals.Add("Ticker Text");
                    literals.Add("Text Color");
                    literals.Add("Scroll Direction");
                    literals.Add("Font");
                    literals.Add("Back Color");
                    literals.Add("Ticker Speed (Number)");
                    break;
                case "EVENT":
                    literals.Add("Event Definition");
                    literals.Add("Event Type");
                    literals.Add("Parameter");
                    literals.Add("Events");
                    literals.Add("Name");
                    literals.Add("Dynamic Event Query");
                    break;
                case "SETUPPANEL":
                    literals.Add("Panel");
                    literals.Add("Panel Name");
                    literals.Add("Location");
                    literals.Add("PC Name");
                    literals.Add("Shutdown Sec");
                    literals.Add("Horizontal (Pixels)");
                    literals.Add("Vertical (Pixels)");
                    literals.Add("Display Group");
                    literals.Add("Local Folder");
                    literals.Add("Preview");
                    literals.Add("Start PC");
                    literals.Add("Shutdown PC");
                    literals.Add("Restart PC");
                    literals.Add("Display Group Filter");
                    literals.Add("Time");
                    literals.Add("As On");
                    literals.Add("Shoutdown?");
                    literals.Add("End Time should be greater than Start Time");
                    literals.Add("Panels");
                    break;
                case "SETUPSCREENDESIGN":
                    literals.Add("Screen Designs");
                    literals.Add("Screen Name");
                    literals.Add("Alignment");
                    literals.Add("Rows");
                    literals.Add("Columns");
                    literals.Add("Add/Update Zones");
                    literals.Add("Content Map");
                    literals.Add("Screen Content Map");
                    break;
                case "SETUPSCREENDESIGNZONE":
                    literals.Add("Zone Details");
                    literals.Add("Screen ID");
                    literals.Add("Name");//Added on 08-Apr-2019
                    literals.Add("Top Left");
                    literals.Add("Bottom Right");
                    literals.Add("Top Offset");
                    literals.Add("Bottom Offset");
                    literals.Add("Left Offset");
                    literals.Add("Right Offset");
                    literals.Add("View Zone");
                    break;
                case "SETUPSCREENDESIGNCONTENTMAP":
                    literals.Add("Screen Content Map");
                    literals.Add("Screen ID");
                    literals.Add("GetZone");
                    literals.Add("Zones");
                    literals.Add("Name");//Added on 08-Apr-2019
                    literals.Add("Content");
                    literals.Add("Content Type");
                    literals.Add("Background Image");
                    literals.Add("Back Color");
                    literals.Add("Refresh(Secs)");
                    literals.Add("Video Repeat");
                    literals.Add("Lookup Refresh(Secs)");
                    literals.Add("Ticker Scroll Direction");
                    literals.Add("Ticker Speed");
                    literals.Add("Ticker Refresh(Secs)");
                    break;
                case "SCREENTRANSITION":
                    literals.Add("Screen Transitions");
                    literals.Add("Theme Name");
                    literals.Add("Theme Type");
                    literals.Add("Initial Screen");
                    literals.Add("From Screen");
                    literals.Add("To Screen");
                    literals.Add("Event");
                    literals.Add("Theme List");
                    literals.Add("Theme Number");
                    break;
                case "SIGNAGESCHEDULE":
                    literals.Add("Signage Schedules");
                    literals.Add("Schedule Id");
                    literals.Add("Schedule Name");
                    literals.Add("Recur Flag");
                    literals.Add("Recur Frequency");
                    literals.Add("Recur End Date");
                    literals.Add("Recur Type");
                    literals.Add("Panel");
                    literals.Add("Date");
                    literals.Add("Panel Theme Mappings");
                    literals.Add("Recurrence Type");
                    literals.Add("Theme");
                    literals.Add("Active");
                    literals.Add("Recurrence");
                    literals.Add("Incl/Excl");
                    literals.Add("Continue");
                    literals.Add("Schedule");
                    literals.Add("Schedule Exclusion");
                    break;
                case "SIGNAGESCHEDULEEXCLUSION":
                    literals.Add("Schedule Exclusion");
                    literals.Add("Schedule Name");
                    literals.Add("Exclusion Id");
                    literals.Add("Exclusion Date");
                    literals.Add("Day");
                    literals.Add("Include Date?");
                    break;
                case "CONTENTPATTERN":
                    literals.Add("Name");
                    literals.Add("Pattern");
                    break;
            }
            log.LogMethodExit(literals);
            return literals;
        }
    }
}