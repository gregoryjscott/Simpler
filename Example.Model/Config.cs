using System;

namespace Example.Model
{
    public static class Config
    {
        public static void SetDataDirectory()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
        }

        public static string DatabaseName = "ExampleData";
    }
}