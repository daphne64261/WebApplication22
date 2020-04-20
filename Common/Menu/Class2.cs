using Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Common.Menu
{
    public class Popup
    {
        [Display(Name = "編號")]
        public string Value { get; set; }
        [Display(Name = "名稱")]
        public string Text { get; set; }

    }
    public class My_Select_List
    {
        protected TransartEntities Db;
        protected IOrderedQueryable<Popup> popup_list;
        public My_Select_List()
        {
            Db = new TransartEntities();
        }
        public IOrderedQueryable<Popup> GetSelectList()
        {
            return popup_list;
        }
    }
    public class Dept_Select_List : My_Select_List
    {
        public Dept_Select_List()
        {
            popup_list = Db.dept
                           .Select(x => new Popup()
                           {
                               Value = x.dp_no.Trim(),
                               Text = x.dp_name,
                           })
                           .OrderBy(r => r.Value);
        }
    }
    public class Emp_Select_List : My_Select_List
    {
        public Emp_Select_List()
        {
            popup_list = Db.employee
                           .Select(x => new Popup()
                           {
                               Value = x.em_no.Trim(),
                               Text = x.em_cname,
                           })
                           .OrderBy(r => r.Value);
        }
        public Emp_Select_List(string dpno)
        {
            popup_list = Db.employee
                           .Where(x => string.IsNullOrEmpty(dpno) || x.em_dpno.Trim() == dpno.Trim())
                           .Select(x => new Popup()
                           {
                               Value = x.em_no.Trim(),
                               Text = x.em_cname,
                           })
                           .OrderBy(r => r.Value);
        }
    }
    public class Car_Type_List : SelectList
    {
        public Car_Type_List() : base(sss(), "Value", "Text", 0)
        { }
        private static List<SelectListItem> sss()
        {
            return new List<SelectListItem>
                     {
                         new SelectListItem() { Text = "     ", Value = "0", Selected = false },
                         new SelectListItem() { Text = "自用車", Value = "1", Selected = false },
                         new SelectListItem() { Text = "公務車", Value = "2", Selected = false }
                     };
        }
    }
    public class Car_Kind_List : SelectList
    {
        public Car_Kind_List() : base(ttt(), "Value", "Text", 0)
        { }
        private static List<SelectListItem> ttt()
        {
            return new List<SelectListItem>
                     {
                         new SelectListItem() { Text = "   ", Value = "0", Selected = false },
                         new SelectListItem() { Text = "汽車", Value = "1", Selected = false },
                         new SelectListItem() { Text = "機車", Value = "2", Selected = false }
                     };
        }
    }
}