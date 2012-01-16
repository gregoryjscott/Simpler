namespace Simpler.Web.Models
{
    public class EditResult<TModel>
    {
        public bool ErrorOccurred { get; set; }
        public string ErrorMessage { get; set; }
        public TModel Model { get; set; }
    }
}
