using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Windows.Data;
using System.ComponentModel;

namespace AccountingUI.Wpf.Dialogs.PrintingDataGrid
{
    //Originally from http://www.codeproject.com/Articles/138233/Custom-Data-Grid-Document-Paginator
    //Added comment from user 'Member 10789205'
    //Updated with changes from http://khalidbudajaja.blogspot.com/2011/12/wpf-printing.html
    //Reworked to support ListViews as well as GridViews
    //Reworked to support variable height rows and text wrapping
    //Could use some efficiency work, but its effective
    /* Usage sample 
          PrintDialog printDialog = new PrintDialog();
          printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
          if (printDialog.ShowDialog() == false)
             return;

          string documentTitle = "Header Text"; 
          Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

          //styles for the printing are in the AppResourceDictionary.xaml
          CustomDataGridDocumentPaginator paginator = new CustomDataGridDocumentPaginator(view, documentTitle, pageSize, new Thickness(30, 20, 30, 20), true, App.Current.Resources);

          printDialog.PrintDocument(paginator, "Print Description - default filename for PDF printers");
    */
    public class CustomDataGridDocumentPaginator : DocumentPaginator
    {
        #region Private Members

        private ICollectionView _viewSource;
        private List<double> _actualHeights;
        private List<int> _startRows;
        private List<DependencyObject> _columns;

        private Collection<ColumnDefinition> _tableColumnDefinitions;

        private double _availableHeight;
        private int _pageCount;

        #endregion

        #region Constructor

        public CustomDataGridDocumentPaginator(DataGrid documentSource, string documentTitle, Size pageSize, Thickness pageMargin, bool wrap_text)
        {
            _tableColumnDefinitions = new Collection<ColumnDefinition>();
            _viewSource = documentSource.Items;
            _columns = documentSource.Columns.Select(c => (DependencyObject)c).ToList();

            this.DocumentTitle = documentTitle;
            this.PageSize = pageSize;
            this.PageMargin = pageMargin;
            this.PageDirection = documentSource.FlowDirection;
            this.WrapText = wrap_text;

            ReadStyles();

            if (_viewSource != null)
                MeasureElements();
        }

        public CustomDataGridDocumentPaginator(ListView documentSource, string documentTitle, Size pageSize, Thickness pageMargin, bool wrap_text)
        {
            _tableColumnDefinitions = new Collection<ColumnDefinition>();
            _viewSource = documentSource.Items;
            var view = documentSource.View as GridView;
            if (view != null)
                _columns = view.Columns.Select(c => (DependencyObject)c).ToList();

            this.DocumentTitle = documentTitle;
            this.PageSize = pageSize;
            this.PageMargin = pageMargin;
            this.PageDirection = documentSource.FlowDirection;
            this.WrapText = wrap_text;

            ReadStyles();

            if (view != null)
                MeasureElements();
        }

        private void ReadStyles()
        {
            var resources = new ResourceDictionary { Source = new Uri("ResourceDictionary.xaml", UriKind.Relative) };
            if (resources != null)
            {
                this.DocumentHeaderTextStyle = (Style)resources["DocumentHeaderTextStyle"];
                this.AlternatingRowBorderStyle = (Style)resources["AlternatingRowBorderStyle"];
                this.DocumentFooterTextStyle = (Style)resources["DocumentFooterTextStyle"];
                this.TableCellTextStyle = (Style)resources["TableCellTextStyle"];
                this.TableHeaderTextStyle = (Style)resources["TableHeaderTextStyle"];
                this.TableHeaderBorderStyle = (Style)resources["TableHeaderBorderStyle"];
                this.GridContainerStyle = (Style)resources["GridContainerStyle"];
            }
        }

        #endregion

        #region Public Properties

        #region Styling

        //changing most of these requires a re-Measure(), if you make the set's non-private you'll want to address that
        public Style AlternatingRowBorderStyle { get; private set; }
        public Style DocumentHeaderTextStyle { get; private set; }
        public Style DocumentFooterTextStyle { get; private set; }
        public Style TableCellTextStyle { get; private set; }
        public Style TableHeaderTextStyle { get; private set; }
        public Style TableHeaderBorderStyle { get; private set; }
        public Style GridContainerStyle { get; private set; }
        public Style RowBorders { get; private set; }

        #endregion

        //changing most of these requires a re-Measure(), if you make the set's non-private you'll want to address that
        public string DocumentTitle { get; private set; }
        public Thickness PageMargin { get; private set; }
        public FlowDirection PageDirection { get; private set; }
        public override Size PageSize { get; set; }

        public override bool IsPageCountValid { get { return true; } }

        public override int PageCount { get { return _pageCount; } }

        public override IDocumentPaginatorSource Source { get { return null; } }

        public bool WrapText { get; private set; }

        #endregion

        #region Public Methods

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPage page = null;
            List<object> itemsSource = new List<object>();

            if (_viewSource != null)
            {
                foreach (object item in _viewSource)
                    itemsSource.Add(item);
            }

            if (itemsSource != null)
            {
                int rowIndex = 1;
                int start = _startRows[pageNumber];
                int end = _startRows.Count > pageNumber + 1 ? _startRows[pageNumber + 1] : itemsSource.Count;

                //Create a new grid
                Grid tableGrid = CreateTable() as Grid;
                AddGridRow(tableGrid, GridLength.Auto);

                for (int index = start; index < end && index < itemsSource.Count; index++)
                {
                    AddGridRow(tableGrid, GridLength.Auto);

                    if (rowIndex > 0)
                    {
                        object item = itemsSource[index];
                        int columnIndex = 0;

                        foreach (var column in _columns)
                        {
                            if (AddTableCell(tableGrid, column, item, columnIndex, rowIndex))
                                columnIndex++;
                        }

                        if (this.AlternatingRowBorderStyle != null)
                        {
                            Border alernatingRowBorder = new Border();

                            alernatingRowBorder.Style = this.AlternatingRowBorderStyle;
                            alernatingRowBorder.SetValue(Grid.RowProperty, rowIndex);
                            alernatingRowBorder.SetValue(Grid.ColumnSpanProperty, columnIndex);
                            alernatingRowBorder.SetValue(Grid.ZIndexProperty, -1);
                            tableGrid.Children.Add(alernatingRowBorder);
                        }
                    }

                    rowIndex++;
                }

                page = ConstructPage(tableGrid, pageNumber);
            }

            return page;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function measures the heights of the page header, page footer and grid header and the first row in the grid
        /// in order to work out how manage pages might be required.
        /// </summary>
        private void MeasureElements()
        {
            double allocatedSpace = 0;

            //Measure the page header
            ContentControl pageHeader = new ContentControl();
            pageHeader.Content = CreateDocumentHeader();
            allocatedSpace = MeasureHeight(pageHeader);

            //Measure the page footer
            ContentControl pageFooter = new ContentControl();
            pageFooter.Content = CreateDocumentFooter(0);
            allocatedSpace += MeasureHeight(pageFooter);

            //Measure the table header
            ContentControl tableHeader = new ContentControl();
            tableHeader.Content = CreateTable();
            allocatedSpace += MeasureHeight(tableHeader);

            //Include any margins
            allocatedSpace += this.PageMargin.Bottom + this.PageMargin.Top;

            //Work out how much space we need to display the grid
            _availableHeight = this.PageSize.Height - allocatedSpace;

            //Calculate the height of the first row
            _actualHeights = new List<double>();
            _startRows = new List<int>();
            _startRows.Add(0);
            double height_so_far = 0;
            foreach (object item in _viewSource)
            {
                double h = MeasureHeight(CreateTempRow(item));
                if (height_so_far + h >= _availableHeight)
                {
                    //new page
                    height_so_far = h;
                    _startRows.Add(_actualHeights.Count);
                }
                else
                {
                    height_so_far += h;
                }
                _actualHeights.Add(h);
            }

            //Count the rows in the document source
            double rowCount = _actualHeights.Count;

            //Calculate the number of pages that we will need
            if (rowCount > 0)
                _pageCount = _startRows.Count;
        }

        /// <summary>
        /// This method constructs the document page (visual) to print
        /// </summary>
        private DocumentPage ConstructPage(Grid content, int pageNumber)
        {
            if (content == null)
                return null;

            //Build the page inc header and footer
            Grid pageGrid = new Grid();

            //Header row
            AddGridRow(pageGrid, GridLength.Auto);

            //Content row
            AddGridRow(pageGrid, new GridLength(1.0d, GridUnitType.Star));

            //Footer row
            AddGridRow(pageGrid, GridLength.Auto);

            ContentControl pageHeader = new ContentControl();
            pageHeader.Content = this.CreateDocumentHeader();
            pageGrid.Children.Add(pageHeader);

            if (content != null)
            {
                content.SetValue(Grid.RowProperty, 1);
                pageGrid.Children.Add(content);
            }

            ContentControl pageFooter = new ContentControl();
            pageFooter.Content = CreateDocumentFooter(pageNumber + 1);
            pageFooter.SetValue(Grid.RowProperty, 2);
            pageFooter.FlowDirection = PageDirection;

            pageGrid.Children.Add(pageFooter);

            double width = this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right);
            double height = this.PageSize.Height - (this.PageMargin.Top + this.PageMargin.Bottom);

            pageGrid.Measure(new Size(width, height));
            pageGrid.Arrange(new Rect(this.PageMargin.Left, this.PageMargin.Top, width, height));

            return new DocumentPage(pageGrid, PageSize, new Rect(content.DesiredSize), new Rect(content.DesiredSize));
        }

        /// <summary>
        /// Creates a default header for the document; containing the doc title
        /// </summary>
        private object CreateDocumentHeader()
        {
            Border headerBorder = new Border();
            TextBlock titleText = new TextBlock();
            titleText.Style = this.DocumentHeaderTextStyle;
            if (WrapText)
                titleText.TextWrapping = TextWrapping.Wrap;
            else
                titleText.TextTrimming = TextTrimming.CharacterEllipsis;
            titleText.Text = this.DocumentTitle;
            titleText.HorizontalAlignment = HorizontalAlignment.Center;
            titleText.TextAlignment = TextAlignment.Center;

            headerBorder.Child = titleText;
            headerBorder.FlowDirection = PageDirection;

            return headerBorder;
        }

        /// <summary>
        /// Creates a default page footer consisting of datetime and page number
        /// </summary>
        private object CreateDocumentFooter(int pageNumber)
        {
            Grid footerGrid = new Grid();
            footerGrid.Margin = new Thickness(0, 10, 0, 0);

            ColumnDefinition colDefinition = new ColumnDefinition();
            colDefinition.Width = new GridLength(0.5d, GridUnitType.Star);

            TextBlock dateTimeText = new TextBlock();
            dateTimeText.Style = this.DocumentFooterTextStyle;
            switch (PageDirection)
            {
                case FlowDirection.LeftToRight:
                    dateTimeText.Text = DateTime.Now.ToString("dd-MMM-yyy HH:mm");

                    break;
                case FlowDirection.RightToLeft:
                    dateTimeText.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
            }
            dateTimeText.FlowDirection = PageDirection;

            footerGrid.Children.Add(dateTimeText);

            TextBlock pageNumberText = new TextBlock();
            pageNumberText.Style = this.DocumentFooterTextStyle;
            pageNumberText.Text = "Page " + pageNumber.ToString() + " of " + this.PageCount.ToString();
            pageNumberText.SetValue(Grid.ColumnProperty, 1);
            pageNumberText.HorizontalAlignment = HorizontalAlignment.Right;
            pageNumberText.FlowDirection = PageDirection;

            footerGrid.Children.Add(pageNumberText);

            return footerGrid;
        }

        /// <summary>
        /// The following function creates a temp table with a single row so that it can be measured and used to 
        /// calculate the total number of req'd pages
        /// </summary>
        /// <returns></returns>
        private Grid CreateTempRow(object item)
        {
            Grid tableRow = new Grid();
            tableRow.Style = this.GridContainerStyle;

            if (_viewSource != null)
            {
                foreach (ColumnDefinition colDefinition in _tableColumnDefinitions)
                {
                    ColumnDefinition copy = XamlReader.Parse(XamlWriter.Save(colDefinition)) as ColumnDefinition;
                    tableRow.ColumnDefinitions.Add(copy);
                }

                int columnIndex = 0;
                if (_columns != null)
                    foreach (var column in _columns)
                        if (AddTableCell(tableRow, column, item, columnIndex, 0))
                            columnIndex++;
            }

            return tableRow;
        }

        /// <summary>
        /// This function counts the number of rows in the document
        /// </summary>
        private object CreateTable()
        {
            if (_viewSource == null)
                return null;

            Grid table = new Grid();
            table.Style = this.GridContainerStyle;

            int columnIndex = 0;

            if (_columns != null)
            {
                foreach (var column in _columns)
                {
                    if (AddTableColumn(table, columnIndex, column))
                        columnIndex++;
                }
            }

            if (this.TableHeaderBorderStyle != null)
            {
                Border headerBackground = new Border();
                headerBackground.Style = this.TableHeaderBorderStyle;
                headerBackground.SetValue(Grid.ColumnSpanProperty, columnIndex);
                headerBackground.SetValue(Grid.ZIndexProperty, -1);

                table.Children.Add(headerBackground);
            }

            return table;
        }

        /// <summary>
        /// Measures the height of an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private double MeasureHeight(FrameworkElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            double width = this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right);
            double height = this.PageSize.Height - (this.PageMargin.Top + this.PageMargin.Bottom);

            element.Measure(new Size(width, height));
            return element.DesiredSize.Height;
        }

        /// <summary>
        /// Adds a column to a grid
        /// </summary>
        /// <param name="grid">Grid to add the column to</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="column">Source column defintition which will be used to calculate the width of the column</param>
        private bool AddTableColumn(Grid grid, int columnIndex, DependencyObject column)
        {
            double proportion = 0;
            string header_text;
            if (column is DataGridColumn)
            {
                var datagridcolumn = column as DataGridColumn;
                if (datagridcolumn.Visibility != Visibility.Visible)
                    return false;

                proportion = datagridcolumn.ActualWidth / (this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right));
                header_text = datagridcolumn.Header.ToString();
            }
            else if (column is GridViewColumn)
            {
                var gridviewcolumn = column as GridViewColumn;
                if (gridviewcolumn.ActualWidth < 2)
                    return false;

                proportion = gridviewcolumn.ActualWidth / (this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right));
                header_text = gridviewcolumn.Header.ToString();
            }
            else
                return false;

            ColumnDefinition colDefinition = new ColumnDefinition();
            colDefinition.Width = new GridLength(proportion, GridUnitType.Star);

            grid.ColumnDefinitions.Add(colDefinition);

            TextBlock text = new TextBlock();
            text.Style = this.TableHeaderTextStyle;
            if (WrapText)
                text.TextWrapping = TextWrapping.Wrap;
            else
                text.TextTrimming = TextTrimming.CharacterEllipsis;
            text.Text = header_text;
            text.SetValue(Grid.ColumnProperty, columnIndex);

            grid.Children.Add(text);
            _tableColumnDefinitions.Add(colDefinition);
            return true;
        }

        /// <summary>
        /// Adds a cell to a grid
        /// </summary>
        /// <param name="grid">Grid to add teh cell to</param>
        /// <param name="column">Source column definition which contains binding info</param>
        /// <param name="item">The binding source</param>
        /// <param name="columnIndex">Column index</param>
        /// <param name="rowIndex">Row index</param>
        private bool AddTableCell(Grid grid, DependencyObject column, object item, int columnIndex, int rowIndex)
        {
            if (column is DataGridColumn)
            {
                var datagridcolumn = column as DataGridColumn;
                if (datagridcolumn.Visibility != Visibility.Visible)
                    return false;

                if (column is DataGridTemplateColumn)
                {
                    DataGridTemplateColumn templateColumn = column as DataGridTemplateColumn;
                    ContentControl contentControl = new ContentControl();

                    contentControl.Focusable = true;
                    contentControl.ContentTemplate = templateColumn.CellTemplate;
                    contentControl.Content = item;

                    contentControl.SetValue(Grid.ColumnProperty, columnIndex);
                    contentControl.SetValue(Grid.RowProperty, rowIndex);

                    grid.Children.Add(contentControl);
                    return true;
                }
                else if (column is DataGridTextColumn)
                {
                    DataGridTextColumn textColumn = column as DataGridTextColumn;
                    TextBlock text = new TextBlock { Text = "Text" };

                    text.Style = this.TableCellTextStyle;
                    if (WrapText)
                        text.TextWrapping = TextWrapping.Wrap;
                    else
                        text.TextTrimming = TextTrimming.CharacterEllipsis;
                    text.DataContext = item;

                    Binding binding = textColumn.Binding as Binding;

                    //if (!string.IsNullOrEmpty(column.DisplayFormat))
                    //binding.StringFormat = column.DisplayFormat;

                    text.SetBinding(TextBlock.TextProperty, binding);

                    text.SetValue(Grid.ColumnProperty, columnIndex);
                    text.SetValue(Grid.RowProperty, rowIndex);

                    grid.Children.Add(text);
                    return true;
                }
            }
            if (column is GridViewColumn)
            {
                var gridviewcolumn = column as GridViewColumn;
                if (gridviewcolumn.ActualWidth < 2)
                    return false;

                if (gridviewcolumn.CellTemplate != null)
                {
                    ContentControl contentControl = new ContentControl();

                    contentControl.Focusable = true;
                    contentControl.ContentTemplate = gridviewcolumn.CellTemplate;
                    contentControl.Content = item;

                    contentControl.SetValue(Grid.ColumnProperty, columnIndex);
                    contentControl.SetValue(Grid.RowProperty, rowIndex);

                    grid.Children.Add(contentControl);
                    return true;
                }
                else //if (column is DataGridTextColumn)
                {
                    //DataGridTextColumn textColumn = column as DataGridTextColumn;
                    TextBlock text = new TextBlock { Text = "Text" };

                    text.Style = this.TableCellTextStyle;
                    if (WrapText)
                        text.TextWrapping = TextWrapping.Wrap;
                    else
                        text.TextTrimming = TextTrimming.CharacterEllipsis;
                    text.DataContext = item;

                    Binding binding = gridviewcolumn.DisplayMemberBinding as Binding;

                    //if (!string.IsNullOrEmpty(column.DisplayFormat))
                    //binding.StringFormat = column.DisplayFormat;

                    text.SetBinding(TextBlock.TextProperty, binding);

                    text.SetValue(Grid.ColumnProperty, columnIndex);
                    text.SetValue(Grid.RowProperty, rowIndex);

                    grid.Children.Add(text);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a row to a grid
        /// </summary>
        private void AddGridRow(Grid grid, GridLength rowHeight)
        {
            if (grid == null)
                return;

            RowDefinition rowDef = new RowDefinition();

            if (rowHeight != null)
                rowDef.Height = rowHeight;

            grid.RowDefinitions.Add(rowDef);
        }

        #endregion

    }
}
