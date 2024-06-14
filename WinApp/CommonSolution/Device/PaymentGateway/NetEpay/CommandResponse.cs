using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CommandResponse
    {
        private string responseOrigin;
        private string dSIXReturnCode;
        private string cmdStatus;
        private string textResponse;
        private string sequenceNo;
        private string userTrace;

        public string ResponseOrigin
        {
            get
            {
                return responseOrigin;
            }
            set
            {
                responseOrigin = value;
            }
        }
        public string DSIXReturnCode
        {
            get
            {
                return dSIXReturnCode;
            }
            set
            {
                dSIXReturnCode = value;
            }
        }
        public string CmdStatus
        {
            get
            {
                return cmdStatus;
            }
            set
            {
                cmdStatus = value;
            }
        }
        public string TextResponse
        {
            get
            {
                return textResponse;
            }
            set
            {
                textResponse = value;
            }
        }
        public string SequenceNo
        {
            get
            {
                return sequenceNo;
            }
            set
            {
                sequenceNo = value;
            }
        }
        public string UserTrace
        {
            get
            {
                return userTrace;
            }
            set
            {
                userTrace = value;
            }
        }
    }
}
