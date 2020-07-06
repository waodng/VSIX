using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Waodng.CodeMaid.DBCHM.Common
{
    /// <summary>
    /// 解析VS生成的 XML文档文件
    /// </summary>
    public class XmlAnalyze
    {
        private Dictionary<KeyValuePair<string,string>,List<KeyValuePair<string,string>>> _data = new Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, string>>>();
        public Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, string>>> Data { 
            get{
                return _data;
            }
            set{
                _data=value;
            }
        }

        private List<string> _entityNames=new List<string>();
        public List<string> EntityNames { 
            get 
            {
                return _entityNames;
            }
            set    
            {
               _entityNames=value;
            }
        }


           private Dictionary<string, string> _entityComments=new Dictionary<string, string>();
        public Dictionary<string, string> EntityComments { 
            get 
            {
                return _entityXPaths;
            }
            set    
            {
               _entityXPaths=value;
            }
        }

        private Dictionary<string, string> _entityXPaths=new Dictionary<string, string>();
        public Dictionary<string, string> EntityXPaths { 
            get 
            {
                return _entityXPaths;
            }
            set    
            {
               _entityXPaths=value;
            }
        }

        public XmlAnalyze(string path)
        {
            var content = File.ReadAllText(path, Encoding.UTF8);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            var rootNode = doc.DocumentElement;

            var nodeList = rootNode.SelectNodes("//members/member[starts-with(@name,'T:')]");

            foreach (XmlNode node in nodeList)
            {
                string value=string.Empty;
                if (node.Attributes["name"]!=null)
	            {
                    value = node.Attributes["name"].Value;
	            }

                string entityName =string.Empty;
                if (!string.IsNullOrEmpty(value))
	            {
                    entityName = value.Split('.').LastOrDefault();
	            }

                //实体名 必须由 字母数字或下划线组成
                if (Regex.IsMatch(entityName, @"^[a-z\d_]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
                {
                    var comment = node.InnerText.Trim();
                    EntityNames.Add(entityName);
                    EntityComments.Add(entityName, comment);

                    if (!string.IsNullOrEmpty(value))
	                {
                        var xpath = value.Replace("T:", "P:") + ".";
                        EntityXPaths.Add(entityName, xpath);
                    }
                }
            }

            foreach (var item in EntityXPaths)
            {

                var nodes = rootNode.SelectNodes(string.Format("//members/member[starts-with(@name,'{0}')]",item.Value));

                var ecKey = new KeyValuePair<string, string>(item.Key, EntityComments[item.Key]);

                var lstKV = new List<KeyValuePair<string, string>>();

                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["name"]!=null)
	                {
                        var value = node.Attributes["name"].Value;
                        if (!string.IsNullOrEmpty(value))
	                    {
                            var propName = value.Split('.').LastOrDefault();
                            var comment = node.InnerText != null ? node.InnerText.Trim() : "";

                            lstKV.Add(new KeyValuePair<string, string>(propName, comment));
                         }
	                }
                    
                }
                Data.Add(ecKey, lstKV);
            }
        }
    }
}
