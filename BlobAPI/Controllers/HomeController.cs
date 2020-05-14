using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlobAPI.Models;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.IO;

namespace BlobAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Blob()
        {
            // string connectionString = Environment.GetEnvironmentVariable("DefaultEndpointsProtocol=https;AccountName=pffile;AccountKey=U9RuCZ5BV3xORbOb//LLh8KW09jVpE41oVzoEY9ZHGZeor4oXKTIIHXd/9bDQvNzON7Eo6ka4HgVe+5HC099lg==;EndpointSuffix=core.windows.net");

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=pffile;AccountKey=U9RuCZ5BV3xORbOb//LLh8KW09jVpE41oVzoEY9ZHGZeor4oXKTIIHXd/9bDQvNzON7Eo6ka4HgVe+5HC099lg==;EndpointSuffix=core.windows.net");

            //Create a unique name for the container
            string containerName = "monthlystatement";

            // Create the container and return a container client object
            //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);


            List<BlobInfoClass> lst = new List<BlobInfoClass>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                lst.Add( new BlobInfoClass { fileName = blobItem.Name });
            }

            var qry = lst.FirstOrDefault();


            BlobClient blobClient = containerClient.GetBlobClient(qry.fileName);
            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            //using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
            //{
            //    await download.Content.CopyToAsync(downloadFileStream);
            //    downloadFileStream.Close();
            //}

            return Ok(download.Content);
        }

        public class BlobInfoClass
        {
            public string fileName { get; set; }

        }


    }
}
