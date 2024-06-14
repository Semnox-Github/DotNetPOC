/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - CARD CONFIGURATION DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-Aug-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class CardConfigurationDTO
    {
        private string command;
        private string cardNumber;
        private int approvalId;
        public CardConfigurationDTO()
        {
            this.command = string.Empty;
            this.cardNumber = string.Empty;
            this.approvalId = -1;
        }

        public CardConfigurationDTO(string cardNumber, string command, int approvalId)
        {
            this.command = command;
            this.cardNumber = cardNumber;
            this.approvalId = approvalId;
        }

        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }
        public string Command
        {
            get
            {
                return command;
            }
            set
            {
                command = value;
            }
        }
        public int ApprovalId
        {
            get
            {
                return approvalId;
            }
            set
            {
                approvalId = value;
            }
        }
    }
}
