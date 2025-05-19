using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class PreparatImageItem : ViewModelBase
    {
        public string ImagePath { get; set; } 
        public string FullPath { get; set; } 
    }
}
