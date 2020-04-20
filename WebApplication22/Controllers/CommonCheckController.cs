using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication22.Controllers
{
    public class CommonCheckController : Controller
    {
        //資料庫物件
        TransartEntities db = new TransartEntities();

        // GET: CommonCheck
        public ActionResult Index()
        {
            return View();
        }

        public string checkEmp(string emno)
        {
            string resultReturn = "";
            employee emp;
            if (emno != null & emno.Trim() != "")
            {
                emp = db.employee.Find(emno.Trim());
                if (emp != null)
                {
                    resultReturn = emp.em_cname.Trim();
                    dept dept = db.dept.Find(emp.em_dpno.Trim());
                    if (dept != null)
                    {
                        resultReturn = resultReturn + "!" + dept.dp_no.Trim() + "!" + dept.dp_name.Trim();
                    }
                }
            }

            if (resultReturn == "")
            {
                resultReturn = "fail";
            }
            return resultReturn;
        }
    }
}