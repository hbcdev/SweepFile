using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SweepFile.Models;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace SweepFile.Controllers
{
    public class HomeController : Controller
    {
        string file_name = null;
        List<FileDirModel> ListFile = new List<FileDirModel>();
        List<fundcode> fund = new List<fundcode>();
        public IActionResult Index(string name)
        {
            ViewBag.title = "Datawarehouse";
           
            string filePath = @"//192.168.96.7/Datawarehouse/";
            Debug.WriteLine("Path : "+filePath);
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            DirectoryInfo rootdir = dirInfo.Root;

            DirectoryInfo[] dirInfos = rootdir.GetDirectories("*.*");
            Debug.WriteLine("***************** Directory **************");
            foreach (DirectoryInfo d in dirInfos)
            {

                if (d.Name.Length < 6)
                {
                    //Debug.WriteLine("Direc Name : " + d.Name);

                    DirectoryInfo[] subDir = d.GetDirectories("*.*");
                    foreach (DirectoryInfo sb in subDir)
                    {
                        
                        if (sb.Name == "Wait" || sb.Name == "wait") {
                            //  Debug.WriteLine("---------------- " + sb.Name);
                            fundcode fc = new fundcode();
                            fc.fundecode = d.Name;

                            fund.Add(fc);
                            DirectoryInfo[] sub2 = sb.GetDirectories("*.*");
                            if (sub2.Length > 0)
                            {
                                GetDirInWait(sb,d.Name);
                            }
                            else {                             

                                FileInfo[] getFileW = sb.GetFiles("*.*");

                                int i = 0; 
                                foreach (FileInfo files2 in getFileW)
                                {
                                    FileDirModel map = new FileDirModel();
                                    if (i == 0)
                                    {
                                        map.path = sb.FullName;
                                        map.fileName = files2.Name;
                                        map.fundcode = d.Name;
                                    }
                                    else {
                                        map.path = "";
                                        map.fileName = files2.Name;
                                        map.fundcode = d.Name;
                                    }
                                    i++;
                                    ListFile.Add(map);
                                    //Debug.WriteLine("************************* " + files2.Name);
                                }                               
                            }
                        }                        
                    }
                }
            }
            file_name = name;
            ViewBag.file_name = file_name;
            ViewBag.fund = fund;
            ViewBag.listfile= ListFile;

            
            return View();
        }


        public List<FileDirModel> GetDirInWait( DirectoryInfo rootDir,string fundcode) {

           
            DirectoryInfo[] sub2 = rootDir.GetDirectories("*.*");
            if (sub2.Length > 0)
            {
               
                foreach (DirectoryInfo sb2 in sub2)
                {
                   // Debug.WriteLine("==================" + sb2.FullName+"===============" );

                    DirectoryInfo[] sub3 =  sb2.GetDirectories("*.*");
                    if (sub3.Length > 0) {
                       
                        FileInfo[] getFileW = sb2.GetFiles("*.*");
                       

                        int i = 0;
                        foreach (FileInfo files2 in getFileW)
                        {
                            FileDirModel mapping = new FileDirModel();
                            if (i == 0)
                            {
                                mapping.path = sb2.FullName;
                                mapping.fileName = files2.Name;
                                mapping.fundcode = fundcode;
                            }
                            else
                            {
                                mapping.path = "";
                                mapping.fileName = files2.Name;
                                mapping.fundcode = fundcode;
                            }
                            i++;
                            // Debug.WriteLine("************************* " + files2.Name);
                            ListFile.Add(mapping);
                        }

                        
                        GetDirInWait(sb2, fundcode);
                    }
                    else {
                        

                        FileInfo[] getFileW = sb2.GetFiles("*.*");
                        int i = 0;
                        foreach (FileInfo files2 in getFileW)
                        {
                            FileDirModel mapping = new FileDirModel();
                            //Debug.WriteLine("************************* " + files2.Name);

                            if (i == 0)
                            {
                                mapping.path = sb2.FullName;
                                mapping.fileName = files2.Name;
                                mapping.fundcode = fundcode;
                            }
                            else {
                                mapping.path = "";
                                mapping.fileName = files2.Name;
                                mapping.fundcode = fundcode;
                            }
                            i++;
                            ListFile.Add(mapping);
                        }
                        
                    }
                }
                
            }
            else
            {
                FileDirModel mapping = new FileDirModel();
              
                FileInfo[] getFileW = rootDir.GetFiles("*.*");
                int i = 0;
                foreach (FileInfo files2 in getFileW)
                {
                    if (i == 0)
                    {
                        mapping.path = rootDir.FullName;
                        mapping.fileName = files2.Name;
                        mapping.fundcode = fundcode;
                    }
                    else {
                        mapping.path = " ";
                        mapping.fileName = files2.Name;
                        mapping.fundcode = fundcode;
                    }
                    
                  //  Debug.WriteLine("************************* " + files2.Name);
                    i++;
                }

                ListFile.Add(mapping);
            }

            return ListFile;
        }
        public ExcelWorksheet createExcel(ExcelPackage package, List<FileDirModel> data,string fundecode) {
            Debug.WriteLine("Fund : " + fundecode);
         
                var sheet = package.Workbook.Worksheets.Add(fundecode);

                sheet.Cells["A1"].Value = fundecode;
                sheet.Cells["A1"].Style.Font.Size = 18;

                sheet.Cells["A4"].Value = "Path";
                sheet.Cells["B4"].Value = "File Name";
                sheet.Cells["C4"].Value = "Note";

                int i = 5;
                foreach (FileDirModel item in data) {
                    if (item.fundcode == fundecode) {

                    sheet.Column(1).Width = 79;
                    sheet.Column(2).Width = 108;
                    sheet.Column(3).Width = 55;


                    sheet.Cells[i, 1, i, 1].Value = item.path;
                    sheet.Cells[i, 2, i, 2].Value = item.fileName;
                    sheet.Cells[i, 3, i, 3].Value = item.note;
                    i++;
                }
                   
                }
            return sheet;
        }
       /* [HttpPost("/")]
        public IActionResult test() {
            file_name = "test.xlsx";
            return RedirectToAction("Index", "Home", new { name = file_name });
        }*/

        [HttpPost ("/")]
        public IActionResult submitDataNote(string[] note,string[] path,string[] filename,string[] fundcode,string usersub) {
          Console.WriteLine("************* POST DATA **************");
            List<FileDirModel> dataFile = new List<FileDirModel>();
              for (int i = 0; i < path.Length; i++) {
                FileDirModel map = new FileDirModel();
                map.path = path[i];
                map.fileName = filename[i];
                map.fundcode = fundcode[i];
                map.note = note[i];

                dataFile.Add(map);
            }

            try {
                using (var package = new ExcelPackage())
                {
                    Debug.WriteLine(" dataFile :" + dataFile.Count);
                    Debug.WriteLine(" fund :" + fund.Count);
                    var workbook = package.Workbook;
                    foreach (fundcode fc in getFund())
                    {
                        createExcel(package, dataFile, fc.fundecode);
                    }
                    DateTime currDate = DateTime.Now;
                    string dateNow = currDate.Year + "-" + currDate.Month + "-" + currDate.Day;
                    FileInfo newFile = new FileInfo(@"\\192.168.96.7\Datawarehouse\Report\Upload_Wait_Report_" + dateNow + "___"+usersub+".xlsx");

                    file_name = "Upload_Wait_Report_" + dateNow + "___" + usersub + ".xlsx";
                    ViewBag.file_name = file_name;
                    package.SaveAs(newFile);
                }
                return RedirectToAction("Index", "Home", new { name = file_name });
            } catch (Exception ex) {
                return RedirectToAction("Index", "Home", new { name = ex });
            }
            
            //file_name = "Upload Wait Report.xlsx";
            
        }




        public List<fundcode> getFund() {
            string filePath = @"//192.168.96.7/Datawarehouse/";
          
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            DirectoryInfo rootdir = dirInfo.Root;


            List<fundcode> fund = new List<fundcode>();
            DirectoryInfo[] dirInfos = rootdir.GetDirectories("*.*");
            foreach (DirectoryInfo d in dirInfos)
            {
                if (d.Name.Length < 6)
                {
                    DirectoryInfo[] subDir = d.GetDirectories("*.*");
                    foreach (DirectoryInfo sb in subDir)
                    {

                        if (sb.Name == "Wait" || sb.Name == "wait")
                        {
                            //  Debug.WriteLine("---------------- " + sb.Name);
                            fundcode fc = new fundcode();
                            fc.fundecode = d.Name;

                            fund.Add(fc);
                            
                        }
                    }
                }
            }

            return fund;
        }
        static async Task RunAsync()
        {

            HttpClient client = new HttpClient();
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://api.hbc.in.th/api/fund/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                CallFundcode product = new CallFundcode
                {
                    fundcode = "Gizmo",
                    date_off = "Widgets"
                };
                // Get the product
                HttpResponseMessage response = await client.GetAsync("http://api.hbc.in.th/api/fund/");
                Debug.WriteLine("===========out if ====="+ response.IsSuccessStatusCode);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("=========in if 1 =======");
                    Debug.WriteLine("****************" + product.fundcode + "*****************");
                    Debug.WriteLine("****************" + product.date_off + "*****************");
                    string gg = await response.Content.ReadAsStringAsync();

                    
                  string[] splitStr =   gg.Split('[',']');
                    // foreach (var word in splitStr)
                    // {

                    //  }
                  //  Debug.WriteLine("========= JSON DATA  =======" + splitStr[1].ToString());
                    JObject json = JObject.Parse(splitStr[1].ToString());
                    Debug.WriteLine("========= JSON DATA  =======" + json.Count); 

                }
                else {
                    Debug.WriteLine("=========else=======");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }


        private IActionResult ResponseMessage(object response)
        {
            throw new NotImplementedException();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
