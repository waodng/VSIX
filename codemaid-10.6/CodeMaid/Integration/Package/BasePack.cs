using System;
using System.Collections.Generic;
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
    internal abstract class BasePack
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


        #endregion Methods
    }
}
