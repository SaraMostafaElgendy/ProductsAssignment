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
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;


        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        // GET: Products
        /// <summary>
        /// Get All products in the DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProducts()
        {
            Result output = new Result();
            try
            {
                if (_context.Products.Count() == 0)
                {
                    output.success = true;
                    output.messages[0] = "There's no products in DB";
                    return Ok(output);
                }
                output.success = true;
                output.results = _context.Products;
                return Ok(output);

            }
            catch (Exception ex)
            {
                output.success = false;
                output.messages[0] = "Error while retrieving data from DB";
                return NotFound(output);

            }
        }

        // GET: Products/Details/5
        /// <summary>
        /// Gt product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Details(int? id)
        {
            Result output = new Result();
            try
            {
                if (id == null)
                {
                    output.messages[0] = "ID is null please enter valid ID";
                    output.success = false;
                    return NotFound(output);
                }
                var products = _context.Products
                    .FirstOrDefault(m => m.ID == id);
                if (products == null)
                {
                    output.success = true;
                    output.messages[0] = "there's no product in DB with this ID";
                    return NotFound(output);
                }

                output.success = true;
                output.results = products;
                return Ok(output);


            }
            catch (Exception ex)
            {
                output.success = false;
                output.messages[0] = "Error while retrieving data from DB";
                return NotFound();

            }
        }

        /// <summary>
        /// List all products under category id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("categoryId")]
        public IActionResult Category([FromQueryAttribute] int? categoryId)
        {
            Result output = new Result();
            try
            {
                if (categoryId == null)
                {
                    output.messages[0] = "ID is null please enter valid ID";
                    output.success = false;
                    return NotFound(output);
                }
                var products = _context.Products
                    .Where(m => m.Category.ID == categoryId).ToList();
                if (products == null || products.Count() == 0)
                {
                    output.success = true;
                    output.messages[0] = "there's no product in DB with this ID";
                    return NotFound(output);
                }

                output.success = true;
                output.results = products;
                return Ok(output);


            }
            catch (Exception ex)
            {
                output.success = false;
                output.messages[0] = "Error while retrieving data from DB";
                return NotFound();

            }
        }

        /// <summary>
        /// cretae new product
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] ProductInput products)
        {
            Result output = new Result();
            try
            {
                if (ModelState.IsValid)
                {
                    Products prd = new Products()
                    {
                        Name = products.Name,
                        Category = _context.Category.FirstOrDefault(x => x.ID == products.CategoryID),
                        ImgURL = products.ImgURL,
                        Price = products.Price,
                        Quantity = products.Quantity
                    };

                    _context.Add(prd);
                    var xx = _context.SaveChanges();
                    output.success = true;
                    output.messages[0] = "Record added successfully";
                    return StatusCode(201, output);
                }
                output.success = false;
                output.messages[0] = "model is not valid";
                return NotFound(products);
            }
            catch (Exception ex)
            {
                output.success = false;
                output.messages[0] = "can't add record in DB";
                return NotFound();
            }
        }


        /// <summary>
        /// update certain product object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [Bind("ID,Name,Price,Quantity,ImgURL")] Products products)
        {
            Result output = new Result();
            try
            {

                if (id != products.ID)
                {
                    output.success = false;
                    output.messages[0] = "error in the ID";
                    return NotFound(output);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(products);
                        _context.SaveChangesAsync();
                        output.success = true;
                        output.results = products;
                        output.messages[0] = "data updated successfully";
                        return Ok(output);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductsExists(products.ID))
                        {
                            output.success = false;
                            output.messages[0] = "product not exist in db";
                            return NotFound(output);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                output.success = true;
                output.messages[0] = "error in the model";
                return Ok(output);
            }
            catch (Exception ex)
            {
                output.success = false;
                output.messages[0] = "Error while update data in DB";
                return NotFound();
            }
        }



        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
