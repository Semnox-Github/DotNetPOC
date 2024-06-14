/********************************************************************************************
* Project Name - Utilities
* Description  - Class contains validation error details
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.100.0     12-Nov-2020   Lakshminarayana         Modified - Add methods for supporting serialization 
********************************************************************************************/
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Class contains validation error details
    /// </summary>
    [Serializable]
    public class ValidationError:ISerializable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int entityId;
        private int recordIndex;
        private string entityName;
        private string fieldName;
        private string message;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="entityName">entity name</param>
        /// <param name="fieldName">field name</param>
        /// <param name="message">message</param>
        public ValidationError(string entityName, string fieldName, string message, int entityId = -1, int recordIndex = -1)
        {
            log.LogMethodEntry(entityName, fieldName, message, entityId, recordIndex);
            this.recordIndex = recordIndex;
            this.entityId = entityId;
            this.EntityName = entityName;
            this.FieldName = fieldName;
            this.Message = message;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ValidationError():this("", "", "")
        {
            log.LogMethodEntry(EntityName, FieldName, Message);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of entityName field
        /// </summary>
        public string EntityName
        {
            get
            {
                return entityName;
            }

            set
            {
                entityName = value;
            }
        }

        /// <summary>
        /// Get/Set method of fieldName field
        /// </summary>
        public string FieldName
        {
            get
            {
                return fieldName;
            }

            set
            {
                fieldName = value;
            }
        }

        /// <summary>
        /// Get/Set method of message field
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }
        /// <summary>
        /// Get/Set method of message field
        /// </summary>
        public int EntityId
        {
            get
            {
                return entityId;
            }

            set
            {
                entityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of recordIndex field
        /// </summary>
        public int RecordIndex
        {
            get
            {
                return recordIndex;
            }

            set
            {
                recordIndex = value;
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ValidationError(SerializationInfo info, StreamingContext context)
        {
            log.LogMethodEntry();
            entityId = info.GetInt32("entityId");
            recordIndex = info.GetInt32("recordIndex");
            entityName = info.GetString("entityName");
            fieldName = info.GetString("fieldName");
            message = info.GetString("message");
            log.LogMethodExit();
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            log.LogMethodEntry();
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("entityId", entityId);
            info.AddValue("recordIndex", recordIndex);
            info.AddValue("entityName", entityName);
            info.AddValue("fieldName", fieldName);
            info.AddValue("message", message);
            log.LogMethodExit();
        }
    }
}
