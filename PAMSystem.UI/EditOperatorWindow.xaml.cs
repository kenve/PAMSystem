using PAMSystem.BLL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
    /// EditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditOpreatorWindow : Window
    {
        public bool IsInsert { get; set; }
        public Guid EditingId { get; set; }
        public EditOpreatorWindow()
        {
            InitializeComponent();
        }
        #region 窗口或按钮样式
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
        #endregion

        private void brnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            return;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //判断是插入 还是编辑
            //添加
            if (IsInsert)
            {

                if (txtUserName.Text.Length <= 0)
                {
                    MessageBox.Show("请输入用户名！");
                }
                if (pwdPassword.Password.Length <= 0)
                {
                    MessageBox.Show("请输入密码！");
                }
                else if (pwdRePassword.Password.Length <= 0)
                {
                    MessageBox.Show("请再次输入密码！");
                }
                else if (pwdPassword.Password != pwdRePassword.Password)
                {
                    MessageBox.Show("两次输入密码不一致！");
                }
                else
                {
                    //添加用户
                    string userName = txtUserName.Text.Trim();
                    string password = pwdPassword.Password;
                    OperatorBLL bll = new OperatorBLL();
                    int i = 0, j = 0;
                    //打开Distributed Transaction Coordinator服务
                    // 引用System.Transaction
                    //用分布式事务实现添加新用户的的同时添加改账户的余额 
                    using (TransactionScope ts = new TransactionScope())
                    {

                        Operator oper = new OperatorBLL().GetOperatorByUserName(userName);
                        //数据库中是否已有此用户名
                        if (oper == null)    //没有就添加
                        {
                            i = bll.AddOpertator(userName, password);
                            Operator op = bll.GetOperatorByUserName(userName);
                            new BalanceBLL().AddNew(op.Id);
                        }
                        //若有是否已删除
                        else
                        {
                            //恢复已删除用户

                            if (oper.IsDeleted)
                            {

                                new OperatorBLL().ReuseOperator(oper.Id);  //恢复
                                j = new OperatorBLL().EditOperator(oper.Id, password);//更改密码
                                new BalanceBLL().ReuseByOperatorId(oper.Id);   //恢复余额
                                new BalanceBLL().Update(oper.Id, 0);          //初始化余额为0

                            }
                            else
                            {
                                MessageBox.Show("用户名重复，请更换用户名！");
                            }
                        }

                        ts.Complete();
                    }
                    if (i >= 1 || j > 0)
                    {

                        MessageBox.Show("成功添加用户！");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("添加用户失败！");
                        this.Close();
                    }
                }
            }
            //编辑
            else
            {
                if (pwdPassword.Password.Length <= 0)
                {
                    MessageBox.Show("请输入密码！");
                }
                else if (pwdRePassword.Password.Length <= 0)
                {
                    MessageBox.Show("请再次输入密码！");
                }
                else if (pwdPassword.Password != pwdRePassword.Password)
                {
                    MessageBox.Show("两次输入密码不一致！");
                }
                else
                {
                    txtUserName.IsReadOnly = true;
                    string pwd = pwdPassword.Password;
                    Guid operatorId = LoginWindow.GetOperatorId();
                    int i = new OperatorBLL().EditOperator(operatorId, pwd);
                    if (i >= 1)
                    {
                        MessageBox.Show("修改成功");
                        this.Close();
                    }
                    if (i <= 0)
                    {
                        MessageBox.Show("修改出错");
                    }
                }
            }

        }
        //窗口加载时
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (IsInsert)
            {

            }
            else
            {
                //修改密码时 动态为tbName 的Text属性赋值
                tbName.Text = "您要修改的用户";
                txtUserName.IsReadOnly = true;
                //把当前用户名填到txtUsername中
                EditingId = LoginWindow.GetOperatorId();
                Operator op = new OperatorBLL().GetOperatorById(EditingId);
                txtUserName.Text = op.UserName;
            }
        }
    }
}
