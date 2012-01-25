using Simpler.Web;

namespace MvcExample.Resources
{
    public class PlayersResource //: Resource<PlayersResource.Data>
    {
        public class Data
        {
            public int? PlayerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public int TeamId { get; set; }
            public string Team { get; set; }
        }

        public string Url
        {
            get { return "/Players"; }
        }

        public ResourceActions[] Actions
        {
            get
            {
                return new[]
                       {
                           ResourceActions.Index,
                           ResourceActions.Show,
                           ResourceActions.Edit,
                           ResourceActions.Update 
                       };
            }
        }
    }
}