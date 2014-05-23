using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.BLL
{
    //显示登录结果的枚举
    public enum LoginResult
    {
       // NoUserName,                //无用户
        ErrorNameOrPwd,           //用户名或密码错误,包括用户名不存在和密码错误
        LoginSuccessful           //登录成功

    }
}
