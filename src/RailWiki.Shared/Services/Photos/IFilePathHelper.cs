using Microsoft.Extensions.Options;
using RailWiki.Shared.Configuration;

namespace RailWiki.Shared.Services.Photos
{
    public interface IFilePathHelper
    {
        string ResolveFilePath(int albumId, string fileName = null, string sizeKey = null);
    }

    public class FilePathHelper : IFilePathHelper
    {
        private readonly ImageConfig _imageConfig;

        public FilePathHelper(IOptions<ImageConfig> imageOptions)
        {
            _imageConfig = imageOptions.Value;
        }

        public string ResolveFilePath(int albumId, string fileName = null, string sizeKey = null)
        {
            var path = _imageConfig.BasePath.Replace("\\", "/").TrimEnd('/');
            path = $"{path}/albums/{albumId}";

            if (!string.IsNullOrEmpty(fileName))
            {
                if (!string.IsNullOrEmpty(sizeKey))
                {
                    fileName = $"{sizeKey}_{fileName}";
                }

                path = $"{path}/{fileName}";
            }

            return path;
        }
    }
}
