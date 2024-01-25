using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.AdvertResourses
{
    public class DBAdvertAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ErrorMessage { get; set; }
        public string AccountURL { get; set; }
        public AdvertAccountStatus AdvertAccountStatus { get; set; }

        [ForeignKey(nameof(DBAdvertAccountGroup))]
        public int AdvertAccountGroupId { get; set; }
        public DBAdvertAccountGroup AdvertAccountGroup { get; set; }
    }

    public enum AdvertAccountStatus
    {
        NotProcessed,
        Processed,
        ProcessTreatment,
        Error
    }
}