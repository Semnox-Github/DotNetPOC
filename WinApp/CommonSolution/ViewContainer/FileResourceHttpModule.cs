/********************************************************************************************
 * Project Name -ViewContainer
 * Description  -FileResourceHttpModule class to handle files 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00     30-Sep-2021       Lakshminarayana           Created
 ********************************************************************************************/
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Semnox.Core.GenericUtilities.FileResources;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;

namespace Semnox.Parafait.ViewContainer
{
    public class FileResourceHttpModule : IHttpModule
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            log.LogMethodEntry();
            // It wraps the Task-based method
            EventHandlerTaskAsyncHelper asyncHelper =
               new EventHandlerTaskAsyncHelper(OnBeginRequest);

            //asyncHelper's BeginEventHandler and EndEventHandler eventhandler that is used
            //as Begin and End methods for Asynchronous HTTP modules
            context.AddOnPostAuthorizeRequestAsync(
            asyncHelper.BeginEventHandler, asyncHelper.EndEventHandler);
            log.LogMethodExit();
        }

        private async Task OnBeginRequest(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            HttpApplication app = (HttpApplication)sender;
            if (IsFileResourceRequest(app))
            {
                string path = app.Context.Request.Path;
                string[] pathSegments = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if(pathSegments.Length == 2)
                {
                    try
                    {
                        string defaultValueName = pathSegments[0];
                        string fileName = pathSegments[1];
                        ExecutionContext executionContext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                        FileResource fileResource = FileResourceFactory.GetFileResource(executionContext, defaultValueName, fileName, false);
                        using (Stream stream = await fileResource.Get())
                        {
                            app.Context.Response.ContentType = System.Web.MimeMapping.GetMimeMapping(fileName);
                            await stream.CopyToAsync(app.Context.Response.OutputStream);
                            app.Context.Response.Flush();
                            app.Context.Response.End();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            log.LogMethodExit();
        }

        private bool IsFileResourceRequest(HttpApplication app)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return app.Context.Request.Path.ToLower().EndsWith(".jpg") || 
                app.Context.Request.Path.ToLower().EndsWith(".png") ||
                app.Context.Request.Path.ToLower().EndsWith(".pptx");
        }
    }
}
