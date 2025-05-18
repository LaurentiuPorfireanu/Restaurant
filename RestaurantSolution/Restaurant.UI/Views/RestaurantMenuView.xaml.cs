using System.Windows.Controls;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.RestaurantMenu;

namespace Restaurant.UI.Views.Menu
{
    public partial class RestaurantMenuView : UserControl
    {
        public RestaurantMenuView(ICategoryService categoryService,
                                  IPreparatService preparatService,
                                  IMenuService menuService)
        {
            InitializeComponent();

            // Inițializare ViewModel și setare ca DataContext
            var viewModel = new RestaurantMenuViewModel(
                categoryService,
                preparatService,
                menuService);

            DataContext = viewModel;

            // Încărcare date la inițializare
            Loaded += async (s, e) => await viewModel.LoadDataAsync();
        }
    }
}