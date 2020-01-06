﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CSS.Core;
using Microsoft.CSS.Editor;
using Microsoft.Scss.Core;
using Microsoft.Web.Editor;

namespace MadsKristensen.EditorExtensions.BrowserLink.UnusedCss
{
    public class ScssDocument : DocumentBase
    {
        private ScssDocument(string file)
            : base(file) { }

        protected override ICssParser CreateParser()
        {
            return CssParserLocator.FindComponent(Mef.GetContentType(ScssContentTypeDefinition.ScssContentType)).CreateParser();
        }

        internal static IDocument For(string fullPath, bool createIfRequired)
        {
            return For(fullPath, createIfRequired, f => new ScssDocument(f));
        }


        public static IEnumerable<string> GetSelectorNames(RuleSet ruleSet, ScssMixinAction mixinAction)
        {
            if (ruleSet.Block.Directives.Any(d => d is ScssMixinDirective))
            {
                switch (mixinAction)
                {
                    case ScssMixinAction.Skip:
                        return Enumerable.Empty<string>();
                    case ScssMixinAction.Literal:
                        break;
                }
            }

            var parentBlock = ruleSet.FindType<ScssRuleBlock>();

            if (parentBlock == null)
                return ruleSet.Selectors.Select(CssExtensions.SelectorText);

            var parentSet = parentBlock.FindType<RuleSet>();

            if (parentSet == null)
                return ruleSet.Selectors.Select(CssExtensions.SelectorText);

            // Cache the computed parents to avoid re-computing them
            // for every child permutation.
            var parentSelectors = GetSelectorNames(parentSet, mixinAction).ToList();

            return ruleSet.Selectors.SelectMany(child =>
                CombineSelectors(parentSelectors, child.SelectorText())
            );
        }

        private static IEnumerable<string> CombineSelectors(ICollection<string> parents, string child)
        {
            if (!child.Contains("&"))
                return parents.Select(p => p + " " + child);

            return parents.Select(p => child.Replace("&", p));
        }

        public override string GetSelectorName(RuleSet ruleSet)
        {
            return GetScssSelectorName(ruleSet, false);
        }

        internal static string GetScssSelectorName(RuleSet ruleSet, bool includeShellSelectors = true)
        {
            if (!includeShellSelectors)
            {
                var block = ruleSet.Block as ScssRuleBlock;

                if (block == null || ruleSet.Block.Declarations.Count == 0 && ruleSet.Block.Directives.Count == 0 && block.RuleSets.Any())
                {
                    //If we got here, the element won't be included in the output but has children that might be
                    return null;
                }
            }

            string name = string.Join(",\r\n", GetSelectorNames(ruleSet, ScssMixinAction.Skip));

            if (name.Length == 0)
                return null;

            var oldName = name;

            while (oldName != (name = name.Replace(" >", ">").Replace("> ", ">")))
            {
                oldName = name;
            }

            return oldName.Replace(">", " > ");
        }
    }
    ///<summary>Specifies how to handle mixins (and selectors within mixins) when constructing generated CSS selectors from Scss rulesets.</summary>
    public enum ScssMixinAction
    {
        ///<summary>Return null for any selector in a mixin.</summary>
        Skip,
        ///<summary>Return the literal text of the mixin declaration.  (this is not very useful)</summary>
        Literal
    }
}
