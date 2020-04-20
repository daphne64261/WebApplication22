using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common.BasicLibrary
{
    public class GetDropList
    {
     
    }

    public class GetList
    {
        TransartEntities Db = new TransartEntities();

        public List<SelectListItem> deptList(string depNo)
        {
            //var dp_list = new List<SelectListItem>();//設定部門選單
            List<SelectListItem> dp_list = new List<SelectListItem>();
            //設定部門選單            
            dp_list.Add(new SelectListItem() { Text = "- - -", Value = "", Selected = false });
            foreach (var Dept_item in Db.dept.Where(s => s.dp_no != "000").OrderBy(s => s.dp_no).ToList())
            {
                dp_list.Add(new SelectListItem()
                {
                    Text = Dept_item.dp_name.Trim(),
                    Value = Dept_item.dp_no.Trim(),
                    Selected = Dept_item.dp_no.Trim().Equals(depNo)
                });
            }

            return dp_list;
        }
    }
}