using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public static class WebServiceInvoker
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Dictionary<string, Type> availableTypes;

        /// <summary>
        /// Text description of the available services within this web service.
        /// </summary>
        public static List<string> AvailableServices
        {
            get { return services; }
        }

        /// <summary>
        /// Creates the service invoker using the specified web service.
        /// </summary>
        /// <param name="webServiceUri"></param>
        public static void WebServiceInvoke(Uri webServiceUri)
        {
            log.LogMethodEntry(webServiceUri);

            try
            {
                services = new List<string>(); // available services
                availableTypes = new Dictionary<string, Type>(); // available types

                // create an assembly from the web service description
                webServiceAssembly = BuildAssemblyFromWSDL(webServiceUri);

                // see what service types are available
                Type[] types = webServiceAssembly.GetExportedTypes();

                // and save them
                foreach (Type type in types)
                {
                    services.Add(type.FullName);
                    availableTypes.Add(type.FullName, type);
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while invoking web services", ex);
                log.LogMethodExit(null, "Throwing Exception" + ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of all methods available for the specified service.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static List<string> EnumerateServiceMethods(string serviceName)
        {
            log.LogMethodEntry(serviceName);

            List<string> methods = new List<string>();

            if (!availableTypes.ContainsKey(serviceName))
            {
                log.LogMethodExit(null, "Throwing Exception - Service Not Available");
                throw new Exception("Service Not Available");
            }
            else
            {
                Type type = availableTypes[serviceName];

                // only find methods of this object type (the one we generated)
                // we don't want inherited members (this type inherited from SoapHttpClientProtocol)
                foreach (MethodInfo minfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    methods.Add(minfo.Name);

                log.LogMethodExit(methods);
                return methods;
            }
        }

        /// <summary>
        /// Invokes the specified method of the named service.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="serviceName">The name of the service to use.</param>
        /// <param name="methodName">The name of the method to call.</param>
        /// <param name="args">The arguments to the method.</param>
        /// <returns>The return value from the web service method.</returns>
        public static T InvokeMethod<T>(string serviceName, string methodName, params object[] args)
        {
            log.LogMethodEntry(serviceName, methodName, args);

            // create an instance of the specified service
            // and invoke the method
            try
            {
                object obj = webServiceAssembly.CreateInstance(serviceName);

                Type type = obj.GetType();

                T returnValueNew = (T)type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, obj, args);
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while invoking method", ex);
                throw;
            }
        }

        /// <summary>
        /// Builds the web service description importer, which allows us to generate a proxy class based on the 
        /// content of the WSDL described by the XmlTextReader.
        /// </summary>
        /// <param name="xmlreader">The WSDL content, described by XML.</param>
        /// <returns>A ServiceDescriptionImporter that can be used to create a proxy class.</returns>
        private static ServiceDescriptionImporter BuildServiceDescriptionImporter(XmlTextReader xmlreader)
        {
            log.LogMethodEntry(xmlreader);

            // make sure xml describes a valid wsdl
            if (!ServiceDescription.CanRead(xmlreader))
            {
                log.LogMethodExit(null, "Throwing Exception - Invalid Web Service Description");
                throw new Exception("Invalid Web Service Description");
            }

            // parse wsdl
            ServiceDescription serviceDescription = ServiceDescription.Read(xmlreader);
            // build an importer, that assumes the SOAP protocol, client binding, and generates properties
            ServiceDescriptionImporter descriptionImporter = new ServiceDescriptionImporter();
            try
            {
                descriptionImporter.ProtocolName = "Soap";
                descriptionImporter.AddServiceDescription(serviceDescription, null, null);
                descriptionImporter.Style = ServiceDescriptionImportStyle.Client;
                descriptionImporter.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while initializing values to Desription Importer", ex);
                log.LogMethodExit(null, "");
                throw;
            }

            log.LogMethodExit(descriptionImporter);
            return descriptionImporter;
        }

        /// <summary>
        /// Compiles an assembly from the proxy class provided by the ServiceDescriptionImporter.
        /// </summary>
        /// <param name="descriptionImporter"></param>
        /// <returns>An assembly that can be used to execute the web service methods.</returns>
        private static Assembly CompileAssembly(ServiceDescriptionImporter descriptionImporter)
        {
            log.LogMethodEntry(descriptionImporter);

            // a namespace and compile unit are needed by importer
            CodeNamespace codeNamespace = new CodeNamespace();
            CodeCompileUnit codeUnit = new CodeCompileUnit();

            codeUnit.Namespaces.Add(codeNamespace);

            ServiceDescriptionImportWarnings importWarnings = descriptionImporter.Import(codeNamespace, codeUnit);

            if (importWarnings == 0) // no warnings
            {
                // create a c# compiler
                CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");

                // include the assembly references needed to compile
                //string[] references = new string[2] { "System.Web.Services.dll", "System.Xml.dll" };
                string[] references = new string[3] { "System.Web.Services.dll", "System.Xml.dll", "System.Data.dll" };

                CompilerParameters parameters = new CompilerParameters(references);

                // compile into assembly
                CompilerResults results = compiler.CompileAssemblyFromDom(parameters, codeUnit);

                foreach (CompilerError oops in results.Errors)
                {
                    // trap these errors and make them available to exception object
                    log.LogMethodExit(null, "Throwing Exception - Compilation Error Creating Assembly");
                    throw new Exception("Compilation Error Creating Assembly");
                }

                // all done....
                log.LogMethodExit(results.CompiledAssembly);
                return results.CompiledAssembly;
            }
            else
            {
                // warnings issued from importers, something wrong with WSDL
                log.LogMethodExit(null, "Throwing Exception - Invalid WSDL");
                throw new Exception("Invalid WSDL");
            }
        }

        /// <summary>
        /// Builds an assembly from a web service description.
        /// The assembly can be used to execute the web service methods.
        /// </summary>
        /// <param name="webServiceUri">Location of WSDL.</param>
        /// <returns>A web service assembly.</returns>
        private static Assembly BuildAssemblyFromWSDL(Uri webServiceUri)
        {
            log.LogMethodEntry(webServiceUri);

            if (String.IsNullOrEmpty(webServiceUri.ToString()))
            {
                log.LogMethodExit(null, "Throwing Exception - Web Service Not Found");
                throw new Exception("Web Service Not Found");
            }

            XmlTextReader xmlreader = new XmlTextReader(webServiceUri.ToString() + "?wsdl");

            ServiceDescriptionImporter descriptionImporter = BuildServiceDescriptionImporter(xmlreader);

            Assembly returnValueNew = CompileAssembly(descriptionImporter);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        static Assembly webServiceAssembly;
        static List<string> services;
    }
}
