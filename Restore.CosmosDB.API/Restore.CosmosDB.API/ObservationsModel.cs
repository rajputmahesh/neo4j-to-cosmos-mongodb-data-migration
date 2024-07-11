namespace Restore.CosmosDB.API
{
    public class ObservationsModel
    {
        public int ObservationSyncId { get; set; }
        public long Id { get; set; }
        public int IndicatorId { get; set; }
        public int CountryId { get; set; }
        public int YearId { get; set; }
        public decimal? Value { get; set; }
        public bool IsPublished { get; set; }
        public char Status { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
