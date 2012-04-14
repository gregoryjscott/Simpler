using System.Data;
using System.Linq;

namespace Simpler.Sql.Jobs
{
    /// <summary>
    /// Job that will create an object of the given type T using the result of the given command.  
    /// 
    /// Single() is used to make sure only one object is returned.  If you aren't sure that just one record 
    /// exists, you should use FetchListOf instead (and then call ObjectsFetched.FirstOrDefault(), for example).
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    public class FetchSingleOf<T> : Job
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T ObjectFetched { get; private set; }

        // Sub-jobs
        public virtual FetchListOf<T> FetchListOfT { get; set; }

        public override void Run()
        {
            // Create the sub-jobs.
            if (FetchListOfT == null) FetchListOfT = new FetchListOf<T>();

            FetchListOfT.SelectCommand = SelectCommand;
            FetchListOfT.Run();
            ObjectFetched = FetchListOfT.ObjectsFetched.Single();
        }
    }
}
