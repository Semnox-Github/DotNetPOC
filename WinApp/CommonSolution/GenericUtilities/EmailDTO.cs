/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Email object to send emails
 **************
 **Version Log
 **************
  *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Muhammed Mehraj          Created 
 *2.100.0     04-Sep-2020   Mushahid Faizan           Modified
 ********************************************************************************************/
using System;
using System.Collections.Generic;


namespace Semnox.Core.GenericUtilities
{
    public class EmailDTO
    {
        public string ReplyTo { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<FileAttachment> Attachments { get; set; }
        public string Status { get; set; }
    }

    public class FileAttachment
    {
        public string FileContentBase64 { get; set; }
        public FileInformation Info { get; set; }
    }

    public class FileInformation
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
