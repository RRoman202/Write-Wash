using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Write_Wash.ViewModels
{
    internal class ProductOrderViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly PointService _pointService;
        private readonly OrderService _orderService;
        public List<Product> Products { get; set; }

        public int MaxProd { get; set; }
        public int CurrentProd { get; set; }

        
        public List<Points> Points { get; set; }

        public string _pattern;

        public Visibility NullProduct { get; set; }

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        public Visibility BasketVisible { get; set; } = Global.CurrentUser.Name == string.Empty ? Visibility.Collapsed : Visibility.Visible;

        public string SumOrder { get; set; }
        public string DiscountOrder { get; set; }

        public ProductOrderViewModel(PageService pageService, ProductService productService, PointService pointService, OrderService orderService)
        {
            _pageService = pageService;
            _productService = productService;
            _pointService = pointService;
            _orderService = orderService;
            Task.Run(async () =>
            {
                Points = await _pointService.GetPoints();
                Products = Global.OrderProductList.ToList();
                
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                SumOrder = CheckSumOrder().ToString();
                DiscountOrder = DiscountOrderCheck().ToString();
                CheckNullProduct();
                
            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);
            
        }
        float? CheckSumOrder()
        {
            float? sumorder = 0;
            foreach(Product p in Products)
            {
                if(p.Discount != 0)
                {
                    sumorder += p.DisplayedPrice * p.ProductCount;
                }
                else
                {
                    sumorder += p.Price * p.ProductCount;
                }
            }
            return sumorder;
        }
        float? DiscountOrderCheck()
        {
            float? disc;
            float pricenondisc = 0;
            float? pricedisc = 0;
            foreach(Product p in Products)
            {
                if(p.Discount > 0)
                {
                    pricedisc += p.DisplayedPrice;
                }
                else
                {
                    pricedisc += p.Price;
                }
                pricenondisc += p.Price;
            }
            disc = (float?)Math.Truncate((decimal)(100 - (pricedisc * 100 / pricenondisc)));
            return disc;
        }
        void CheckNullProduct()
        {
            if (CurrentProd > 0) { NullProduct = Visibility.Hidden; } else { NullProduct = Visibility.Visible; }
        }

        public int SelectedProduct { get; set; }
        
        public DelegateCommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new BrowseProduct());
        });
        public DelegateCommand DeleteInOrder => new DelegateCommand(() =>
        {
            if(Products.Count() > 0)
            {
                Global.OrderProductList.RemoveAt(SelectedProduct);
                Products = Global.OrderProductList.ToList();
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                CheckNullProduct();
                SumOrder = CheckSumOrder().ToString();
                if (Global.OrderProductList.Count > 0)
                    DiscountOrder = DiscountOrderCheck().ToString();
                else
                    DiscountOrder = null;
            }
            
        });
        public DelegateCommand RemoveProductInOrder => new DelegateCommand(() =>
        {
            try
            {
                if (Products[SelectedProduct].ProductCount > 1)
                {
                    Global.OrderProductList[SelectedProduct].ProductCount--;
                    Products = Global.OrderProductList.ToList();
                    MaxProd = Products.Count();
                    CurrentProd = Products.Count();
                    CheckNullProduct();
                    SumOrder = CheckSumOrder().ToString();
                    DiscountOrder = DiscountOrderCheck().ToString();
                }
                else if (Products[SelectedProduct].ProductCount == 1)
                {
                    Global.OrderProductList.RemoveAt(SelectedProduct);
                    Products = Global.OrderProductList.ToList();
                    MaxProd = Products.Count();
                    CurrentProd = Products.Count();
                    CheckNullProduct();
                    SumOrder = CheckSumOrder().ToString();
                    DiscountOrder = DiscountOrderCheck().ToString();
                }
            }
            catch { }

        });
        public DelegateCommand AddProductInOrder => new DelegateCommand(() =>
        {
            try
            {
                Global.OrderProductList[SelectedProduct].ProductCount++;
                Products = Global.OrderProductList.ToList();
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                CheckNullProduct();
                SumOrder = CheckSumOrder().ToString();
                DiscountOrder = DiscountOrderCheck().ToString();
            }
            catch { }
            

        });
        public DelegateCommand OrderBut => new DelegateCommand(() =>
        {
            _orderService.NewOrder();
        });
        public DelegateCommand Pdf => new DelegateCommand(() =>
        {

            CreatePDF();
            

            
            
        });
        private void CreatePDF()
        {
            // Создаем документ и задаем размер страницы
            Document document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Создаем файл для записи
            FileStream fs = new FileStream("test.pdf", FileMode.Create);

            // Создаем объект для записи PDF
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            // Открываем документ для записи
            document.Open();
            

            // Добавляем текст в документ
            Paragraph paragraph = new Paragraph("Талон", font);
            
            document.Add(paragraph);
            Paragraph paragraph2 = new Paragraph(DateTime.Now.ToString());
            document.Add(paragraph2);
            foreach(Product p in Global.OrderProductList)
            {
                if(p.Discount == 0)
                {
                    Paragraph pp = new Paragraph(p.Title + " " + p.Manufacturer + " " + p.ProductCount + "шт " + (p.Price * p.ProductCount).ToString() + "₽", font);
                    document.Add(pp);
                }
                else
                {
                    Paragraph pp = new Paragraph(p.Title + " " + p.Manufacturer + " " + p.ProductCount + "шт " + (p.DisplayedPrice * p.ProductCount).ToString() + "₽", font);
                    document.Add(pp);
                }
                

            }
            Paragraph p1 = new Paragraph("Общая стоимость: " + SumOrder + "₽", font);
            document.Add(p1);
            Paragraph p2 = new Paragraph("Общая скидка: " + DiscountOrder + "%", font);
            document.Add(p2);
            string code = "";
            for(int i = 0; i < 3; i++)
            {
                Random rnd = new Random();
                int r = rnd.Next(9);
                code += r.ToString();
            }
            Paragraph p3 = new Paragraph("Код получения: " + code, font);
            document.Add(p3);
            // Закрываем документ
            document.Close();
            string pdfViewerPath = @"C:\Program Files\Adobe\Acrobat DC\Acrobat\Acrobat.exe";

            // Открываем PDF файл
            System.Diagnostics.Process.Start(pdfViewerPath, "test.pdf");
        }

    }
}
