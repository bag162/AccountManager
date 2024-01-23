namespace BASAccountManager.Services
{
    public static class ImageService
    {
        public static async Task<string> AddBase64ImageAsync(string imageFormat, string base64Image, string imagePath, string fileName)
        {
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            if (File.Exists(imagePath + "/" + fileName + "." + imageFormat))
            {
                File.Delete(imagePath + "/" + fileName + "." + imageFormat);
            }
            byte[] bytes = Convert.FromBase64String(base64Image);
            File.WriteAllBytes(imagePath + "/" + fileName + "." + imageFormat, bytes);
            return imagePath + "/" + fileName + "." + imageFormat;
        }
    }
}