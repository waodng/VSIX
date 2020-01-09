﻿using AspNetMvcScaffolder.Telemetry;
using AspNetMvcScaffolder.UserInterface;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using System;
using System.Collections.Generic;
using System.IO;

namespace AspNetMvcScaffolder
{
    public abstract class ControllerScaffolder<TFramework> : InteractiveScaffolder<ControllerScaffolderModel, TFramework>
        where TFramework : IFrameworkDependency
    {
        private const string TemplateName = "Controller";

        internal ScaffoldingTelemetry tc = new ScaffoldingTelemetry();

        protected ControllerScaffolder(CodeGenerationContext context, CodeGeneratorInformation information)
            : base(context, information)
        {
        }

        // accessible for unit tests
        protected internal abstract string TemplateFolderName
        {
            get;
        }

        public override IEnumerable<string> TemplateFolders
        {
            get
            {
                return GetSearchFolders(TemplateFolderName);
            }
        }

        protected sealed override ControllerScaffolderModel CreateModel()
        {
            ControllerScaffolderModel model = new ControllerScaffolderModel(Context);
            model.ControllerName = model.GetGeneratedName(MvcProjectUtil.ControllerName, model.CodeFileExtension);
            OnModelCreated(model);
            return model;
        }

        protected override ValidatingDialogWindow CreateDialog()
        {
            return new ControllerScaffolderDialog();
        }

        protected override object CreateViewModel(ControllerScaffolderModel model)
        {
            return new ControllerScaffolderViewModel(model);
        }

        protected virtual void OnModelCreated(ControllerScaffolderModel model)
        {
            // intentionally empty
        }

        protected internal override void Scaffold()
        {
            try
            {
                GenerateController(new Dictionary<string, object>(StringComparer.Ordinal));
                tc.TrackEvent(TelemetryEventNames.AddControllerWithAction);
            }
            catch (Exception e)
            {
                tc.TrackEvent(TelemetryEventNames.ActionScaffolderFailure);
                tc.TrackException(e);
                throw;
            }
            finally
            {
                Framework.RecordControllerTelemetryOptions(Context, Model);
                tc.Flush();
            }
        }

        protected void GenerateController(IDictionary<string, object> templateParameters)
        {
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }

            AddTemplateParameters(templateParameters);

            if (Model.IsViewFolderRequired)
            {
                string folderName = Path.Combine(Model.AreaRelativePath, CommonFolderNames.Views, Model.ControllerRootName);
                AddFolder(Context.ActiveProject, folderName);
            }

            // If the controllers folder is created during this operation and the user selects the project root, controller will be added
            // to the controllers folder.
            Model.SelectionRelativePath = CommonFolderNames.Controllers;

            // OutputPath needs to contain the relative path along with the name of the file to be generated (without extension).
            string outputPath = Path.Combine(Model.SelectionRelativePath, Model.ControllerName);
            AddFileFromTemplate(Context.ActiveProject, outputPath, TemplateName, templateParameters, skipIfExists: !Model.IsOverwritingFiles);
        }

        protected void GenerateViewModel(IDictionary<string, object> templateParameters)
        {
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }

            AddTemplateParameters(templateParameters);

            Model.SelectionRelativePath = CommonFolderNames.Controllers;

            string outputPath = Path.Combine(Model.SelectionRelativePath, Model.ControllerName);
            AddFileFromTemplate(Context.ActiveProject, outputPath, TemplateName, templateParameters, skipIfExists: !Model.IsOverwritingFiles);
        }

        protected virtual void AddTemplateParameters(IDictionary<string, object> templateParameters)
        {
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }

            if (String.IsNullOrEmpty(Model.ControllerName))
            {
                throw new InvalidOperationException(Resources.InvalidControllerName);
            }

            templateParameters.Add("ControllerName", Model.ControllerName);
            templateParameters.Add("Namespace", Model.ControllerNamespace);
            templateParameters.Add("ControllerRootName", Model.ControllerRootName);
            templateParameters.Add("AreaName", Model.AreaName ?? String.Empty);
        }

        protected HashSet<string> GetRequiredNamespaces(IEnumerable<CodeType> codeTypes)
        {
            if (codeTypes == null)
            {
                throw new ArgumentNullException("codeTypes");
            }

            SortedSet<string> requiredNamespaces = CreateHashSetBasedOnCodeLanguage(Context.ActiveProject.GetCodeLanguage());

            requiredNamespaces.Add(Model.ControllerNamespace);
            foreach (CodeType codeType in codeTypes)
            {
                string codeTypeNamespace = codeType.Namespace == null ? null : codeType.Namespace.FullName;
                // In the case of C# projects, the codeType.FullName and codeType.Name will be always the same. In the case of VB projects, because of the
                // project root namespace, codeType.FullName and codeType.Name may be different.
                if (!String.IsNullOrEmpty(codeTypeNamespace))
                {
                    requiredNamespaces.Add(codeTypeNamespace);
                }
                else if (!String.Equals(codeType.FullName, codeType.Name, StringComparison.OrdinalIgnoreCase))
                {
                    // If the codetype does not provide a namespace declaration and a default namespace is required to identify the codetype,
                    // then the namespace is computed from the fully qualified name of the codetype.
                    int lastIndex = codeType.FullName.LastIndexOf("." + codeType.Name, StringComparison.Ordinal);
                    string projectNamespace = codeType.FullName.Remove(lastIndex);
                    requiredNamespaces.Add(projectNamespace);
                }
            }
            requiredNamespaces.Remove(Model.ControllerNamespace);

            return new HashSet<string>(requiredNamespaces);
        }

        private static SortedSet<String> CreateHashSetBasedOnCodeLanguage(ProjectLanguage projectLanguage)
        {
            if (projectLanguage == ProjectLanguage.CSharp)
            {
                return new SortedSet<string>(StringComparer.Ordinal);
            }
            else if (projectLanguage == ProjectLanguage.VisualBasic)
            {
                return new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            throw new InvalidOperationException(Resources.ScaffoldLanguageNotSupported);
        }
    }
}
