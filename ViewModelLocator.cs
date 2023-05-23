

namespace Write_Wash
{
    internal class ViewModelLocator
    {
        private static ServiceProvider _provider;
        public static IConfiguration Configuration { get; private set; }
        public static void Init()
        {
           

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<SignViewModel>();
            services.AddTransient<BrowseProductViewModel>();
            services.AddTransient<ProductOrderViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<AdminBrowseProductViewModel>();
            services.AddTransient<AdminBrowseOrderViewModel>();
            services.AddTransient<ChangeOrderViewModel>();
            services.AddTransient<AddProductViewModel>();
            services.AddTransient<ChangeProductViewModel>();
            services.AddTransient<AddDeliveryViewModel>();
            services.AddTransient<AddManufactureViewModel>();
            services.AddTransient<DeliveryViewModel>();

            #endregion

            #region Connection

            services.AddDbContext<DataContext>(options =>
            {
                try
                {
                    options.UseMySql(
                    "server=localhost;user=root;password=2703123rrr;database=trade;",
                     new MySqlServerVersion(new Version(8, 0, 30)));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.EnableSensitiveDataLogging();

                }
                catch (MySqlConnector.MySqlException)
                {

                    options.UseMySql(
                    "server=localhost;user=root;password=2703123rrr;database=trade;",
                     new MySqlServerVersion(new Version(8, 0, 30)));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.EnableSensitiveDataLogging();
                }
            }, ServiceLifetime.Transient);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<PointService>();
            services.AddTransient<OrderService>();
            services.AddSingleton<AdminService>();
            


            #endregion

            _provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                _provider.GetRequiredService(service.ServiceType);
                
            }
        }
        public MainWindowViewModel MainWindowViewModel => _provider.GetRequiredService<MainWindowViewModel>();
        public SignViewModel SignViewModel => _provider.GetRequiredService<SignViewModel>();
        public BrowseProductViewModel BrowseProductViewModel => _provider.GetRequiredService<BrowseProductViewModel>();
        public ProductOrderViewModel ProductOrderViewModel => _provider.GetRequiredService<ProductOrderViewModel>();
        public RegistrationViewModel RegistrationViewModel => _provider.GetRequiredService<RegistrationViewModel>();
        public AdminBrowseProductViewModel AdminBrowseProductViewModel => _provider.GetRequiredService<AdminBrowseProductViewModel>();
        public AdminBrowseOrderViewModel AdminBrowseOrderViewModel => _provider.GetRequiredService<AdminBrowseOrderViewModel>();
        public ChangeOrderViewModel ChangeOrderViewModel => _provider.GetRequiredService<ChangeOrderViewModel>();
        public AddProductViewModel AddProductViewModel => _provider.GetRequiredService<AddProductViewModel>();
        public ChangeProductViewModel ChangeProductViewModel => _provider.GetRequiredService<ChangeProductViewModel>();
        public AddManufactureViewModel AddManufactureViewModel => _provider.GetRequiredService<AddManufactureViewModel>();
        public AddDeliveryViewModel AddDeliveryViewModel => _provider.GetRequiredService<AddDeliveryViewModel>();
        public DeliveryViewModel DeliveryViewModel => _provider.GetRequiredService<DeliveryViewModel>();
    }
}
