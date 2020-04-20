using Common.Menu;
using Data.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication22.Controllers
{
    public class PopupController : Controller
    {
        TransartEntities db = new TransartEntities();
        public ActionResult Show(string PopupID, string FieldKind, int? PageNumber, int? PageSize)
        {
            int m_pagesize = PageSize ?? 10;
            int m_pagenumber = PageNumber ?? 1;
            ViewBag.PopupID = PopupID;
            ViewBag.PageNumber = m_pagenumber;
            ViewBag.PageSize = m_pagesize;
            ViewBag.FieldKind = FieldKind;

            My_Select_List mL;
            if (FieldKind == "dept")
            {
                mL = new Dept_Select_List();
            }
            else
            {
                mL = new Emp_Select_List();
            }
            IOrderedQueryable<Popup> sL = mL.GetSelectList();
            IPagedList<Popup> d = sL.ToPagedList(m_pagenumber, m_pagesize);
            //return PartialView("Show", d); 測試此法, 點選下一頁後, 會轉變成網頁畫面而非維持視窗
            return PartialView(d);
        }
        public ActionResult EmpOfDept(string PopupID, string DPNO, int? PageNumber, int? PageSize)
        {
            int m_pagesize = PageSize ?? 10;
            int m_pagenumber = PageNumber ?? 1;
            string m_dpno = DPNO == "undefined" ? "" : DPNO;
            ViewBag.PopupID = PopupID;
            ViewBag.PageNumber = m_pagenumber;
            ViewBag.PageSize = m_pagesize;
            ViewBag.DPNO = m_dpno;

            //My_Select_List mL = new Emp_Select_List(m_dpno);//受限部門代碼篩選員工
            My_Select_List mL = new Emp_Select_List();//取得所有員工
            IOrderedQueryable<Popup> sL = mL.GetSelectList();
            IPagedList<Popup> d = sL.ToPagedList(m_pagenumber, m_pagesize);
            return PartialView(d);
        }

        public ActionResult ShowList_employee(int? page_number, int? page_size)
        {
            //員工全選
            int m_page_number = page_number ?? 1;
            int m_page_size = page_size ?? 15; //對應現行系統筆數
            ViewBag.page_number = m_page_number;
            ViewBag.page_size = m_page_size;

            IOrderedQueryable<employee> w = db.employee
                                              .OrderBy(x => x.em_no);
            IPagedList z = w.ToPagedList(m_page_number, m_page_size);
            if (z.TotalItemCount > 0)
                return PartialView(z);
            else
                return HttpNotFound();
        }
    }
}