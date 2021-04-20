namespace Accounting.DataManager.Models
{
    public class BookAccountsSettingsModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Side { get; set; }
        public bool Prefix { get; set; }
    }
}
