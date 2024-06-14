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
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class CellGridViewModel: ViewModelBase
    {
        private int rowCount = 0;
        private int columnCount = 0;
        private List<CellViewModel> cellViewModelList = new List<CellViewModel>();
        private Dictionary<int, Dictionary<int, CellViewModel>> cellViewModelMatrix = new Dictionary<int, Dictionary<int, CellViewModel>>();

        public CellGridViewModel(int rowCount, int columnCount)
        {
            AddRows(rowCount);
            AddColumns(columnCount);
        }


        public CellViewModel this[int rowIndex, int columnIndex]
        {
            get
            {
                if (IsValidIndeces(rowIndex, columnIndex) == false)
                {
                    return CellViewModel.Null;
                }
                return cellViewModelMatrix[rowIndex][columnIndex];
            }
        }

        private bool IsValidIndeces(int rowIndex, int columnIndex)
        {
            return cellViewModelMatrix.ContainsKey(rowIndex) &&
                    cellViewModelMatrix[rowIndex].ContainsKey(columnIndex);
        }

        public List<CellViewModel> CellViewModelList
        {
            get
            {
                return cellViewModelList;
            }
        }

        public List<CellViewModel> GetColumn(int columnIndex)
        {
            return GetCellViewModelListOfRange(0, columnIndex, rowCount, 1);
        }

        private List<CellViewModel> GetOccupyableCellViewModelList(ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel)
        {
            return GetCellViewModelListOfRange(menuPanelContentSetupViewModel.RowIndex, menuPanelContentSetupViewModel.ColumnIndex, menuPanelContentSetupViewModel.ButtonType.VerticalCellCount, menuPanelContentSetupViewModel.ButtonType.HorizontalCellCount);
        }

        public List<CellViewModel> GetEmptyCellViewModelList(int rowIndex, int columnIndex, int verticalCellCount, int horizontalCellCount)
        {
            List<CellViewModel> cellViewModels = GetCellViewModelListOfRange(rowIndex, columnIndex, verticalCellCount, horizontalCellCount);
            if (cellViewModels.Any(x => x.IsOccupied))
            {
                return new List<CellViewModel>();
            }
            return cellViewModels;
        }

        public List<CellViewModel> GetCellViewModelListOfRange(int rowIndex, int columnIndex, int verticalCellCount, int horizontalCellCount)
        {
            List<CellViewModel> cellViewModels = new List<CellViewModel>();

            for (int i = 0; i < horizontalCellCount; i++)
            {
                for (int j = 0; j < verticalCellCount; j++)
                {
                    if (IsValidIndeces(rowIndex + j, columnIndex + i) == false)
                    {
                        return new List<CellViewModel>();
                    }
                    CellViewModel cellViewModel = this[rowIndex + j, columnIndex + i];
                    cellViewModels.Add(cellViewModel);
                }
            }
            return cellViewModels;
        }



        public List<CellViewModel> GetRow(int rowIndex)
        {
            return GetCellViewModelListOfRange(rowIndex, 0, 1, columnCount);
        }

        private void AddRows(int noOfRows)
        {
            for (int i = rowCount; i < rowCount + noOfRows; i++)
            {
                cellViewModelMatrix.Add(i, new Dictionary<int, CellViewModel>());
                for (int j = 0; j < columnCount; j++)
                {
                    CellViewModel cell = new CellViewModel(i, j);
                    cellViewModelMatrix[i].Add(j, cell);
                    cellViewModelList.Add(cell);
                }
            }
            rowCount += noOfRows;
        }

        private void RemoveRows(int noOfRows)
        {
            for (int i = 0; i < noOfRows; i++)
            {
                List<CellViewModel> cellViewModels = GetRow(rowCount - 1);
                foreach (var cellViewModel in cellViewModels)
                {
                    RemoveCellViewModel(cellViewModel);
                }
                rowCount--;
            }
        }

        private void AddColumns(int noOfColumns)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = columnCount; j < columnCount + noOfColumns; j++)
                {
                    CellViewModel cell = new CellViewModel(i, j);
                    cellViewModelMatrix[i].Add(j, cell);
                    cellViewModelList.Add(cell);
                }
            }
            columnCount += noOfColumns;
        }

        private void RemoveColumns(int noOfColumns)
        {
            for (int i = 0; i < noOfColumns; i++)
            {
                List<CellViewModel> cellViewModels = GetColumn(columnCount - 1);
                foreach (var cellViewModel in cellViewModels)
                {
                    RemoveCellViewModel(cellViewModel);
                }
                columnCount--;
            }
        }

        private void RemoveCellViewModel(CellViewModel cellViewModel)
        {
            cellViewModelList.Remove(cellViewModel);
            if(cellViewModelMatrix.ContainsKey(cellViewModel.RowIndex) && cellViewModelMatrix[cellViewModel.RowIndex].ContainsKey(cellViewModel.ColumnIndex))
            {
                cellViewModelMatrix[cellViewModel.RowIndex].Remove(cellViewModel.ColumnIndex);
                if(cellViewModelMatrix[cellViewModel.RowIndex].Count == 0)
                {
                    cellViewModelMatrix.Remove(cellViewModel.RowIndex);
                }
            }
        }

        internal void AddProductMenuPanelContentSetupViewModel(ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel)
        {
            foreach (var cellViewModel in GetOccupyableCellViewModelList(menuPanelContentSetupViewModel))
            {
                cellViewModel.Occupy(menuPanelContentSetupViewModel);
            }
        }

        internal void RemoveProductMenuPanelContentSetupViewModel(ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel)
        {
            foreach (var cellViewModel in GetOccupyableCellViewModelList(menuPanelContentSetupViewModel))
            {
                cellViewModel.Leave(menuPanelContentSetupViewModel);
            }
        }

        internal bool CanButtonBePlacedAt(int rowIndex, int columnIndex, int verticalCellCount, int horizontalCellCount)
        {
            CellViewModel topLeftCellModel = this[rowIndex, columnIndex];
            
            return cellViewModelList.Any() && cellViewModelList.Where(x => x.IsOccupied).Any(x => x.ProductMenuPanelContentSetupViewModel.IsContentAssigned) == false;
        }

        public List<ProductMenuPanelContentSetupViewModel> GetProductMenuPanelContentSetupViewModelListInRange(int rowIndex, int columnIndex, int verticalCellCount, int horizontalCellCount)
        {
            List<CellViewModel> cellViewModelList = GetCellViewModelListOfRange(rowIndex, columnIndex, verticalCellCount, horizontalCellCount);
            return cellViewModelList.Where(x => x.IsOccupied).Select(x => x.ProductMenuPanelContentSetupViewModel).Distinct().ToList();
        }

        public int RowCount
        {
            get
            {
                return rowCount;
            }
            set
            {
                if (value > rowCount)
                {
                    AddRows(value - rowCount);
                }
                else if (value < rowCount)
                {
                    RemoveRows(rowCount - value);
                }
            }
        }

        public int ColumnCount
        {
            get
            {
                return columnCount;
            }
            set
            {
                if (value > columnCount)
                {
                    AddColumns(value - columnCount);
                }
                else if (value < columnCount)
                {
                    RemoveColumns(columnCount - value);
                }
            }
        }
    }
}
