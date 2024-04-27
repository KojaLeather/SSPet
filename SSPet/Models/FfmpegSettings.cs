namespace SSPet.Models
{
    public class FfmpegSettings
    {
        public const string Key = "FfmpegSettings";
        public string FfmpegPath { get; set; }
        public string InputUrl { get; set; }
        public string OutputTsUrl { get; set; }
        public string OutputManifestUrl { get; set; }
    }
}
