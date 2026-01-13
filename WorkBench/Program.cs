namespace WorkBench
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
                
                // Excel to SQL Import Example
                ExcelToSqlExample.ImportWithFileDialog();

                //ApplicationConfiguration.Initialize();
                //Application.Run(new DataHandling());
             
        }
    }
}