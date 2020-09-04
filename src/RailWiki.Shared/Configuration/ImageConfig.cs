using System.Collections.Generic;

namespace RailWiki.Shared.Configuration
{
    public class ImageConfig
    {
        public string BasePath { get; set; }

        public List<ImageSizeProfile> SizeProfiles { get; set; } = new List<ImageSizeProfile>();        
    }

    public class ImageSizeProfile
    {
        public string Key { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
