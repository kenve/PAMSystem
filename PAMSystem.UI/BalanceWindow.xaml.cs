using PAMSystem.BLL;
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
    /// BalanceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BalanceWindow : Window
    {
        public bool IsShowBalance { get; set; }
        public BalanceWindow()
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
        //添加余额
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (!IsShowBalance)
            {
                Balance balance = new Balance();
                balance.Balances = decimal.Parse(txtBalance.Text);    //把string类型转为decimal
                bool i = new BalanceBLL().Update(LoginWindow.GetOperatorId(), balance.Balances);
                if (i)
                {
                    new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "添加余额成功！");
                    MessageBox.Show("添加数据成功！");
                    this.Close();
                }
                else
                {
                    new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "添加余额失败！");
                    MessageBox.Show("添加数据错误！");
                    this.Close();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsShowBalance)
            {
                txtBalance.IsReadOnly = true;        //查询时txtBalance为只读
                Balance b = new BalanceBLL().GetBalance(LoginWindow.GetOperatorId());  //根据用户Id查询余额
                txtBalance.Text = b.Balances.ToString();
                btnYes.Visibility = Visibility.Collapsed;  //隐藏“确定”按钮
                if (b.Balances < 0)
                {
                    MessageBox.Show("你已经入不敷出了，快点赚钱吧！");
                }
            }
            else
            {
                tbBalanceText.Text = "添加余额";
            }

        }
    }
}
