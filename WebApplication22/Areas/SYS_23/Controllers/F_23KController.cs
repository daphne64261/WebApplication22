using Common.Menu;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23KController : Controller
    {
        public TransartEntities Db { get; set; }
        public ActionResult P_23K_e()
        {
            Sysfunclist_Info.Get_sfl_id(this);
            Db = new TransartEntities();
            //List<ISODOC> doc = Db.ISODOC.Where(x => x.The_Kind == 0).ToList();
            string strSQL = "select * from ISODOC where The_Kind = 0";
            List<ISODOC> doc = Db.Database.SqlQuery<ISODOC>(strSQL).ToList();
            doc.Add(new ISODOC());
            return View(doc);
        }
        [HttpPost]
        public ActionResult Save(List<ISODOC> x)
        {
            int i = 0, RowsAffected = 0;
            ISODOC r;
            string strSQL;
            Db = new TransartEntities();
            for (i = 0; i < x.Count - 1; i++)
            {
                r = x[i];
                if ((r.The_No != null) && (string.IsNullOrEmpty(r.The_Name)))
                {
                    strSQL = "delete from ISODOC where The_Kind = " + r.The_Kind +
                             " and The_No = '" + r.The_No + "'";
                }
                else
                {
                    strSQL = "update ISODOC set The_Name = '" + r.The_Name + "' ";
                    if (r.v1 != null)
                        strSQL = strSQL + ", v1 = '" + r.v1 + "' ";
                    else
                        strSQL = strSQL + ", v1 = null ";
                    if (r.v2 != null)
                        strSQL = strSQL + ", v2 = '" + r.v2 + "' ";
                    else
                        strSQL = strSQL + ", v2 = null ";
                    if (r.v3 != null)
                        strSQL = strSQL + ", v3 = '" + r.v3 + "' ";
                    else
                        strSQL = strSQL + ", v3 = null ";
                    if (r.v4 != null)
                        strSQL = strSQL + ", v4 = '" + r.v4 + "' ";
                    else
                        strSQL = strSQL + ", v4 = null ";
                    if (r.v5 != null)
                        strSQL = strSQL + ", v5 = '" + r.v5 + "' ";
                    else
                        strSQL = strSQL + ", v5 = null ";
                    if (r.v6 != null)
                        strSQL = strSQL + ", v6 = '" + r.v6 + "' ";
                    else
                        strSQL = strSQL + ", v6 = null ";
                    if (r.v7 != null)
                        strSQL = strSQL + ", v7 = '" + r.v7 + "' ";
                    else
                        strSQL = strSQL + ", v7 = null ";
                    if (r.v8 != null)
                        strSQL = strSQL + ", v8 = '" + r.v8 + "' ";
                    else
                        strSQL = strSQL + ", v8 = null ";
                    if (r.v9 != null)
                        strSQL = strSQL + ", v9 = '" + r.v9 + "' ";
                    else
                        strSQL = strSQL + ", v9 = null ";
                    if (r.v10 != null)
                        strSQL = strSQL + ", v10 = '" + r.v10 + "' ";
                    else
                        strSQL = strSQL + ", v10 = null ";
                    if (r.v11 != null)
                        strSQL = strSQL + ", v11 = '" + r.v11 + "' ";
                    else
                        strSQL = strSQL + ", v11 = null ";
                    if (r.v12 != null)
                        strSQL = strSQL + ", v12 = '" + r.v12 + "' ";
                    else
                        strSQL = strSQL + ", v12 = null ";
                    if (r.v13 != null)
                        strSQL = strSQL + ", v13 = '" + r.v13 + "' ";
                    else
                        strSQL = strSQL + ", v13 = null ";
                    if (r.v14 != null)
                        strSQL = strSQL + ", v14 = '" + r.v14 + "' ";
                    else
                        strSQL = strSQL + ", v14 = null ";
                    if (r.v15 != null)
                        strSQL = strSQL + ", v15 = '" + r.v15 + "' ";
                    else
                        strSQL = strSQL + ", v15 = null ";

                    strSQL = strSQL + " where The_Kind = 0 and The_No = '" + r.The_No + "'";
                }
                RowsAffected = Db.Database.ExecuteSqlCommand(strSQL);
            }
            r = x[i];
            if (!string.IsNullOrEmpty(r.The_Name))
            {
                r.The_No = "TA-" + (int.Parse(x[i - 1].The_No.Substring(3, 5)) + 1).ToString();
                strSQL = "Insert into ISODOC (The_Kind, The_No, The_Name, ";
                strSQL = strSQL + "v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, ";
                strSQL = strSQL + "v11, v12, v13, v14, v15) values (" + r.The_Kind;
                strSQL = strSQL + ", '" + r.The_No + "', '" + r.The_Name + "' ";
                if (r.v1 != null)
                    strSQL = strSQL + ", '" + r.v1 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v2 != null)
                    strSQL = strSQL + ", '" + r.v2 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v3 != null)
                    strSQL = strSQL + ", '" + r.v3 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v4 != null)
                    strSQL = strSQL + ", '" + r.v4 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v5 != null)
                    strSQL = strSQL + ", '" + r.v5 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v6 != null)
                    strSQL = strSQL + ", '" + r.v6 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v7 != null)
                    strSQL = strSQL + ", '" + r.v7 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v8 != null)
                    strSQL = strSQL + ", '" + r.v8 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v9 != null)
                    strSQL = strSQL + ", '" + r.v9 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v10 != null)
                    strSQL = strSQL + ", '" + r.v10 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v11 != null)
                    strSQL = strSQL + ", '" + r.v11 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v12 != null)
                    strSQL = strSQL + ", '" + r.v12 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v13 != null)
                    strSQL = strSQL + ", '" + r.v13 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v14 != null)
                    strSQL = strSQL + ", '" + r.v14 + "' ";
                else
                    strSQL = strSQL + ", null ";
                if (r.v15 != null)
                    strSQL = strSQL + ", '" + r.v15 + "' ";
                else
                    strSQL = strSQL + ", null ";
                strSQL = strSQL + ")";
                RowsAffected = Db.Database.ExecuteSqlCommand(strSQL);
            }
            Db.SaveChanges();
            return RedirectToAction("P_23K_e");
        }
    }
}