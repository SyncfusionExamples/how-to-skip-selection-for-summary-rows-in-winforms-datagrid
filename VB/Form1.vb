Imports Microsoft.VisualBasic
Imports Syncfusion.WinForms.Controls
Imports Syncfusion.WinForms.DataGrid.Styles
Imports System.Windows.Forms
Imports System.Linq
Imports Syncfusion.WinForms.DataGrid.Enums
Imports Syncfusion.WinForms.GridCommon.ScrollAxis
Imports Syncfusion.WinForms.DataGrid
Imports Syncfusion.Data
Imports Syncfusion.WinForms.DataGrid.Interactivity
Imports Syncfusion.WinForms.DataGrid.Renderers
Imports Syncfusion.WinForms.Input

Namespace GettingStarted
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			Dim data = New OrderInfoCollection()
			sfDataGrid.DataSource = data.OrdersListDetails

			Dim tableSummaryRow1 As New GridTableSummaryRow()
			tableSummaryRow1.Name = "TableSummary"
			tableSummaryRow1.ShowSummaryInRow = True
			tableSummaryRow1.Title = " Total Product Count: {ProductName}"
			tableSummaryRow1.Position = VerticalPosition.Bottom

			Dim summaryColumn1 As New GridSummaryColumn()
			summaryColumn1.Name = "ProductName"
			summaryColumn1.SummaryType = SummaryType.CountAggregate
			summaryColumn1.Format = "{Count}"
			summaryColumn1.MappingName = "ProductName"

			tableSummaryRow1.SummaryColumns.Add(summaryColumn1)

			Me.sfDataGrid.TableSummaryRows.Add(tableSummaryRow1)


			Dim groupSummaryRow1 As New GridSummaryRow()
			groupSummaryRow1.Name = "GroupSummary"
			groupSummaryRow1.ShowSummaryInRow = True
			groupSummaryRow1.Title = " Total Product Count: {ProductName}"


			' Adds the GridSummaryColumn in SummaryColumns collection.
			groupSummaryRow1.SummaryColumns.Add(summaryColumn1)

			' Adds the summary row in the GroupSummaryRows collection.
			Me.sfDataGrid.GroupSummaryRows.Add(groupSummaryRow1)


			Me.sfDataGrid.SelectionController = New CustomRowSelectionController(Me.sfDataGrid)

			Me.sfDataGrid.AutoExpandGroups = True
			Me.sfDataGrid.GroupColumnDescriptions.Add(New GroupColumnDescription() With {.ColumnName = "OrderDate"})
			Me.sfDataGrid.ClearSelection()
		End Sub
	End Class

	Public Class CustomRowSelectionController
		Inherits RowSelectionController
		Private DataGrid As SfDataGrid

		Public Sub New(ByVal sfDataGrid As SfDataGrid)
			MyBase.New(sfDataGrid)
			Me.DataGrid = sfDataGrid
		End Sub

		Protected Overrides Sub HandlePointerOperations(ByVal args As Syncfusion.WinForms.DataGrid.Events.DataGridPointerEventArgs, ByVal rowColumnIndex As RowColumnIndex)
			If Me.IsCaptionSummaryRow(rowColumnIndex.RowIndex) OrElse Me.IsGroupSummaryRow(rowColumnIndex.RowIndex) Then
				Return
			End If
			MyBase.HandlePointerOperations(args, rowColumnIndex)
		End Sub

		Protected Overrides Sub ProcessArrowKeysForSingleMultipleSelection(ByVal args As KeyEventArgs)
			If args.KeyCode = Keys.Up Then
				Me.DataGrid.MoveToCurrentCell(New RowColumnIndex(Me.GetPreviousRecordRowIndex(Me.DataGrid.CurrentCell.RowIndex), Me.DataGrid.CurrentCell.ColumnIndex))
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
End Namespace
