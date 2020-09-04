using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWiki.Shared.Services.FileStorage
{
    /// <summary>
    /// Interface for interacting with file stores
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Checks whether or not a file exists at the given path
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>True if it exists, false otherwise</returns>
        Task<bool> FileExistsAsync(string path);

        /// <summary>
        /// Resolves the full path to a file
        /// </summary>
        /// <param name="path">Relative path, including file name</param>
        /// <returns>Resolved path to the file</returns>
        string ResolveFilePath(string path);

        /// <summary>
        /// Resolves the full URL to a file
        /// </summary>
        /// <param name="path">Relative path, including file name</param>
        /// <returns>URL to the file</returns>
        string ResolveFileUrl(string path);

        /// <summary>
        /// Saves a file
        /// </summary>
        /// <param name="path">Path of file</param>
        /// <param name="contentType">The mime type of the file</param>
        /// <param name="stream">File stream</param>
        Task SaveFileAsync(string path, string contentType, Stream stream);

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="path">Path of file</param>
        Task DeleteFileAsync(string path);
    }
}
