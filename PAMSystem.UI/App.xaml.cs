using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PAMSystem.UI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        
        /// <summary>
        /// 程序异常处理的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("程序奔溃鸟！！！,请联系开发人员！ 错误为："+e.Exception.Message);
            e.Handled = true;//异常已处理,程序可以继续使用。

        }
    }
}
