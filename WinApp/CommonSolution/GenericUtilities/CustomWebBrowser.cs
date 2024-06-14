/* Project Name - CustomWebBrowser
* Description  - Custom web browser
* 
**************
**Version Log
**************
*Version        Date           Modified By          Remarks          
********************************************************************************************* 
*2.150.3        01-Jun-2023    Guru S A             Created
********************************************************************************************/
//using Microsoft.Web.WebView2.WinForms;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public class CustomWebBrowser//: WebView2
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private object browserObject = null;
        private string urlValue;
        private string fileNameValue;
        /// <summary>
        /// UrlValue
        /// </summary>
        public string UrlValue { get { return urlValue; } }
        /// <summary>
        /// FileNameValue
        /// </summary>
        public string FileNameValue { get { return fileNameValue; } }
        /// <summary>
        /// CustomWebBrowser - Include Microsoft.Web.WebView2.WinForms.WebView2, Microsoft.Web.WebView2.WinForms as OTS
        /// </summary> 
        public CustomWebBrowser(ExecutionContext executionContext, string webUrl, int browserWidth, int browserHeight):base()

        {
            log.LogMethodEntry(executionContext, webUrl, browserWidth, browserHeight);
            if (string.IsNullOrWhiteSpace(webUrl) == false)
            {
                if (webUrl.ToLower().StartsWith("http") == false)
                { 
                    webUrl = "http://" + webUrl;
                }
                urlValue = webUrl;
                fileNameValue = string.Empty;
                //this.Source = new Uri(webUrl);
                //this.Size = new System.Drawing.Size(browserWidth, browserHeight);
                Type webView2Type = Type.GetType("Microsoft.Web.WebView2.WinForms.WebView2, Microsoft.Web.WebView2.WinForms");
                if (webView2Type != null)
                {
                    browserObject = Activator.CreateInstance(webView2Type);
                    if (browserObject != null)
                    {
                        Uri uri = new Uri(webUrl);
                        webView2Type.InvokeMember("Source", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                              Type.DefaultBinder, browserObject, new object[] { uri });
                        webView2Type.InvokeMember("Size",
                               BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                               Type.DefaultBinder, browserObject, new object[] { new System.Drawing.Size(browserWidth, browserHeight) });
                    }
                    else
                    {

                        string msg = "Unable to launch Webbrowser";
                        throw new ValidationException(msg);
                    }
                }
                else
                {
                    string msg = "Microsoft.Web.WebView2.WinForms.WebView2 DLL is missing";
                    throw new ValidationException(msg);

                }
            }
            else
            {
                string msg = "URL is not provided";
                throw new ValidationException(msg);
            }
            log.LogMethodExit(); 
        }
        /// <summary>
        /// CustomWebBrowser
        /// </summary> 
        public CustomWebBrowser(ExecutionContext executionContext, string fileName, Size browserSize) : base()

        {
            log.LogMethodEntry(executionContext, fileName, browserSize);
            if (string.IsNullOrWhiteSpace(fileName) == false)
            {
                fileNameValue = fileName;
                urlValue = string.Empty;
                //this.Source = new Uri(webUrl);
                //this.Size = new System.Drawing.Size(browserWidth, browserHeight);
                Type webView2Type = Type.GetType("Microsoft.Web.WebView2.WinForms.WebView2, Microsoft.Web.WebView2.WinForms");
                if (webView2Type != null && browserSize != null)
                {
                    browserObject = Activator.CreateInstance(webView2Type);
                    if (browserObject != null)
                    {
                        Uri uri = new Uri(fileName);
                        webView2Type.InvokeMember("Source", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                              Type.DefaultBinder, browserObject, new object[] { uri });
                        webView2Type.InvokeMember("Size",
                               BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                               Type.DefaultBinder, browserObject, new object[] { new System.Drawing.Size(browserSize.Width, browserSize.Height) });  
                    }
                    else
                    {

                        string msg = "Unable to launch Webbrowser";
                        throw new ValidationException(msg);
                    }
                }
                else
                {
                    string msg = "WebView2 related DLl are missing";
                    throw new ValidationException(msg);

                }
            }
            else
            {
                string msg = "File is not provided";
                throw new ValidationException(msg);
            }
            log.LogMethodExit();
        }
        

        /// <summary>
        /// GetBrowerControl
        /// </summary>
        /// <returns></returns>
        public Control GetBrowerControl()
        {
            log.LogMethodEntry();
            Control browserCtrl = null;
            if (browserObject != null)
            {
                browserCtrl = (Control)browserObject; 
            }
            log.LogMethodExit();
            return browserCtrl;
        }
    }
}
