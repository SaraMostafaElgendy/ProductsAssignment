using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsAssignment;

namespace ProductsAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ProductContext _context;

        public CategoriesController(ProductContext context)
        {
            _context = context;
        }

        // GET: Categories
        [HttpGet]
        public IActionResult GetCategories()
        {

            Result output = new Result();
            try
            {
                if (_context.Category.Count() == 0)
                {
                    output.success = true;
                    output.messages[0] = "There's no categories in DB";
                    return Ok(output);
                }
                output.success = true;
                output.results = _context.Category;
                return Ok(output);
            }
            catch (Exception ex)
            {
                output.success = false;
                output.messages[0] = "Error while retrieving data from DB";
                return NotFound(output);
            }
        }



    }
}
