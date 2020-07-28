using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ==============================================================================
 * 创建日期：2020/7/3 16:48:42
 * 创 建 者：
 * 功能描述：Startup  
 * ==============================================================================*/
namespace Waodng.CodeMaid.DBCHM
{
    public class Startup
    {
        /// <summary>
        /// 判断是否是管理员操作
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 窗体运行主窗体
        /// </summary>
        public static void Show()
        {
            //如果是管理员，则直接运行
            new MainForm().ShowDialog();
        }
    }
    
}
