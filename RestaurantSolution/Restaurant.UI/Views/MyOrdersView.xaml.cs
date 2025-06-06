﻿using System.Windows.Controls;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Order;
using Restaurant.ViewModels.Order.Extras;
using Restaurant.ViewModels.State;

namespace Restaurant.UI.Views.Order
{
    public partial class MyOrdersView : UserControl
    {
        public MyOrdersView(IOrderService orderService,
                           ICategoryService categoryService,
                           IPreparatService preparatService,
                           IMenuService menuService)
        {
            InitializeComponent();

         
            var currentUser = CurrentUserState.Instance.CurrentUser;
            var viewModel = new MyOrdersViewModel(
                orderService,
                categoryService,
                preparatService,
                menuService,
                currentUser);

            DataContext = viewModel;



           
            Loaded += async (s, e) => await viewModel.LoadOrdersAsync();
        }
    }
}