using System.Windows.Controls;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Admin;

namespace Restaurant.UI.Views.Admin
{
    public partial class AdminPanelView : UserControl
    {
        public AdminPanelView(ICategoryService categoryService,
                             IPreparatService preparatService,
                             IAlergenService alergenService,
                             IMenuService menuService,
                             IOrderService orderService,
                             IUserService userService)
        {
            InitializeComponent();

     
            var viewModel = new AdminPanelViewModel(
                categoryService,
                preparatService,
                alergenService,
                menuService,
                orderService,
                userService);

            DataContext = viewModel;


            Loaded += async (s, e) => await viewModel.LoadInitialDataAsync();
        }

       
    }
}