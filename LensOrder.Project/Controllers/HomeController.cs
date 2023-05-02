using AutoMapper;
using ClosedXML.Excel;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using LensOrder.Project.Entities;
using LensOrder.Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace LensOrder.Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        public HomeController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Product(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _mapper.Map<Order>(model);

                string productName = order.ProductName;

                if(productName == "Acuvue")
                {
                    order.ProductPrice = 150;
                }
                else if (productName == "Pure Vision")
                {
                    order.ProductPrice = 250;
                }
                else if (productName == "Air Optix")
                {
                    order.ProductPrice = 120;
                }
                else if (productName == "BioTrue")
                {
                    order.ProductPrice = 350;
                }
                else
                {
                    order.ProductPrice = 190;
                }
                order.ProductTotalPrice = order.ProductPrice* order.ProductCount;

                _databaseContext.Orders.Add(order);
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(OrderHistory));
            }
            else
            {
                return View(model);
            }
            
        }

        public IActionResult OrderHistory()
        {
            List<OrderViewModel> orders = _databaseContext.Orders.ToList().Select(x => _mapper.Map<OrderViewModel>(x)).ToList();
            //List<OrderViewModel> orders =
            //  _databaseContext.Orders.ToList()
            //      .Select(x => _mapper.Map<OrderViewModel>(x)).ToList();

            return View(orders);


        }

        public IActionResult Order(int id) 
        {
            Order order = _databaseContext.Orders.Where(x => x.OrderId == id).First();
            OrderViewModel model = _mapper.Map<OrderViewModel>(order);
         

            return View(model);
        }
        public IActionResult GeneratePdf(int id)
        {
            Order order = _databaseContext.Orders.Where(x => x.OrderId == id).First();
            OrderViewModel model = _mapper.Map<OrderViewModel>(order);

            float col = 300f;
            float[] colWidth = { col, col };
            Table table = new Table(colWidth);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            Cell cell11 = new Cell(1, 2)
               .SetFont(font)
                .SetFontSize(10)
               .SetBold()
               .SetPaddingBottom(20f)
               .SetBorder(Border.NO_BORDER)
               .Add(new Paragraph($"Order# {model.OrderId}\nhttps://www.lensci.com\nTarih: {model.OrderDate}"));
            Cell cell21 = new Cell(1, 1)
                .SetFont(font)
                 .SetFontSize(10)
                .SetBold()
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Fatura bilgileri:"));
            Cell cell22 = new Cell(1, 1)
               .SetFont(font)
              .SetFontSize(10)
               .SetBold()
               .SetBorder(Border.NO_BORDER)
               .Add(new Paragraph("Teslimat Bilgisi:"));
            Cell cell31 = new Cell(1, 1)
               .SetFont(font)
               .SetFontSize(10)
               .SetPaddingLeft(10f)
               .SetBorder(Border.NO_BORDER)
               .Add(new Paragraph($"Ad: {model.FullName}\nTel: {model.PhoneNumber}\nAdres: {model.Address}\n\nÖdeme şekli: {model.PaymentMethod}"));
            Cell cell32 = new Cell(1, 1)
               .SetFont(font)
                .SetFontSize(10)
               .SetPaddingLeft(10f)
               .SetBorder(Border.NO_BORDER)
               .Add(new Paragraph($"Ad: {model.FullName}\nTel: {model.PhoneNumber}\nAdres: {model.Address}\n\nTeslimat yöntemi: {model.DeliveryMethod}"));
            Cell cell41 = new Cell(1, 2)
                .SetFont(font)
                .SetBold()
                 .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetPaddingTop(20f)
                .Add(new Paragraph("Ürün(ler)"));

            float col2 = 300f;
            float col3 = 120f;
            float col4 = 60f;
            float[] colWidths = { col2, col3, col4, col3 };

            Table productTable = new Table(colWidths);
       

            Cell cell51 = new Cell(1, 1)
                .SetFontSize(10)
                 .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(font)
                .Add(new Paragraph("Ürün adi"));
            Cell cell52 = new Cell(1, 1)
                .SetFontSize(10)
                 .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(font)
               .Add(new Paragraph("Fiyat"));
            Cell cell53 = new Cell(1, 1)
                .SetFontSize(10)
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(font)
               .Add(new Paragraph("Adet"));
            Cell cell54 = new Cell(1, 1)
               .SetFontSize(10)
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(font)
               .Add(new Paragraph("Toplam"));

            Cell cell61 = new Cell(1, 1)
              .SetFontSize(10)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(font)
              .Add(new Paragraph($"{model.ProductName}"));
            Cell cell62 = new Cell(1, 1)
                .SetFontSize(10)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(font)
               .Add(new Paragraph($"{model.ProductPrice}"));
            Cell cell63 = new Cell(1, 1)
                .SetFontSize(10)               
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(font)
               .Add(new Paragraph($"{model.ProductCount}"));
            Cell cell64 = new Cell(1, 1)
               .SetFontSize(10)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(font)
               .Add(new Paragraph($"{model.ProductTotalPrice}"));




            table.AddCell(cell11);
            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell31);
            table.AddCell(cell32);
            table.AddCell(cell41);
            productTable.AddCell(cell51);
            productTable.AddCell(cell52);
            productTable.AddCell(cell53);
            productTable.AddCell(cell54);
            productTable.AddCell(cell61);
            productTable.AddCell(cell62);
            productTable.AddCell(cell63);
            productTable.AddCell(cell64);


            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            using (var doc = new Document(pdf))
            {
                doc.Add(table);
                doc.Add(productTable);
                doc.Close();
                doc.Flush();
                pdfBytes = stream.ToArray();
            }
            return new FileContentResult(pdfBytes, "application/pdf");   

        }
        public IActionResult GenerateExcel(int id)
        {
            Order order = _databaseContext.Orders.Where(x => x.OrderId == id).First();
            OrderViewModel model = _mapper.Map<OrderViewModel>(order);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Order");
                var currentRow = 1;

                #region Header
                worksheet.Cell(1,currentRow).Value = "Sipariş Kodu: ";
                worksheet.Cell(2,currentRow).Value = "Sipariş Tarihi: ";
                worksheet.Cell(3,currentRow).Value = "Ad-Soyad: ";
                worksheet.Cell(4,currentRow).Value = "Telefon: ";
                worksheet.Cell(5,currentRow).Value = "Adres: ";
                worksheet.Cell(6,currentRow).Value = "Ödeme Yöntemi: ";
                worksheet.Cell(7,currentRow).Value = "Teslimat Yöntemi: ";
                worksheet.Cell(8,currentRow).Value = "Ürün Adı: ";
                worksheet.Cell(9,currentRow).Value = "Ürün Fiyatı: ";
                worksheet.Cell(10,currentRow).Value = "Ürün Sayısı: ";
                worksheet.Cell(11,currentRow).Value = "Toplam Fiyat: ";
                worksheet.Cell(1,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(2,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(3,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(4,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(5,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(7,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(8,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(9,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(10,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(11,currentRow).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                #endregion
                currentRow++;
                #region Header
                worksheet.Cell(1,currentRow).Value = model.OrderId;
                worksheet.Cell(2,currentRow).Value = model.OrderDate;
                worksheet.Cell(3,currentRow).Value = model.FullName;
                worksheet.Cell(4,currentRow).Value = model.PhoneNumber;
                worksheet.Cell(5,currentRow).Value = model.Address;
                worksheet.Cell(6,currentRow).Value = model.PaymentMethod;
                worksheet.Cell(7,currentRow).Value = model.DeliveryMethod;
                worksheet.Cell(8,currentRow).Value = model.ProductName;
                worksheet.Cell(9,currentRow).Value = model.ProductPrice;
                worksheet.Cell(10,currentRow).Value = model.ProductCount;
                worksheet.Cell(11,currentRow).Value = model.ProductTotalPrice;
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Siparis.xlsx"
                        );
                }

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}