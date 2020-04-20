using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_23.Models
{
    public class COM_KIND
    {
        public SelectList com_kind { get; set; }
        public COM_KIND(int which = 1)
        {
            List<SelectListItem> x = new List<SelectListItem>
            {
            new SelectListItem() { Value = "0", Text = "- - -",   Selected = false },
            new SelectListItem() { Value = "1", Text = "電腦",   Selected = false },
            new SelectListItem() { Value = "2", Text = "伺服器",  Selected = false },
            new SelectListItem() { Value = "3", Text = "顯示器",  Selected = false },
            new SelectListItem() { Value = "4", Text = "印表機",   Selected = false },
            new SelectListItem() { Value = "5", Text = "網路設備", Selected = false },
            new SelectListItem() { Value = "6", Text = "其他",    Selected = false }
            };
            com_kind = new SelectList(x, "Value", "Text", which);
        }
        //public COM_KIND() : this(1)
        //{ }
    }
    public class MONEY_KIND
    {
        public SelectList money_kind { get; set; }
        public MONEY_KIND(string which = "NT")
        {
            List<SelectListItem> x = new List<SelectListItem>
            {
            new SelectListItem() { Value = "NT", Text = "NT",  Selected = false },
            new SelectListItem() { Value = "RMB", Text = "RMB", Selected = false }
            };
            money_kind = new SelectList(x, "Value", "Text", which);
        }
        //public MONEY_KIND() : this(1)
        //{ }
    }
    public class GetComDep
    {
        public SelectList ComDep { get; set; }
        public GetComDep(string dp_name = "")
        {
            TransartEntities db = new TransartEntities();
            List<String> s = db.computer
                               .Where(t => t.com_del == 0)
                               .Select(t => t.com_dpname.Trim())
                               .Distinct()
                               .ToList();
            List<SelectListItem> x = new List<SelectListItem>();
            foreach (string j in s)
            {
                SelectListItem k = new SelectListItem()
                {
                    Value = j,
                    Text = j,
                    Selected = false
                };
                x.Add(k);
            }
            ComDep = new SelectList(x, "Value", "Text", dp_name);
            db.Dispose();
        }
    }
    public class Edit_Model
    {
        public Data.Models.computer Computer { get; set; }
        public bool bool_com_usb
        {
            get
            {
                return (Computer.com_usb ?? 0) < 1 ? false : true;
            }
            set
            {
                if (value == true)
                    Computer.com_usb = 1;
                else
                    Computer.com_usb = 0;
            }
        }
        public bool bool_com_admin
        {
            get
            {
                return (Computer.com_admin ?? 0) < 1 ? false : true;
            }
            set
            {
                if (value == true)
                    Computer.com_admin = 1;
                else
                    Computer.com_admin = 0;
            }
        }
        public bool bool_com_del
        {
            get
            {
                return (Computer.com_del ?? 0) < 1 ? false : true;
            }
            set
            {
                if (value == true)
                    Computer.com_del = 1;
                else
                    Computer.com_del = 0;
            }
        }
    }
}