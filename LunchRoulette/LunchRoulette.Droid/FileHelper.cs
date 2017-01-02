using System.IO;
using LunchRoulette.Droid;
using LunchRoulette.Services;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(FileHelper))]
namespace LunchRoulette.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}