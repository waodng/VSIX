using System;
using System.Collections.Generic;
using SteveCadwallader.CodeMaid.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ==============================================================================
 * 创建日期：2020/1/9 13:13:20
 * 创 建 者：wgd
 * 功能描述：BasePack  
 * ==============================================================================*/
namespace SteveCadwallader.CodeMaid.Integration.Package
{
    /// <summary>
    /// base package
    /// </summary>
    internal abstract class BasePack : ISwitchableFeature
    {
        #region Constructors
        public BasePack(CodeMaidPackage package)
        {
            Package = package;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        protected CodeMaidPackage Package { get; private set; }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Called to execute the package.
        /// </summary>
        public virtual void OnExecute()
        {
            OutputWindowHelper.DiagnosticWriteLine($"Other Package: {GetType().Name}.OnExecute invoked");
        }


        public virtual void Switch(bool on)
        {
            if (on)
            {
              
            }
            else if (!on)
            {
                
            }
        }

        #endregion Methods
    }
}
