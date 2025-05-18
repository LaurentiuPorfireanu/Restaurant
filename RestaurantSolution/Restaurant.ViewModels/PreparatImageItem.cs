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
        public string ImagePath { get; set; } // Calea relativă care va fi stocată în baza de date
        public string FullPath { get; set; } // Calea completă pentru afișare în aplicație
    }
}
