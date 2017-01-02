using System;
using System.IO;
using LunchRoulette.iOS;
using LunchRoulette.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace LunchRoulette.iOS
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}