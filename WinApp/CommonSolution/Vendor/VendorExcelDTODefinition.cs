/********************************************************************************************
 * Project Name - Vendor 
 * Description  - VendorExcelDTODefinition  object of vendor Excel DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    14-Oct-2020         Mushahid Faizan        Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
//using Semnox.Parafait.Product;

namespace Semnox.Parafait.Vendor
{
    public class VendorExcelDTODefinition : ComplexAttributeDefinition
    {

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public VendorExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(VendorDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorId", "VendorId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Name", "Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Website", "Website", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorCode", "Code", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxRegistrationNumber", "VAT/Tax No.", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseTaxId", "PurchaseTaxId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorMarkupPercent", "VendorMarkupPercent", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("GoodsReturnPolicy", "GoodsReturnPolicy", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DefaultPaymentTermsId", "Payment Terms", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("CountryId", "Country", new CountryValueConverter(executionContext)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("StateId", "StateId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ContactName", "ContactName", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Email", "Email", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Phone", "Phone", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Fax", "Fax", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Address1", "Address1", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Address2", "Address2", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("City", "City", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("State", "State", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PostalCode", "Zip/Pin Code", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Country", "Country", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AddressRemarks", "AddressRemarks", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "LastUpdateDate", new NullableDateTimeValueConverter()));

        }
    }

    //class TaxValueConverter : ValueConverter
    //{
    //    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    private ExecutionContext executionContext;
    //    List<KeyValuePair<int, TaxDTO>> taxIdtaxDTOKeyValuePair;
    //    List<KeyValuePair<string, TaxDTO>> taxNameTaxDTOKeyValuePair;

    //    /// <summary>
    //    /// Parameterized constructor
    //    /// </summary>
    //    /// <param name="executionContext"></param>
    //    public TaxValueConverter(ExecutionContext executionContext)
    //    {
    //        log.LogMethodEntry(executionContext);
    //        this.executionContext = executionContext;
    //        taxNameTaxDTOKeyValuePair = new List<KeyValuePair<string, TaxDTO>>();
    //        taxIdtaxDTOKeyValuePair = new List<KeyValuePair<int, TaxDTO>>();
    //        List<TaxDTO> taxDTOList = new List<TaxDTO>();

    //        TaxList taxList = new TaxList(executionContext);
    //        List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
    //        searchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
    //        taxDTOList = taxList.GetAllTaxes(searchParams);
    //        if (taxDTOList != null && taxDTOList.Count > 0)
    //        {
    //            foreach (TaxDTO taxDTO in taxDTOList)
    //            {
    //                taxIdtaxDTOKeyValuePair.Add(new KeyValuePair<int, TaxDTO>(taxDTO.TaxId, taxDTO));
    //                taxNameTaxDTOKeyValuePair.Add(new KeyValuePair<string, TaxDTO>(taxDTO.TaxName, taxDTO));
    //            }
    //        }
    //        log.LogMethodExit();
    //    }

    //    /// <summary>
    //    /// Converts TaxName to TaxID
    //    /// </summary>
    //    /// <param name="stringValue"></param>
    //    /// <returns></returns>
    //    public override object FromString(string stringValue)
    //    {
    //        log.LogMethodEntry(stringValue);
    //        int CountryId = -1;
    //        for (int i = 0; i < taxNameTaxDTOKeyValuePair.Count; i++)
    //        {
    //            if (taxNameTaxDTOKeyValuePair[i].Key == stringValue)
    //            {
    //                taxNameTaxDTOKeyValuePair[i] = new KeyValuePair<string, TaxDTO>(taxNameTaxDTOKeyValuePair[i].Key, taxNameTaxDTOKeyValuePair[i].Value);
    //                CountryId = taxNameTaxDTOKeyValuePair[i].Value.TaxId;
    //            }
    //        }

    //        log.LogMethodExit(CountryId);
    //        return CountryId;
    //    }
    //    /// <summary>
    //    /// Converts TaxID to TaxName
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>

    //    public override string ToString(object value)
    //    {
    //        log.LogMethodEntry(value);
    //        string CountryName = string.Empty;

    //        for (int i = 0; i < taxIdtaxDTOKeyValuePair.Count; i++)
    //        {
    //            if (taxIdtaxDTOKeyValuePair[i].Key == Convert.ToInt32(value))
    //            {
    //                taxIdtaxDTOKeyValuePair[i] = new KeyValuePair<int, TaxDTO>(taxIdtaxDTOKeyValuePair[i].Key, taxIdtaxDTOKeyValuePair[i].Value);

    //                CountryName = taxIdtaxDTOKeyValuePair[i].Value.TaxName;
    //            }
    //        }
    //        log.LogMethodExit(CountryName);
    //        return CountryName;
    //    }
    //}

    class CountryValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<KeyValuePair<int, CountryDTO>> CountryIdCountryDTOKeyValuePair;
        List<KeyValuePair<string, CountryDTO>> CountryNameCountryDTOKeyValuePair;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CountryValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            CountryNameCountryDTOKeyValuePair = new List<KeyValuePair<string, CountryDTO>>();
            CountryIdCountryDTOKeyValuePair = new List<KeyValuePair<int, CountryDTO>>();
            List<CountryDTO> countryDTOList = new List<CountryDTO>();

            CountryList countryList = new CountryList();
            CountryParams countryParams = new CountryParams();
            countryParams.SiteId = executionContext.GetSiteId();
            countryDTOList = countryList.GetCountryList(countryParams);
            if (countryDTOList != null && countryDTOList.Count > 0)
            {
                foreach (CountryDTO countryDTO in countryDTOList)
                {
                    CountryIdCountryDTOKeyValuePair.Add(new KeyValuePair<int, CountryDTO>(countryDTO.CountryId, countryDTO));
                    CountryNameCountryDTOKeyValuePair.Add(new KeyValuePair<string, CountryDTO>(countryDTO.CountryName, countryDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts Countryname to Countryid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int CountryId = -1;
            for (int i = 0; i < CountryNameCountryDTOKeyValuePair.Count; i++)
            {
                if (CountryNameCountryDTOKeyValuePair[i].Key == stringValue)
                {
                    CountryNameCountryDTOKeyValuePair[i] = new KeyValuePair<string, CountryDTO>(CountryNameCountryDTOKeyValuePair[i].Key, CountryNameCountryDTOKeyValuePair[i].Value);
                    CountryId = CountryNameCountryDTOKeyValuePair[i].Value.CountryId;
                }
            }

            log.LogMethodExit(CountryId);
            return CountryId;
        }
        /// <summary>
        /// Converts Countryid to Countryname
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string CountryName = string.Empty;

            for (int i = 0; i < CountryIdCountryDTOKeyValuePair.Count; i++)
            {
                if (CountryIdCountryDTOKeyValuePair[i].Key == Convert.ToInt32(value))
                {
                    CountryIdCountryDTOKeyValuePair[i] = new KeyValuePair<int, CountryDTO>(CountryIdCountryDTOKeyValuePair[i].Key, CountryIdCountryDTOKeyValuePair[i].Value);

                    CountryName = CountryIdCountryDTOKeyValuePair[i].Value.CountryName;
                }
            }
            log.LogMethodExit(CountryName);
            return CountryName;
        }
    }

}
