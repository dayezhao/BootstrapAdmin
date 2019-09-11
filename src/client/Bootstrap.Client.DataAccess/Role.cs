﻿using Bootstrap.Security.DataAccess;
using Longbow.Configuration;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrievesByUserName(string userName) => DbHelper.RetrieveRolesByUserName(userName);

        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators -> GroupNavigatorRole -> Role查查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrievesByUrl(string url) => DbHelper.RetrieveRolesByUrl(url, ConfigurationManager.GetValue("AppId", "2"));
    }
}
