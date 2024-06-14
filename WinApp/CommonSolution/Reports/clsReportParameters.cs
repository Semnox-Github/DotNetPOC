/********************************************************************************************
 * Project Name - clsReportParameters Programs 
 * Description  - Data object of the clsReportParameters class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *1.00        04-October-2017   Rakshith           Updated 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// clsReportParameters class
    /// </summary>
    public class clsReportParameters
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// _reportId property
        /// </summary>
        public int _reportId;


        /// <summary>
        /// _reportId lsit
        /// </summary>
        public List<ReportParameter> lstParameters = new List<ReportParameter>();


        /// <summary>
        /// data types
        /// </summary>
        public enum DataType
        {

            /// <summary>
            /// ENUM TEXT
            /// </summary>
            TEXT,
            /// <summary>
            /// ENUM NUMBER
            /// </summary>
            NUMBER,
            /// <summary>
            /// ENUM DATETIME
            /// </summary>
            DATETIME
        }


        /// <summary>
        /// type of data sources
        /// STATIC_LIST is comma separated static list
        /// query data source can be value, display or just value look up query
        /// in case of value, display pair, combo box will show the "display" column returning "value"
        /// else both selected value and display value will be "value" column
        /// </summary>
        public enum DataSourceType
        {

            /// <summary>
            /// ENUM CONSTANT
            /// </summary>
            CONSTANT,
            /// <summary>
            /// ENUM STATIC_LIST
            /// </summary>
            STATIC_LIST,
            /// <summary>
            /// ENUM QUERY
            /// </summary>
            QUERY
        }


        /// <summary>
        /// Default operator uses the operator used in query
        /// INLIST is a collection of values used for an IN query
        /// </summary>
        public enum Operator
        {
            /// <summary>
            /// ENUM Default
            /// </summary>
            Default,
            /// <summary>
            /// ENUM INLIST
            /// </summary>
            INLIST
        }


        /// <summary>
        /// object class used to retrieve entered parameter values
        /// </summary>
        public class SQLParameter
        {
            /// <summary>
            /// ParameterName property
            /// </summary>
            public string ParameterName;
            /// <summary>
            /// ParameterValue property
            /// </summary>
            public object ParameterValue;
            /// <summary>
            /// StringValue property
            /// </summary>
            public string StringValue;
            /// <summary>
            /// Operator property
            /// </summary>
            public Operator Operator;
            /// <summary>
            /// DisplayValue property
            /// </summary>
            public string DisplayValue; //Added 20-Oct-2016


            /// <summary>
            /// Parameterised constructor
            /// </summary>
            /// <param name="name">name</param>
            /// <param name="value">value</param>
            /// <param name="stringValue">stringValue</param>
            /// <param name="Op">Op</param>
            /// <param name="displayValue">displayValue</param>
            public SQLParameter(string name, object value, string stringValue, Operator Op, string displayValue = null) //Updated signature to include DisplayValue param 20-Oct-2016
            {
                log.LogMethodEntry(name, value, stringValue, Op, displayValue);
                ParameterName = name;
                ParameterValue = value;
                StringValue = stringValue;
                Operator = Op;
                DisplayValue = displayValue;
                log.LogMethodExit();
            }
        }

        //

        /// <summary>
        ///  parameter Class
        /// </summary>
        public class ReportParameter
        {

            /// <summary>
            /// ParameterId field
            /// </summary>
            public int ParameterId;


            /// <summary>
            ///   ParameterName field
            /// </summary>
            public string ParameterName;


            /// <summary>
            ///  SQLParameter field
            /// </summary>
            public string SQLParameter;


            /// <summary>
            ///  Description field
            /// </summary>
            public string Description;


            /// <summary>
            ///  DataType field
            /// </summary>
            public DataType DataType;


            /// <summary>
            ///  DataSourceType field
            /// </summary>
            public DataSourceType DataSourceType;


            /// <summary>
            ///  Operator field
            /// </summary>
            public Operator Operator;


            /// <summary>
            /// DataSource field
            /// </summary>
            public string DataSource;


            /// <summary>
            ///  Active field
            /// </summary>
            public bool Active;


            /// <summary>
            ///  MasterEntityId field
            /// </summary>
            public int DisplayOrder;


            /// <summary>
            ///   Mandatory field
            /// </summary>
            public bool Mandatory;


            /// <summary>
            ///   UIControl field
            /// </summary>
            public System.Windows.Forms.Control UIControl;


            /// <summary>
            ///   Value field
            /// </summary>
            public object Value; // single value object



            /// <summary>
            ///   ListDataSource field
            /// </summary>
            public DataTable ListDataSource = null; // list datasource


            /// <summary>
            /// individual inlsit value 
            /// </summary>
            public class clsInListValueObject
            {
                /// <summary>
                /// value property
                /// </summary>
                public object value;
                /// <summary>
                /// description property
                /// </summary>
                public object description;


                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="_value">_value</param>
                /// <param name="_desc">_desc</param>
                public clsInListValueObject(object _value, object _desc)
                {
                    value = _value;
                    description = _desc;
                }
            }



            /// <summary>
            /// clsInListValue class
            /// </summary>
            public class clsInListValue
            {

                /// <summary>
                /// InListValueList list Property
                /// </summary>
                public List<clsInListValueObject> InListValueList = new List<clsInListValueObject>();


                /// <summary>
                /// addValue method
                /// </summary>
                /// <param name="value">value</param>
                /// <param name="description">description</param>
                public void addValue(object value, object description)
                {
                    log.LogMethodEntry(value, description);
                    clsInListValueObject val = InListValueList.Find(delegate (clsInListValueObject objVal) { return (objVal.value.ToString() == value.ToString()); });
                    if (val == null)
                    {
                        InListValueList.Add(new clsInListValueObject(value, description));
                        refreshDT();
                    }
                    log.LogMethodExit();
                }

                /// <summary>
                /// remove method
                /// </summary>
                /// <param name="value">value</param>
                public void remove(object value)
                {
                    log.LogMethodEntry(value);
                    clsInListValueObject val = InListValueList.Find(delegate (clsInListValueObject objVal) { return (objVal.value.ToString() == value.ToString()); });
                    if (val != null)
                    {
                        InListValueList.Remove(val);
                        refreshDT();
                    }
                    log.LogMethodExit();
                }


                /// <summary>
                /// InListValueDT datatable property
                /// </summary>
                public DataTable InListValueDT; // data table used as data source by UI components


                /// <summary>
                /// Constructor
                /// </summary>
                public clsInListValue()
                {
                    log.LogMethodEntry();
                    InListValueDT = new DataTable();
                    InListValueDT.Columns.Add("Value");
                    InListValueDT.Columns.Add("Description");
                    log.LogMethodExit();
                }


                /// <summary>
                /// refreshDT method
                /// </summary>
                public void refreshDT()
                {
                    log.LogMethodEntry();
                    InListValueDT.Rows.Clear();
                    foreach (clsInListValueObject o in InListValueList)
                    {
                        InListValueDT.Rows.Add(o.value, o.description);
                    }
                    log.LogMethodExit();
                }


                /// <summary>
                /// hasValues property
                /// </summary>
                public bool hasValues
                {
                    get { return (InListValueList.Count > 0 ? true : false); }
                }
            }
            /// <summary>
            /// InListValue property
            /// </summary>
            public clsInListValue InListValue = new clsInListValue();


            /// <summary>
            /// Constructor with parameter
            /// </summary>
            public ReportParameter(object parameterId,
                                object parameterName,
                                object sQLParameter,
                                object description,
                                object dataType,
                                object dataSourceType,
                                object dataSource,
                                object inOperator,
                                object active,
                                object displayOrder,
                                object mandatory)
            {
                ParameterId = Convert.ToInt32(parameterId);
                ParameterName = parameterName.ToString();
                SQLParameter = sQLParameter.ToString();
                Description = description.ToString();
                DataType = (DataType)Enum.Parse(typeof(DataType), dataType.ToString(), true);
                DataSourceType = (DataSourceType)Enum.Parse(typeof(DataSourceType), dataSourceType.ToString(), true);
                Operator = (Operator)Enum.Parse(typeof(Operator), inOperator.ToString(), true); ;
                DataSource = dataSource.ToString();
                Active = Convert.ToBoolean(active);
                if (displayOrder != DBNull.Value)
                    DisplayOrder = Convert.ToInt32(displayOrder);
                Mandatory = Convert.ToBoolean(mandatory);

                if (DataSourceType.Equals(DataSourceType.STATIC_LIST))
                {
                    string[] dArray = DataSource.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    ListDataSource = new DataTable();
                    ListDataSource.Columns.Add("Value");
                    ListDataSource.Rows.Add(DBNull.Value);
                    for (int i = 0; i < dArray.Length; i++)
                        ListDataSource.Rows.Add(dArray[i].Trim());
                }
                else if (DataSourceType.Equals(DataSourceType.QUERY))
                {
                    ListDataSource = Common.Utilities.executeDataTable(DataSource);
                    DataRow dr = ListDataSource.NewRow();
                    dr[0] = DBNull.Value;
                    if (ListDataSource.Columns.Count > 1)
                        dr[1] = DBNull.Value;
                    ListDataSource.Rows.InsertAt(dr, 0);
                }
            }



            /// <summary>
            /// save schedule parameter values
            /// </summary>
            /// <param name="reportScheduleReportId">reportScheduleReportId</param>
            public void saveReportParameterValue(int reportScheduleReportId)
            {
                log.LogMethodEntry(reportScheduleReportId);
                ReportParameterValuesList reportParameterValuesList = new ReportParameterValuesList();
                ReportParameterValuesDTO reportParameterValuesDTO = new ReportParameterValuesDTO();
                List<ReportParameterValuesDTO> reportParameterValuesDTOList = new List<ReportParameterValuesDTO>();
                List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>> reportParameterValuesSearchByParameters = new List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>>();
                reportParameterValuesSearchByParameters.Add(new KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>(ReportParameterValuesDTO.SearchByParameters.REPORT_SCHEDULE_REPORT_ID, reportScheduleReportId.ToString()));
                reportParameterValuesSearchByParameters.Add(new KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>(ReportParameterValuesDTO.SearchByParameters.PARAMETER_ID, ParameterId.ToString()));
                reportParameterValuesDTOList = reportParameterValuesList.GetReportsParameterValuesList(reportParameterValuesSearchByParameters);
                if (reportParameterValuesDTOList != null && reportParameterValuesDTOList.Count>0)
                {
                    reportParameterValuesDTO = reportParameterValuesDTOList[0];
                }
                else
                {
                    reportParameterValuesDTO.ReportParameterValueId = -1;
                    reportParameterValuesDTO.ReportScheduleReportId = reportScheduleReportId;
                    reportParameterValuesDTO.ParameterId = ParameterId;
                }
                reportParameterValuesDTO.ParameterValue = (Value == null) ? "" : Value.ToString();
                ReportParameterValues reportParameterValues = new ReportParameterValues(reportParameterValuesDTO);
                reportParameterValues.Save();
                if (Operator == clsReportParameters.Operator.INLIST) // save the inlist values individually in inlsit table
                {
                    ReportParameterInListValuesList reportParameterInListValuesList = new ReportParameterInListValuesList();
                    reportParameterInListValuesList.DeleteReportParameterInListValues(reportParameterValuesDTO.ReportParameterValueId);

                    foreach (clsInListValueObject valObj in InListValue.InListValueList)
                    {
                        ReportParameterInListValuesDTO reportParameterInListValuesDTO = new ReportParameterInListValuesDTO();
                        reportParameterInListValuesDTO.ReportParameterValueId = reportParameterValuesDTO.ReportParameterValueId;
                        reportParameterInListValuesDTO.InListValue = valObj.value.ToString();
                        ReportParameterInListValues ReportParameterInListValues = new ReportParameterInListValues(reportParameterInListValuesDTO);
                        ReportParameterInListValues.Save();
                    }
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// SelectedParameterValue class
        /// </summary>
        public class SelectedParameterValue
        {
            /// <summary>
            /// parameterName Property
            /// </summary>
            public string parameterName;

            /// <summary>
            /// parameterValue Property
            /// </summary>
            public object[] parameterValue;


            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="parameterName">parameterName</param>
            /// <param name="parameterValue">parameterValue</param>
            public SelectedParameterValue(string parameterName, params object[] parameterValue)
            {
                log.LogMethodEntry(parameterName, parameterValue);
                this.parameterName = parameterName;
                this.parameterValue = parameterValue;
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// populate the parameters for the reports
        /// </summary>
        /// <param name="reportId">reportId</param>
        public clsReportParameters(int reportId)
        {
            log.LogMethodEntry(reportId);
            _reportId = reportId;
            List<ReportParametersDTO> reportParametersDTOList = new List<ReportParametersDTO>();
            ReportsParameterList reportsParameterList = new ReportsParameterList();
            reportParametersDTOList = reportsParameterList.GetReportParameterListByReport(_reportId);

            foreach (ReportParametersDTO reportParametersDTO in reportParametersDTOList)
            {
                lstParameters.Add(new ReportParameter(reportParametersDTO.ParameterId,
                                                       reportParametersDTO.ParameterName,
                                                       reportParametersDTO.SqlParameter,
                                                       reportParametersDTO.Description,
                                                       reportParametersDTO.DataType,
                                                       reportParametersDTO.DataSourceType,
                                                       reportParametersDTO.DataSource,
                                                       reportParametersDTO.Operator,
                                                       reportParametersDTO.ActiveFlag,
                                                       reportParametersDTO.DisplayOrder,
                                                       reportParametersDTO.Mandatory));
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// returns parameters as a concatenated string value
        /// </summary>
        /// <returns>returns string</returns>
        public string getParameterString()
        {
            log.LogMethodEntry();
            List<SQLParameter> lst = getParameterList();
            string paramString = "";
            foreach (SQLParameter p in lst)
            {
                paramString += p.ParameterName + "=" + p.StringValue.ToString() + " ";
            }
            log.LogMethodExit(paramString.TrimEnd());
            return paramString.TrimEnd();
        }


        /// <summary>
        /// mandatory validations
        /// </summary>
        public void validate()
        {
            log.LogMethodEntry();
            foreach (ReportParameter param in lstParameters)
            {
                if (param.Mandatory)
                {
                    if (param.Operator == Operator.INLIST)
                    {
                        if (!param.InListValue.hasValues)
                        {
                            throw new ApplicationException(param.ParameterName + " is mandatory");
                        }
                    }
                    else
                    {
                        if (param.Value == null || param.Value == DBNull.Value)
                            throw new ApplicationException(param.ParameterName + " is mandatory");
                    }
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// returns the list of paramter objects
        /// </summary>
        /// <returns> returns  List SQLParameter </returns>
        public List<SQLParameter> getParameterList()
        {
            log.LogMethodEntry();
            // validate();

            List<SQLParameter> ParamList = new List<SQLParameter>();

            foreach (ReportParameter param in lstParameters)
            {
                if (param.Operator == Operator.INLIST)
                {
                    if (!param.InListValue.hasValues)
                    {
                        ParamList.Add(new SQLParameter(param.SQLParameter, "(NULL)", "(NULL)", param.Operator, null));
                    }
                    else
                    {
                        string paramString = "(";
                        string paramDisplayValue = "";
                        switch (param.DataType)
                        {
                            case DataType.DATETIME:
                                {
                                    foreach (ReportParameter.clsInListValueObject valueObject in param.InListValue.InListValueList)
                                    {
                                        paramString += "Convert(DateTime, '" + Convert.ToDateTime(valueObject.value.ToString()).ToString(Common.ParafaitEnv.DATETIME_FORMAT) + "'), ";
                                    }
                                    break;
                                }
                            case DataType.NUMBER:
                                {
                                    foreach (ReportParameter.clsInListValueObject valueObject in param.InListValue.InListValueList)
                                    {
                                        paramString += valueObject.value.ToString() + ", ";
                                        paramDisplayValue += valueObject.description.ToString() + ", ";
                                    }
                                    break;
                                }
                            case DataType.TEXT:
                                {
                                    foreach (ReportParameter.clsInListValueObject valueObject in param.InListValue.InListValueList)
                                    {
                                        paramString += "'" + valueObject.value.ToString() + "', ";
                                        paramDisplayValue += valueObject.description.ToString() + ", ";
                                    }
                                    break;
                                }
                        }

                        paramString = paramString.TrimEnd(',', ' ') + ")";
                        paramDisplayValue = paramDisplayValue.TrimEnd(',', ' ');
                        ParamList.Add(new SQLParameter(param.SQLParameter, paramString, paramString, param.Operator, paramDisplayValue));
                    }
                }
                else
                {
                    if (param.Value == null || param.Value == DBNull.Value)
                    {
                        ParamList.Add(new SQLParameter(param.SQLParameter, DBNull.Value, "'NULL'", param.Operator));
                    }
                    else
                    {
                        object paramObject;
                        string stringValue = "";
                        string displayValue = "";
                        switch (param.DataType)
                        {
                            case DataType.DATETIME:
                                {
                                    paramObject = Convert.ToDateTime(param.Value.ToString());
                                    stringValue = Convert.ToDateTime(paramObject).ToString(Common.ParafaitEnv.DATETIME_FORMAT);
                                    break;
                                }
                            case DataType.NUMBER:
                                {
                                    paramObject = param.Value;
                                    stringValue = param.Value.ToString();
                                    displayValue = param.Description;
                                    break;
                                }
                            case DataType.TEXT:
                                {
                                    paramObject = param.Value;
                                    stringValue = "'" + param.Value.ToString() + "'";
                                    displayValue = param.Description;
                                    break;
                                }

                        }
                        ParamList.Add(new SQLParameter(param.SQLParameter, param.Value, stringValue, param.Operator, displayValue));
                    }
                }
            }
            log.LogMethodExit(ParamList);
            return ParamList;
        }


        /// <summary>
        /// getScheduleParameterValue method
        /// </summary>
        /// <param name="reportScheduleReportId">reportScheduleReportId</param>
        /// <param name="parameterId">parameterId</param>
        /// <returns>returns object</returns>
        public object getScheduleParameterValue(int reportScheduleReportId, int parameterId)
        {
            log.LogMethodEntry(reportScheduleReportId, parameterId);
            try
            {
                ReportParameterValuesList reportParameterValuesList = new ReportParameterValuesList();
                List<ReportParameterValuesDTO> reportParameterValuesDTOList = new List<ReportParameterValuesDTO>();
                List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>> reportParameterValuesSearchByParameters = new List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>>();
                reportParameterValuesSearchByParameters.Add(new KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>(ReportParameterValuesDTO.SearchByParameters.REPORT_SCHEDULE_REPORT_ID, reportScheduleReportId.ToString()));
                reportParameterValuesSearchByParameters.Add(new KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>(ReportParameterValuesDTO.SearchByParameters.PARAMETER_ID, parameterId.ToString()));
                reportParameterValuesDTOList = reportParameterValuesList.GetReportsParameterValuesList(reportParameterValuesSearchByParameters);
                if (reportParameterValuesDTOList == null)
                    return null;
                else
                    return reportParameterValuesDTOList[0].ParameterValue;
            }
            catch (Exception)
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// getScheduleParameterValue method
        /// </summary>
        /// <param name="reportScheduleReportId">reportScheduleReportId</param>
        /// <param name="parameterId">parameterId</param>
        /// <returns>returns object</returns>
        public object getScheduleParameterValueByReportId(int reportId, int parameterId)
        {
            log.LogMethodEntry(reportId, parameterId);
            try
            {
                ReportParameterValuesList reportParameterValuesList = new ReportParameterValuesList();
                List<ReportParameterValuesDTO> reportParameterValuesDTOList = new List<ReportParameterValuesDTO>();
                List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>> reportParameterValuesSearchByParameters = new List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>>();
                reportParameterValuesSearchByParameters.Add(new KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>(ReportParameterValuesDTO.SearchByParameters.REPORT_SCHEDULE_REPORT_ID, reportId.ToString()));
                reportParameterValuesSearchByParameters.Add(new KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>(ReportParameterValuesDTO.SearchByParameters.PARAMETER_ID, parameterId.ToString()));
                reportParameterValuesDTOList = reportParameterValuesList.GetReportsParameterValuesList(reportParameterValuesSearchByParameters);
                if (reportParameterValuesDTOList == null)
                    return null;
                else
                    log.LogMethodExit(reportParameterValuesDTOList[0].ParameterValue);
                    return reportParameterValuesDTOList[0].ParameterValue;
            }
            catch (Exception)
            {
                log.LogMethodExit(null);
                return null;
            }
        }




        /// <summary>
        /// getScheduleParameterInListValue method
        /// </summary>
        /// <param name="reportScheduleReportId">reportScheduleReportId</param>
        /// <param name="parameterId">parameterId</param>
        /// <returns>returns DataTable</returns>
        public DataTable getScheduleParameterInListValue(int reportScheduleReportId, int parameterId)
        {
            log.LogMethodEntry(reportScheduleReportId, parameterId);
            ReportParameterInListValuesDatahandler reportParameterInListValuesDatahandler = new ReportParameterInListValuesDatahandler();
            DataTable dataTable = reportParameterInListValuesDatahandler.getScheduleParameterInListValue(reportScheduleReportId, parameterId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// getScheduleParameterInListValue method
        /// </summary>
        /// <param name="reportId">reportScheduleReportId</param>
        /// <param name="parameterId">parameterId</param>
        /// <param name="scheduleid">scheduleid</param>
        /// <returns>returns DataTable</returns>
        public DataTable getScheduleParameterInListValueByReportId(int reportId, int parameterId,int scheduleid)
        {
            log.LogMethodEntry(reportId, parameterId, scheduleid);
            ReportParameterInListValuesDatahandler reportParameterInListValuesDatahandler = new ReportParameterInListValuesDatahandler();
            DataTable dataTable = reportParameterInListValuesDatahandler.GetScheduleParameterInListValueByReportId(reportId, parameterId, scheduleid);
            log.LogMethodExit(dataTable);
            return dataTable;
        }


        /// <summary>
        ///  get the description of in list values, if in list's data source was a list so that the inlist is ready for display in UI controls
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="param">param</param>
        /// <returns>returns object</returns>
        public object getInlistValueDescription(object value, ReportParameter param)
        {
            log.LogMethodEntry(value, param);
            if (param.DataSourceType == DataSourceType.QUERY
                || param.DataSourceType == DataSourceType.STATIC_LIST)
            {
                foreach (DataRow dr in param.ListDataSource.Rows)
                {
                    if (dr[0].ToString() == value.ToString())
                    {
                        if (dr.Table.Columns.Count > 1)
                            return dr[1];
                        else
                            return dr[0];
                    }
                }
            }
            else
            {
                log.LogMethodExit(value);
                return value;
            }
            log.LogMethodExit(DBNull.Value);
            return DBNull.Value;
        }


        /// <summary>
        ///  create the parameter values from database for the report schedule
        /// </summary>
        /// <param name="reportScheduleReportId">reportScheduleReportId</param>
        public void getScheduleParameters(int reportScheduleReportId)
        {
            log.LogMethodEntry(reportScheduleReportId);
            foreach (ReportParameter param in lstParameters)
            {
                if (param.Operator == Operator.Default)
                {
                    param.Value = getScheduleParameterValue(reportScheduleReportId, param.ParameterId);
                }
                else
                {
                    DataTable dt = getScheduleParameterInListValue(reportScheduleReportId, param.ParameterId);
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            object desc = getInlistValueDescription(dr[0], param);
                            if (desc != DBNull.Value)
                                param.InListValue.addValue(dr[0], desc);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        ///  create the parameter values from database for the report schedule
        /// </summary>
        /// <param name="reportId">reportId</param>
        /// <param name="scheduleid">scheduleid</param>
        public void getScheduleParametersByReportId(int reportId,int scheduleid, int reportScheduleReportId)
        {
            log.LogMethodEntry(reportId, scheduleid, reportScheduleReportId);
            foreach (ReportParameter param in lstParameters)
            {               
                if (param.Operator == Operator.Default)
                {
                    param.Value = getScheduleParameterValue(reportScheduleReportId, param.ParameterId);
                    if(param.Value != null && param.Value.ToString() == "")
                    {
                        //if No parameter value is selected in static list, null value is passed
                        param.Value = null;
                    }
                    //if (dt != null)
                    //{
                    //    if(dt.Rows.Count > 0)
                    //    {
                    //        param.Value = dt.Rows[0]["InListValue"].ToString();
                    //    }                       
                    //}
                }
                else if(param.Operator == Operator.INLIST)
                {
                    DataTable dt = getScheduleParameterInListValueByReportId(reportId, param.ParameterId, scheduleid);
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            object desc = dr[0].ToString();
                            
                            if (desc != DBNull.Value)
                                param.InListValue.addValue(dr[0], desc);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
