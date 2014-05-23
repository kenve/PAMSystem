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
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        #region 窗口等的样式

        //鼠标进入最小化按钮时Foreground为黑色
        private void btnMinimiz_MouseEnter(object sender, MouseEventArgs e)
        {
            btnMinimiz.Foreground = new SolidColorBrush(Colors.Black);
        }
        //鼠标离开最小化按钮时前景色为白色
        private void btnMinimiz_MouseLeave(object sender, MouseEventArgs e)
        {
            btnMinimiz.Foreground = new SolidColorBrush(Colors.White);
        }
        //鼠标进入关闭按钮时Foreground为黑色
        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Foreground = new SolidColorBrush(Colors.Black);
        }
        //鼠标离开关闭按钮时前景色为白色
        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            btnClose.Foreground = new SolidColorBrush(Colors.White);
        }
        //点击最小化按钮，实现窗口最小化
        private void btnMinimiz_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        //g关闭窗口
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //实现窗口的拖动
        //private void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DragMove();
        //}
        #endregion

        /// 获得登录用户的Id
        public static Guid GetOperatorId()
        {
            Guid operatorId = (Guid)Application.Current.Properties["OperatorId"];
            return operatorId;
        }
        //点击登录按钮，实现登录
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text.Length <= 0)
            {
                MessageBox.Show("请输入用户名！");
            }
            else if (pwdPassword.Password.Length <= 0)
            {
                MessageBox.Show("请输入密码！");
            }
            else
            {
                Operator op = new Operator();
                string userName = txtUserName.Text.Trim();

                Application.Current.Properties["OperatorName"] = userName;

                string password = pwdPassword.Password;
                //OperatorBLL bll = new OperatorBLL();
                LoginResult result = new OperatorBLL().UserLoginResult(userName, password);
                OperatorBLL bll = new OperatorBLL();
                if (result == LoginResult.ErrorNameOrPwd)
                {
                    op = bll.GetOperatorByUserName(userName);
                    if (op != null)
                    {
                        new OperationLogBLL().Insert(op.Id, "尝试登陆失败！");
                    }
                        MessageBox.Show("用户名或密码错误");
                }
                else
                {
                    //MessageBox.Show("登录成功");
                    //获得登录用户的Id，存到Application.Current.Properties里面的在程序其他地方也可以取
                    op = bll.GetOperatorByUserName(userName);
                    Application.Current.Properties["OperatorId"] = op.Id;
                    new OperationLogBLL().Insert(op.Id, "登陆成功！");
                    //关闭LoginWindow，进入主窗口
                    DialogResult = true;
                }
            }
        }
        //点击取消按钮
        private void brnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        //跳转到注册页面
        private void txtRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {

            EditOpreatorWindow editOperatorWindow = new EditOpreatorWindow();
            //为判断是否为添加的属性IsInsert赋值为true
            editOperatorWindow.IsInsert = true;
            editOperatorWindow.ShowDialog();
        }

    }
}

