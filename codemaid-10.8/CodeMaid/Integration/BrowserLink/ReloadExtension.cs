using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Web.BrowserLink;
using Waodng.CodeMaid.Helpers;

namespace Waodng.CodeMaid.Integration.BrowserLink
{
    public class ReloadExtension : BrowserLinkExtension, IDisposable
    {
        IEnumerable<string> _extensions = CodeMaidPackage.Options.FileExtensions.Split(';');
        IEnumerable<string> _ignorePatterns = CodeMaidPackage.Options.GetIgnorePatterns();
        List<BrowserLinkConnection> _connections = new List<BrowserLinkConnection>();

        bool _isDisposed;
        Project _project;
        Timer _timer;
        FileSystemWatcher _watcher;
        int _state;

        public ReloadExtension(Project project)
        {
            _project = project;
            string folder = project.GetRootFolder();

            if (string.IsNullOrEmpty(folder))
                return;

            _watcher = new FileSystemWatcher(folder);
            _watcher.Changed += FileChanged;
            _watcher.Renamed += FileChanged;
            _watcher.IncludeSubdirectories = true;
            _watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName;
            //是否启用文件夹变化监控
            _watcher.EnableRaisingEvents = CodeMaidPackage.Options.EnableReload;

            _timer = new Timer(TimerElapsed, null, 0, CodeMaidPackage.Options.Delay);

            CodeMaidPackage.Options.Saved += OptionsSaved;
        }

        public override void OnConnected(BrowserLinkConnection connection)
        {
            if (connection.Project == _project)
                _connections.Add(connection);

            base.OnConnected(connection);
        }

        public override void OnDisconnecting(BrowserLinkConnection connection)
        {
            if (_connections.Contains(connection))
                _connections.Remove(connection);

            base.OnDisconnecting(connection);
        }
        /// <summary>
        /// 调用刷新事件
        /// </summary>
        public void Reload()
        {
            Browsers.Clients(_connections.ToArray()).Invoke("reload");
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                CodeMaidPackage.Options.Saved -= OptionsSaved;
                _watcher.Dispose();
                _timer.Dispose();

                _isDisposed = true;
            }
        }

        void FileChanged(object sender, FileSystemEventArgs e)
        {
            string file = e.FullPath.ToLowerInvariant();
            string ext = Path.GetExtension(file).TrimStart('.');
            System.Diagnostics.Debug.WriteLine(file);

            if (!string.IsNullOrEmpty(ext) &&
                !ext.Contains('~') &&
                !_ignorePatterns.Any(p => file.Contains(p)) &&
                _extensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
            {
                Interlocked.Exchange(ref _state, 2);
            }
        }

        void TimerElapsed(object state)
        {
            //Move from changed + refresh pending to just refresh pending
            if (Interlocked.CompareExchange(ref _state, 1, 2) == 2)
            {
                return;
            }

            //Move from refresh pending without a recent change to no refresh pending
            if (Interlocked.CompareExchange(ref _state, 0, 1) == 1)
            {
                Reload();
            }
        }

        /// <summary>
        /// 工具选项中设置保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OptionsSaved(object sender, EventArgs e)
        {
            _extensions = CodeMaidPackage.Options.FileExtensions.Split(';');
            _ignorePatterns = CodeMaidPackage.Options.GetIgnorePatterns();
            _watcher.EnableRaisingEvents = CodeMaidPackage.Options.EnableReload;
        }
    }
}
