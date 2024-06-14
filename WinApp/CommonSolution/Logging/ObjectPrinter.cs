/********************************************************************************************
 * Project Name - ObjectPrinter
 * Description  - Business Logic for ObjectPrinter
 *  
 **************
 * Version Log
 **************
 * Version       Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60.2      13-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        26-Nov-2019      Lakshminarayana      Prevented Loginrequest object from logged
 **********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logging
{
    /// <summary>
    /// Converts the object to the formated string. 
    /// </summary>
    public class ObjectPrinter
    {
        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <param name="element">object to be dumped</param>
        /// <returns></returns>
        public static string GetString(object element)
        {
            StringWriter sw = new StringWriter();
            ObjectPrinter objectPrinter = new ObjectPrinter(2);
            objectPrinter.writer = sw;
            objectPrinter.WriteObject(null, element);
            return sw.ToString();
        }

        TextWriter writer;
        int pos;
        int level;
        int depth;

        private ObjectPrinter(int depth)
        {
            this.depth = depth;
        }

        private void Write(string s)
        {
            if (s != null)
            {
                writer.Write(s);
                pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < level; i++) writer.Write("  ");
        }

        private void WriteLine()
        {
            writer.WriteLine();
            pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (pos % 8 != 0) Write(" ");
        }

        private bool IsProhibitedType(string typeName)
        {
            bool returnValue = false;
            if (typeName.Contains("System.Data") ||
                typeName.Contains("System.Net") ||
                typeName.Contains("System.IO") ||
                typeName.Contains("System.Windows") ||
                typeName.Contains("System.Web") ||
                typeName.Contains("Microsoft.") ||
                typeName.Contains("Semnox.Parafait.Transaction.CustomerCreditCardsDTO") ||
                typeName.Contains("Semnox.Parafait.Device.PaymentGateway.CCTransactionsPGWDTO") ||
                typeName.Contains("Semnox.Parafait.Transaction.V2.PaymentTransactionDTO") ||
                typeName.Contains("Semnox.Parafait.Transaction.V2.TransactionPaymentDTO") ||
                typeName.Contains("Semnox.Core.Utilities.Utilities") ||
                typeName.Contains("Semnox.Parafait.User.UsersDTO") ||
                typeName.Contains("Semnox.Parafait.User.UserAuthParams") ||
                typeName.Contains("Semnox.Core.Utilities.CoreKeyValueStruct") ||
                typeName.Contains("Semnox.Parafait.Customer.CustomerParams") ||
                typeName.Contains("Semnox.Parafait.Customer.CustomersDTO") ||
                typeName.Contains("Semnox.Parafait.Customer.CustomerDTO") ||
                typeName.Contains("Semnox.Parafait.Customer.ProfileDTO") ||
                typeName.Contains("Semnox.Parafait.Customer.ContactDTO") ||
                typeName.Contains("Semnox.Parafait.Customer.AddressDTO") ||
                typeName.Contains("Semnox.Core.Utilities.LoginRequest") ||
                typeName.Contains("System.Net.NetworkCredential") ||
                typeName.Contains("Parafait_POS.frmWaiverSignature") ||
                typeName.Contains("Semnox.Core.Utilities.MifareKeyContainerDTO") ||
                typeName.Contains("Semnox.Core.Utilities.SystemOptionContainerDTO") ||
                typeName.Contains("Semnox.Core.GenericUtilities.UltralightCKey"))
            {
                returnValue = true;
            }
            return returnValue;
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                //WriteLine();
            }
            else
            {
                string typeName = element.GetType().FullName;
                if (IsProhibitedType(typeName))
                {
                    Write(typeName);
                    return;
                }
                IEnumerable enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (level < depth)
                            {
                                level++;
                                WriteObject(prefix, item);
                                level--;
                            }
                        }
                        else
                        {
                            WriteObject(string.IsNullOrWhiteSpace(prefix) ? " " : prefix, item);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    WriteIndent();
                    Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo m in members)
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            if (propWritten)
                            {
                                WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }
                            Write(m.Name);
                            Write("=");
                            Type t = f != null ? f.FieldType : p.PropertyType;
                            if (t.IsValueType || t == typeof(string))
                            {
                                try
                                {
                                    WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                                }
                                catch (Exception)
                                {
                                    WriteValue("undefined!!");
                                }
                            }
                            else
                            {
                                if (typeof(IEnumerable).IsAssignableFrom(t))
                                {
                                    Write("...");
                                }
                                else
                                {
                                    Write("{ }");
                                }
                            }
                        }
                    }
                    if (propWritten) WriteLine();
                    if (level < depth)
                    {
                        foreach (MemberInfo m in members)
                        {
                            FieldInfo f = m as FieldInfo;
                            PropertyInfo p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                Type t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    try
                                    {
                                        object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                        if (value != null)
                                        {
                                            level++;
                                            WriteObject(m.Name + ": ", value);
                                            level--;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
}
