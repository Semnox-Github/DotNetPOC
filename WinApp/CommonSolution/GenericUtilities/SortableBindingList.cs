/********************************************************************************************
 * Project Name - Sortable Binding List
 * Description  - Sortable Binding List is usefull for grid sort
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Feb-2016   Raghuveera          Created 
 *2.70.2        10-Aug-2019   Deeksha             Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{

    /// <summary>
    /// This class can be used as follows
    /// yourDTOListObject can be binded with your grid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortableBindingList<T> : BindingList<T>
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isSortedValue;
        ListSortDirection sortDirectionValue;
        PropertyDescriptor sortPropertyValue;
        /// <summary>
        /// Default constructor
        /// </summary>
        public SortableBindingList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="list"></param>
        public SortableBindingList(IList<T> list)
        {
            log.LogMethodEntry(list);
            foreach (object o in list)
            {
                this.Add((T)o);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ApplySortCore
        /// </summary>
        /// <param name="prop"> PropertyDescriptor</param>
        /// <param name="direction">ListSortDirection</param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            log.LogMethodEntry(prop, direction);
            Type interfaceType = prop.PropertyType.GetInterface("IComparable");

            if (interfaceType == null && prop.PropertyType.IsValueType)
            {
                Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                if (underlyingType != null)
                {
                    interfaceType = underlyingType.GetInterface("IComparable");
                }
            }

            if (interfaceType != null)
            {
                sortPropertyValue = prop;
                sortDirectionValue = direction;

                IEnumerable<T> query = base.Items;

                if (direction == ListSortDirection.Ascending)
                {
                    query = query.OrderBy(i => prop.GetValue(i));
                }
                else
                {
                    query = query.OrderByDescending(i => prop.GetValue(i));
                }

                int newIndex = 0;
                foreach (object item in query)
                {
                    this.Items[newIndex] = (T)item;
                    newIndex++;
                }

                isSortedValue = true;
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
            {
                throw new NotSupportedException("Cannot sort by " + prop.Name + ". This" + prop.PropertyType.ToString() + " does not implement IComparable");
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get method for SortPropertyCore
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortPropertyValue; }
        }
        /// <summary>
        /// Get method for SortDirectionCore
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirectionValue; }
        }
        /// <summary>
        /// Get method for SupportsSortingCore
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }
        /// <summary>
        /// Get method for IsSortedCore
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return isSortedValue; }
        }
    }

}
