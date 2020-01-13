using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;

/* ==============================================================================
 * 创建日期：2020/1/13 9:38:24
 * 创 建 者：wgd
 * 功能描述：EditorExtensionsPackage  
 * ==============================================================================*/
namespace Waodng.EditorExtensions
{
    public class EditorExtensionsPackage
    {
        #region Singleton

        public static EditorExtensionsPackage Instance { get; private set; }
        private static DTE2 _dte;
        private static Package _pack;
        private static IVsRegisterPriorityCommandTarget _pct;
        internal static DTE2 DTE
        {
            get
            {
                if (_dte == null)
                    _dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE2;

                return _dte;
            }
        }
        internal static IVsRegisterPriorityCommandTarget PriorityCommandTarget
        {
            get
            {
                if (_pct == null)
                    _pct = ServiceProvider.GlobalProvider.GetService(typeof(SVsRegisterPriorityCommandTarget)) as IVsRegisterPriorityCommandTarget;

                return _pct;
            }
        }

        internal static Package Package
        {
            get
            {
                if (_pack == null)
                    _pack = ServiceProvider.GlobalProvider.GetService(typeof(Package)) as Package;

                return _pack;
            } 
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="package"></param>
        public static void Initialize(Package package)
        {
            Instance = new EditorExtensionsPackage(package);
        }

        #endregion Singleton

        #region Constructors

        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="package">窗口唯一包管理对象</param>
        internal EditorExtensionsPackage(Package package)
        {
          
        }
        #endregion Constructors

        #region 方法

         /// <summary>
        /// 获得visual studio的DTE对象
        /// </summary>
        /// <returns></returns>
        public static DTE2 GetDTE()
        {
            return ServiceProvider.GlobalProvider.GetService(typeof(DTE2)) as DTE2;
        }

        public static T GetGlobalService<T>(Type type = null) where T : class
        {
            return Microsoft.VisualStudio.Shell.Package.GetGlobalService(type ?? typeof(T)) as T;
        }

        public static IComponentModel ComponentModel
        {
            get { return GetGlobalService<IComponentModel>(typeof(SComponentModel)); }
        }

        #endregion



       
    }
}
