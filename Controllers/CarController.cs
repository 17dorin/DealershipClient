using DealershipClient.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealershipClient.Controllers
{
    public class CarController : Controller
    {
        private CarDAL carInv = new CarDAL();

        public IActionResult Index()
        {
            List<Car> inv = carInv.GetInventory();

            return View(inv);
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(int year, string make, string model, string color)
        {
            //Handles setting the year search parameter to a string, alternatively could alter your table to have year as a string
            string yearSearch;
            if(year != 0)
            {
                yearSearch = year.ToString();
            }
            else
            {
                yearSearch = null;
            }

            //Creates a dictionary where the key is the API endpoint, and the value is the parameter to search that endpoint with
            Dictionary<string, string> searches = new Dictionary<string, string>();
            searches.Add("year", yearSearch);
            searches.Add("make", make);
            searches.Add("model", model);
            searches.Add("color", color);

            //Parses dictionary down to only key value pairs where the value != null
            Dictionary<string, string> notNullSearches = searches.Where(x => x.Value != null).ToDictionary(x => x.Key, x=> x.Value);

            //Gets initial list of cars based on first item in search dictionary
            List<Car> inv = new List<Car>();
            if(notNullSearches.Count != 0)
            {
                try
                {
                    inv = carInv.GetInventory(notNullSearches.First().Key, notNullSearches.First().Value);
                }
                catch
                {
                    return View("Index", inv);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

            //Removes item from dictionary
            notNullSearches.Remove(notNullSearches.First().Key);

            //Performs LINQ statements on inventory list to parse it down, if any search items remain in the dictionary
            if(notNullSearches.Count != 0)
            {
                foreach(KeyValuePair<string, string> kvp in notNullSearches)
                { 
                    inv = inv.Where(x => x.ToString().ToLower().Contains(kvp.Value.ToLower())).ToList();
                }
            }

            return View("Index", inv);
        }
    }
}
