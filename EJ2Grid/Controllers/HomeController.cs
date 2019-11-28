using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EJ2Grid.Controllers
{
    public class HomeController : Controller
    {
        public static List<Orders> order = new List<Orders>();
        public ActionResult Index()
        {
            if (order.Count == 0)
                BindDataSource();
            ViewBag.data = order;
            return View();
        }
        public ActionResult UrlDatasource(DataManagerRequest dm)
        {
            IEnumerable DataSource = order;
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Orders>().Count();
            if (dm.Select != null)
            {
                DataSource = operation.PerformSelect(DataSource, dm.Select);  // Selected the columns value based on the filter request
                DataSource = DataSource.Cast<dynamic>().Distinct().AsEnumerable(); // Get the distinct values from the selected column
            }
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public void BindDataSource()
        {
            order.Add(new Orders(10248, 1, "VINET", "Reims"));
            order.Add(new Orders(10249, 2, "TOMSP", "Münster"));
            order.Add(new Orders(10250, 3, "SUPRD", "Rio"));
            order.Add(new Orders(10251, 4, "HANAR", "Lyon"));
            order.Add(new Orders(10252, 5, "CHOPS", "Resende"));
            order.Add(new Orders(10253, 2, "HILAA", "Graz"));
        }
        
        public class Orders
        {
            public Orders()
            {

            }
            public Orders(int OrderID, int EmployeeID, string CustomerID, string ShipCity)
            {
                this.OrderID = OrderID;
                this.EmployeeID = EmployeeID;
                this.CustomerID = CustomerID;
                this.ShipCity = ShipCity;
            }
            public int OrderID { get; set; }
            public int EmployeeID { get; set; }
            public string CustomerID { get; set; }
            public string ShipCity { get; set; }
        }
    }
}