using System;
using System.Data;
using System.IO;
using Microsoft.Data.SqlClient;
using ClosedXML.Excel;

namespace WorkBench
{
    public class ExcelToSqlImporter
    {
        private string connectionString;

        public ExcelToSqlImporter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Import Excel file to SQL Server table
        /// </summary>
        /// <param name="excelFilePath">Path to Excel file</param>
        /// <param name="tableName">Target SQL table name</param>
        /// <param name="worksheetIndex">Worksheet index (default: 1)</param>
        /// <param name="hasHeaders">Whether first row contains headers (default: true)</param>
        /// <param name="createTableIfNotExists">Create table if it doesn't exist (default: false)</param>
        public void ImportExcelToSql(string excelFilePath, string tableName, int worksheetIndex = 1, 
            bool hasHeaders = true, bool createTableIfNotExists = false)
        {
            if (!File.Exists(excelFilePath))
            {
                throw new FileNotFoundException($"Excel file not found: {excelFilePath}");
            }

            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheet(worksheetIndex);
                if (worksheet == null)
                {
                    throw new Exception($"Worksheet {worksheetIndex} not found");
                }

                // Get data from worksheet
                DataTable dataTable = ConvertWorksheetToDataTable(worksheet, hasHeaders);

                // Create table if requested
                if (createTableIfNotExists)
                {
                    CreateTableFromDataTable(tableName, dataTable);
                }

                // Insert data
                BulkInsertData(tableName, dataTable);
            }
        }

        /// <summary>
        /// Convert Excel worksheet to DataTable
        /// </summary>
        private DataTable ConvertWorksheetToDataTable(IXLWorksheet worksheet, bool hasHeaders)
        {
            DataTable dt = new DataTable();

            var firstRowUsed = worksheet.FirstRowUsed();
            var lastRowUsed = worksheet.LastRowUsed();
            var firstCellUsed = worksheet.FirstCellUsed();
            var lastCellUsed = worksheet.LastCellUsed();

            if (firstRowUsed == null || lastRowUsed == null)
            {
                return dt; // Empty worksheet
            }

            int startRow = firstRowUsed.RowNumber();
            int endRow = lastRowUsed.RowNumber();
            int startCol = firstCellUsed.Address.ColumnNumber;
            int endCol = lastCellUsed.Address.ColumnNumber;

            // Create columns
            if (hasHeaders)
            {
                for (int col = startCol; col <= endCol; col++)
                {
                    var headerValue = worksheet.Cell(startRow, col).GetString();
                    string columnName = string.IsNullOrWhiteSpace(headerValue) ? $"Column{col}" : headerValue;
                    dt.Columns.Add(columnName);
                }
                startRow++; // Skip header row for data
            }
            else
            {
                for (int col = startCol; col <= endCol; col++)
                {
                    dt.Columns.Add($"Column{col}");
                }
            }

            // Add rows
            for (int row = startRow; row <= endRow; row++)
            {
                DataRow dataRow = dt.NewRow();
                bool isEmptyRow = true;

                for (int col = startCol; col <= endCol; col++)
                {
                    var value = worksheet.Cell(row, col).GetString();
                    dataRow[col - startCol] = string.IsNullOrEmpty(value) ? DBNull.Value : value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        isEmptyRow = false;
                    }
                }

                // Only add non-empty rows
                if (!isEmptyRow)
                {
                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }

        /// <summary>
        /// Create SQL table based on DataTable structure
        /// </summary>
        private void CreateTableFromDataTable(string tableName, DataTable dataTable)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if table exists
                string checkTableQuery = $@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')
                    BEGIN
                        CREATE TABLE [{tableName}] (";

                foreach (DataColumn column in dataTable.Columns)
                {
                    checkTableQuery += $"\n[{column.ColumnName}] NVARCHAR(MAX),";
                }

                checkTableQuery = checkTableQuery.TrimEnd(',');
                checkTableQuery += "\n)\nEND";

                using (var command = new SqlCommand(checkTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Bulk insert data into SQL table
        /// </summary>
        private void BulkInsertData(string tableName, DataTable dataTable)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.BulkCopyTimeout = 300; // 5 minutes

                    // Map columns
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        /// <summary>
        /// Import specific Excel range to SQL
        /// </summary>
        public void ImportExcelRangeToSql(string excelFilePath, string tableName, string range, 
            bool hasHeaders = true, bool createTableIfNotExists = false)
        {
            if (!File.Exists(excelFilePath))
            {
                throw new FileNotFoundException($"Excel file not found: {excelFilePath}");
            }

            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rangeData = worksheet.Range(range);

                DataTable dataTable = new DataTable();

                int startRow = rangeData.FirstRow().RowNumber();
                int endRow = rangeData.LastRow().RowNumber();
                int startCol = rangeData.FirstColumn().ColumnNumber();
                int endCol = rangeData.LastColumn().ColumnNumber();

                if (hasHeaders)
                {
                    for (int col = startCol; col <= endCol; col++)
                    {
                        var headerValue = worksheet.Cell(startRow, col).GetString();
                        string columnName = string.IsNullOrWhiteSpace(headerValue) ? $"Column{col}" : headerValue;
                        dataTable.Columns.Add(columnName);
                    }
                    startRow++;
                }
                else
                {
                    for (int col = startCol; col <= endCol; col++)
                    {
                        dataTable.Columns.Add($"Column{col}");
                    }
                }

                // Add rows
                for (int row = startRow; row <= endRow; row++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int col = startCol; col <= endCol; col++)
                    {
                        var value = worksheet.Cell(row, col).GetString();
                        dataRow[col - startCol] = string.IsNullOrEmpty(value) ? DBNull.Value : value;
                    }
                    dataTable.Rows.Add(dataRow);
                }

                if (createTableIfNotExists)
                {
                    CreateTableFromDataTable(tableName, dataTable);
                }

                BulkInsertData(tableName, dataTable);
            }
        }

        /// <summary>
        /// Get list of worksheets in Excel file
        /// </summary>
        public List<string> GetWorksheetNames(string excelFilePath)
        {
            var worksheetNames = new List<string>();

            using (var workbook = new XLWorkbook(excelFilePath))
            {
                foreach (var worksheet in workbook.Worksheets)
                {
                    worksheetNames.Add(worksheet.Name);
                }
            }

            return worksheetNames;
        }

        /// <summary>
        /// Import multiple Excel files (first sheet only) into new SQL tables
        /// </summary>
        /// <param name="excelFiles">Array of Excel file paths</param>
        /// <param name="tablePrefix">Optional prefix for table names (default: "")</param>
        /// <param name="hasHeaders">Whether first row contains headers (default: true)</param>
        /// <param name="useFileNameAsTableName">Use Excel filename as table name (default: true)</param>
        /// <returns>Dictionary of file paths and their corresponding table names</returns>
        public Dictionary<string, string> ImportMultipleExcelsToSql(
            string[] excelFiles, 
            string tablePrefix = "", 
            bool hasHeaders = true,
            bool useFileNameAsTableName = true)
        {
            var results = new Dictionary<string, string>();

            foreach (var excelFile in excelFiles)
            {
                if (!File.Exists(excelFile))
                {
                    throw new FileNotFoundException($"Excel file not found: {excelFile}");
                }

                // Generate table name
                string tableName;
                if (useFileNameAsTableName)
                {
                    // Use filename without extension as table name
                    string fileName = Path.GetFileNameWithoutExtension(excelFile);
                    // Clean up the filename to be SQL-safe
                    fileName = CleanTableName(fileName);
                    tableName = tablePrefix + fileName;
                }
                else
                {
                    tableName = tablePrefix + $"ImportedData_{DateTime.Now:yyyyMMddHHmmss}";
                }

                try
                {
                    // Import the first sheet
                    ImportExcelToSql(
                        excelFilePath: excelFile,
                        tableName: tableName,
                        worksheetIndex: 1,
                        hasHeaders: hasHeaders,
                        createTableIfNotExists: true
                    );

                    results.Add(excelFile, tableName);
                    Console.WriteLine($"✓ Imported {Path.GetFileName(excelFile)} -> {tableName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Failed to import {Path.GetFileName(excelFile)}: {ex.Message}");
                    throw;
                }
            }

            return results;
        }

        /// <summary>
        /// Import multiple Excel files with file dialog
        /// </summary>
        public Dictionary<string, string> ImportMultipleExcelsWithDialog(string tablePrefix = "", bool hasHeaders = true)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select Excel Files to Import";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return ImportMultipleExcelsToSql(openFileDialog.FileNames, tablePrefix, hasHeaders);
                }
            }

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Clean table name to be SQL-safe
        /// </summary>
        private string CleanTableName(string name)
        {
            // Remove invalid SQL characters
            string cleaned = System.Text.RegularExpressions.Regex.Replace(name, @"[^\w]", "_");
            
            // Ensure it doesn't start with a number
            if (char.IsDigit(cleaned[0]))
            {
                cleaned = "T_" + cleaned;
            }

            return cleaned;
        }

        /// <summary>
        /// Import all Excel files from a folder into one SQL table
        /// </summary>
        /// <param name="folderPath">Folder containing Excel files</param>
        /// <param name="tableName">Target SQL table name</param>
        /// <param name="headerRow1">First header row number (1-based)</param>
        /// <param name="headerRow2">Second header row number (1-based)</param>
        /// <param name="dataStartRow">Row where data starts (1-based)</param>
        /// <param name="createTableIfNotExists">Create table if it doesn't exist</param>
        /// <returns>Number of files imported</returns>
        public int ImportFolderToSingleTable(
            string folderPath,
            string tableName,
            int headerRow1 = 9,
            int headerRow2 = 10,
            int dataStartRow = 11,
            bool createTableIfNotExists = true)
        {
            // Use connection string from f class
            this.connectionString = f.GetSkiConnectionString();
            
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");
            }

            // Get all Excel files in folder
            var excelFiles = Directory.GetFiles(folderPath, "*.xlsx")
                .ToArray();

            if (excelFiles.Length == 0)
            {
                throw new Exception($"No Excel files found in folder: {folderPath}");
            }

            DataTable masterTable = null;
            int filesImported = 0;

            foreach (var excelFile in excelFiles)
            {
                try
                {
                    using (var workbook = new XLWorkbook(excelFile))
                    {
                        var worksheet = workbook.Worksheet(1); // First sheet only

                        if (masterTable == null)
                        {
                            // First file - create the master table structure
                            masterTable = CreateDataTableFromHeaders(worksheet, headerRow1, headerRow2);
                            
                            if (createTableIfNotExists)
                            {
                                CreateTableFromDataTable(tableName, masterTable);
                            }
                        }

                        // Read data from this file
                        ReadDataIntoTable(worksheet, masterTable, dataStartRow);
                    }

                    filesImported++;
                    Console.WriteLine($"✓ Imported {Path.GetFileName(excelFile)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Failed to import {Path.GetFileName(excelFile)}: {ex.Message}");
                }
            }

            // Bulk insert all data at once
            if (masterTable != null && masterTable.Rows.Count > 0)
            {
                BulkInsertData(tableName, masterTable);
                Console.WriteLine($"\n✓ Total rows imported: {masterTable.Rows.Count}");
            }

            return filesImported;
        }

        /// <summary>
        /// Import all Excel files from a folder selected via dialog into one SQL table
        /// </summary>
        public int ImportFolderToSingleTableWithDialog(
            string tableName,
            int headerRow1 = 9,
            int headerRow2 = 10,
            int dataStartRow = 11)
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder containing Excel files";
                folderDialog.ShowNewFolderButton = false;

                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return ImportFolderToSingleTable(
                        folderPath: folderDialog.SelectedPath,
                        tableName: tableName,
                        headerRow1: headerRow1,
                        headerRow2: headerRow2,
                        dataStartRow: dataStartRow,
                        createTableIfNotExists: true
                    );
                }
            }

            return 0;
        }

        /// <summary>
        /// Hebrew to English word translation dictionary
        /// </summary>
        private Dictionary<string, string> GetHebrewToEnglishDictionary()
        {
            return new Dictionary<string, string>
            {
                {"תאריך", "Date"},
                {"הפקדה", "Deposit"},
                {"קליטה", "Import"},
                {"סכום", "Amount"},
                {"תשלום", "Payment"},
                {"מטבע", "Currency"},
                {"ריבית", "Interest"},
                {"עמלה", "Fee"},
                {"חברה", "Company"},
                {"סוג", "Type"},
                {"כרטיס", "Card"},
                {"מספר", "Number"},
                {"ריכוז", "Batch"},
                {"עסקה", "Transaction"},
                {"תשלומים", "Installments"},
                {"מתוך", "Of"},
                {"בית", "Business"},
                {"עסק", "Merchant"},
                {"תקין", "Valid"},
                {"התחשבנות", "Settlement"},
                {"זיכוי", "Credit"},
                {"חיוב", "Debit"},
                {"בפועל", "Actual"},
                {"הנחה", "Discount"},
                {"שבא", "Shva"},
                {"שובר", "Voucher"},
                {"מסוף", "Terminal"},
                {"דמי", "Service"},
                {"ניהול", "Management"},
                {"שער", "Rate"},
                {"המרה", "Exchange"},
                {"ניכיון", "Deduction"},
                {"ברור", "Reversal"},
                {"פרוט", "Detail"},
                {"סיבת", "Reason"},
                {"הערות", "Notes"},
                {"שם", "Name"},
                {"ערך", "Value"},
                {"קוד", "Code"},
                {"תיאור", "Description"},
                {"ברוטו", "Gross"},
                {"נטו", "Net"},
                {"לזיכוי", "ToCredit"},
                {"אמצעי", "PaymentMethod"},
                {"מהות", "Identity"},
                {"מזהה", "ID"},
                {"ARN", "ARN"}
            };
        }

        /// <summary>
        /// Get list of required column headers (Hebrew)
        /// </summary>
        private List<string> GetRequiredColumns()
        {
            return new List<string>
            {
                "תאריך הפקדה",
                "תאריך זיכוי/חיוב",
                "תאריך עסקה",
                "סוג עסקה",
                "סכום עסקה",
                "ברוטו לזיכוי",
                "נטו לזיכוי",
                "מספר ריכוז",
                "אמצעי תשלום",
                "מהות",
                "מזהה ARN"
            };
        }

        /// <summary>
        /// Translate Hebrew words to English
        /// </summary>
        private string TranslateToEnglish(string hebrewText)
        {
            var dictionary = GetHebrewToEnglishDictionary();
            
            // Split by spaces and translate each word
            var words = hebrewText.Split(new[] { ' ', '_', '-', '/' }, StringSplitOptions.RemoveEmptyEntries);
            var translatedWords = new List<string>();

            foreach (var word in words)
            {
                string trimmedWord = word.Trim();
                if (dictionary.ContainsKey(trimmedWord))
                {
                    translatedWords.Add(dictionary[trimmedWord]);
                }
                else if (!string.IsNullOrEmpty(trimmedWord))
                {
                    // If not in dictionary, keep original
                    translatedWords.Add(trimmedWord);
                }
            }

            // Join into one word (PascalCase)
            return string.Join("", translatedWords);
        }

        /// <summary>
        /// Create DataTable structure from two header rows
        /// </summary>
        private DataTable CreateDataTableFromHeaders(IXLWorksheet worksheet, int headerRow1, int headerRow2)
        {
            DataTable dt = new DataTable();

            var firstCellUsed = worksheet.FirstCellUsed();
            var lastCellUsed = worksheet.LastCellUsed();

            if (firstCellUsed == null || lastCellUsed == null)
            {
                throw new Exception("Worksheet is empty");
            }

            int startCol = firstCellUsed.Address.ColumnNumber;
            int endCol = lastCellUsed.Address.ColumnNumber;
            var requiredColumns = GetRequiredColumns();
            var columnMapping = new Dictionary<int, string>(); // Maps Excel column index to DataTable column name

            // Combine headers from rows 9 and 10 and check if they're required
            for (int col = startCol; col <= endCol; col++)
            {
                var header1 = worksheet.Cell(headerRow1, col).GetString().Trim();
                var header2 = worksheet.Cell(headerRow2, col).GetString().Trim();

                string combinedHeader;
                if (!string.IsNullOrEmpty(header1) && !string.IsNullOrEmpty(header2))
                {
                    combinedHeader = $"{header1} {header2}";
                }
                else if (!string.IsNullOrEmpty(header1))
                {
                    combinedHeader = header1;
                }
                else if (!string.IsNullOrEmpty(header2))
                {
                    combinedHeader = header2;
                }
                else
                {
                    continue; // Skip empty columns
                }

                // Clean the combined header for comparison
                string cleanedHeader = combinedHeader.Trim();
                
                // Check if this column is in the required list
                bool isRequired = requiredColumns.Any(req => 
                    cleanedHeader.Contains(req.Trim()) || 
                    req.Trim().Contains(cleanedHeader));

                if (!isRequired)
                {
                    continue; // Skip non-required columns
                }

                // Translate to English
                string columnName = TranslateToEnglish(combinedHeader);
                
                // If translation resulted in empty string, use default
                if (string.IsNullOrEmpty(columnName))
                {
                    columnName = $"Column{col}";
                }

                // Clean column name
                columnName = System.Text.RegularExpressions.Regex.Replace(columnName, @"[^\w]", "");
                
                // Ensure unique column name
                string originalName = columnName;
                int counter = 1;
                while (dt.Columns.Contains(columnName))
                {
                    columnName = $"{originalName}{counter}";
                    counter++;
                }

                dt.Columns.Add(columnName);
                columnMapping[col] = columnName;
            }

            // Store column mapping for later use
            dt.ExtendedProperties["ColumnMapping"] = columnMapping;

            return dt;
        }

        /// <summary>
        /// Read data from worksheet into DataTable
        /// </summary>
        private void ReadDataIntoTable(IXLWorksheet worksheet, DataTable dataTable, int dataStartRow)
        {
            var lastRowUsed = worksheet.LastRowUsed();
            if (lastRowUsed == null) return;

            int endRow = lastRowUsed.RowNumber();
            var columnMapping = (Dictionary<int, string>)dataTable.ExtendedProperties["ColumnMapping"];

            for (int row = dataStartRow; row <= endRow; row++)
            {
                DataRow dataRow = dataTable.NewRow();
                bool isEmptyRow = true;

                foreach (var mapping in columnMapping)
                {
                    int excelCol = mapping.Key;
                    string dataTableCol = mapping.Value;

                    var value = worksheet.Cell(row, excelCol).GetString();
                    dataRow[dataTableCol] = string.IsNullOrEmpty(value) ? DBNull.Value : value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        isEmptyRow = false;
                    }
                }

                // Only add non-empty rows
                if (!isEmptyRow)
                {
                    dataTable.Rows.Add(dataRow);
                }
            }
        }
    }
}
