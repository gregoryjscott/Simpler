using Simpler;
//using Simpler.Data;

public class CheckTables : InTask<CheckTables.Input>
{
    public class Input
    {
        public string[] TableNames { get; set; }
    }

    public override void Execute()
    {
        const string selectEverythingFrom = @"select * from @tableName";

//        using(var connection = Db.Connect("BaseballDatabase"))
//        {
//            foreach (var tableName in In.TableNames)
//            {
//                Db.NonQuery(connection, selectEverythingFrom, new {tableName});
//            }
//        }
    }
}
