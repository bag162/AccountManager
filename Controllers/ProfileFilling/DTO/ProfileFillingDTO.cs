using BASAccountManager.DB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.Controllers.ProfileFilling.DTO
{
    public class ProfileFillingTableDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountLinkedAccounts { get; set; }
    }

    public class GetProfileFillingDTO
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public bool EnableRecomendations { get; set; }
        public bool ClosedAccount { get; set; }
        public string? AvatarPath { get; set; }
        public string? AboutMe { get; set; }
        public string? NameOrSurnameGenString { get; set; }
        public string? UsernameGenString { get; set; }
    }

    public class AddFillingDataDTO
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public bool EnableRecomendations { get; set; }
        public bool ClosedAccount { get; set; }
        public string AvatarBASE64 { get; set; }
        public string AvatarFormat { get; set; }

        public string? AboutMe { get; set; }
        public string? NameOrSurnameGenString { get; set; }
        public string? UsernameGenString { get; set; }
    }
}