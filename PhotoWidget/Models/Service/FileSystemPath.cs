using System;
using System.IO;
using System.Web;

namespace PhotoWidget.Models.Service
{
    public class FileSystemPath
    {
        private readonly string _basePath;
        private string _webPath;
        private readonly string _dirDlmtrStr;
        private readonly char _dirDlmtrChar = Path.DirectorySeparatorChar;

        public FileSystemPath(string basePath, string webPath)
        {
            _dirDlmtrStr = Convert.ToString(_dirDlmtrChar);

            _basePath = basePath.Replace('/', _dirDlmtrChar);
            _webPath = webPath.Replace('/', _dirDlmtrChar).Trim(new[] { _dirDlmtrChar });
        }

        public string Absolute()
        {
            var relativePath = _webPath.Trim(new[] { _dirDlmtrChar });
            var basePath = _basePath.TrimEnd(new[] { _dirDlmtrChar });

            return HttpContext.Current != null
                ? HttpContext.Current.Server.MapPath(basePath + _dirDlmtrChar + relativePath)
                : basePath + _dirDlmtrChar + relativePath;
        }

        public string Web()
        {
            return _webPath.Trim(new[] { _dirDlmtrChar }).Replace(_dirDlmtrChar, '/');
        }

        public FileSystemPath Cd(string directoryPath)
        {
            var relativePath = _webPath.Trim(new[] { _dirDlmtrChar });
            var subPathTrimmed = directoryPath.Trim(new[] { _dirDlmtrChar });
            _webPath = relativePath + _dirDlmtrChar + subPathTrimmed;

            return this;
        }

        public FileSystemPath CdToParent()
        {
            var relativePath = _webPath.Trim(new[] { _dirDlmtrChar });
            var relativePathParts = relativePath.Split(_dirDlmtrChar);
            var partsCount = relativePathParts.Length;
            var newRelativePath = String.Join(_dirDlmtrStr, relativePathParts, 0, partsCount - 1);

            _webPath = newRelativePath;

            return this;
        }

        public FileSystemPath GoToFile(string filename)
        {
            var relativePath = _webPath.Trim(new[] { _dirDlmtrChar });
            _webPath = relativePath + _dirDlmtrChar + filename;

            return this;
        }
    }
}