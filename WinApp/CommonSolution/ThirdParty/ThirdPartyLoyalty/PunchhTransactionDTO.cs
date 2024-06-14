/********************************************************************************************
* Project Name - Loyalty
* Description  - Punchh Loyalty Class 
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// MenuItem
    /// </summary>
    public class MenuItem
    {
        public string item_name { get; set; }
        public int item_qty { get; set; }
        public decimal item_amount { get; set; }
        public string menu_item_type { get; set; }
        public int menu_item_id { get; set; }
        public string menu_family { get; set; }
        public string menu_major_group { get; set; }
        public double serial_number { get; set; }

        /// <summary>
        /// MenuItem
        /// </summary>
        public MenuItem()
        {
            this.item_name = string.Empty;
            this.menu_item_type = string.Empty;
            this.menu_family = string.Empty;
            this.menu_major_group = string.Empty;
            this.serial_number = 1.0;
            this.item_qty = 0;
            this.item_amount = 0;
            this.menu_item_id = 0;
        }

        /// <summary>
        /// MenuItem
        /// </summary>
        /// <param name="item_name"></param>
        /// <param name="item_qty"></param>
        /// <param name="item_amount"></param>
        /// <param name="menu_item_type"></param>
        /// <param name="menu_item_id"></param>
        /// <param name="menu_family"></param>
        /// <param name="menu_major_group"></param>
        /// <param name="serial_number"></param>
        public MenuItem(string item_name, int item_qty, decimal item_amount, string menu_item_type, int menu_item_id,
                        string menu_family, string menu_major_group, double serial_number)
        {
            this.item_name = item_name;
            this.menu_item_type = menu_item_type;
            this.menu_family = menu_family;
            this.menu_major_group = menu_major_group;
            this.serial_number = serial_number;
            this.item_qty = item_qty;
            this.item_amount = item_amount;
            this.menu_item_id = menu_item_id;
        }
    }

    /// <summary>
    /// PunchhTransactionDTO
    /// </summary>
    public class PunchhTransactionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string phone { get; set; }
        public decimal receipt_amount { get; set; }
        public DateTime receipt_datetime { get; set; }
        public int sequence_no { get; set; }
        public string punchh_key { get; set; }
        public string transaction_no { get; set; }
        public decimal subtotal_amount { get; set; }
        public string channel { get; set; }
        public string revenue_id { get; set; }
        public string revenue_code { get; set; }
        public string employee_id { get; set; }
        public string employee_name { get; set; }
        public double payable { get; set; }
        public string pos_type { get; set; }
        public string external_uid { get; set; }
        public string pos_version { get; set; }
        public string amp { get; set; }
        public string cc_last4 { get; set; }
        public bool process { get; set; }
        public List<MenuItem> menu_items { get; set; }

        /// <summary>
        /// PunchhTransactionDTO
        /// </summary>
        public PunchhTransactionDTO()
        {
            log.LogMethodEntry();
            this.phone = string.Empty;
            this.transaction_no = string.Empty;
            this.channel = string.Empty;
            this.amp = string.Empty;
            this.pos_type = string.Empty;
            this.external_uid = string.Empty;
            this.revenue_id = string.Empty;
            this.revenue_code = string.Empty;
            this.employee_name = string.Empty;
            this.pos_version = string.Empty;
            this.employee_id = string.Empty;
            this.cc_last4 = string.Empty;
            this.punchh_key = string.Empty;
            this.receipt_amount = 0;
            this.subtotal_amount = 0;
            this.payable = 0;
            this.process = true;
            this.menu_items = new List<MenuItem>();
            this.receipt_datetime = ServerDateTime.Now;
            this.sequence_no = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// PunchhTransactionDTO
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="receipt_amount"></param>
        /// <param name="receipt_datetime"></param>
        /// <param name="sequence_no"></param>
        /// <param name="punchh_key"></param>
        /// <param name="transaction_no"></param>
        /// <param name="subtotal_amount"></param>
        /// <param name="channel"></param>
        /// <param name="revenue_id"></param>
        /// <param name="revenue_code"></param>
        /// <param name="employee_id"></param>
        /// <param name="employee_name"></param>
        /// <param name="payable"></param>
        /// <param name="menu_items"></param>
        /// <param name="pos_type"></param>
        /// <param name="external_uid"></param>
        /// <param name="amp"></param>
        /// <param name="process"></param>
        /// <param name="pos_version"></param>
        /// <param name="cc_last4"></param>
        public PunchhTransactionDTO(string phone, decimal receipt_amount, DateTime receipt_datetime, int sequence_no,
                                     string punchh_key, string transaction_no, decimal subtotal_amount, string channel,
                                     string revenue_id, string revenue_code, string employee_id, string employee_name,
                                     double payable, List<MenuItem> menu_items, string pos_type, string external_uid,
                                     string amp, bool process,string pos_version,string cc_last4)
        {
            log.LogMethodEntry(phone,receipt_amount,receipt_datetime,sequence_no,punchh_key,transaction_no,subtotal_amount,
                               channel,revenue_id,revenue_code,employee_id,employee_name,payable,menu_items,pos_type,external_uid,
                               amp,process, pos_version, cc_last4);
            this.phone = phone;
            this.transaction_no = transaction_no;
            this.channel = channel;
            this.revenue_id = revenue_id;
            this.revenue_code = revenue_code;
            this.employee_name = employee_name;
            this.employee_id = employee_id;
            this.punchh_key = punchh_key;
            this.receipt_amount = receipt_amount;
            this.subtotal_amount = subtotal_amount;
            this.payable = payable;
            this.menu_items = menu_items;
            this.receipt_datetime = receipt_datetime;
            this.sequence_no = sequence_no;
            this.external_uid = external_uid;
            this.pos_type = pos_type;
            this.amp = amp;
            this.process = process;
            this.pos_version = pos_version;
            this.cc_last4 = cc_last4;
            log.LogMethodExit();
        }
    }
}

