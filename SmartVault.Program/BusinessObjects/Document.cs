namespace SmartVault.Program.BusinessObjects
{
    public partial class Document : BusinessObjectBase
    {
        public int DocumentId { get; set; }
        public string? Name { get; set; }
        public string? FilePath { get; set; }
        public int Length { get; set; }
        public int AccountId { get; set; }
    }
}
