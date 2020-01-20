﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Waodng.CodeMaid.Integration.BrowserLink
{
    public class Options : DialogPage
    {
        const string fileExtensions = "css;js;html;htm;cshtml;vbhtml;aspx;ascx;master;jpg;jpeg;gif;png;svg";
        const string ignoreList = @"\node_modules\;\bower_components\;\typings\;\lib\;\vendor\";
        const int delay = 250;

        private bool _enableReload=true;
        private int _delay = delay;
        private string _fileExtensions = fileExtensions;
        private string _ignoreList = ignoreList;
                                                               

        [Category("General")]
        [DisplayName("Enable reload on save")]
        [Description("When enabled, every time a file is saved the connected browsers will reload.")]
        [DefaultValue(true)]
        public bool EnableReload { 
            get{
           return _enableReload;
            }
            set{
            _enableReload=value;
            }
        }

        [Category("General")]
        [DisplayName("Delay")]
        [Description("The delay from a file has changed to the reload is triggered. Default is 250 milliseconds.")]
        [DefaultValue(delay)]
        public int Delay { get{
            return _delay;
        }
            set {
                _delay = value;
            }
        }

        [Category("Filter")]
        [DisplayName("File extensions")]
        [Description("A semicolon-separated list of file extensions to listen to.")]
        [DefaultValue(fileExtensions)]
        public string FileExtensions { get{
            return _fileExtensions;
        }
            set
            {
                _fileExtensions = value;
            }
        }

        [Category("Filter")]
        [DisplayName("Ignore patterns")]
        [Description("A semicolon-separated list of strings. Any file containing one of the strings in the path will be ignored.")]
        [DefaultValue(ignoreList)]
        public string IgnorePatterns { get{
            return _ignoreList;
        }
            set
            {
                _ignoreList = value;
            }
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            if (Saved != null)
                Saved(this, EventArgs.Empty);
        }

        public IEnumerable<string> GetIgnorePatterns()
        {
            var raw = IgnorePatterns.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pattern in raw)
            {
                yield return pattern;
            }
        }

        public event EventHandler<EventArgs> Saved;
    }
}
