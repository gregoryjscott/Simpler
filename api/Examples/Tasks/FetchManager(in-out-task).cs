using Simpler;
//using Simpler.Data;
using System.Linq;

public class FetchManager : InOutTask<FetchManager.Input, FetchManager.Output>
{
    public class Input
    {
        public string TeamName { get; set; }
    }

    public class Output
    {
        public Manager Manager { get; set; }
    }

    public override void Execute()
    {
        const string selectManager = @"
            select
                m.Name
            from
                Managers m
                inner join
                Teams t on
                    (m.Id = t.ManagerId)
            where
                t.Name = @TeamName
            ";

//        using (var connection = Db.Connect("BaseballDatabase"))
//        {
//            Out.Manager = Db.Get<Manager>(
//                connection,
//                selectManager,
//                new {In.TeamName}
//            ).Single();
//        }
    }
}
