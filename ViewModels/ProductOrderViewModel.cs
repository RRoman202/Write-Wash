using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;

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

        public Points SelectedPoint { get; set; }
        public List<Points> Points { get; set; }

        public string _pattern;

        public Visibility NullProduct { get; set; }

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        public Visibility BasketVisible { get; set; } = Global.CurrentUser.Name == string.Empty ? Visibility.Collapsed : Visibility.Visible;

        public string SumOrder { get; set; }
        public string DiscountOrder { get; set; }

        public List<Product> allp { get; set; }

        public float? FullPrice { get; set; }

        public ProductOrderViewModel(PageService pageService, ProductService productService, PointService pointService, OrderService orderService)
        {
            _pageService = pageService;
            _productService = productService;
            _pointService = pointService;
            _orderService = orderService;
            Task.Run(async () =>
            {
                Points = await _pointService.GetPoints();
                
                Products = await _productService.GetCart();
                SelectedPoint = Points[0];
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                SumOrder = CheckSumOrder().ToString();
                
                DiscountOrder = DiscountOrderCheck().ToString();
                CheckNullProduct();
                FullPrice= CheckFullPrice();

            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);
            
        }
        public void GetProducts()
        {
            
            List<Product> ppp = new List<Product>();
            foreach (OrderProduct o in Global.Cart.ToList())
            {
                for (int i = 0; i < allp.ToList().Count(); i++)
                {
                    
                    if (o.ProductArticleNumber == allp[i].ProductArticleNumber)
                    {
                        ppp.Add(allp[i]);
                        ppp[Global.Cart.Count() - 1].ProductCount = o.ProductCount;
                    }
                }

            }
            Products = ppp;
            
            
        }
        float? CheckSumOrder()
        {

            float? sumorder = 0;
            foreach (Product p in Products)
            {
                foreach(OrderProduct o in Global.Cart)
                {
                    if(o.ProductArticleNumber == p.ProductArticleNumber)
                    {
                        if (p.CurrentDiscount != 0)
                        {
                            sumorder += p.DisplayedPrice * o.ProductCount;
                        }
                        else
                        {
                            sumorder += p.ProductCost * o.ProductCount;
                        }
                    }
                }
                
            }
            return sumorder;
        }
        float? CheckFullPrice()
        {
            float? sumfullprice = 0;
            foreach (Product p in Products)
            {
                foreach (OrderProduct o in Global.Cart)
                {
                    if (o.ProductArticleNumber == p.ProductArticleNumber)
                    {
                        sumfullprice += p.FullPrice;
                    }
                }

            }
            return sumfullprice;
        }
        float? DiscountOrderCheck()
        {
            float? disc;
            float pricenondisc = 0;
            float? pricedisc = 0;
            foreach(Product p in Products)
            {
                if(p.CurrentDiscount > 0)
                {
                    pricedisc += p.DisplayedPrice;
                }
                else
                {
                    pricedisc += p.ProductCost;
                }
                pricenondisc += p.ProductCost;
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
            try
            {
                if (Products.Count() > 0)
                {

                    Global.Cart.RemoveAt(SelectedProduct);
                    Task.Run(async () =>
                    {
                        Products = await _productService.GetCart();
                        MaxProd = Products.Count();
                        CurrentProd = Products.Count();
                        SumOrder = CheckSumOrder().ToString();
                        DiscountOrder = DiscountOrderCheck().ToString();
                        CheckNullProduct();
                        FullPrice = CheckFullPrice();


                    }).WaitAsync(TimeSpan.FromMilliseconds(10))
                    .ConfigureAwait(false);
                    if (Global.Cart.Count > 0)
                        DiscountOrder = DiscountOrderCheck().ToString();
                    else
                        DiscountOrder = null;

                }
            }
            catch { }
            
        });
        public DelegateCommand RemoveProductInOrder => new DelegateCommand(() =>
        {
            try
            {
                if (Global.Cart[SelectedProduct].ProductCount > 1)
                {
                    Global.Cart[SelectedProduct].ProductCount--;
                    Task.Run(async () =>
                    {
                        Products = await _productService.GetCart();
                        MaxProd = Products.Count();
                        CurrentProd = Products.Count();
                        SumOrder = CheckSumOrder().ToString();
                        DiscountOrder = DiscountOrderCheck().ToString();
                        CheckNullProduct();
                        FullPrice = CheckFullPrice();


                    }).WaitAsync(TimeSpan.FromMilliseconds(10))
                .ConfigureAwait(false);
                }
                else if (Global.Cart[SelectedProduct].ProductCount == 1)
                {
                    Global.Cart.RemoveAt(SelectedProduct);
                    Task.Run(async () =>
                    {
                        Products = await _productService.GetCart();
                        MaxProd = Products.Count();
                        CurrentProd = Products.Count();
                        SumOrder = CheckSumOrder().ToString();
                        DiscountOrder = DiscountOrderCheck().ToString();
                        CheckNullProduct();
                        FullPrice = CheckFullPrice();


                    }).WaitAsync(TimeSpan.FromMilliseconds(10))
                .ConfigureAwait(false);
                }
            }
            catch { }

        });
        public DelegateCommand AddProductInOrder => new DelegateCommand(() =>
        {
            try
            {
                foreach(var p in Products)
                {
                    if(Global.Cart[SelectedProduct].ProductArticleNumber == p.ProductArticleNumber)
                    {
                        if((p.ProductQuantityInStock - Global.Cart[SelectedProduct].ProductCount) > 0)
                        {
                            Global.Cart[SelectedProduct].ProductCount++;
                            Task.Run(async () =>
                            {
                                Products = await _productService.GetCart();
                                MaxProd = Products.Count();
                                CurrentProd = Products.Count();
                                SumOrder = CheckSumOrder().ToString();
                                DiscountOrder = DiscountOrderCheck().ToString();
                                CheckNullProduct();
                                FullPrice = CheckFullPrice();


                            }).WaitAsync(TimeSpan.FromMilliseconds(10))
                            .ConfigureAwait(false);
                        }
                        else
                        {
                            MessageBox.Show("Товара на складе не осталось");
                        }
                    }
                }
                
            }
            catch { }


        });
        public int Code { get; set; }
        public DelegateCommand OrderBut => new DelegateCommand(async () =>
        {
            if(Code == 0)
            {
                string code = "";
                for (int i = 0; i < 3; i++)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(9);
                    code += r.ToString();
                }

                Code = int.Parse(code);
            }
            
            Global.point = SelectedPoint;
            _orderService.NewOrder(Code);
            await Task.Delay(100);
            Global.Cart.Clear();
            _pageService.ChangePage(new BrowseProduct());
        });
        public DelegateCommand Pdf => new DelegateCommand(() =>
        {
            CreatePDF(); 
        });
        private void CreatePDF()
        {
            Global.point = SelectedPoint;
            string code = "";
            for (int i = 0; i < 3; i++)
            {
                Random rnd = new Random();
                int r = rnd.Next(9);
                code += r.ToString();
            }

            Code = int.Parse(code);
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
            Paragraph punct = new Paragraph($"Пункт выдачи: {Global.point.PointIndex} {Global.point.PointCity} {Global.point.PointStreet} {Global.point.PointHome}", font);
            document.Add(punct);
            foreach (Product p in Products)
            {
                if (p.CurrentDiscount == 0)
                {
                    Paragraph pp = new Paragraph(p.ProductName + " " + p.Manufacturer + " " + p.ProductCount + "шт " + (p.ProductCost * p.ProductCount).ToString() + "₽", font);
                    document.Add(pp);
                }
                else
                {
                    Paragraph pp = new Paragraph(p.ProductName + " " + p.Manufacturer + " " + p.ProductCount + "шт " + (p.DisplayedPrice * p.ProductCount).ToString() + "₽", font);
                    document.Add(pp);
                }


            }
            Paragraph p1 = new Paragraph("Общая стоимость: " + SumOrder + "₽", font);
            document.Add(p1);
            Paragraph p2 = new Paragraph("Общая скидка: " + DiscountOrder + "%", font);
            document.Add(p2);
            
            Paragraph p3 = new Paragraph("Код получения: " + Code.ToString(), font);
            document.Add(p3);
            // Закрываем документ
            document.Close();
            string pdfViewerPath = @"C:\Program Files\Adobe\Acrobat DC\Acrobat\Acrobat.exe";

            // Открываем PDF файл
            System.Diagnostics.Process.Start(pdfViewerPath, "test.pdf");
        }

    }
}
