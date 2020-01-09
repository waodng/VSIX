using AspNetMvcScaffolder.VisualStudio;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.NuGet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace AspNetMvcScaffolder.Scaffolders.CustomScaffolder
{
    [Export]
    public class EmptyFrameworkDependency : IFrameworkDependency
    {
        [ImportingConstructor]
        public EmptyFrameworkDependency(INuGetRepository repository, IVisualStudioIntegration visualStudioIntegration)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            if (visualStudioIntegration == null)
            {
                throw new ArgumentNullException("visualStudioIntegration");
            }

            Repository = repository;
            VisualStudioIntegration = visualStudioIntegration;
        }

        protected IVisualStudioIntegration VisualStudioIntegration
        {
            get;
            private set;
        }

        protected INuGetRepository Repository
        {
            get;
            private set;
        }

        public bool IsDependencyInstalled(CodeGenerationContext context)
        {
            return true;
        }

        public IEnumerable<NuGetPackage> GetRequiredPackages(CodeGenerationContext context)
        {
            return Enumerable.Empty<NuGetPackage>();
        }

        public FrameworkDependencyStatus EnsureDependencyInstalled(CodeGenerationContext context)
        {
            return FrameworkDependencyStatus.InstallSuccessful;
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "editor", Justification = "This is called for a side-effect.")]
        public void UpdateConfiguration(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Project project = context.ActiveProject;
            string webConfigPath = Path.Combine(project.GetFullPath(), CommonFilenames.WebConfig);

            IEditorInterfaces editor = VisualStudioIntegration.Editor.GetOrOpenDocument(webConfigPath);
        }


        /// <summary>
        /// OData scaffolders are only supported for C# Projects without a reference to a previous version of WebAPI or OData
        /// </summary>
        public bool IsSupported(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return ScaffolderFilter.DisplayWebApiScaffolders(context)
                   && ScaffolderFilter.DisplayODataScaffolders(context);
        }

        public void RecordControllerTelemetryOptions(CodeGenerationContext context, ControllerScaffolderModel model)
        {

        }
    }
}
