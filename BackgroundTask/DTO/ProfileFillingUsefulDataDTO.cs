using BASAccountManager.DB.Models;

namespace BASAccountManager.BackgroundTask.DTO
{
    public class ProfileFillingUsefulDataDTO
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public bool EnableRecomendations { get; set; }
        public bool ClosedAccount { get; set; }
        public string? AvatarPath { get; set; }
        public string? AboutMe { get; set; }

        public string? NameOrSurnameGenString { get; set; }
        public string? UsernameGenString { get; set; }
    }
}