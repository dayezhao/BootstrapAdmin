﻿using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class GroupsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Group> Get([FromUri]QueryGroupOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Group Get(int id)
        {
            return GroupHelper.RetrieveGroups().FirstOrDefault(t => t.ID == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Group value)
        {
            return GroupHelper.SaveGroup(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return GroupHelper.DeleteGroup(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Group> Post(int id, [FromBody]JObject value)
        {
            var ret = new List<Group>();
            dynamic json = value;
            switch ((string)json.type)
            {
                case "user":
                    ret = GroupHelper.RetrieveGroupsByUserId(id).ToList();
                    break;
                default:
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put(int id, [FromBody]JObject value)
        {
            var ret = false;
            dynamic json = value;
            string groupIds = json.groupIds;
            switch ((string)json.type)
            {
                case "user":
                    ret = GroupHelper.SaveGroupsByUserId(id, groupIds);
                    break;
                default:
                    break;
            }
            return ret;
        }
    }
}
