using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SK310UR04DLL
{
    public class IntSK310UR04
    {
        //打开串口
        [DllImport("SK_310_UR04.dll")]
        public static extern UInt32 SK310ROpen(string port);

        //按指定的波特率打开串口
        [DllImport("SK_310_UR04.dll")]
        public static extern UInt32 SK310ROpenWithBaut(string port, UInt32 Baudrate);

        //关闭串口
        [DllImport("SK_310_UR04.dll")]
        public static extern int SK310RClose(UInt32 ComHandle);

        [DllImport("SK_310_UR04.dll")]
        public static extern int RS232_ExeCommand(UInt32 ComHandle, byte TxCmCode, byte TxPmCode, UInt16 TxDataLen, byte[] TxData, ref byte RxReplyType, ref byte RxStCode0, ref byte RxStCode1, ref UInt16 RxDataLen, byte[] RxData);



       /////////==================================================================================

        //打开设备
        [DllImport("SK_310_UR04.dll")]
        public static extern UInt32 SK310UOpen();

        //关闭设备
        [DllImport("SK_310_UR04.dll")]
        public static extern int SK310UClose(UInt32 ComHandle);

        [DllImport("SK_310_UR04.dll")]
        public static extern int USB_ExeCommand(UInt32 ComHandle, byte TxCmCode, byte TxPmCode, UInt16 TxDataLen, byte[] TxData, ref byte RxReplyType, ref byte RxStCode0, ref byte RxStCode1, ref UInt16 RxDataLen, byte[] RxData);

    }
}
