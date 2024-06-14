/********************************************************************************************
 * Project Name - Game Module
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.40        29-Nov-2018   Jagan Mohana Rao          Created
 *2.60        28-Mar-2019   Mushahid Faizan           Modified Literals.
 *2.80        12-Aug-2019   Deeksha                   Added logger methods.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Localization
{
    public class GamesLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for Games Locallization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public GamesLocalizationBL(ExecutionContext executionContext, string entityName)
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

        private void GetLiteralsAndMessages(string entity, List<string> literalsOrMessageList)
        {
            log.LogMethodEntry(entity, literalsOrMessageList);
            string localizedValue = "";
            foreach (string literalsOrMessages in literalsOrMessageList)
            {
                localizedValue = MessageContainerList.GetMessage(executionContext, literalsOrMessages);
                if (!listHeadersList.ContainsKey(literalsOrMessages))
                {
                    listHeadersList.Add(literalsOrMessages, localizedValue);
                }
            }
            log.LogMethodExit();
        }

        public string GetLocalizedLabelsAndHeaders()
        {
            log.LogMethodEntry();
            string literalsMessagesList = string.Empty;
            if (!string.IsNullOrEmpty(entityName))
            {
                List<string> literalsOrMessage = GetLiterals(entityName);
                GetLiteralsAndMessages(entityName, literalsOrMessage);
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
                case "HUB":
                    literals.Add("Hub Id");
                    literals.Add("Hub Name");
                    literals.Add("Port Number");
                    literals.Add("Baud Rate");
                    literals.Add("Address");
                    literals.Add("Frequency");
                    literals.Add("Notes");
                    literals.Add("Server Machine");
                    literals.Add("Direct Mode");
                    literals.Add("MAC Address");
                    literals.Add("IP Address");
                    literals.Add("TCP Port");
                    literals.Add("Configure");
                    literals.Add("Hubs");
                    literals.Add("Selected Site");
                    literals.Add("Please select a site");
                    literals.Add("Configure Hub");
                    literals.Add("NRF");
                    literals.Add("SP1ML");
                    literals.Add("Hub Address");
                    literals.Add("Configure");
                    literals.Add("Data Rate");
                    literals.Add("Advanced");
                    literals.Add("Register");
                    literals.Add("Value");
                    literals.Add("Set");
                    literals.Add("Read All Config");
                    literals.Add("IsEBYTE");
                    literals.Add("EBYTE Configuration");
                    literals.Add("UART Parity");
                    literals.Add("Frequency/Channel");
                    literals.Add("Transmission Mode");
                    literals.Add("IO Drive Mode");
                    literals.Add("Wakeup Time");
                    literals.Add("FEC Switch");
                    literals.Add("Output Power");
                    break;
                case "HUBAUDIT":
                    literals.Add("Audit Log");
                    literals.Add("Hub");
                    literals.Add("Timestamp");
                    literals.Add("DateOfLog");
                    literals.Add("Type");
                    literals.Add("UserName");
                    literals.Add("master_id");
                    literals.Add("master_name");
                    literals.Add("port_number");
                    literals.Add("baud_rate");
                    literals.Add("notes");
                    literals.Add("active_flag");
                    literals.Add("address");
                    literals.Add("frequency");
                    literals.Add("ServerMachine");
                    literals.Add("DirectMode");
                    literals.Add("Guid");
                    literals.Add("site_id");
                    literals.Add("SynchStatus");
                    literals.Add("IPAddress");
                    literals.Add("TCPPort");
                    literals.Add("MACAddress");
                    literals.Add("RestartAP");
                    literals.Add("MasterEntityId");
                    literals.Add("MACAddress");
                    literals.Add("IsEBYTE");
                    break;
                case "GAME_PROFILE":
                    literals.Add("Game Profile Id");
                    literals.Add("Profile Name");
                    literals.Add("Normal Price");
                    literals.Add("VIP Price");
                    literals.Add("Credit Allowed");
                    literals.Add("Bonus Allowed");
                    literals.Add("Courtesy Allowed");
                    literals.Add("Time Allowed");
                    literals.Add("Ticket Allowed On Credit");
                    literals.Add("Ticket Allowed On Courtesy");
                    literals.Add("Ticket Allowed On Bonus");
                    literals.Add("Ticket Allowed On Time");
                    literals.Add("Token Redemption Machine");
                    literals.Add("Token Price");
                    literals.Add("Redeem Token To");
                    literals.Add("Force Redeem To Card");
                    literals.Add("User Identifier");
                    literals.Add("Profile Identifier");
                    break;
                case "GAME_PROFILE_AUDIT":
                    literals.Add("Audit Log");
                    literals.Add("Timestamp");
                    literals.Add("DateOfLog");
                    literals.Add("Type");
                    literals.Add("UserName");
                    literals.Add("User");
                    literals.Add("notes");
                    literals.Add("game_profile_id");
                    literals.Add("profile_name");
                    literals.Add("courtesy_allowed");
                    literals.Add("time_allowed");
                    literals.Add("ticket_allowed_on_credit");
                    literals.Add("ticket_allowed_on_courtesy");
                    literals.Add("ticket_allowed_on_bonus");
                    literals.Add("ticket_allowed_on_time");
                    literals.Add("play_credits");
                    literals.Add("vip_play_credits");
                    literals.Add("last_updated_date");
                    literals.Add("last_updated_user");
                    literals.Add("TokenRedemption");
                    literals.Add("InternetKey");
                    literals.Add("PhysicalToken");
                    literals.Add("TokenPrice");
                    literals.Add("RedeemTokenTo");
                    literals.Add("bonus_allowed");
                    literals.Add("ThemeNumber");
                    literals.Add("ShowAd");
                    literals.Add("TicketEater");
                    literals.Add("credit_allowed");
                    literals.Add("UserIdentifier");
                    literals.Add("CustomDataSetId");
                    literals.Add("ThemeId");
                    literals.Add("ForceRedeemToCard");
                    literals.Add("ISActive");
                    literals.Add("ProfileIdentifier");
                    break;
                case "GAMES":
                    literals.Add("Games");
                    literals.Add("Game Id");
                    literals.Add("Game Name");
                    literals.Add("Game Description");
                    literals.Add("Game Profile");
                    literals.Add("Profile Normal Price");
                    literals.Add("Profile VIP Price");
                    literals.Add("Normal Price");
                    literals.Add("VIP Price");
                    literals.Add("Repeat Play Discount %");
                    literals.Add("User Identifier");
                    literals.Add("Game Company Name");
                    literals.Add("Game Tag");
                    break;
                case "GAMESAUDIT":
                    literals.Add("Audit Log");
                    literals.Add("Timestamp");
                    literals.Add("DateOfLog");
                    literals.Add("Type");
                    literals.Add("UserName");
                    literals.Add("notes");
                    literals.Add("Games");
                    literals.Add("game_id");
                    literals.Add("game_name");
                    literals.Add("game_description");
                    literals.Add("game_company_name");
                    literals.Add("play_credits");
                    literals.Add("vip_play_credits");
                    literals.Add("game_profile_id");
                    literals.Add("InternetKey");
                    literals.Add("repeat_play_discount");
                    literals.Add("UserIdentifier");
                    literals.Add("ProductId");
                    literals.Add("CustomDataSetId");
                    literals.Add("ISActive");
                    literals.Add("GameTag");
                    break;
                case "MACHINES":
                    literals.Add("Machine Status");
                    literals.Add("Machines");
                    literals.Add("Machine Id");
                    literals.Add("Game Name");
                    literals.Add("Machine Name");
                    literals.Add("Hub Name");
                    literals.Add("Hub Address");
                    literals.Add("Machine Address");
                    literals.Add("Effective Machine Address");
                    literals.Add("MAC Address");
                    literals.Add("Reference Machine");
                    literals.Add("Reader Type");
                    literals.Add("Description");
                    literals.Add("Serial Number");
                    literals.Add("Machine Tag");
                    literals.Add("Software Version");
                    literals.Add("Ticket Mode");
                    literals.Add("Normal Price");
                    literals.Add("VIP Price");
                    literals.Add("Ticket Allowed");
                    literals.Add("Timer Machine");
                    literals.Add("Interval (Sec.)");
                    literals.Add("Group Timer");
                    literals.Add("Purchase Price");
                    literals.Add("Inventory Location");
                    literals.Add("Payout Cost");
                    literals.Add("External Machine Reference");
                    literals.Add("Active Machines Only"); //Active Machines only
                    literals.Add("Game Plays");
                    literals.Add("Previous Machine");
                    literals.Add("Next Machine");
                    literals.Add("Transfer Machine");
                    literals.Add("Input Devices");

                    literals.Add("Machine Arrival Date"); // Added on 16-Apr-2020 by Mushahid Faizan

                    break;
                case "MACHINESAUDIT":
                    literals.Add("Audit Log");
                    literals.Add("DateOfLog");
                    literals.Add("Type");
                    literals.Add("UserName");
                    literals.Add("Timestamp");
                    literals.Add("machines");
                    literals.Add("machine_id");
                    literals.Add("machine_name");
                    literals.Add("machine_address");
                    literals.Add("game_id");
                    literals.Add("master_id");
                    literals.Add("timer_machine");
                    literals.Add("timer_interval");
                    literals.Add("group_timer");
                    literals.Add("number_of_coins");
                    literals.Add("ticket_allowed");
                    literals.Add("ticket_mode");
                    literals.Add("Description");
                    literals.Add("CustomDataSetId");
                    literals.Add("ThemeNumber");
                    literals.Add("ShowAd");
                    literals.Add("IPAddress");
                    literals.Add("TCPPort");
                    literals.Add("MACAddress");
                    literals.Add("SerialNumber");
                    literals.Add("SoftwareVersion");
                    literals.Add("PurchasePrice");
                    literals.Add("ReaderType");
                    literals.Add("PayoutCost");
                    literals.Add("InventoryLocationId");
                    literals.Add("ReferenceMachineId");
                    literals.Add("ExternalMachineReference");
                    literals.Add("ThemeId");
                    literals.Add("MachineTag");
                    literals.Add("PreviousMachineId");
                    literals.Add("NextMachineId");
                    literals.Add("CommunicationSuccessRatio");
                    literals.Add("active_flag");
                    literals.Add("notes");
                    literals.Add("MachineArrivalDate");
                    break;
                case "GENERICCALENDER":
                    literals.Add("Generic Calendar- THEME");
                    literals.Add("Id"); //id
                    literals.Add("Calendar Type");
                    literals.Add("Game Profile");
                    literals.Add("Machines");
                    literals.Add("Day");
                    literals.Add("From Time");
                    literals.Add("To Time");
                    literals.Add("Theme Number");
                    literals.Add("Theme");
                    literals.Add("THEME");
                    literals.Add("Enable Out Of Service");
                    break;

                case "TRANSFERMACHINE":
                    literals.Add("Inter-Site Machine Transfer");
                    literals.Add("Assign Game");
                    literals.Add("Transfer Machine");
                    literals.Add("Transfer");
                    literals.Add("From Site");
                    literals.Add("To Site");
                    break;
                case "READERTHEMES":
                    literals.Add("Reader Themes");
                    literals.Add("Theme Id");
                    literals.Add("Theme Type"); //theme type
                    literals.Add("Theme Name");
                    literals.Add("Theme Number");
                    break;
                case "INPUTDEVICES":
                    literals.Add("Device Id");
                    literals.Add("Device Name");
                    literals.Add("Device Type");
                    literals.Add("Device Model");
                    literals.Add("Port No");
                    literals.Add("Template Format");
                    literals.Add("Devices");
                    literals.Add("Active");
                    break;
                case "CUSTOMATTRIBUTES":
                    literals.Add("Custom Attributes");
                    literals.Add("Custom Attributes for &1");
                    literals.Add("Id");//id
                    literals.Add("Attribute Name");
                    literals.Add("Sequence");
                    literals.Add("Type");
                    literals.Add("Applicability");
                    literals.Add("Access");
                    literals.Add("Attribute Values");
                    literals.Add("Value");
                    literals.Add("Default?");
                    literals.Add("Games");
                    literals.Add("Game Profile");
                    literals.Add("Machines");
                    literals.Add("Location");
                    break;
                case "GAMEPLAYSCARDACTIVITY":
                    literals.Add("Game Play details [recent 1000] for Machine");
                    literals.Add("GameplayId");
                    literals.Add("MachineId");
                    literals.Add("CardId");
                    literals.Add("CardNumber");
                    literals.Add("Credits");
                    literals.Add("Courtesy");
                    literals.Add("Bonus");
                    literals.Add("PlayDate");
                    literals.Add("Notes");
                    literals.Add("TicketCount");
                    literals.Add("TicketMode");
                    literals.Add("Guid");
                    literals.Add("SiteId");
                    literals.Add("SynchStatus");
                    literals.Add("CardGame");
                    literals.Add("CPCardBalance");
                    literals.Add("CPCredits");
                    literals.Add("CPBonus");
                    literals.Add("CardGameId");
                    literals.Add("PayoutCost");
                    literals.Add("MasterEntityId");
                    literals.Add("Game");
                    literals.Add("ETickets");
                    literals.Add("ManualTickets");
                    literals.Add("TicketEaterTickets");
                    literals.Add("Mode");
                    literals.Add("Site");
                    literals.Add("TaskId");
                    literals.Add("IsChanged");
                    literals.Add("Card Activity for Card Number");
                    literals.Add("Purchases and Tasks");
                    literals.Add("Show Extended");
                    literals.Add("Consolidated View");
                    literals.Add("Export To Excel");
                    literals.Add("AccountId");
                    literals.Add("Date");
                    literals.Add("Amount");
                    literals.Add("Credits");
                    literals.Add("Courtesy");
                    literals.Add("Bonus");
                    literals.Add("Time");
                    literals.Add("Tokens");
                    literals.Add("Tickets");
                    literals.Add("LoyaltyPoints");
                    literals.Add("Site");
                    literals.Add("POS");
                    literals.Add("UserName");
                    literals.Add("Quantity");
                    literals.Add("Price");
                    literals.Add("RefId");
                    literals.Add("ActivityType");
                    break;
                case "DISPLAYSCHEDULE":
                    literals.Add("Display Schedule");
                    literals.Add("Filter");
                    literals.Add("Show Only Active Entries");
                    literals.Add("Schedule Name");
                    literals.Add("Machine");
                    literals.Add("Date");
                    literals.Add("Search");
                    literals.Add("Schedule Id");
                    literals.Add("Schedule Name");
                    literals.Add("Start Time");
                    literals.Add("End Time");
                    literals.Add("Recur Flag");
                    literals.Add("Recur Frequency");
                    literals.Add("Recur End Date");
                    literals.Add("Recur Type");
                    literals.Add("Active?");
                    literals.Add("New");
                    literals.Add("Refresh");
                    literals.Add("Close");
                    literals.Add("Machine Theme Map");
                    literals.Add("Schedule");
                    literals.Add("Recurrence");
                    literals.Add("Active");
                    literals.Add("Recurrence Type");
                    literals.Add("Show Active Only");
                    literals.Add("Theme");
                    literals.Add("Machine");
                    literals.Add("Game");
                    literals.Add("Game Profile");
                    literals.Add("Machine Theme Mappings");
                    literals.Add("SI#");
                    literals.Add("Incl/Excl");
                    break;
                case "MACHINEBULKUPLOAD":
                    literals.Add("Machine Id");
                    literals.Add("Game Name");
                    literals.Add("Machine Name");
                    literals.Add("Hub Name");
                    literals.Add("Mac Address");
                    literals.Add("TCP Port");
                    literals.Add("IP Address");
                    literals.Add("Reference Machine");
                    literals.Add("Reader Type");
                    literals.Add("Description");
                    literals.Add("Serial Number");
                    literals.Add("Machine Tag");
                    literals.Add("Software Version");
                    literals.Add("Ticket Mode");
                    literals.Add("Ticket Allowed");
                    literals.Add("Interval(Sec.)");
                    literals.Add("Group Timer");
                    literals.Add("Purchase Price");
                    literals.Add("Notes");
                    literals.Add("Theme");
                    literals.Add("Payout Cost");
                    literals.Add("External Machine Reference");
                    literals.Add("Start In Physical Ticket Mode");
                    literals.Add("Number Of Coins");
                    literals.Add("Ticket Pulse Width");
                    literals.Add("Ticket Pulse Gap");
                    literals.Add("Power On Ticket Delay");
                    literals.Add("Default Theme");
                    literals.Add("Ticket Eater Card Wait Interval");
                    literals.Add("Start Screen Number");
                    literals.Add("Balance Delay");
                    literals.Add("Enable Reset Pulse");
                    literals.Add("Min Seconds Between Repeat Play");
                    literals.Add("Queue Setup Required");
                    literals.Add("Enable Ext Antenna");
                    literals.Add("Out Of Service Theme");
                    literals.Add("Free Play Theme");
                    literals.Add("Initial Led Pattern");
                    literals.Add("Ticket Delay");
                    literals.Add("Show Big Balance");
                    literals.Add("Ad Interval");
                    literals.Add("GamePlay Multiplier");
                    literals.Add("Ticket Multiplier");
                    literals.Add("Show Ad");
                    literals.Add("Coin Pulse Width");
                    literals.Add("Coin Pulse Gap");
                    literals.Add("Sensor Interval");
                    literals.Add("Disable Tickets");
                    literals.Add("Show Static Ad");
                    literals.Add("Ticket Eater");
                    literals.Add("Ticket Eater Ticket Wait Interval");
                    literals.Add("Coin Pusher Machine");
                    literals.Add("Debug Mode");
                    literals.Add("Card Retries");
                    literals.Add("Display Language");
                    literals.Add("Max Tickets Per Game");
                    literals.Add("Out Of Service");
                    literals.Add("GamePlay Duration");
                    literals.Add("Coin Interrupt Delay");
                    literals.Add("Reader Volume");
                    literals.Add("Enable Invalid Light");
                    literals.Add("Ad Delay");
                    literals.Add("Ad Impression");
                    literals.Add("Audio Theme Number");
                    literals.Add("Attendance Reader");
                    break;
            }
            log.LogMethodExit(literals);
            return literals;
        }
    }
}
