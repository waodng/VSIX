using AspNetMvcScaffolder.Metadata;
using AspNetMvcScaffolder.Telemetry;
using AspNetMvcScaffolder.UserInterface;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace AspNetMvcScaffolder.Scaffolders.CustomScaffolder
{
    public class ControllerWithEntityScaffolder : InteractiveScaffolder<ControllerWithEntityScaffolderModel, EmptyFrameworkDependency>
    {
        private const string TemplateName = "Controller";

        private const string ViewModelTemplateName = "ViewModel";

        private const string ValidatorTemplateName = "Validator";

        internal ScaffoldingTelemetry tc = new ScaffoldingTelemetry();

        public ControllerWithEntityScaffolder(CodeGenerationContext context, CodeGeneratorInformation information) : base(context, information)
        {

        }

        protected internal virtual string TemplateFolderName
        {
            get
            {
                return "MvcControllerWithEntity";
            }
        }

        public override IEnumerable<string> TemplateFolders
        {
            get { return GetSearchFolders(TemplateFolderName); }
        }

        protected sealed override ControllerWithEntityScaffolderModel CreateModel()
        {
            ControllerWithEntityScaffolderModel model = new ControllerWithEntityScaffolderModel(Context);
            model.ControllerName = model.GetGeneratedName(MvcProjectUtil.ControllerName, model.CodeFileExtension);
            OnModelCreated(model);
            return model;
        }

        protected override ValidatingDialogWindow CreateDialog()
        {
            return new ControllerWithEntityScaffolderDialog();
        }

        protected override object CreateViewModel(ControllerWithEntityScaffolderModel model)
        {
            return new ControllerWithEntityScaffolderViewModel(model);
        }

        protected virtual void OnModelCreated(ControllerScaffolderModel model)
        {
            model.ControllerName = null;
            model.IsModelClassSupported = true;
            model.IsDataContextSupported = false;
            model.IsViewFolderRequired = true;
            model.IsViewGenerationSupported = true;
            model.IsViewGenerationSelected = true;
            model.IsAsyncSupported = true;
        }

        protected internal override void Scaffold()
        {
            try
            {
                var templateParameters = new Dictionary<string, object>(StringComparer.Ordinal);
                AddTemplateParameters(templateParameters);
                GenerateViewModel(templateParameters);
                GenerateController(templateParameters);
                GenerateValidator(templateParameters);

                if (Model.ServiceProject != null&&Model.IsServiceClassSupported)
                {
                    GenerateService(templateParameters);
                }
                if (Model.IsViewModelSupported)
                {
                    GenerateViews(templateParameters);
                }
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

            Model.SelectionRelativePath = CommonFolderNames.Controllers;

            string outputPath = Path.Combine(Model.SelectionRelativePath, Model.ControllerName);
            AddFileFromTemplate(Context.ActiveProject, outputPath, TemplateName, templateParameters, !Model.IsOverwritingFiles);
        }

        protected void GenerateViewModel(IDictionary<string, object> templateParameters)
        {
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }
            string viewModelShortName = new Regex(@"\b([_\d\w]*)$").Match(Model.ViewModelTypeName).Result("$1");
            string outputPath = Path.Combine(CommonFolderNames.Models, viewModelShortName);
            AddFileFromTemplate(Context.ActiveProject, outputPath, ViewModelTemplateName, templateParameters, true);
        }

        protected void GenerateValidator(IDictionary<string, object> templateParameters)
        {
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }
            string outputPath = Path.Combine(CommonFolderNames.Validator, Model.ModelType.ShortTypeName + "Validator");
            AddFileFromTemplate(Context.ActiveProject, outputPath, ValidatorTemplateName, templateParameters, true);
        }

        protected void GenerateService(IDictionary<string, object> templateParameters)
        {
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }
            AddFileFromTemplate(Model.ServiceProject, "I" + Model.ModelType.ShortTypeName + "Service", "IService", templateParameters, true);
            AddFileFromTemplate(Model.ServiceProject, Model.ModelType.ShortTypeName + "Service", "Service", templateParameters, true);
        }

        protected void GenerateViews(IDictionary<string, object> templateParameters)
        {
            string[] viewNames = new string[] { MvcViewTemplates.Create, MvcViewTemplates.Delete, MvcViewTemplates.Details, MvcViewTemplates.Edit, "List" };
            if (templateParameters == null)
            {
                throw new ArgumentNullException("templateParameters");
            }

            templateParameters["LayoutPageFile"] = base.Model.LayoutPageFile ?? string.Empty;
            templateParameters["IsPartialView"] = base.Model.IsPartialViewSelected;
            templateParameters["IsLayoutPageSelected"] = base.Model.IsLayoutPageSelected;
            templateParameters["ReferenceScriptLibraries"] = base.Model.IsReferenceScriptLibrariesSelected;
            templateParameters["JQueryVersion"] = string.Empty;
            templateParameters["MvcVersion"] = ProjectReferences.GetAssemblyVersion(base.Context.ActiveProject, AssemblyVersions.MvcAssemblyName);

            if (base.Model.ModelType == null)
            {
                templateParameters["ViewDataTypeName"] = string.Empty;
            }
            else
            {
                CodeType codeType = base.Model.ModelType.CodeType;
                templateParameters["ViewDataTypeName"] = codeType.FullName;
                templateParameters["ViewDataTypeShortName"] = codeType.Name;
                templateParameters["ViewDataType"] = codeType;
            }

            foreach (var viewName in viewNames)
            {
                templateParameters["ViewName"] = viewName;
                string viewFileName = viewName == "List" ? "Index" : viewName;
                string outputFolder = Path.Combine(Model.AreaRelativePath, "Views", Model.ControllerRootName, viewFileName);
                AddFileFromTemplate(Context.ActiveProject, outputFolder, viewName, templateParameters, true);
            }
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
            templateParameters.Add("ControllerNamespace", Model.ControllerNamespace);
            templateParameters.Add("AreaName", Model.AreaName ?? String.Empty);
            templateParameters.Add("ServiceName", Model.ServiceType.ShortTypeName);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\b([_\d\w]*)$");
            string viewModelShortName = regex.Match(Model.ViewModelTypeName).Result("$1");
            templateParameters.Add("ViewModelPropertys", Model.ViewModelPropertys.Where(p => p.Checked).ToList());

            CodeType modelCodeType = Context.ServiceProvider.GetService<ICodeTypeService>().GetCodeType(Context.ActiveProject, Model.ModelType.TypeName);

            CodeModelModelMetadata modelMetadata = new CodeModelModelMetadata(modelCodeType);

            CodeType modelType = Model.ModelType.CodeType;
            templateParameters.Add("ModelMetadata", modelMetadata);

            string modelTypeNamespace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            templateParameters.Add("ModelTypeNamespace", modelTypeNamespace);

            string viewModelNamespace = Model.ActiveProject.Name + "." + CommonFolderNames.Models;
            templateParameters.Add("ViewModelNamespace", viewModelNamespace);

            string validatorNamespace = Model.ActiveProject.Name + "." + CommonFolderNames.Validator;
            templateParameters.Add("ValidatorNamespace", validatorNamespace);

            string serviceNamespace = Model.ServiceProject.GetDefaultNamespace();
            templateParameters.Add("ServiceNamespace", serviceNamespace);

            string serviceShortTypeName = Model.ServiceType.ShortTypeName;
            templateParameters.Add("ServiceShortTypeName", serviceShortTypeName);

            string viewModelShortTypeName = Model.ViewModelType.ShortTypeName;
            templateParameters.Add("ViewModelShortTypeName", viewModelShortName);

            HashSet<string> requiredNamespaces = GetRequiredNamespaces(new List<CodeType>() { modelType });
            templateParameters.Add("RequiredNamespaces", requiredNamespaces);
            string modelTypeName = modelType.Name;
            templateParameters.Add("ModelTypeName", modelTypeName);
            templateParameters.Add("UseAsync", Model.IsAsyncSelected);

            CodeDomProvider provider = ValidationUtil.GenerateCodeDomProvider(Model.ActiveProject.GetCodeLanguage());
            string modelVariable = provider.CreateEscapedIdentifier(Model.ModelType.ShortTypeName.ToLowerInvariantFirstChar());
            templateParameters.Add("ModelVariable", modelVariable);
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

                if (!String.IsNullOrEmpty(codeTypeNamespace))
                {
                    requiredNamespaces.Add(codeTypeNamespace);
                }
                else if (!String.Equals(codeType.FullName, codeType.Name, StringComparison.OrdinalIgnoreCase))
                {
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
