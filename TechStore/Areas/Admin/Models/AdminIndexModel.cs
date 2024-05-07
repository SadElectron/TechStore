namespace TechStore.Areas.Admin.Models
{
    public class AdminIndexModel
    {
        public Dictionary<string, int> RecordCount;
        public AdminIndexModel()
        {
            
        }
        public AdminIndexModel(Dictionary<string, int> dictionary)
        {
            this.RecordCount = dictionary;
        }
    }
}