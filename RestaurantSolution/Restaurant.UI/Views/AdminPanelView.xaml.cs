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

            // Inițializare ViewModel și setare ca DataContext
            var viewModel = new AdminPanelViewModel(
                categoryService,
                preparatService,
                alergenService,
                menuService,
                orderService,
                userService);

            DataContext = viewModel;

            // Încărcare date la inițializare
            Loaded += async (s, e) => await viewModel.LoadInitialDataAsync();
        }

       
    }
}