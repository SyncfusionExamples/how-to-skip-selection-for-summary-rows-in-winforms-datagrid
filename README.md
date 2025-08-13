# How to skip selection for summary rows?

## About the example

This example illustrates how to create a custom selection controller for the SfDataGrid to skip selection for the caption summary and group summary rows.

By default, the group summary and caption summary rows will be selected on both mouse click and arrow keys navigation. You can skip the pointer selection and arrow key navigation for these summary rows by creating a custom SelectionController and overriding the [HandlePointerOperations](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.Interactivity.RowSelectionController.html#Syncfusion_WinForms_DataGrid_Interactivity_RowSelectionController_HandlePointerOperations_Syncfusion_WinForms_DataGrid_Events_DataGridPointerEventArgs_Syncfusion_WinForms_GridCommon_ScrollAxis_RowColumnIndex_) and [ProcessArrowKeysForSingleMultipleSelection](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.Interactivity.RowSelectionController.html#Syncfusion_WinForms_DataGrid_Interactivity_RowSelectionController_ProcessArrowKeysForSingleMultipleSelection_System_Windows_Forms_KeyEventArgs_) methods respectively.

KB article: https://support.syncfusion.com/kb/article/8461/how-to-skip-selection-for-summary-rows-in-winforms-datagrid-sfdatagrid
