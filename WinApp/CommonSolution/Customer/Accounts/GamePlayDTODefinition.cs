/********************************************************************************************
 * Project Name - Customer.Accounts
 * Description  - Class for  of GamePlayDTODefinition      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Customer DTO definition class
    /// </summary>
    public class GamePlayDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        /// <param name="detailed"></param>
        public GamePlayDTODefinition(ExecutionContext executionContext, string fieldName, bool detailed) : base(fieldName, typeof(GamePlayDTO))
        {
            log.LogMethodEntry(executionContext, fieldName, detailed);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PlayDate", MessageContainerList.GetMessage(executionContext, "Date"), new DateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Game", MessageContainerList.GetMessage(executionContext, "Game"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Credits", MessageContainerList.GetMessage(executionContext, "Credits"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            if(detailed)
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CPCardBalance", MessageContainerList.GetMessage(executionContext, "CPCardBalance"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CPCredits", MessageContainerList.GetMessage(executionContext, "CPCredits"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CardGame", MessageContainerList.GetMessage(executionContext, "CardGame"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            }
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Courtesy", MessageContainerList.GetMessage(executionContext, "Courtesy"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Bonus", MessageContainerList.GetMessage(executionContext, "Bonus"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            if (detailed)
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CPBonus", MessageContainerList.GetMessage(executionContext, "CPBonus"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            }
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Time", MessageContainerList.GetMessage(executionContext, "Time"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TicketCount", MessageContainerList.GetMessage(executionContext, "Tickets"), new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ETickets", MessageContainerList.GetMessage(executionContext, "e-Tickets"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ManualTickets", MessageContainerList.GetMessage(executionContext, "Manual Tickets"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TicketEaterTickets", MessageContainerList.GetMessage(executionContext, "T.Eater Tickets"), new DoubleValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Mode", MessageContainerList.GetMessage(executionContext, "Mode"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Site", MessageContainerList.GetMessage(executionContext, "Site"), new StringValueConverter()));
            log.LogMethodExit();
        }
    }
}
