/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - column 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// represents a database column
    /// </summary>
    public abstract class Column
    {
        protected string name;
        protected string displayName;
        protected bool browsable;
        protected string defaultValue;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="browsable"></param>
        public Column(string name, string displayName = null, string defaultValue = null, bool browsable = true)
        {
            log.LogMethodEntry(name, displayName, defaultValue, browsable);
            this.name = name;
            this.displayName = displayName;
            if (displayName == null)
            {
                this.displayName = name;
            }
            this.browsable = browsable;
            this.defaultValue = defaultValue;
            log.LogMethodExit();
        }
        /// <summary>
        /// returns the valid operators applicable operators on the column
        /// </summary>
        /// <returns></returns>
        public abstract List<Operator> GetApplicableOperators();
        /// <summary>
        /// returns the column type
        /// </summary>
        /// <returns></returns>
        public abstract ColumnType GetColumnType();

        /// <summary>
        /// returns the formatted parameter string
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract string GetFormatedString(object parameter);

        /// <summary>
        /// returns the formatted parameter strings
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string[] GetFormatedString(object[] parameters)
        {
            log.LogMethodEntry(parameters);
            List<string> formatedValueList = new List<string>();
            if (parameters != null && parameters.Length == 1 && parameters[0] is Array)
            {
                List<object> objectList = new List<object>();
                foreach (var item in (parameters[0] as Array))
                {
                    objectList.Add(item);
                }
                parameters = objectList.ToArray();
            }
            foreach (var parameter in parameters)
            {
                formatedValueList.Add(GetFormatedString(parameter));
            }
            log.LogMethodExit();
            string[] returnValue= formatedValueList.ToArray();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// returns the query strings
        /// </summary>
        /// <returns></returns>
        public virtual string GetQueryString(Operator @operator)
        {
            log.LogMethodEntry();
            string returnValue = string.Empty;
            if (defaultValue != null)
            {
                returnValue = "Isnull(" + name + ", " + defaultValue + ")";
            }
            else
            {
                returnValue = name;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// returns the name of the db column
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// returns the name of the db column
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }
        /// <summary>
        /// whether to display on UI
        /// </summary>
        public bool Browsable
        {
            get
            {
                return browsable;
            }
        }
    }
}
