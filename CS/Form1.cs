using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid.Styles;
using System.Windows.Forms;
using System.Linq;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid.Renderers;
using Syncfusion.WinForms.Input;

namespace GettingStarted
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var data = new OrderInfoCollection();
            sfDataGrid.DataSource = data.OrdersListDetails;

            GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
            tableSummaryRow1.Name = "TableSummary";
            tableSummaryRow1.ShowSummaryInRow = true;
            tableSummaryRow1.Title = " Total Product Count: {ProductName}";
            tableSummaryRow1.Position = VerticalPosition.Bottom;

            GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
            summaryColumn1.Name = "ProductName";
            summaryColumn1.SummaryType = SummaryType.CountAggregate;
            summaryColumn1.Format = "{Count}";
            summaryColumn1.MappingName = "ProductName";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

            this.sfDataGrid.TableSummaryRows.Add(tableSummaryRow1);


            GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
            groupSummaryRow1.Name = "GroupSummary";
            groupSummaryRow1.ShowSummaryInRow = true;
            groupSummaryRow1.Title = " Total Product Count: {ProductName}";


            // Adds the GridSummaryColumn in SummaryColumns collection.
            groupSummaryRow1.SummaryColumns.Add(summaryColumn1);

            // Adds the summary row in the GroupSummaryRows collection.
            this.sfDataGrid.GroupSummaryRows.Add(groupSummaryRow1);


            this.sfDataGrid.SelectionController = new CustomRowSelectionController(this.sfDataGrid);

            this.sfDataGrid.AutoExpandGroups = true;
            this.sfDataGrid.GroupColumnDescriptions.Add(new GroupColumnDescription() { ColumnName = "OrderDate" });
            this.sfDataGrid.ClearSelection();
        }
    }

    public class CustomRowSelectionController : RowSelectionController
    {
        SfDataGrid DataGrid;

        public CustomRowSelectionController(SfDataGrid sfDataGrid)
            : base(sfDataGrid)
        {
            this.DataGrid = sfDataGrid;
        }

        protected override void HandlePointerOperations(Syncfusion.WinForms.DataGrid.Events.DataGridPointerEventArgs args, RowColumnIndex rowColumnIndex)
        {
            if (this.IsCaptionSummaryRow(rowColumnIndex.RowIndex) || this.IsGroupSummaryRow(rowColumnIndex.RowIndex))
                return;
            base.HandlePointerOperations(args, rowColumnIndex);
        }

        protected override void ProcessArrowKeysForSingleMultipleSelection(KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Up)
            {
                this.DataGrid.MoveToCurrentCell(new RowColumnIndex(this.GetPreviousRecordRowIndex(this.DataGrid.CurrentCell.RowIndex), this.DataGrid.CurrentCell.ColumnIndex));
            }
            else if (args.KeyCode == Keys.Down)
            {
                this.DataGrid.MoveToCurrentCell(new RowColumnIndex(this.GetNextRecordRowIndex(this.DataGrid.CurrentCell.RowIndex), this.DataGrid.CurrentCell.ColumnIndex));
            }
            else
                base.ProcessArrowKeysForSingleMultipleSelection(args);
        }

        bool IsCaptionSummaryRow(int rowIndex)
        {
            var startIndex = this.DataGrid.TableControl.ResolveStartIndexBasedOnPosition();
            var record = this.DataGrid.View.TopLevelGroup.DisplayElements[rowIndex - startIndex];

            return record != null && record is Group;
        }

        bool IsGroupSummaryRow(int rowIndex)
        {
            var startIndex = this.DataGrid.TableControl.ResolveStartIndexBasedOnPosition();
            var record = this.DataGrid.View.TopLevelGroup.DisplayElements[rowIndex - startIndex];

            return record != null && record is SummaryRecordEntry;
        }



        private int GetNextRecordRowIndex(int currentRowIndex)
        {
            int nextRecordRowIndex = currentRowIndex + 1;

            if (nextRecordRowIndex > this.GetLastRowIndex(this.DataGrid))
                return this.DataGrid.CurrentCell.RowIndex;

            if (!this.IsCaptionSummaryRow(nextRecordRowIndex) && !this.IsGroupSummaryRow(nextRecordRowIndex))
                return nextRecordRowIndex;
            else
                return GetNextRecordRowIndex(nextRecordRowIndex);
        }

        private int GetLastRowIndex(SfDataGrid dataGrid)
        {
            if (dataGrid.View.Records.Count == 0)
                return -1;
            var footerCount = dataGrid.GetUnboundRowsCount(VerticalPosition.Bottom, true);
            int count = 0;
            int index = dataGrid.RowCount - (dataGrid.TableControl.GetTableSummaryCount(VerticalPosition.Bottom) + footerCount + 1);
            if (dataGrid.AddNewRowPosition == RowPosition.Bottom)
                index -= 1;
            if (dataGrid.FilterRowPosition == RowPosition.Bottom)
                index -= 1;
            for (int start = index; start >= 0; start--)
            {
                if (!dataGrid.TableControl.RowHeights.GetHidden(start, out count))
                    return start;
            }

            return index;
        }

        private int GetPreviousRecordRowIndex(int currentRowIndex)
        {
            int previousRecordRowIndex = currentRowIndex - 1;

            if (previousRecordRowIndex <= 0)
                return this.DataGrid.CurrentCell.RowIndex;

            if (!this.IsCaptionSummaryRow(previousRecordRowIndex) && !this.IsGroupSummaryRow(previousRecordRowIndex))
                return previousRecordRowIndex;
            else
                return GetPreviousRecordRowIndex(previousRecordRowIndex);
        }
    }
}
