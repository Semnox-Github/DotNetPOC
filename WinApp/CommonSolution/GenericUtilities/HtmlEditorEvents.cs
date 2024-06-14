/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - HtmlEditorEvents
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
#region Using directives

using System;

#endregion

namespace Semnox.Core.GenericUtilities
{
    # region Application delegate definitions

    // Define delegate for raising an editor exception
    public delegate void HtmlExceptionEventHandler(object sender, HtmlExceptionEventArgs e);

    // Define delegate for handling navigation events
    public delegate void HtmlNavigationEventHandler(object sender, HtmlNavigationEventArgs e);

    // delegate declarations required for the find and replace dialog
    internal delegate void FindReplaceResetDelegate();
    internal delegate bool FindFirstDelegate(string findText, bool matchWhole, bool matchCase);
    internal delegate bool FindNextDelegate(string findText, bool matchWhole, bool matchCase);
    internal delegate bool FindReplaceOneDelegate(string findText, string replaceText, bool matchWhole, bool matchCase);
    internal delegate int  FindReplaceAllDelegate(string findText, string replaceText, bool matchWhole, bool matchCase);

    #endregion

    #region Navigation Event Arguments

    /// <summary>
    /// On a user initiated navigation create an event with the following EventArgs
    /// User can set the cancel property to cancel the navigation
    /// </summary>
    public class HtmlNavigationEventArgs : EventArgs
    {
        //private variables
        private string _url = string.Empty;
        private bool _cancel = false;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for event args
        /// </summary>
        public HtmlNavigationEventArgs(string url) : base()
        {
            log.LogMethodEntry(url);
            _url = url;
            log.LogMethodExit();

        } //HtmlNavigationEventArgs

        /// <summary>
        /// Property for the Form Url
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }

        } //Url

        /// <summary>
        /// Defintion of the cancel property
        /// Also allows a set operation
        /// </summary>
        public bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

    } //HtmlNavigationEventArgs

    #endregion

    #region HtmlException defintion and Event Arguments

    /// <summary>
    /// Exception class for HtmlEditor
    /// </summary>
    public class HtmlEditorException : ApplicationException
    {
        private string _operationName;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Property for the operation name
        /// </summary>
        public string Operation
        {
            get
            {
                return _operationName;
            }
            set
            {
                _operationName = value;
            }

        } //OperationName


        /// <summary>
        /// Default constructor
        /// </summary>
        public HtmlEditorException () : base()
        {
            log.LogMethodEntry();
            _operationName = string.Empty;
            log.LogMethodExit();
        }
   
        /// <summary>
        /// Constructor accepting a single string message
        /// </summary>
        public HtmlEditorException (string message) : base(message)
        {
            log.LogMethodEntry(message);
            _operationName = string.Empty;
            log.LogMethodExit();
        }
   
        /// <summary>
        /// Constructor accepting a string message and an inner exception
        /// </summary>
        public HtmlEditorException(string message, Exception inner) : base(message, inner)
        {
            log.LogMethodEntry(message, inner);
            _operationName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor accepting a single string message and an operation name
        /// </summary>
        public HtmlEditorException(string message, string operation) : base(message)
        {
            log.LogMethodEntry(message, operation);
            _operationName = operation;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor accepting a string message an operation and an inner exception
        /// </summary>
        public HtmlEditorException(string message, string operation, Exception inner) : base(message, inner)
        {
            log.LogMethodEntry(message, operation, inner);
            _operationName = operation;
            log.LogMethodExit();
        }

    } //HtmlEditorException


    /// <summary>
    /// Defintion of the Event Arguement for an Html Exception
    /// If capturing an exception internally throw an event with the following EventArgs
    /// </summary>
    public class HtmlExceptionEventArgs : EventArgs
    {
        //private variables
        private string _operation;
        private Exception _exception;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for event args
        /// </summary>
        public HtmlExceptionEventArgs(string operation, Exception exception) : base()
        {
            log.LogMethodEntry(operation, exception);
            _operation = operation;
            _exception = exception;
            log.LogMethodExit();

        } //HtmlEditorExceptionEventArgs

        /// <summary>
        /// Property for the operation name
        /// </summary>
        public string Operation
        {
            get
            {
                return _operation;
            }

        } //Operation

        /// <summary>
        /// Property for the Exception for which the event is being raised
        /// </summary>
        /// <value></value>
        public Exception ExceptionObject
        {
            get
            {
                return _exception;
            }

        } //ExceptionObject

    } //HtmlExceptionEventArgs

    #endregion
}
