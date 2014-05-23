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
    /// EditIncomeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditAccountWindow : Window
    {

        public bool IsAddNew { get; set; }//是否为添加
        public Guid EditingId { get; set; } //编辑的Id
        public decimal BeforeEditingMoney { get; set; }  //编辑前的钱的数量
        public Guid BeforeCostType { get; set; }   //编辑前的收支类别
        public EditAccountWindow()
        {
            InitializeComponent();
        }
        //鼠标进入btnClose改变前景色
        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //点击取消按钮的事件
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //点击保存按钮的
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Account account = (Account)gridEditAccount.DataContext;
            AccountBLL accountBll = new AccountBLL();
            //判断金额大于0
            if (cbType.SelectedIndex > -1)
            {
                if (account.Money <= 0)
                {
                    MessageBox.Show("你输入的收入金额不合法，请重新输入！");
                    return;
                }
            }
            if (cbType.SelectedIndex < 0)
            {
                MessageBox.Show("请选择收支！");
                return;
            }
            else if (cbItem.SelectedIndex < 0)
            {
                MessageBox.Show("请选择项目！");
                return;
            }
            //有问题
            else
            {
                //获得名字为收入的Id值
                IdName idname = new IdNameBLL().GetByName("收入");
                Balance balance = new Balance();
                #region 添加收支
                //添加收支
                if (IsAddNew)
                {
                    int i;
                    //分布式事务处理收支和余额，可实现回滚
                    using (TransactionScope ts = new TransactionScope())
                    {
                        i = accountBll.AddNew(account);
                        //判断是否为收入,收入为正的，支出为负数
                        if (account.CostType == idname.Id)    //收入
                        {
                            balance.Balances = (decimal)account.Money;
                        }
                        else //支出
                        {
                            balance.Balances = (decimal)(-account.Money);
                        }

                        new BalanceBLL().Update(account.OperatorId, balance.Balances);
                        ts.Complete();//表示已完成
                    }
                    //严谨点Update也应该返回true or false 
                    if (i < 0)
                    {
                        new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "添加收支，更新余额失败！");
                        MessageBox.Show("添加错误！");
                    }
                    else if (i > 1)
                    {
                        new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "添加收支时错误！");
                        throw new Exception("数据发生错误！");
                    }
                    else
                    {
                        new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "添加收支，更新余额成功！");//日志
                        MessageBox.Show("添加成功！");
                        this.Close();
                    }
                }
                #endregion

                #region 编辑收支
                //编辑收支
                else
                {
                    bool i;//插入余额是否成功
                    bool isAddbalanceSuc;//插入余额是否成功
                    //分布式处理
                    using (TransactionScope ts = new TransactionScope())
                    {
                        //更新收支
                        i = accountBll.Update(account);

                        //更新前的数据类型是否为收入，并计算出差额，
                        if (BeforeCostType == idname.Id)
                        {
                            //收入改为收入 只是金额的更改
                            if (account.CostType == idname.Id) //更改后收支类型，为收入
                            {
                                if ((decimal)account.Money != BeforeEditingMoney)    //更新后的钱不等更新前的前
                                {
                                    balance.Balances = (decimal)account.Money - BeforeEditingMoney;   //加上前后的差额（后大于前取+，否则为—）
                                }
                                else
                                {
                                    balance.Balances = 0;
                                }
                            }
                            //收入改为支出，前为收，后为支
                            else
                            {
                                // 先减去更新前的收入，再减去更新后的支出金额
                                balance.Balances = -BeforeEditingMoney - (decimal)account.Money;
                            }
                        }
                        //若更新前为支出，计算出差额
                        else
                        {
                            //判断更新后的类型
                            if (account.CostType == idname.Id)//更新后的类型为收入，更新前为支出
                            {
                                balance.Balances = BeforeEditingMoney + (decimal)account.Money;  //先加上更新前的支出，再加上更新后的收入
                            }
                            //更新后为支出，更新前为支出
                            else
                            {
                                if (BeforeEditingMoney != (decimal)account.Money)            //更新前的钱不等于更新后的钱，求钱差额
                                {
                                    balance.Balances = BeforeEditingMoney - (decimal)account.Money;   //更新前 减去更新后  若更新前大，那就加上差额
                                }
                                else
                                {
                                    balance.Balances = 0;
                                }
                            }
                        }
                        isAddbalanceSuc = new BalanceBLL().Update(account.OperatorId, balance.Balances);
                        ts.Complete();
                    }
                    if (i && isAddbalanceSuc)
                    {
                        new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "编辑收支成功！");
                        MessageBox.Show("更改成功！");
                        this.Close();
                    }
                    else
                    {
                        new OperationLogBLL().Insert(LoginWindow.GetOperatorId(), "编辑收支失败！");
                        MessageBox.Show("更新失败！");
                    }
                }
                #endregion

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IdNameBLL idnamebll = new IdNameBLL();
            Account account = new Account();
            //获得用户的Id
            account.OperatorId = LoginWindow.GetOperatorId();


            // MessageBox.Show("执行了！");
            //添加
            if (IsAddNew)
            {

                //设置默认值
                account.Date = DateTime.Now;
                account.Money = 0;
                account.Remarks = "无";
                gridEditAccount.DataContext = account;
                cbType.ItemsSource = idnamebll.GetByCategory("收支类型");
                //cbType.SelectedIndex = 0;
            }
            //编辑
            else
            {
                //拿出要编辑的对象的值
                account = new AccountBLL().GetById(EditingId);
                gridEditAccount.DataContext = account;
                cbType.ItemsSource = idnamebll.GetByCategory("收支类型");

            }
        }
        //cbType 与cbItem的联动
        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IdNameBLL bll = new IdNameBLL();
            if (cbType.SelectedIndex == 0)
            {
                cbItem.ItemsSource = bll.GetByCategory("收入类型");
            }
            else
            {
                cbItem.ItemsSource = bll.GetByCategory("支出类型");
            }
        }
    }
}
