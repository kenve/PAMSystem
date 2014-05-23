using PAMSystem.DAL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.BLL
{

   public class OperatorBLL
    {
       /// <summary>
       /// 用户登录的判断
       /// </summary>
       /// <param name="userName"></param>
       /// <param name="password"></param>
       /// <returns></returns>
       public LoginResult UserLoginResult(string userName,string password)
       {
           OperatorDAL dal = new OperatorDAL();
         Operator model = dal.GetByUserName(userName);
           //获得用户登录的Id

          if (model==null)
          {
              return LoginResult.ErrorNameOrPwd;
          }
          else
          {
              //判断密码是否相等
               string dbMd5 = model.Password; //数据库中存储的密码值
                string mymd5 = CommonHelper.GetMD5(password +CommonHelper.GetPasswordSalt());
                if (dbMd5 == mymd5)
                {
                    return LoginResult.LoginSuccessful;
                }
                else
                {
                    //密码不正确，返回ErrorNameOrPwd密码或用户名错误
                    return LoginResult.ErrorNameOrPwd;
                }
          }
       }
       /// <summary>
       /// 添加用户
       /// </summary>
       /// <param name="name"></param>
       /// <param name="password"></param>
       /// <returns></returns>
       public int AddOpertator(string name,string password)
       {
           OperatorDAL dal = new OperatorDAL();
           string dbPassword = CommonHelper.GetMD5(password + CommonHelper.GetPasswordSalt());
           Operator operators=new Operator();
           operators.UserName=name;
           operators.Password=dbPassword;
           int i=dal.Insert(operators);
           return i;
       }
       //编辑用户信息,修改密码
       public int EditOperator(Guid operatorId,string password)
       {
           OperatorDAL dal = new OperatorDAL();
           string md5password = CommonHelper.GetMD5(password + CommonHelper.GetPasswordSalt());
           int i= dal.Update(operatorId, md5password);
           return i;
       }
       /// <summary>
       /// 根据用户名获得用户信息
       /// </summary>
       /// <param name="userName"></param>
       /// <returns></returns>
       public Operator GetOperatorByUserName(string userName)
       {
           OperatorDAL dal = new OperatorDAL();
           Operator model = dal.GetByUserName(userName);
           //获得用户登录的Id
           if (model == null)
           {
               return null;
           }
            else
            {
                   return model;
            }
           
       }
       //根据Id获得用户信息
       public Operator GetOperatorById(Guid id)
       {
           return new OperatorDAL().GetById(id);
       }
       //列出所有的用户，用于绑定数据源
      public Operator[] ListAll()
       {
           return new OperatorDAL().ListAll(); 
       }
       //删除用户
       public bool DeleteOperator(Guid operatorId)
       {
         int i=  new OperatorDAL().DeleteById(operatorId);
         if (i>0)
         {
             return true;
         }
         else
         {
             return false;
         }
       }
       //恢复删除用户
       public bool ReuseOperator(Guid operatorId)
       {
           int i = new OperatorDAL().ReuseOperator(operatorId);
           if (i > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }

    }
}
