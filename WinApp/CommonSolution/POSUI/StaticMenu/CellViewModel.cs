/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using Semnox.Parafait.CommonUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class CellViewModel: ViewModelBase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly CellViewModel Null = new CellViewModel(-1, -1);
        private int rowIndex;
        private int columnIndex;
        private ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel;

        public CellViewModel(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
        }

        public int RowIndex
        {
            get
            {
                return rowIndex;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return columnIndex;
            }
        }

        private bool IsNullCellViewModel()
        {
            return rowIndex == -1 || columnIndex == -1;
        }

        public void Occupy(ProductMenuPanelContentSetupViewModel value)
        {
            if(IsNullCellViewModel())
            {
                throw new InvalidOperationException();
            }
            if(menuPanelContentSetupViewModel != null)
            {
                throw new InvalidOperationException();
            }
            menuPanelContentSetupViewModel = value;
        }

        public void Leave(ProductMenuPanelContentSetupViewModel value)
        {
            if(IsNullCellViewModel())
            {
                throw new InvalidOperationException();
            }
            if(menuPanelContentSetupViewModel == null || value != menuPanelContentSetupViewModel)
            {
                throw new InvalidOperationException();
            }
            menuPanelContentSetupViewModel = null;
        }

        public ProductMenuPanelContentSetupViewModel ProductMenuPanelContentSetupViewModel
        {
            get
            {
                return menuPanelContentSetupViewModel;
            }
        }

        public bool IsOccupied
        {
            get
            {
                return menuPanelContentSetupViewModel != null;
            }
        }


        private static bool EqualOperator(CellViewModel left, CellViewModel right)
        {
            log.LogMethodEntry(left, right);
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                log.LogMethodExit(false);
                return false;
            }
            bool returnvalue = ReferenceEquals(left, null) || left.Equals(right);
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        private static bool NotEqualOperator(CellViewModel left, CellViewModel right)
        {
            log.LogMethodEntry(left, right);
            bool returnvalue = !(EqualOperator(left, right));
            log.LogMethodEntry(returnvalue);
            return returnvalue;
        }

        protected IEnumerable<object> GetAtomicValues()
        {
            yield return rowIndex;
            yield return columnIndex;
        }

        public override bool Equals(object obj)
        {
            log.LogMethodEntry(obj);
            if (obj == null || obj.GetType() != GetType())
            {
                log.LogMethodEntry(false);
                return false;
            }

            CellViewModel other = (CellViewModel)obj;
            using (IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator())
            {
                using (IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator())
                {
                    while (thisValues.MoveNext() && otherValues.MoveNext())
                    {
                        if (ReferenceEquals(thisValues.Current, null) ^
                            ReferenceEquals(otherValues.Current, null))
                        {
                            log.LogMethodEntry(false);
                            return false;
                        }

                        if (thisValues.Current != null &&
                            !thisValues.Current.Equals(otherValues.Current))
                        {
                            log.LogMethodEntry(false);
                            return false;
                        }
                    }
                    bool returnvalue = !thisValues.MoveNext() && !otherValues.MoveNext();
                    log.LogMethodEntry(returnvalue);
                    return returnvalue;
                }
            }
        }

        public override int GetHashCode()
        {
            log.LogMethodEntry();
            int returnValue = GetAtomicValues()
             .Select(x => x != null ? x.GetHashCode() : 0)
             .Aggregate((x, y) => x ^ y);
            log.LogMethodEntry(returnValue);
            return returnValue;
        }

        public static bool operator ==(CellViewModel cellViewModel1, CellViewModel cellViewModel2)
        {
            log.LogMethodEntry(cellViewModel1, cellViewModel2);
            bool returnValue = EqualOperator(cellViewModel1, cellViewModel2);
            log.LogMethodEntry(returnValue);
            return returnValue;
        }

        public static bool operator !=(CellViewModel cellViewModel1, CellViewModel cellViewModel2)
        {
            log.LogMethodEntry(cellViewModel1, cellViewModel2);
            bool returnValue = NotEqualOperator(cellViewModel1, cellViewModel2);
            log.LogMethodEntry(returnValue);
            return returnValue;
        }

        public override string ToString()
        {
            return "R:" + rowIndex + "C:" + columnIndex; 
        }

    }

}
