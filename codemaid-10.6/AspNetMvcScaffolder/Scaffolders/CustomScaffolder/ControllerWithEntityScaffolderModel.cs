using AspNetMvcScaffolder.UserInterface;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetMvcScaffolder.Scaffolders.CustomScaffolder
{
    public class ControllerWithEntityScaffolderModel : ControllerScaffolderModel
    {
        public ControllerWithEntityScaffolderModel(CodeGenerationContext context): base(context)
        {

            ServiceTypes = ServiceProvider.GetService<ICodeTypeService>().GetAllCodeTypes(ActiveProject)
                                                                 .Where(codeType => codeType.IsInterfaceType())
                                                                 .Select(ct => new ModelType(ct));

            ViewModelTypes = ServiceProvider.GetService<ICodeTypeService>().GetAllCodeTypes(ActiveProject)
                                                                 .Where(codeType => codeType.IsValidWebProjectEntityType())
                                                                 .Select(ct => new ModelType(ct));
            this.IsServiceClassSupported = true;
            this.IsViewModelSupported = true;
        }

        public bool IsViewModelSupported { get; set; }

        public bool IsServiceClassSupported { get; set; }

        public IEnumerable<ModelType> ServiceTypes { get; private set; }

        public ModelType ViewModelType { get; set; }

        public string ViewModelTypeName { get; set; }

        public string ServiceTypeName { get; set; }

        public ModelType ServiceType { get; set; }

        public IEnumerable<ModelType> ViewModelTypes { get; private set; }

        public List<ViewModelProperty> ViewModelPropertys { get;  set; }

        public string GenerateDefaultViewModelTypeName()
        {
            Project project = ActiveProject;
            string modelsNamespace = MvcProjectUtil.GetDefaultModelsNamespace(project.GetDefaultNamespace());
            CodeDomProvider provider = ValidationUtil.GenerateCodeDomProvider(project.GetCodeLanguage());
            ViewModelTypeName = modelsNamespace + "." + ModelType.ShortTypeName + MvcProjectUtil.ViewModelSuffix;

            return ViewModelTypeName;
        }

        public string GenerateDefaultServiceTypeName()
        {
            Project project = ServiceProject;
            string serviceNamespace = project.GetDefaultNamespace();
            CodeDomProvider provider = ValidationUtil.GenerateCodeDomProvider(project.GetCodeLanguage());
            ViewModelTypeName = serviceNamespace + ".I" + ModelType.ShortTypeName + MvcProjectUtil.ServiceTypeSuffix;

            return ViewModelTypeName;
        }

        public string ValidateViewModelTypeName(string viewModelName)
        {
            if (String.IsNullOrWhiteSpace(viewModelName))
            {
                return "视图模型名称不能为空。";
            }

            return null;
        }

        public string ValidateServiceTypeName(string viewModelName)
        {
            if (String.IsNullOrWhiteSpace(viewModelName))
            {
                return "服务接口名称不能为空。";
            }

            return null;
        }

        public string ValidateViewModelType(ModelType viewModelType)
        {
            // TODO: this is not a complete validation
            if (viewModelType == null)
            {
                return "视图模型必须非空。";
            }

            return null;
        }

        public string ValidateServiceType(ModelType serviceType)
        {
            // TODO: this is not a complete validation
            if (serviceType == null)
            {
                return "服务接口必须非空。";
            }

            return null;
        }

        public Project ServiceProject
        {
            get
            {
                foreach (Project project in ActiveProject.DTE.Solution.Projects)
                {
                    if (project.Name.ToLower().Contains(".service"))
                    {
                        return project;
                    }
                }

                return null;
            }
        }
    }
}
