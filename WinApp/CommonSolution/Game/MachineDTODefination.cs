/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Bulk Upload Mapper MachineDTODefination Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created 
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/

using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Reflection;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Machine DTO Defination Class
    /// </summary>
    public class MachineDTODefination : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public MachineDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(MachineDTO))
        {
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MachineId", "Machine Id", new ForeignKeyValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MachineName", "Machine Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MachineAddress", "Machine Address", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("GameId", "Game Name", new GameValueConverter(executionContext)));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MasterId", "Hub Name", new HubValueConverter(executionContext)));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Notes", "Notes", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TicketAllowed", "Ticket Allowed", new GameDefaultTrueValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "isActive", new GameDefaultTrueValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TimerMachine", "Timer Machine", new GameDefaultFalseValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TimerInterval", "Timer Interval", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("GroupTimer", "Group Timer", new GameDefaultFalseValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("NumberOfCoins", "Number Of Coins", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TicketMode", "Ticket Mode", new TicketModeValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ThemeId", "Theme Name", new ThemeValueConverter(executionContext)));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ThemeNumber", "Theme Number", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShowAd", "ShowAd", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IPAddress", "IPAddress", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TCPPort", "TCPPort", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MacAddress", "MacAddress", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "Description", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("SerialNumber", "SerialNumber", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MachineTag", "Machine Tag", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("SoftwareVersion", "Software Version", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchasePrice", "Purchase Price", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReaderType", "Reader Type", new ReaderTypeValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PayoutCost", "Payout Cost", new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExternalMachineReference", "External Machine Reference", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReferenceMachineId", "Reference Machine", new MachineValueConverter(executionContext)));

            attributeDefinitionList.Add(new MachineAttributesDefinationDTO(executionContext, "GameMachineAttributes"));
        }

        /// <summary>
        /// Deserialize method to deserialize sheet rows
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow, row, currentIndex);
            object result = null;
            if (headerRow.Cells.Count > currentIndex &&
                DisplayName == headerRow[currentIndex].Value)
            {
                result = classType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                int startIndex = currentIndex;
                foreach (var attributeDefinition in attributeDefinitionList)
                {
                    object value = attributeDefinition.Deserialize(headerRow, row, ref currentIndex);
                    PropertyInfo propertyInfo = classType.GetProperty(attributeDefinition.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    propertyInfo.SetValue(result, value);
                }
                int endIndex = currentIndex;
                if (startIndex < endIndex)
                {
                    bool foundData = false;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (string.IsNullOrWhiteSpace(row[i].Value) == false)
                        {
                            foundData = true;
                        }
                    }
                    if (foundData == false)
                    {
                        result = null;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;

        }

    }
}
