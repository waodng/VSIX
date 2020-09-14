using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace myAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class myAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "myAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public static CompilationUnitSyntax root;
        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);

            string text = "WebConfig.GetValue(\"SupplyRoomID\")";
            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            root = tree.GetCompilationUnitRoot();

            //监控所有语法节点中调用表达式的节点
            context.RegisterSyntaxNodeAction(AnalyzeDepartment, SyntaxKind.InvocationExpression);
        }


        /// <summary>
        /// 分析哪些代码语句含有多个供应室科室ID的问题
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeDepartment(SyntaxNodeAnalysisContext context)
        {
            //var node = context.Node as InvocationExpressionSyntax;

            //var invoExp =node?.Expression;

            //if (invoExp == null) return;

            var model = context.SemanticModel;

            // Use the syntax model to find the literal string:
            LiteralExpressionSyntax helloWorldString = root.DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .Single();

            // Use the semantic model for type information:
            TypeInfo literalInfo = model.GetTypeInfo(helloWorldString);

            if (literalInfo.Type == null) return;

            Console.WriteLine(literalInfo.Type.Name);

            //// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            //var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            //// Find just those named type symbols with names containing lowercase letters.
            //if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            //{
            //    // For all such symbols, produce a diagnostic.
            //    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            //    context.ReportDiagnostic(diagnostic);
            //}
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    class TypeParameterVisitor : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitTypeParameterList(TypeParameterListSyntax node)
        {
            var syntaxList = new SeparatedSyntaxList<TypeParameterSyntax>();
            syntaxList = syntaxList.Add(SyntaxFactory.TypeParameter("TParameter"));

            var lessThanToken = this.VisitToken(node.LessThanToken);
            var greaterThanToken = this.VisitToken(node.GreaterThanToken);
            return node.Update(lessThanToken, syntaxList, greaterThanToken);
        }

        public override SyntaxNode VisitFixedStatement(FixedStatementSyntax node)
        {
            return base.VisitFixedStatement(node);
        }
    }
}
