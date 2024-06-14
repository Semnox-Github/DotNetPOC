/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Data Transfer object holds transaction receipt details 
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.2         26-Nov-2019   Nitin Pai            Created for Virtual store enhancement
 *2.80.0      04-Jun-2020   Nitin Pai                Moved from iTransaction to Transaction Project
 ********************************************************************************************/

using System.Drawing;

namespace Semnox.Parafait.Transaction
{
    public class ReceiptDTO
    {
        public int TotalLines;
        public class line
        {
            public string TemplateSection;
            public string[] Data;
            public string[] Alignment;
            public int colCount;
            public Font LineFont;
            public string BarCodeBase64;
            public string BarCodeTag;
            public int LineId;
            public int LineHeight;//to hold Line Height metadata per receipt line
            public bool CancelledLine = false;

            public line()
            {
                Data = new string[5];
                Alignment = new string[5];
                colCount = 0;
            }
        }
        public line[] ReceiptLines;

        private ReceiptDTO()
        {

        }
        public ReceiptDTO(int maxLines)
        {
            ReceiptLines = new line[maxLines];
            for (int i = 0; i < maxLines; i++)
                ReceiptLines[i] = new line();
        }
        
    }
}
