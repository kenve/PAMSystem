using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PAMSystem.BLL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PAMSystem.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //载入数据都DataGrid
        public void LoadData()
        {
            Operator op = new Operator();
            // columnOperator.ItemsSource = new OperatorBLL().ListAll();
            columnItem.ItemsSource = new IdNameBLL().ListAll();
            columnCostType.ItemsSource = new IdNameBLL().GetByCategory("收支类型");
            dataGridShow.ItemsSource = new AccountBLL().GetByOperatorId(LoginWindow.GetOperatorId());
        }
        //主窗口加载时
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 设置窗口加载顺序 ，显示登录页面
            this.Hide();  //隐藏主窗口
            LoginWindow loginWin = new LoginWindow();
            if (loginWin.ShowDialog() != true)
            {
                //退出程序
                Application.Current.Shutdown();
                return;
            }
            #endregion
            #region MainWindow Loeded时为DataGrid 填充数据
            // Operator op = new Operator();
            //// columnOperator.ItemsSource = new OperatorBLL().ListAll();
            // columnItem.ItemsSource = new IdNameBLL().ListAll();
            // columnCostType.ItemsSource = new IdNameBLL().GetByCategory("收支类型");
            #region 显示搜索栏内容
            cmbSearchCostType.ItemsSource = new IdNameBLL().GetByCategory("收支类型");
            cmbSearchItem.IsEnabled = false;    //不可用需选中CostType
            ckbSearchItem.IsEnabled = false;
            dpSearchBeginDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);  //前一个月
            dpSearchEndDate.SelectedDate = DateTime.Now; //默认当前时间

            #endregion
            LoadData();
            this.ShowDialog();
            #endregion
        }
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
        // qq联系我
        private void btnCallSa_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=969199560&site=qq&menu=yes");
        }
        //修改密码
        private void menuEditOperator_Click(object sender, RoutedEventArgs e)
        {
            EditOpreatorWindow editOp = new EditOpreatorWindow();
            editOp.IsInsert = false;
            editOp.ShowDialog();
        }
        //添加收入
        private void menuAddIncome_Click(object sender, RoutedEventArgs e)
        {
            EditAccountWindow editAccountWin = new EditAccountWindow();
            editAccountWin.IsAddNew = true;
            if (editAccountWin.ShowDialog() != true)
            {
                LoadData();
            }
        }
        //添加支出
        private void menuAddExpense_Click(object sender, RoutedEventArgs e)
        {
            EditAccountWindow editAccountWin = new EditAccountWindow();
            editAccountWin.IsAddNew = true;
            if (editAccountWin.ShowDialog() != true)
            {
                LoadData();
            }

        }
        //退出系统
        private void menuExist_Click(object sender, RoutedEventArgs e)
        {


            Application.Current.Shutdown();
        }
        //添加用户
        private void menuAddOperator_Click(object sender, RoutedEventArgs e)
        {
            EditOpreatorWindow editOperatorWindow = new EditOpreatorWindow();
            //为判断是否为添加的属性IsInsert赋值为true
            editOperatorWindow.IsInsert = true;
            editOperatorWindow.ShowDialog();


        }
        //注销重启
        private void menuLogOff_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
            System.Reflection.Assembly.GetEntryAssembly();
            string startpath = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start(startpath + "/PAMSystem.UI.exe");  //xxxx.exe为要启动的程序
        }
        //实现拖动窗口
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //更新DataGuid按钮
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// 导出到Excel中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            //保存
            SaveFileDialog sdfExport = new SaveFileDialog();
            sdfExport.Filter = "Excel文件|*.xls";        //定义文件格式
            //判断是否保存
            if (sdfExport.ShowDialog() != true)
            {
                return;
            }
            //导出的文件名
            string filename = sdfExport.FileName;
            HSSFWorkbook workbook = new HSSFWorkbook();
            //创建一个表（sheet）
            ISheet sheet = workbook.CreateSheet("我的收支信息");
            //表头行
            IRow rowHeader = sheet.CreateRow(0);

            //创建单元格       （第0个     类型为字符串）
            rowHeader.CreateCell(0, CellType.STRING).SetCellValue("项目");
            rowHeader.CreateCell(1, CellType.STRING).SetCellValue("金额");
            rowHeader.CreateCell(2, CellType.STRING).SetCellValue("收支");
            rowHeader.CreateCell(3, CellType.STRING).SetCellValue("日期");
            rowHeader.CreateCell(4, CellType.STRING).SetCellValue("备注");

            //把查询结果导出到Excel
            Account[] accounts = (Account[])dataGridShow.ItemsSource;
            for (int i = 0; i < accounts.Length; i++)
            {
                Account account = accounts[i];
                //表体              第二行开始  

                IRow row = sheet.CreateRow(i + 1);
                //  row.CreateCell(0, CellType.STRING).SetCellValue(account.Item);
                IdName idname = new IdNameBLL().GetById(account.Item);//获得该对像的Item的ID 再查出名称
                row.CreateCell(0, CellType.STRING).SetCellValue(idname.Name);
                row.CreateCell(1, CellType.STRING).SetCellValue(account.Money);
                idname = new IdNameBLL().GetById(account.CostType);
                row.CreateCell(2, CellType.STRING).SetCellValue(idname.Name);
                row.CreateCell(4, CellType.STRING).SetCellValue(account.Remarks);
                //日期特殊处理
                //用workbook创建CreateCellStyle，CreateDataFormat

                ICellStyle styledate = workbook.CreateCellStyle();//样式
                IDataFormat format = workbook.CreateDataFormat();//格式
                //设置日期的显示格式
                styledate.DataFormat = format.GetFormat("yyyy\"年\"m\"月\"d\"日\"");
                //日期建为NUMERIC类型
                ICell cellDate = row.CreateCell(3, CellType.NUMERIC);
                cellDate.CellStyle = styledate;
                cellDate.SetCellValue(account.Date);
            }
            //文件流写入
            using (Stream stream = File.OpenWrite(filename))
            {
                workbook.Write(stream);
            }
            MessageBox.Show("导出成功！");
        }
        //删除选中数据
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Account account = (Account)dataGridShow.SelectedItem;
            //判断是否选中数据
            if (account == null)
            {
                MessageBox.Show("请选择一条数据！");
                return;
            }
            if (MessageBox.Show("确认删除此条数据吗？", "提醒", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IdName idname = new IdNameBLL().GetByName("收入");
                int i;   //deleted返回
                bool isSuceess;  //判断余额是否更新成功
                using (TransactionScope ts = new TransactionScope())
                {
                    i = new AccountBLL().DeleteById(account.Id);
                    Balance balance = new Balance();
                    if (idname.Id == account.CostType)//要删除的为收入
                    {
                        balance.Balances = (decimal)(-account.Money);
                        isSuceess = new BalanceBLL().Update(account.OperatorId, balance.Balances);
                    }
                    else
                    {
                        balance.Balances = (decimal)account.Money;
                        isSuceess = new BalanceBLL().Update(account.OperatorId, balance.Balances);
                    }
                    if (i > 0 && isSuceess)
                    {
                        MessageBox.Show("删除成功！");
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");
                    }
                    ts.Complete();
                }
                LoadData();
            }
        }
        //编辑修改选中行
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Account account = (Account)dataGridShow.SelectedItem;
            //判断是否选中数据
            if (account == null)
            {
                MessageBox.Show("请选择一条数据！");
                return;
            }
            EditAccountWindow edit = new EditAccountWindow();
            edit.IsAddNew = false;
            edit.EditingId = account.Id;
            edit.BeforeEditingMoney = (decimal)account.Money;//编辑前的钱
            edit.BeforeCostType = account.CostType;         //编辑前的收支类型
            //关闭编辑窗口并更新数据
            if (edit.ShowDialog() != true)
            {
                LoadData();
            }
        }
        //添加数据
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditAccountWindow edit = new EditAccountWindow();
            edit.IsAddNew = true;
            if (edit.ShowDialog() != true)
            {
                LoadData();
            }
        }
        //查看收入
        private void menuShowIncome_Click(object sender, RoutedEventArgs e)
        {
            dataGridShow.ItemsSource = null;
            //获得操作用户的Id
            Guid operatorId = LoginWindow.GetOperatorId();
            //查询收入的Id，并赋值
            IdName idname = new IdNameBLL().GetByName("收入");
            new AccountBLL().GetByOperatorIdAndCostType(operatorId, idname.Id);
            columnCostType.ItemsSource = new IdNameBLL().GetByCategory("收支类型");
            //根据 登录的ID 和收的Id查询 操作者的收入
            dataGridShow.ItemsSource = new AccountBLL().GetByOperatorIdAndCostType(operatorId, idname.Id);

        }
        //查看支出
        private void menuShowExpense_Click(object sender, RoutedEventArgs e)
        {
            dataGridShow.ItemsSource = null;
            //获得操作用户的Id
            Guid operatorId = LoginWindow.GetOperatorId();
            //查询收入的Id，并赋值
            IdName idname = new IdNameBLL().GetByName("支出");
            new AccountBLL().GetByOperatorIdAndCostType(operatorId, idname.Id);
            columnCostType.ItemsSource = new IdNameBLL().GetByCategory("收支类型");
            //根据 登录的ID 和收的Id查询 操作者的支出
            dataGridShow.ItemsSource = new AccountBLL().GetByOperatorIdAndCostType(operatorId, idname.Id);
        }
        //查看余额
        private void menuShowBalance_Click(object sender, RoutedEventArgs e)
        {
            BalanceWindow balance = new BalanceWindow();
            balance.IsShowBalance = true;
            balance.ShowDialog();
        }
        //添加余额
        private void menuAddBalance_Click(object sender, RoutedEventArgs e)
        {
            BalanceWindow balance = new BalanceWindow();
            balance.IsShowBalance = false;
            balance.ShowDialog();
        }
        //查看用户操作日志
        private void menuSearchOperationLog_Click(object sender, RoutedEventArgs e)
        {
            OperatingLogWindow oplog = new OperatingLogWindow();
            oplog.ShowDialog();
        }
        //关于
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("本程序只用于Wteam技术考核之用", "关于", MessageBoxButton.OK);
        }
        //搜索 Search
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            List<string> whereList = new List<string>();
            List<SqlParameter> parameter = new List<SqlParameter>();
            //  是否选中收支类型
            if (ckbSearchItem.IsChecked == true)
            {
                if (cmbSearchItem.SelectedItem == null)
                {
                    MessageBox.Show("请选择收支！");
                    return;
                }
                //添加到SqlParamerter中  
                whereList.Add("CostType=@CostType");
                parameter.Add(new SqlParameter("@CostType", cmbSearchCostType.SelectedValue));
            }
            //是否选中项目
            if (ckbSearchItem.IsChecked == true)
            {
                if (cmbSearchItem.SelectedItem == null)
                {
                    MessageBox.Show("请选择项目！");
                    return;

                }
                whereList.Add("Item=@Item");
                parameter.Add(new SqlParameter("@Item", cmbSearchItem.SelectedValue));

            }
            //选择的日期
            if (ckbSearchDate.IsChecked == true)
            {
                whereList.Add("Date Between @BeginDate and @EndDate");
                parameter.Add(new SqlParameter("@BeginDate", dpSearchBeginDate.SelectedDate));
                parameter.Add(new SqlParameter("@EndDate", dpSearchEndDate.SelectedDate));
            }
            //选择金钱
            if (ckbSearchMoney.IsChecked == true)
            {
                if (txtSearchMoney.Text.Length < 0)
                {
                    MessageBox.Show("请输入金钱！");
                    return;
                }
                whereList.Add("Money = @Money");
                parameter.Add(new SqlParameter("@Money",
                    txtSearchMoney.Text));
            }
            if (whereList.Count <= 0)
            {
                MessageBox.Show("至少选择一个查询条件");//防止查询出的结果过多
                return;
            }
            //拼接Sql语句
            string sql = "select * from T_Account where "
                + string.Join(" and ", whereList) + " and IsDeleted=0";
            //把拼接的SQL语句，和SqlParameter 数组 传入
            Account[] account = new AccountBLL().Search(sql, parameter.ToArray());
            if (account.Length > 0)
            {
                dataGridShow.ItemsSource = account;
            }
            else
            {
                MessageBox.Show("没有相关数据！");
            }
        }
        //搜索coatType Item 联动
        private void cmbSearchCostType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSearchItem.IsEnabled = true;
            IdNameBLL bll = new IdNameBLL();
            if (cmbSearchCostType.SelectedIndex == 0)
            {
                cmbSearchItem.ItemsSource = bll.GetByCategory("收入类型");
            }
            else
            {
                cmbSearchItem.ItemsSource = bll.GetByCategory("支出类型");
            }
        }
        //ckbSearchCostType_Checked  ckbSearchItem可用
        private void ckbSearchCostType_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbSearchCostType.IsChecked == true)
            {
                ckbSearchItem.IsEnabled = true;// 项目选项可用
            }
            else
            {
                ckbSearchItem.IsEnabled = false;
            }
        }
        //删除账户信息
        private void menuDeletedOperator_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除此账户的数据吗？", "提醒", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool isAddBalanceSuccess;
                bool isOperatorSuccess;
                using (TransactionScope ts = new TransactionScope())
                {
                    isAddBalanceSuccess = new BalanceBLL().DeleteByOperatorId(LoginWindow.GetOperatorId());
                    isOperatorSuccess = new OperatorBLL().DeleteOperator(LoginWindow.GetOperatorId());
                    ts.Complete();
                }

                if (isOperatorSuccess && isAddBalanceSuccess)
                {
                    MessageBox.Show("用户删除成功！");
                    this.Close();
                    //注销重启
                    Application.Current.Shutdown();
                    System.Reflection.Assembly.GetEntryAssembly();
                    string startpath = System.IO.Directory.GetCurrentDirectory();
                    System.Diagnostics.Process.Start(startpath + "/PAMSystem.UI.exe");
                }
                else
                {
                    MessageBox.Show("用户删除失败！");
                }


            }


        }
    }
}
