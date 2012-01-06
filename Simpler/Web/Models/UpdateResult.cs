namespace Simpler.Web.Models
{
    public class UpdateResult<TModel>
    {
        public bool ErrorOccurred { get; set; }
        public string ErrorMessage { get; set; }
        public int RowsAffected { get; set; }
        public TModel Model { get; set; }
    }
}
