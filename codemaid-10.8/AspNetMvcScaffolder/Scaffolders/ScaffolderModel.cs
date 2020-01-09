﻿using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using System;
using System.IO;

namespace AspNetMvcScaffolder
{
    public abstract class ScaffolderModel
    {
        private string _selectionRelativePath;

        protected ScaffolderModel(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Context = context;
            ServiceProvider = context.ServiceProvider;
        }

        /// <summary>
        /// Gets the full path to the primary output file of scaffolding.
        /// </summary>
        /// <remarks>
        /// This file will be opened when scaffolding is complete. Return null if no file should be opened.
        /// </remarks>
        public abstract string OutputFileFullPath
        {
            get;
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        internal CodeGenerationContext Context { get; private set; }

        public Project ActiveProject
        {
            get
            {
                return Context.ActiveProject;
            }
        }

        /// <summary>
        /// Gets the full path to the root of the Active Project.
        /// </summary>
        /// <remarks>
        /// This is needed to identify where the areas needs to be created.
        /// </remarks>
        protected string ActiveProjectFullPath
        {
            get
            {
                return ActiveProject.GetFullPath();
            }
        }

        /// <summary>
        /// Gets the ProjectItem representing the current selection.
        /// </summary>
        protected ProjectItem ActiveProjectItem
        {
            get
            {
                return Context.ActiveProjectItem;
            }
        }

        /// <summary>
        /// Gets the code file-extension of the Active Project.
        /// </summary>
        public string CodeFileExtension
        {
            get
            {
                return ActiveProject.GetCodeLanguage().CodeFileExtension;
            }
        }

        /// <summary>
        /// Gets the full path to the Active (selected) Project Item. If no Project  Item is selected, returns
        /// the full path of the Active Project.
        /// </summary>
        public string SelectionFullPath
        {
            get
            {
                return Path.Combine(ActiveProject.GetFullPath(), SelectionRelativePath);
            }
        }

        /// <summary>
        /// Gets the relative path from the Project root to the Active (selected) Project Item. If no Project
        /// Item is selected, returns the empty string.
        /// </summary>
        /// <remarks>
        /// In the case of views and empty controllers, the file will be created in the user selected path.
        /// hence views and controllers will call into the function to get the user selected relative path.
        /// 
        /// This is settable to allow overriding the selection in cases where we want to scaffold to a folder
        /// that does not yet exist. This will often occur using the 'Add View' context menu from the editor.
        /// </remarks>
        public string SelectionRelativePath
        {
            get
            {
                if (_selectionRelativePath == null)
                {
                    return ActiveProjectItem == null ? String.Empty : ActiveProjectItem.GetProjectRelativePath();
                }
                else
                {
                    return _selectionRelativePath;
                }
            }

            protected set
            {
                // This is just being called to validate that the argument is a valid path fragment
                Path.Combine(ActiveProject.GetFullPath(), value ?? String.Empty);

                _selectionRelativePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the AreaName.
        /// </summary>
        /// <remarks>
        /// For 
        /// </remarks>
        public string AreaName { get; set; }

        /// <summary>
        /// Gets the relative path to this Area from the project root.
        /// </summary>
        /// <returns>A relative path from the project root, or the empty string if AreaName is not set.</returns>
        public string AreaRelativePath
        {
            get
            {
                if (String.IsNullOrEmpty(AreaName))
                {
                    return String.Empty;
                }
                else
                {
                    return CommonFolderNames.Areas + Path.DirectorySeparatorChar + AreaName;
                }
            }
        }

        public bool IsOverwritingFiles { get; set; }

        public bool IsValidIdentifier(string text)
        {
            return String.IsNullOrEmpty(GetErrorIfInvalidIdentifier(text));
        }

        public string GetErrorIfInvalidIdentifier(string text)
        {
            return ValidationUtil.GetErrorIfInvalidIdentifier(text, ActiveProject.GetCodeLanguage());
        }

        protected string GetFileFullPath(string name, string codeFileExtension)
        {
            string fileFullPath = String.Empty;
            if (SelectionFullPath != null && IsValidIdentifier(name))
            {
                fileFullPath = Path.Combine(SelectionFullPath, name + "." + codeFileExtension);
            }

            return fileFullPath;
        }
    }
}
