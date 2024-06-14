using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;
using System.Reflection;
using Semnox.Parafait.Device;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    public class K720RF : CardDispenser
    {
        IntPtr handle = IntPtr.Zero;
        protected byte address;

        StringBuilder RecordInfo = new StringBuilder(1024);
        public K720State Flags { get; private set; }
        private bool Has(K720State flag) { return (Flags & flag) == flag; }

        public K720RF(string serialPortNum, int carDispenserAdd) : base(serialPortNum)
        {
            log.LogMethodEntry(serialPortNum, carDispenserAdd);
            address = Convert.ToByte(carDispenserAdd);
            log.LogMethodExit();
        }

        public override void OpenComm()
        {
            handle = OpenCom(portName);

            if (handle != IntPtr.Zero)
            {
                KioskStatic.logToFile("Card dispenser port " + portName + " opened");
            }
            else
            {
                string mes = "Error opening Card dispenser port " + portName;
                KioskStatic.logToFile(mes);
                throw new Exception(mes);
            }
        }

        public override void CloseComm()
        {
            log.LogMethodEntry();
            try
            {
                if (handle != IntPtr.Zero && CloseCom(handle) != 0)
                {
                    throw new Exception($"Error Closing Card dispenser port: {portName}");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Closing Card dispenser port : " + ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        public override bool checkStatus(ref int CardPosition, ref string message)
        {
            try
            {
                OpenComm();

                try
                {
                    if (K720CheckStatus(ref CardPosition, ref message))
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in K720CheckStatus: " + ex.Message);
                    message = ex.Message.ToString();
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in checkStatus: " + ex.Message);
                message = ex.Message.ToString();
            }

            log.LogMethodExit(false);
            return false;
        }

        bool K720CheckStatus(ref int CardPosition, ref string message)
        {
            log.LogMethodEntry(CardPosition, message);

            byte[] response = new byte[4];
            criticalError = false;
            dispenserWorking = true;
            cardLowlevel = false;

            if (SensorQuery(handle, address, response) == 0)
            {
                int status = 0;
                for (int i = 0; i < response.Length; i++)
                {
                    status = status << 4;
                    response[i] = (byte)(response[i] & 0x0f);
                    status += response[i];
                }

                Flags = (K720State)status;

                if (Flags.HasFlag(K720State.Pos1))
                    CardPosition = 3;
                if (Flags.HasFlag(K720State.Pos1) && Flags.HasFlag(K720State.Pos2))
                    CardPosition = 2;
                else if (Flags.HasFlag(K720State.Pos3))
                    CardPosition = 1;

                if (Has(K720State.StackNearEmpty))
                {
                    cardLowlevel = true;
                }

                if (Has(K720State.CannotExecute)
                    | Has(K720State.CaptureCardError)
                    | Has(K720State.CardJammed)
                    | Has(K720State.CardOverlapped)
                    | Has(K720State.StackEmpty)
                    | Has(K720State.PreparingCardFail)
                    | Has(K720State.DispenseCardError))
                {
                    criticalError = true;
                    dispenserWorking = false;
                }

                if (Has(K720State.NoCapturedCard))
                {
                    criticalError = true;
                    dispenserWorking = true;
                }

                message = GetCustomStatusMessage(Flags);

                log.LogMethodExit(true);
                return true;
            }

            criticalError = true;
            dispenserWorking = false;
            log.LogMethodExit(false);
            return false;
        }
        private void Wait()
        {
            K720State curState = Flags;
            do
            {
                int cardPosition = -1;
                string message = string.Empty;
                K720CheckStatus(ref cardPosition, ref message);
                K720State newState = Flags;
                if (curState != newState)
                    KioskStatic.logToFile(string.Format("StateChange: {0:g} -> {1:g}", curState, newState));
                curState = newState;
            }
            while ((Flags & (K720State.CannotExecute | K720State.PreparingCard | K720State.DispensingCard | K720State.CapturingCard)) != 0);
        }

        protected override bool dispenseCard(ref string CardNumber, ref string message)
        {
            try
            {
                OpenComm();

                try
                {
                    if (K720DispenseCard(ref CardNumber, ref message))
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in K720DispenseCard: " + ex.Message);
                    message = ex.Message.ToString();
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in doDispenseCard: " + ex.Message);
                message = ex.Message.ToString();
            }

            log.LogMethodExit(false);
            return false;
        }

        bool K720DispenseCard(ref string CardNumber, ref string message)
        {
            // msg = "Dispensing to Read Position";
            log.LogMethodEntry(message, CardNumber);
            //message = KioskStatic.Utilities.MessageUtils.getMessage(391);
            KioskStatic.logToFile(message);

            Wait();
            int cardPosition = -1;
            if (Flags == K720State.Pos1 | (Has(K720State.Pos1) && Has(K720State.Pos2))) //a card already in read or hold position
            {
                if (!K720CaptureCard(ref message))
                    return false;

                do
                {
                    Wait();
                }
                while (!Has(K720State.Pos3) && !Has(K720State.StackEmpty));
            }

            if (Has(K720State.StackEmpty))
            {
                message = GetEnumDescription(K720State.StackEmpty);
                return false;
            }

            if (Has(K720State.Pos3))
            {
                if (SendCmd(handle, address, Command.DispenseReadCard, Command.DispenseReadCard.Length) == 0)
                {
                    Wait();
                    KioskStatic.logToFile("Card Position: " + cardPosition.ToString());
                    if (Has(K720State.Pos2) && Has(K720State.Pos1))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }

            log.LogMethodExit(false);
            return false;
        }

        public override void DispenseToPosition(string position)
        {
            string command = string.Empty;
            switch (position)
            {
                case "SENSOR2": command = Command.DispenseSensor2; break;
                case "POS2": command = Command.DispenseReadCard; break;
                case "POS1": command = Command.DispenseHoldCard; break;
                case "OUT": command = Command.DispenseMouth; break;
                default: throw new Exception("Invalid position");
            }

            try
            {
                OpenComm();

                if (SendCmd(handle, address, command, command.Length) == 0)
                {
                    Wait();
                }
                else
                {
                    throw new Exception("DispenseToPosition failed");
                }
            }
            finally
            {
                CloseComm();
            }
        }

        public override bool ejectCard(ref string message)
        {
            try
            {
                OpenComm();

                try
                {
                    if (K720EjectCard(ref message))
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in K720EjectCard: " + ex.Message);
                    message = ex.Message.ToString();
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in doEjectCard: " + ex.Message);
                message = ex.Message.ToString();
            }

            log.LogMethodExit(false);
            return false;
        }

        protected override bool captureCard(ref string message)
        {
            try
            {
                OpenComm();

                try
                {
                    if (K720CaptureCard(ref message))
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in K720CaptureCard: " + ex.Message);
                    message = ex.Message.ToString();
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in doRejectCard: " + ex.Message);
                message = ex.Message.ToString();
            }

            log.LogMethodExit(false);
            return false;
        }

        bool K720EjectCard(ref string message)
        {
            log.LogMethodEntry(message);

            bool dropCard = Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "DROP_CARD_FROM_DISPENSER_MOUTH", false);
            if (dropCard == false)
            {//message = KioskStatic.Utilities.MessageUtils.getMessage(392);
                if (SendCmd(handle, address, Command.DispenseHoldCard, Command.DispenseHoldCard.Length) == 0)
                {
                    Wait();

                    if (Has(K720State.Pos1) & !Has(K720State.Pos2))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }
            else
            {
                if (SendCmd(handle, address, Command.DispenseMouth, Command.DispenseMouth.Length) == 0)
                {
                    Wait();

                    if (!Has(K720State.Pos1) & !Has(K720State.Pos2))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                } 
            }

            log.LogMethodExit(false);
            return false;
        }

        bool K720CaptureCard(ref string message)
        {
            log.LogMethodEntry(message);
            Wait();
            if (SendCmd(handle, address, Command.CaptureCard, Command.CaptureCard.Length) == 0)
            {
                Wait();
                int cardPosition = -1;

                if (K720CheckStatus(ref cardPosition, ref message))
                {
                    if (!Has(K720State.Pos1) && !Has(K720State.Pos2))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }

            log.LogMethodExit(false);
            return false;
        }


        // Get enum full string description
        public static string GetEnumDescription(Enum enumValue)
        {
            string enumValueAsString = enumValue.ToString();

            var type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValueAsString);
            object[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                var attribute = (DescriptionAttribute)attributes[0];
                return attribute.Description;
            }

            return enumValueAsString;
        }

        protected virtual IntPtr OpenCom(string port)
        {
            return K720_CommOpen(port);
        }

        protected virtual int CloseCom(IntPtr handle)
        {
            return K720_CommClose(handle);
        }

        protected virtual int SendCmd(IntPtr handle, byte addr, string p_Cmd, int CmdLen)
        {
            return K720_SendCmd(handle, addr, p_Cmd, CmdLen, RecordInfo);
        }

        protected virtual int SensorQuery(IntPtr handle, byte addr, byte[] StateInfo)
        {
            return K720_SensorQuery(handle, addr, StateInfo, RecordInfo);
        }

        #region DLL imports
        private const string DLLNAME = @"K720_Dll.dll";
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"K720_CommOpen")]
        private static extern IntPtr K720_CommOpen(string portNumber);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"K720_CommOpenWithBaud")]
        private static extern IntPtr K720_CommOpenWithBaud(string portNumber, uint baudRate);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"K720_CommClose")]
        private static extern int K720_CommClose(IntPtr handle);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"K720_SendCmd")]
        private static extern int K720_SendCmd(IntPtr handle, byte addr, string p_Cmd, int CmdLen, StringBuilder RecordInfo);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"K720_SensorQuery")]
        private static extern int K720_SensorQuery(IntPtr handle, byte addr, byte[] StateInfo, StringBuilder RecordInfo);
                
        #endregion DLL imports

        public struct Command
        {
            public static string CaptureCard = "CP";
            public static string Check = "AP";
            public static string IssueCard = "DC";
            public static string CollectCard = "CP";
            public static string Reposition = "RS";
            public static string DispenseSensor2 = "FC6";
            public static string DispenseReadCard = "FC7";
            public static string DispenseHoldCard = "FC4";
            public static string DispenseMouth = "FC0";
            public static string AllowBuzz = "BE";
            public static string StopBuzz = "BD";
            public static string BaudRate1200 = "CS0";
            public static string BaudRate2400 = "CS1";
            public static string BaudRate4800 = "CS2";
            public static string BaudRate9600 = "CS3";
        }

        [Flags]
        public enum K720State
        {
            [Description("Position 1")]
            Pos1 = 0x0001,

            [Description("Position 2")]
            Pos2 = 0x0002,

            [Description("Position 3")]
            Pos3 = 0x0004,

            [Description("Stack Empty")]
            StackEmpty = 0x0008,

            [Description("Stack Near Empty")]
            StackNearEmpty = 0x0010,

            [Description("Card Jammed")]
            CardJammed = 0x0020,

            [Description("Card Over Lapped")]
            CardOverlapped = 0x0040,

            [Description("No Card Captured")]
            NoCapturedCard = 0x0080,

            [Description("Card Capture Error")]
            CaptureCardError = 0x0100,

            [Description("Dispense Card Error")]
            DispenseCardError = 0x0200,

            [Description("Capturing Card")]
            CapturingCard = 0x0400,

            [Description("Dispensing Card")]
            DispensingCard = 0x0800,

            [Description("Preparing Card")]
            PreparingCard = 0x1000,

            [Description("Preparing Card Fail")]
            PreparingCardFail = 0x2000,

            [Description("Cannot Execute")]
            CannotExecute = 0x4000,

            [Description("Keep")]
            Keep = 0x8000
        }

        private string GetCustomStatusMessage(K720State flags)
        {
            log.LogMethodEntry(flags);
            string statusMsg = string.Empty; 
            K720State flagValue = flags; 
            if(flags.ToString().Length > 0)
            {
                string[] flagValueList = flags.ToString().Split(',');
                if (flagValueList != null)
                {
                    string flagValueString = flagValueList[0];
                    Enum.TryParse(flagValueString, out flagValue);
                }
            }
            log.LogVariableState("flagValue", flagValue);
            switch (flagValue)
            {
                case K720State.CannotExecute:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, "Cannot execute command");
                    break; 
                case K720State.CaptureCardError:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 386);//*Capture Card Error*
                    break;
                case K720State.CapturingCard:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 384);//Capturing Card. Please Wait...
                    break;
                case K720State.CardJammed:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 389);//*Card Jammed*
                    break;
                case K720State.CardOverlapped:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 388);//*Card Overlapped*
                    break;
                case K720State.DispenseCardError:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 385);//*Dispense Card Error*
                    break;
                case K720State.DispensingCard:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 429,"");//Dispensing Card &1... Please wait.
                    break;
                case K720State.NoCapturedCard:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 387);//*No Captured Card*
                    break;
                case K720State.Pos1:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 396);//Please Remove Card...
                    break;
                case K720State.Pos2:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 395);//Card at Read Position
                    break;
                case K720State.Pos3:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, "Card Dispenser is ready");
                    break;
                case K720State.PreparingCard:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 382);//Preparing Card Please Wait...
                    break;
                case K720State.PreparingCardFail:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 381);//*Preparing Card Failed*
                    break;
                case K720State.StackEmpty:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 390);//*No Card in Dispenser*
                    break;
                case K720State.StackNearEmpty:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, 378);//Card Low Level
                    break;
                case K720State.Keep:
                    statusMsg = MessageViewContainerList.GetMessage(executionContext, "In Keep status");
                    break;
                default:
                    break; 
            }
            if (Flags.HasFlag(K720State.Pos1) && Flags.HasFlag(K720State.Pos2))
            {
                statusMsg = MessageViewContainerList.GetMessage(executionContext, 429, "");//Dispensing Card &1... Please wait.
            }
            log.LogMethodExit(statusMsg);
            return statusMsg;
        }
    }
}
