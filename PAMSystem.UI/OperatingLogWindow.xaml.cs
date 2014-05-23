using PAMSystem.BLL;
using PAMSystem.DAL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PAMSystem.UI
{
    /// <summary>
    /// OperatingLogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OperatingLogWindow : Window
    {
        public OperatingLogWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Operator operators = new OperatorBLL().GetOperatorById(LoginWindow.GetOperatorId());
            //绑定数据源
            columnOperator.ItemsSource = new OperatorBLL().ListAll();
            gridShowLog.ItemsSource = new OperationLogBLL().Search(LoginWindow.GetOperatorId());


        }
    }
}
