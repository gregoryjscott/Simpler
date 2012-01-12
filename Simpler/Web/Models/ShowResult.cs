namespace Simpler.Web.Models
{
    public class ShowResult<TModel>
    {
        public bool ErrorOccurred { get; set; }
        public string ErrorMessage { get; set; }
        public TModel Model { get; set; }
    }
}
