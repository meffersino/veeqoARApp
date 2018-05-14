using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using System.Collections;

//Rename this?
namespace API
{
    public class Product
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Weight { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }
        public int TotalStockLevel { get; set; }
        public string ThumbnailURL { get; set; }

        public StockEntries StockEntries { get; set; }
    }

    public class StockEntries
    {
        public string Id { get; set; }
        public int AllocatedStockLevel { get; set; }
        public bool StockRunningLow { get; set; }
        public string UpdatedAt { get; set; }
        public Warehouse Warehouse { get; set; }
        public int PhysicalStockLevel { get; set; }
        public int AvailableStockLevel { get; set; }
    }

    public class Warehouse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


    class API_Calls
    {

        static public string BASE_URL = "https://api.veeqo.com/";
        static public string PRODUCTS_URL = "products/";

        private static string API_Key = "c3be72f013597ccd941c49d4f14246b1"; //System.IO.File.ReadAllText(@"C:\Users\Thomas Fisher\Documents\GitHub\API Keys\API-KEY.txt");
        /*
        public static void Main()
        {
            String plumbusId = "12583394";
            Product product = getProductById(plumbusId);
            Console.WriteLine(product.Title);

        }*/

        /// <summary>
        /// Makes a get request to the veeqo API for a product
        /// </summary>
        /// <returns>Project object provided by the API</returns>
        /// <param name="URL">URL of the product.</param>
        public static Product getProductByURL(string URL)
        {
            //Fetch API Key
            Debug.Log("API_KEY = " + API_Key);
            //Create get request and attatch API Key
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(URL);
            GETRequest.Method = "GET";
            GETRequest.Accept = "application/json";
            GETRequest.Headers.Add("x-api-key", API_Key);

            //Console.WriteLine("Sending GET Request");
            //Get response and convert into a string
            Debug.Log("Trying GET request...");

            String response = "";
            JObject json;

            //ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpWebResponse GETResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream GETResponseStream = GETResponse.GetResponseStream();
            StreamReader sr = new StreamReader(GETResponseStream);
            response = sr.ReadToEnd();

            //Console.WriteLine("Response from Server");
            //Console.WriteLine(response);

            //Parse string response into a JSON object
            json = JObject.Parse(response);
            
            Debug.Log("GET response received.");

            //Parse JSON object into Product object and return
            return parseToProduct(json);

        }

        /// <summary>
        /// Makes a get request to the veeqo API for a product
        /// </summary>
        /// <returns>Project object provided by the API</returns>
        /// <param name="productId">Product identifier.</param>
        public static Product getProductById(string productId)
        {
            //Create URL
            string URL = BASE_URL + PRODUCTS_URL + productId;
            //Fetch API Key **** EDIT THE FILE PATH ****

            //Create get request and attatch API Key
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(URL);
            GETRequest.Method = "GET";
            GETRequest.Accept = "application/json";
            GETRequest.Headers.Add("x-api-key", API_Key);

            //Console.WriteLine("Sending GET Request");
            //Get response and convert into a string
            HttpWebResponse GETResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream GETResponseStream = GETResponse.GetResponseStream();
            StreamReader sr = new StreamReader(GETResponseStream);
            String response = sr.ReadToEnd();

            //Console.WriteLine("Response from Server");
            //Console.WriteLine(response);

            //Parse string response into a JSON object
            JObject json = JObject.Parse(response);

            //Parse JSON object into Product object and return
            return parseToProduct(json);

        }

        //Parses returned JSON response into a Product Object
        public static Product parseToProduct(JObject json)
        {
            Product product = new Product();
            product.Id = (string)json["id"];
            product.Title = (string)json["title"];
            product.Notes = (string)json["notes"];
            product.Description = (string)json["description"];
            product.Price = (decimal)json["sellables"][0]["price"];
            product.Weight = (decimal)json["weight"];
            product.ThumbnailURL = (string)json["thumbnail_url"];
            product.TotalStockLevel = (int)json["total_stock_level"];

            product.StockEntries = parseToStockEntries(json);
            return product;
        }
        //Parses returned JSON response into a Warehouse object
        private static Warehouse parseToWarehouse(JObject json)
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Id = (string)json["sellables"][0]["stock_entries"][0]["warehouse"]["id"];
            warehouse.Name = (string)json["sellables"][0]["stock_entries"][0]["warehouse"]["name"];

            return warehouse;
        }

        //Parses returned JSON response into a StockEntries Object
        private static StockEntries parseToStockEntries(JObject json)
        {
            StockEntries stockEntries = new StockEntries();
            stockEntries.Id = (string)json["sellables"][0]["stock_entries"][0]["id"];
            stockEntries.UpdatedAt = (string)json["sellables"][0]["stock_entries"][0]["updated_at"];
            stockEntries.StockRunningLow = (bool)json["sellables"][0]["stock_entries"][0]["stock_running_low"];
            stockEntries.AllocatedStockLevel = (int)json["sellables"][0]["stock_entries"][0]["allocated_stock_level"];
            stockEntries.AvailableStockLevel = (int)json["sellables"][0]["stock_entries"][0]["available_stock_level"];
            stockEntries.PhysicalStockLevel = (int)json["sellables"][0]["stock_entries"][0]["physical_stock_level"];

            stockEntries.Warehouse = parseToWarehouse(json);
            return stockEntries;
        }

        public static bool MyRemoteCertificateValidationCallback(System.Object sender,
            X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }
    }


}