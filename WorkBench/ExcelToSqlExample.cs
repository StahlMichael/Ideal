using System;

namespace WorkBench
{
    /// <summary>
    /// Example usage of ExcelToSqlImporter
    /// </summary>
    public class ExcelToSqlExample
    {
        public static void Example()
        {
            // 1. Set up SQL connection string
            string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;User Id=Michael;Password=2585;TrustServerCertificate=True;Encrypt=False;";
            // Or with trusted connection:
            // string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

            // 2. Create importer instance
            var importer = new ExcelToSqlImporter(connectionString);

            // 3. Import Excel file to SQL table
            string excelFilePath = @"C:\path\to\your\file.xlsx";
            string tableName = "YourTableName";

            try
            {
                // Basic import (assumes first row is headers)
                importer.ImportExcelToSql(
                    excelFilePath: excelFilePath,
                    tableName: tableName,
                    worksheetIndex: 1,           // First worksheet
                    hasHeaders: true,            // First row contains column names
                    createTableIfNotExists: true // Auto-create table if it doesn't exist
                );

                Console.WriteLine("Excel data imported successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing Excel: {ex.Message}");
            }
        }

        public static void AdvancedExamples()
        {
            string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;User Id=Michael;Password=2585;TrustServerCertificate=True;Encrypt=False;";
            var importer = new ExcelToSqlImporter(connectionString);

            // Example 1: Import specific worksheet by index
            importer.ImportExcelToSql(
                excelFilePath: @"C:\data\report.xlsx",
                tableName: "SalesData",
                worksheetIndex: 2,  // Second worksheet
                hasHeaders: true,
                createTableIfNotExists: true
            );

            // Example 2: Import specific range
            importer.ImportExcelRangeToSql(
                excelFilePath: @"C:\data\report.xlsx",
                tableName: "SalesData",
                range: "A1:F100",  // Only import this range
                hasHeaders: true,
                createTableIfNotExists: true
            );

            // Example 3: Get list of worksheets in Excel file
            var worksheetNames = importer.GetWorksheetNames(@"C:\data\report.xlsx");
            foreach (var name in worksheetNames)
            {
                Console.WriteLine($"Worksheet: {name}");
            }

            // Example 4: Import without headers
            importer.ImportExcelToSql(
                excelFilePath: @"C:\data\data_no_headers.xlsx",
                tableName: "RawData",
                worksheetIndex: 1,
                hasHeaders: false,  // No headers - columns will be named Column1, Column2, etc.
                createTableIfNotExists: true
            );
        }

        // Example with Windows Forms integration
        public static void ImportWithFileDialog()
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select Excel File to Import";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;User Id=Michael;Password=2585;TrustServerCertificate=True;Encrypt=False;";
                    var importer = new ExcelToSqlImporter(connectionString);

                    try
                    {
                        importer.ImportExcelToSql(
                            excelFilePath: openFileDialog.FileName,
                            tableName: "ImportedData_Diners",
                            worksheetIndex: 1,
                            hasHeaders: true,
                            createTableIfNotExists: true
                        );

                        System.Windows.Forms.MessageBox.Show("Import completed successfully!");
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show($"Import failed: {ex.Message}");
                    }
                }
            }
        }

        // Example: Import multiple Excel files
        public static void ImportMultipleFiles()
        {
            string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;User Id=Michael;Password=2585;TrustServerCertificate=True;Encrypt=False;";
            var importer = new ExcelToSqlImporter(connectionString);

            // Option 1: Import multiple files with file dialog
            try
            {
                var results = importer.ImportMultipleExcelsWithDialog(
                    tablePrefix: "tbl",  // Optional prefix for table names
                    hasHeaders: true
                );

                Console.WriteLine($"Successfully imported {results.Count} files:");
                foreach (var result in results)
                {
                    Console.WriteLine($"  {System.IO.Path.GetFileName(result.Key)} -> {result.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Option 2: Import specific files programmatically
            string[] excelFiles = new string[]
            {
                @"C:\data\sales_2024.xlsx",
                @"C:\data\inventory.xlsx",
                @"C:\data\customers.xlsx"
            };

            try
            {
                var results = importer.ImportMultipleExcelsToSql(
                    excelFiles: excelFiles,
                    tablePrefix: "tbl",
                    hasHeaders: true,
                    useFileNameAsTableName: true  // Uses filename as table name
                );

                Console.WriteLine("Import completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Example: Import all Excel files from a folder into one table
        public static void ImportFolderToOneTable()
        {
            string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;User Id=Michael;Password=2585;TrustServerCertificate=True;Encrypt=False;";
            var importer = new ExcelToSqlImporter(connectionString);

            // Option 1: With folder dialog
            try
            {
                int filesImported = importer.ImportFolderToSingleTableWithDialog(
                    tableName: "CombinedData",
                    headerRow1: 9,      // First header row
                    headerRow2: 10,     // Second header row
                    dataStartRow: 11    // Data starts on row 11
                );

                System.Windows.Forms.MessageBox.Show(
                    $"Successfully imported {filesImported} files into table CombinedData",
                    "Import Complete",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Import failed: {ex.Message}",
                    "Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error
                );
            }

            // Option 2: Programmatically with specific folder
            try
            {
                int filesImported = importer.ImportFolderToSingleTable(
                    folderPath: @"C:\data\monthly_reports",
                    tableName: "MonthlyReports",
                    headerRow1: 9,
                    headerRow2: 10,
                    dataStartRow: 11,
                    createTableIfNotExists: true
                );

                Console.WriteLine($"Imported {filesImported} files successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
