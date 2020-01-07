using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

/* ==============================================================================
 * 创建日期：2019/8/24 0:57:15
 * 创 建 者：wgd
 * 功能描述：HtmlTagFormter
 * ==============================================================================*/
namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// 此方法用于对HTML文件中标签样式进行格式化， 主要针对于CSSDMS旗舰版改版
    /// </summary>
    public class HtmlTagFormatLogic
    {
        #region 字段

        private readonly CodeMaidPackage _package;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// The singleton instance of the <see
        /// cref="HtmlTagFormatLogic" /> class.
        /// </summary>
        private static HtmlTagFormatLogic _instance;

        /// <summary>
        /// Gets an instance of the <see
        /// cref="HtmlTagFormatLogic" /> class.
        /// </summary>
        /// <param name="package">
        /// The hosting package.
        /// </param>
        /// <returns>
        /// An instance of the <see
        /// cref="HtmlTagFormatLogic" /> class.
        /// </returns>
        internal static HtmlTagFormatLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new HtmlTagFormatLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="HtmlTagFormatLogic" /> class.
        /// </summary>
        /// <param name="package">
        /// The hosting package.
        /// </param>
        private HtmlTagFormatLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// html文件内的标签格式化逻辑
        /// </summary>
        /// <param name="textDocument"></param>
        internal void HtmlTagFormat(TextDocument textDocument)
        {
            //读取配置文件
            if (!Settings.Default.Formatting_AspxHtml) return;

            //判断是否是aspx页面全部都替换
            if (Settings.Default.Cleaning_FormattingAspxCleanupAsk == 0)
            {
                //替换aspx中table标签，增加div标签和属性等；
                PrcoessAspxPageList(textDocument);
                //PrcoessAspxPageEdit(textDocument);
            }
           

            //正则表达式  替换aspx中的带有class的Button
            string pattern = "(?is)<asp:Button(((?!class|<asp).)*?)(\\b(class|CssClass)\\s*=\\s*\"(?(txthidden|btn)(?!)|(((?!class|<asp).)*?))\")?(?<center>((?!class|<asp).)*?)(?:width\\s*=\\s*\".*?\"\\s*height\\s*=\\s*\".*?\")?(?<footer>((?!class|<asp).)*?)/>";
            string replaceMent = "<asp:Button $1class=\"btn btn-info m-r-5\"${center}${footer}/>";
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replaceMent);

            //替换aspx中的带有CssClass的TextBox  DropDownList22
            pattern = @"(?is)(<asp:(TextBox|DropDownList)((?!class|</).)*?)(\b(class|CssClass)\s*=\s*\""(?(txthidden)(?!)|(((?!class|/>).)*?))\"")?(?<footer>((?!class).)*?(</asp:\2>|/>))";
            replaceMent = "$1 CssClass=\"form-controlnew\"${footer}";
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replaceMent);
            
            //只有不是全部时才会按照控件gridview来替换，全部按照页面格式来替换
            if (Settings.Default.Cleaning_FormattingAspxCleanupAsk == 1)
            {
                //替换aspx页面中gridview中class样式
                pattern = @"(?is)(?<dataHead><asp:GridView(((?!class|>).)*?))(?<cls>(class|CssClass)\s*=\s*\"".*?\"")?(?<dataFooter>(((?!class|>).)*?)>.*?</asp:GridView>)";
                replaceMent = "${dataHead} class=\"table table-bordered table-hover table-striped\"${dataFooter}";
                TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replaceMent);
            }
        }

        /// <summary>
        /// aspx页面中整体样式替换方法-列表页面
        /// </summary>
        /// <param name="textDocument"></param>
        /// <param name="patternString"></param>
        internal void PrcoessAspxPageList(TextDocument textDocument)
        {
            //string pattern = "(?isx)<table.*?<td[^>]*?>(?<title>[\u4e00-\u9fa5]+)(?:.*?)<table[^>]*?>[^<>]*?(?:<tr[^>]*?>[^<>]*?<td[^>]*?>(?<row>.*?)</td>[^<>]*?</tr>\\s*)+((?!<table).)*?(?<dataHead><asp:GridView(((?!class|>).)*?))(?<cls>(class|CssClass)\\s*=\\s*\".*?\")?(?<dataFooter>(((?!class|>).)*?)>.*?</asp:GridView>)((?!</table>).)*?</table>";
            string pattern = @"(?isx)       #匹配模式（i：忽略大小写 s：单行支持跨行 x：忽略表达式中空格）
                                <table.*?<td[^>]*?>
                                (?<title>\s*[\u4e00-\u9fa5]+\s*)                         #标题匹配
                                </td\s*>(?:.*?)<table[^>]*?>[^<>]*?
                                (?:<tr[^>]*?>[^<>]*?<td[^>]*?>(?<where>.*?)(?<btns><asp:Button(?:(?(?!</td>).)*)/>)\s*</td\s*>[^<>]*?</tr>\s*)+                       #查询条件匹配
                                ((?!<table).)*?(?<dataHead><asp:GridView(((?!class|>).)*?))(?<cls>(class|CssClass)\s*=\s*\"".*?\"")?(?<dataFooter>(((?!class|>).)*?)>.*?</asp:GridView>)
                                ((?!<table).)*?<td.*?(?<page>(?(<%--)<%--|)<uc1:(?:(?!/>).)*?/>(?(--%>)--%>|)\s*).*</td\s*>    #分页匹配
                                ((?!</table>).)*?</table>";

            //替换表达式拼接
            string strBody = "<div class=\"card-body\">";
            string strGroup = "<div class=\"form-group\">";
            StringBuilder replaceMentBody = new StringBuilder();
            replaceMentBody.AppendLine(@"<div class=""container-fluid row"">
          <div class=""col-lg-12"">
          <div class=""card"">");
            replaceMentBody.AppendLine(strBody);
            replaceMentBody.AppendLine("<div class=\"right-title\">${title}</div>");
            replaceMentBody.AppendLine("${where}");
            replaceMentBody.AppendLine(strGroup);//添加查询条件按钮的样式开始标签
            replaceMentBody.AppendLine("${btns}");
            replaceMentBody.AppendLine("</div>");//添加查询条件按钮的样式结束标签
            replaceMentBody.AppendLine("</div>");
            replaceMentBody.AppendLine(strBody);
            replaceMentBody.AppendLine("${dataHead} class=\"table table-bordered table-hover table-striped\"${dataFooter}");
            replaceMentBody.AppendLine("${page}");
            replaceMentBody.AppendLine("</div>");
            replaceMentBody.AppendLine(@"</div>
          </div>
          </div>");
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replaceMentBody.ToString());

            //页面的查询条件控件样式替换表达式
            pattern = @"(?isx)       #匹配模式（i：忽略大小写 s：单行支持跨行 x：忽略表达式中空格）
                        (?<title>[\u4e00-\u9fa5]+[：:])[\s&nbsp;]*(?<body><asp:(?<tag>[^>]*?)\b[^>]*?>(?:(?![\u4e00-\u9fa5]+[：:]).)*</asp:\k<tag>>)\s*(?:&nbsp;)*";

            //替换表达式拼接
            replaceMentBody = new StringBuilder();
            replaceMentBody.AppendLine("<div class=\"form-group\">");
            replaceMentBody.AppendLine("<label class=\"lf formlabel\">${title}</label>");
            replaceMentBody.AppendLine("<div class=\"col-xs-1\">");
            replaceMentBody.AppendLine("${body}");//页面的查询条件样式通过其他替换表达式来完成效果
            replaceMentBody.AppendLine("</div>");
            replaceMentBody.AppendLine("</div>");
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replaceMentBody.ToString());
        }

        /// <summary>
        /// aspx页面中整体样式替换方法--编辑页面
        /// </summary>
        /// <param name="textDocument"></param>
        /// <param name="patternString"></param>
        internal void PrcoessAspxPageEdit(TextDocument textDocument)
        {
            string pattern = @"(?isx)       #匹配模式（i：忽略大小写 s：单行支持跨行 x：忽略表达式中空格）
                                <table.*?<td[^>]*?>
                                (?<title>\s*[\u4e00-\u9fa5]+\s*)                         #标题匹配
                                </td\s*>(?:.*?)<table[^>]*?>[^<>]*?
                                (?:<tr[^>]*?>[^<>]*?<td[^>]*?>(?<where>.*?)</td\s*>[^<>]*?</tr>\s*)+                      #查询条件匹配
                                ((?!<table).)*?(?<dataHead><asp:GridView(((?!class|>).)*?))(?<cls>(class|CssClass)\s*=\s*\"".*?\"")?(?<dataFooter>(((?!class|>).)*?)>.*?</asp:GridView>)
                                ((?!<table).)*?<td[^<]*?(?<page><uc1:(?:(?!/>).)*?/>\s*)*</td\s*>           #分页匹配
                                ((?!</table>).)*?</table>";

            //替换表达式拼接
            string strBody = "<div class=\"card-body\">";
            string strGroup = "<div class=\"form-group\">";
            StringBuilder replaceMentBody = new StringBuilder();
            replaceMentBody.AppendLine(@"<div class=""container-fluid row"">
          <div class=""col-lg-12"">
          <div class=""card"">");
            replaceMentBody.AppendLine(strBody);
            replaceMentBody.AppendLine("<div class=\"right-title\">${title}</div>");
            replaceMentBody.AppendLine(strGroup);
            replaceMentBody.AppendLine("${where}");
            replaceMentBody.AppendLine("</div>");
            replaceMentBody.AppendLine("</div>");
            replaceMentBody.AppendLine(strBody);
            replaceMentBody.AppendLine("${dataHead} class=\"table table-bordered table-hover table-striped\"${dataFooter}");
            replaceMentBody.AppendLine("${page}");
            replaceMentBody.AppendLine("</div>");
            replaceMentBody.AppendLine(@"</div>
          </div>
          </div>");
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replaceMentBody.ToString());
        }

        /// <summary>
        /// aspx页面中关于搜索条件样式替换
        /// </summary>
        /// <param name="textDocument"></param>
        /// <param name="patternString"></param>
        internal void PrcoessWhere(TextDocument textDocument, string patternString)
        {
            var cursor = textDocument.StartPoint.CreateEditPoint();
            EditPoint end = null;
            TextRanges dummy = null;

            while (cursor != null && cursor.FindPattern(patternString, (int)vsFindOptions.vsFindOptionsRegularExpression, ref end, ref dummy))
            {
                EditPoint startPoint = cursor.CreateEditPoint();
                //matches.Add(startPoint);
                cursor = end;
                string src = startPoint.GetText(end);
                string replaceMent = "$1 CssClass=\"form-control\"${footer}";
                startPoint.ReplacePattern(end, patternString, replaceMent, (int)vsFindOptions.vsFindOptionsRegularExpression);
            }
        }

        #endregion 方法
    }
}