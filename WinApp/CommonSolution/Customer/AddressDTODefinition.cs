//using Semnox.Core.Profile;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Address DTO Definition
    /// </summary>
    public class AddressDTODefinition:ComplexAttributeDefinition
    {
        private Semnox.Core.Utilities.ExecutionContext executionContext;
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public AddressDTODefinition(Semnox.Core.Utilities.ExecutionContext executionContext, string fieldName):base(fieldName, typeof(AddressDTO))
        {
            this.executionContext = executionContext;
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("AddressType", "Address Type", new EnumValueConverter(typeof(AddressType))));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Line1", "Line 1", new StringValueConverter()));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Line2", "Line 2", new StringValueConverter()));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Line3", "Line 3", new StringValueConverter()));
            }    
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("City", "City", new StringValueConverter()));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("PostalCode", "Postal Code", new StringValueConverter()));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("StateId", "State", new StateValueConverter(executionContext)));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CountryId", "Country", new CountryValueConverter(executionContext)));
            }
            
        }
    }
}
