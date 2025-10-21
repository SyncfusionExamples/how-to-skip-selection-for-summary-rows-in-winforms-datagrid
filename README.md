# How to Skip Selection for Summary Rows in WinForms DataGrid?

This example illustrates how to create a custom selection controller for the [WinForms DataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid) to skip selection for the caption summary and group summary rows.

By default, the group summary and caption summary rows will be selected on both mouse click and arrow keys navigation. You can skip the pointer selection and arrow key navigation for these summary rows by creating a custom SelectionController and overriding the [HandlePointerOperations](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.Interactivity.RowSelectionController.html#Syncfusion_WinForms_DataGrid_Interactivity_RowSelectionController_HandlePointerOperations_Syncfusion_WinForms_DataGrid_Events_DataGridPointerEventArgs_Syncfusion_WinForms_GridCommon_ScrollAxis_RowColumnIndex_) and [ProcessArrowKeysForSingleMultipleSelection](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.Interactivity.RowSelectionController.html#Syncfusion_WinForms_DataGrid_Interactivity_RowSelectionController_ProcessArrowKeysForSingleMultipleSelection_System_Windows_Forms_KeyEventArgs_) methods respectively.

### C#

``` csharp
public Form1()
{
    InitializeComponent();    
    this.sfDataGrid.SelectionController = new CustomRowSelectionController(this.sfDataGrid);
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
```

### VB

``` vb
Public Sub New()
    InitializeComponent()
    Me.sfDataGrid.SelectionController = New CustomRowSelectionController(Me.sfDataGrid)
End Sub
 
Public Class CustomRowSelectionController Inherits RowSelectionController

    Private DataGrid As SfDataGrid

    Public Sub New(ByVal sfDataGrid As SfDataGrid)
         MyBase.New(sfDataGrid)
         Me.DataGrid = sfDataGrid
    End Sub
 
    Protected Overrides Sub HandlePointerOperations(ByVal args As Syncfusion.WinForms.DataGrid.Events.DataGridPointerEventArgs, ByVal rowColumnIndex As RowColumnIndex)
       If Me.IsCaptionSummaryRow(rowColumnIndex.RowIndex) OrElse         Me.IsGroupSummaryRow(rowColumnIndex.RowIndex) Then
            Return
       End If
       MyBase.HandlePointerOperations(args, rowColumnIndex)
    End Sub
 
    Protected Overrides Sub ProcessArrowKeysForSingleMultipleSelection(ByVal args As KeyEventArgs)
       If args.KeyCode = Keys.Up Then
          Me.DataGrid.MoveToCurrentCell(New RowColumnIndex(Me.GetPreviousRecordRowIndex(Me.DataGrid.CurrentCell.RowIndex),       Me.DataGrid.CurrentCell.ColumnIndex))
       ElseIf args.KeyCode = Keys.Down Then
          Me.DataGrid.MoveToCurrentCell(New RowColumnIndex(Me.GetNextRecordRowIndex(Me.DataGrid.CurrentCell.RowIndex), Me.DataGrid.CurrentCell.ColumnIndex))
       Else
          MyBase.ProcessArrowKeysForSingleMultipleSelection(args)
       End If
    End Sub
 
    Private Function IsCaptionSummaryRow(ByVal rowIndex As Integer) As Boolean
       Dim startIndex = Me.DataGrid.TableControl.ResolveStartIndexBasedOnPosition()
       Dim record = Me.DataGrid.View.TopLevelGroup.DisplayElements(rowIndex - startIndex)
       Return record IsNot Nothing AndAlso TypeOf record Is Group
    End Function
 
    Private Function IsGroupSummaryRow(ByVal rowIndex As Integer) As Boolean
       Dim startIndex = Me.DataGrid.TableControl.ResolveStartIndexBasedOnPosition()
       Dim record = Me.DataGrid.View.TopLevelGroup.DisplayElements(rowIndex - startIndex)
       Return record IsNot Nothing AndAlso TypeOf record Is SummaryRecordEntry
    End Function
 
    Private Function GetNextRecordRowIndex(ByVal currentRowIndex As Integer) As Integer
       Dim nextRecordRowIndex As Integer = currentRowIndex + 1
 
       If nextRecordRowIndex > Me.GetLastRowIndex(Me.DataGrid) Then
          Return Me.DataGrid.CurrentCell.RowIndex
       End If
 
       If (Not Me.IsCaptionSummaryRow(nextRecordRowIndex)) AndAlso (Not Me.IsGroupSummaryRow(nextRecordRowIndex)) Then
          Return nextRecordRowIndex
       Else
          Return GetNextRecordRowIndex(nextRecordRowIndex)
       End If
    End Function
 
    Private Function GetLastRowIndex(ByVal dataGrid As SfDataGrid) As Integer
        If dataGrid.View.Records.Count = 0 Then
           Return -1
        End If
        Dim footerCount = dataGrid.GetUnboundRowsCount(VerticalPosition.Bottom, True)
        Dim count As Integer = 0
        Dim index As Integer = dataGrid.RowCount - (dataGrid.TableControl.GetTableSummaryCount(VerticalPosition.Bottom) + footerCount + 1)
        If dataGrid.AddNewRowPosition = RowPosition.Bottom Then
          index -= 1
       End If
       If dataGrid.FilterRowPosition = RowPosition.Bottom Then
          index -= 1
       End If
       For start As Integer = index To 0 Step -1
          If Not dataGrid.TableControl.RowHeights.GetHidden(start, count) Then
             Return start
          End If
       Next start
       Return index
    End Function
 
    Private Function GetPreviousRecordRowIndex(ByVal currentRowIndex As Integer) As Integer
        Dim previousRecordRowIndex As Integer = currentRowIndex - 1
        If previousRecordRowIndex <= 0 Then
          Return Me.DataGrid.CurrentCell.RowIndex
        End If
        If (Not Me.IsCaptionSummaryRow(previousRecordRowIndex)) AndAlso (Not Me.IsGroupSummaryRow(previousRecordRowIndex)) Then
          Return previousRecordRowIndex
        Else
          Return GetPreviousRecordRowIndex(previousRecordRowIndex)
        End If
    End Function
End Class
```