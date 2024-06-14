﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Semnox.Parafait.Device.FirstdataReg {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="srsServiceBinding", Namespace="http://securetransport.dw/srs/soap")]
    public partial class srsService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SrsOperationOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public srsService() {
            this.Url = global::Semnox.Parafait.Device.Properties.Settings.Default.Device_FirstdataReg_srsService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SrsOperationCompletedEventHandler SrsOperationCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securetransport.dw/srs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("Response", Namespace="http://securetransport.dw/srs/soap")]
        public ResponseType SrsOperation([System.Xml.Serialization.XmlElementAttribute(Namespace="http://securetransport.dw/srs/soap")] RequestType Request) {
            object[] results = this.Invoke("SrsOperation", new object[] {
                        Request});
            return ((ResponseType)(results[0]));
        }
        
        /// <remarks/>
        public void SrsOperationAsync(RequestType Request) {
            this.SrsOperationAsync(Request, null);
        }
        
        /// <remarks/>
        public void SrsOperationAsync(RequestType Request, object userState) {
            if ((this.SrsOperationOperationCompleted == null)) {
                this.SrsOperationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSrsOperationOperationCompleted);
            }
            this.InvokeAsync("SrsOperation", new object[] {
                        Request}, this.SrsOperationOperationCompleted, userState);
        }
        
        private void OnSrsOperationOperationCompleted(object arg) {
            if ((this.SrsOperationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SrsOperationCompleted(this, new SrsOperationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class RequestType {
        
        private ReqClientIDType reqClientIDField;
        
        private object itemField;
        
        private string custom1Field;
        
        private string custom2Field;
        
        private string versionField;
        
        private string clientTimeoutField;
        
        public RequestType() {
            this.versionField = "3";
        }
        
        /// <remarks/>
        public ReqClientIDType ReqClientID {
            get {
                return this.reqClientIDField;
            }
            set {
                this.reqClientIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Activation", typeof(ActivationType))]
        [System.Xml.Serialization.XmlElementAttribute("Registration", typeof(RegistrationType))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        public string Custom1 {
            get {
                return this.custom1Field;
            }
            set {
                this.custom1Field = value;
            }
        }
        
        /// <remarks/>
        public string Custom2 {
            get {
                return this.custom2Field;
            }
            set {
                this.custom2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ClientTimeout {
            get {
                return this.clientTimeoutField;
            }
            set {
                this.clientTimeoutField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class ReqClientIDType {
        
        private string dIDField;
        
        private string appField;
        
        private string authField;
        
        private string clientRefField;
        
        /// <remarks/>
        public string DID {
            get {
                return this.dIDField;
            }
            set {
                this.dIDField = value;
            }
        }
        
        /// <remarks/>
        public string App {
            get {
                return this.appField;
            }
            set {
                this.appField = value;
            }
        }
        
        /// <remarks/>
        public string Auth {
            get {
                return this.authField;
            }
            set {
                this.authField = value;
            }
        }
        
        /// <remarks/>
        public string ClientRef {
            get {
                return this.clientRefField;
            }
            set {
                this.clientRefField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class ActivationResponseType {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class RegistrationResponseType {
        
        private string dIDField;
        
        private string[] uRLField;
        
        /// <remarks/>
        public string DID {
            get {
                return this.dIDField;
            }
            set {
                this.dIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("URL")]
        public string[] URL {
            get {
                return this.uRLField;
            }
            set {
                this.uRLField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class StatusType {
        
        private StatusCodeType statusCodeField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StatusCodeType StatusCode {
            get {
                return this.statusCodeField;
            }
            set {
                this.statusCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public enum StatusCodeType {
        
        /// <remarks/>
        OK,
        
        /// <remarks/>
        AuthenticationError,
        
        /// <remarks/>
        UnknownServiceID,
        
        /// <remarks/>
        Timeout,
        
        /// <remarks/>
        XMLError,
        
        /// <remarks/>
        OtherError,
        
        /// <remarks/>
        AccessDenied,
        
        /// <remarks/>
        InvalidMerchant,
        
        /// <remarks/>
        Failed,
        
        /// <remarks/>
        Duplicated,
        
        /// <remarks/>
        Retry,
        
        /// <remarks/>
        NotFound,
        
        /// <remarks/>
        SOAPError,
        
        /// <remarks/>
        InternalError,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class RespClientIDType {
        
        private string dIDField;
        
        private string clientRefField;
        
        /// <remarks/>
        public string DID {
            get {
                return this.dIDField;
            }
            set {
                this.dIDField = value;
            }
        }
        
        /// <remarks/>
        public string ClientRef {
            get {
                return this.clientRefField;
            }
            set {
                this.clientRefField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class ResponseType {
        
        private RespClientIDType respClientIDField;
        
        private StatusType statusField;
        
        private object itemField;
        
        private string custom1Field;
        
        private string custom2Field;
        
        private string versionField;
        
        public ResponseType() {
            this.versionField = "3";
        }
        
        /// <remarks/>
        public RespClientIDType RespClientID {
            get {
                return this.respClientIDField;
            }
            set {
                this.respClientIDField = value;
            }
        }
        
        /// <remarks/>
        public StatusType Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ActivationResponse", typeof(ActivationResponseType))]
        [System.Xml.Serialization.XmlElementAttribute("RegistrationResponse", typeof(RegistrationResponseType))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        public string Custom1 {
            get {
                return this.custom1Field;
            }
            set {
                this.custom1Field = value;
            }
        }
        
        /// <remarks/>
        public string Custom2 {
            get {
                return this.custom2Field;
            }
            set {
                this.custom2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class ActivationType {
        
        private string serviceIDField;
        
        /// <remarks/>
        public string ServiceID {
            get {
                return this.serviceIDField;
            }
            set {
                this.serviceIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securetransport.dw/srs/soap")]
    public partial class RegistrationType {
        
        private string serviceIDField;
        
        /// <remarks/>
        public string ServiceID {
            get {
                return this.serviceIDField;
            }
            set {
                this.serviceIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void SrsOperationCompletedEventHandler(object sender, SrsOperationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SrsOperationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SrsOperationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResponseType Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResponseType)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591