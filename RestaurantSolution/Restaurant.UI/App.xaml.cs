using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.DataAccess.Repos;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Restaurant.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Example usage of RestaurantContext
            using (var context = new RestaurantContext())
            {
                bool canConnect = new RestaurantContext().Database.CanConnect();
                MessageBox.Show($"Conexiune OK: {canConnect}");
            }


            var repo = new CategoryRepository(new RestaurantContext());
            repo.Insert("TestCat");
            var all = repo.GetAll();
            MessageBox.Show($"Am {all.Count()} categorii (incluzând «TestCat»).");

        }
    }

}
