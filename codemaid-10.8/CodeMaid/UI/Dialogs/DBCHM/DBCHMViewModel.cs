using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Waodng.CodeMaid.Helpers;
using Waodng.CodeMaid.Properties;
using Waodng.CodeMaid.UI;

/* ==============================================================================
 * 创建日期：2020/7/14 11:13:57
 * 创 建 者：wgd
 * 功能描述：DBCHMViewModel  
 * ==============================================================================*/
namespace Waodng.CodeMaid.UI.Dialogs.DBCHM
{
    public class DBCHMViewModel : Bindable
    {
        /// <summary>
        /// 
        /// </summary>
        private DelegateCommand _connectCommand;

        /// <summary>
        /// 数据库连接按钮
        /// </summary>
        public DelegateCommand ConnectCommand => _connectCommand ?? (_connectCommand = new DelegateCommand(OnConnectCommandExecuted));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void OnConnectCommandExecuted(object parameter)
        {
            try
            {
                MessageBox.Show("数据库连接操作");
            }
            catch (Exception ex)
            {
                //OutputWindowHelper.ExceptionWriteLine("数据库操作", ex);
                //MessageBox.Show(ex.Message, "数据库操作", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
