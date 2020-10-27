using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookStoreWeb.Models;
using Newtonsoft.Json;

namespace MyBookStoreWeb.Controllers
{
    public class BookController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();

        Book _book = new Book();
        List<Book> _books = new List<Book>();

        public BookController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
        }


        // GET: BookController
        // Landing Page
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Book> book = new List<Book>();
            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync("https://localhost:44393/api/book"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    book = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }

            return View(book);
        }


        // Add Book Page
        [HttpGet]
        public IActionResult AddBook(int id = 0)
        {
            return View(new Book());
        }

        // Insert Book Data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook([Bind("BookId,Title,Author,Category,Price")] Book book)
        {
            _book = new Book();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

                using (var response = await httpclient.PostAsync("https://localhost:44393/api/book/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Get Book details to edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            _book = new Book();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync("https://localhost:44393/api/book/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
            return View(_book);
        }

        // Submit Edited data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BookId,Title,Author,Category,Price")] Book book)
        {
            _book = new Book();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

                using (var response = await httpclient.PostAsync("https://localhost:44393/api/book/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Fetch Book Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _book = new Book();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync("https://localhost:44393/api/book/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
            return View(_book);
        }

      
        // Remove a Book
        public async Task<IActionResult> Delete(int id)
        {
            _book = new Book();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.DeleteAsync("https://localhost:44393/api/book/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
