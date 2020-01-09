using AspNetMvcScaffolder.Scaffolders.CustomScaffolder;
using Microsoft.AspNet.Scaffolding;
using System.ComponentModel.Composition;

namespace AspNetMvcScaffolder.Scaffolders.CustomScaffolder
{
    [Export(typeof(CodeGeneratorFactory))]
    public class MvcControllerWithEntityScaffolderFactory : ScaffolderFactory<EmptyFrameworkDependency>
    {
        private static CodeGeneratorInformation info = new CodeGeneratorInformation(
            displayName: "包含视图的 MVC 5 控制器(使用领域模型waodng)",
            description: "一个 MVC 控制器，其中包含用于创建、读取、更新、删除和列出条目的操作。",
            author: "零度",
            version: ScaffolderVersions.MvcEntityScaffolderVersion,
            id: "MvcControllerWithEntityScaffolder",
            icon: ScaffolderIcons.Controller,
            gestures: new[] { ScaffoldingGestures.Controller },
            categories: new[] { Categories.Common, Categories.MvcController, Categories.WebApi });

        public MvcControllerWithEntityScaffolderFactory() : base(info)
        {
        }

        protected override ICodeGenerator CreateInstanceInternal(CodeGenerationContext context)
        {
            return new ControllerWithEntityScaffolder(context, Information);
        }
    }
}
