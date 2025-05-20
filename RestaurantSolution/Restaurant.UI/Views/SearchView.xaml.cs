using System.Windows.Controls;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Search;

namespace Restaurant.UI.Views.Search
{
    public partial class SearchView : UserControl
    {
        public SearchView(ICategoryService categoryService,
                  IPreparatService preparatService,
                  IMenuService menuService,
                  IAlergenService alergenService)
        {
            InitializeComponent();

            var viewModel = new SearchViewModel(
                categoryService,
                preparatService,
                menuService,
                alergenService);

            DataContext = viewModel;
        }
    }
}