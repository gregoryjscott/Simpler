using System;

namespace Example.Model.Tests
{
    public static class Config
    {
        public static void SetDataDirectory()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
        }
    }
}