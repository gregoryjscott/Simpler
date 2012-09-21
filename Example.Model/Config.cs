using System;

namespace Example.Model
{
    public static class Config
    {
        /// <summary>
        /// DataDirectory is set automatically in the web application, but must be set manually for testing, etc.
        /// </summary>
        public static void SetDataDirectory()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
        }

        public static string DatabaseName = "ExampleData";
    }
}