/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Data Transfer object holds transaction tickets details 
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.2         26-Nov-2019   Nitin Pai            Created for Virtual store enhancement
 *2.80.0      04-Jun-2020   Nitin Pai                Moved from iTransaction to Transaction Project
 ********************************************************************************************/

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using Semnox.Parafait.POS;

namespace Semnox.Parafait.Transaction
{
    public class TicketDTO
    {
        internal PaperSize PaperSize;
        internal Rectangle TicketBorder;
        internal Margins Margin;
        internal int BorderWidth = 2;
        public class PrintObject
        {
            internal string Text;
            internal Point Location;
            internal Font Font;
            internal int Width;
            internal char Alignment;
            internal string BarCodeBase64;
            internal string BarCodeTag;
            internal string ImageBase64;
            internal char Rotate;
            internal string Color;
            internal int BarCodeHeight;
            internal string BarCodeEncodeType;
            public string TextProperty { get { return Text; } set { Text = value; } }
            public Point LocationProperty { get { return Location; } set { Location = value; } }
            public Font FontProperty { get { return Font; } set { Font = value; } }
            public int WidthProperty { get { return Width; } set { Width = value; } }
            public char AlignmentProperty { get { return Alignment; } set { Alignment = value; } }
            public string BarCodeBase64Property { get { return BarCodeBase64; } set { BarCodeBase64 = value; } }
            public string BarCodeTagProperty { get { return BarCodeTag; } set { BarCodeTag = value; } }
            public string ImageBase64Property { get { return ImageBase64; } set { ImageBase64 = value; } }
            public char RotateProperty { get { return Rotate; } set { Rotate = value; } }
            public string ColorProperty { get { return Color; } set { Color = value; } }
            public int BarCodeHeightProperty { get { return BarCodeHeight; } set { BarCodeHeight = value; } }
            public string BarCodeEncodeTypeProperty { get { return BarCodeEncodeType; } set { BarCodeEncodeType = value; } }


        }
        public List<PrintObject> PrintObjectList = new List<PrintObject>();
        public TicketDTO Backside = null;
        public string CardNumber;
        public string BackgroundImageBase64;

        public int TrxId = -1;
        public int TrxLineId = -1;

        public TicketDTO()
        {

        }
        public PaperSize PaperSizeProperty
        {
            get { return PaperSize; }
            set { PaperSize = value; }
        }
        public Rectangle TicketBorderProperty
        {
            get { return TicketBorder; }
            set { TicketBorder = value; }
        }
        public Margins MarginProperty
        {
            get { return Margin; }
            set { Margin = value; }
        }
        public int BorderWidthProperty
        {
            get { return BorderWidth; }
            set { BorderWidth = value; }
        }

    }

    public class TicketPrinterMapDTO
    {
        internal List<TicketDTO> ticketDTOList;
        internal POSPrinterDTO posPrinterDTO;
        public List<TicketDTO> TicketDTOList
        {
            get { return ticketDTOList; }
            set { ticketDTOList = value; }
        }
        public POSPrinterDTO POSPrinterDTO
        {
            get { return posPrinterDTO; }
            set { posPrinterDTO = value; }
        }
        public TicketPrinterMapDTO()
        {

        }

        public TicketPrinterMapDTO(List<TicketDTO> ticketDTOList, POSPrinterDTO posPrinterDTO)
        {
            this.ticketDTOList = ticketDTOList;
            this.posPrinterDTO = posPrinterDTO;
        }
    }  
}
