using System.Windows.Forms;

namespace Common.IO
{
    /// <summary>
    /// 提供一些简便的文件服务
    /// </summary>
    public static class FileServer
    {
        /// <summary>
        /// 选择一个文件并返回它的路径
        /// </summary>
        /// <param name="filter">文件过滤符</param>
        public static string ChooseFile(string filter = "")
        {
            var fileFetch = new OpenFileDialog() { Filter = filter };
            fileFetch.ShowDialog();

            return fileFetch.FileName;
        }

        /// <summary>
        /// 选择一个文件夹并返回它的路径
        /// </summary>
        public static string ChooseDirectory()
        {
            var fileFetch = new FolderBrowserDialog();
            fileFetch.ShowDialog();

            return fileFetch.SelectedPath;
        }
    }
}
