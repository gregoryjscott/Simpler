using Simpler;
//using Simpler.Data;

public class FetchTeams : OutTask<FetchTeams.Output>
{
    public class Output
    {
        public Team[] Teams { get; set; }
    }

    public override void Execute()
    {
        const string selectTeams = @"select Name from Teams";

//        using (var connection = Db.Connect("BaseballDatabase"))
//        {
//            Out.Teams = Db.Get<Team>(connection, selectTeams);
//        }
    }
}
