using LokalestyringUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.ViewModel
{
    public class UserBookingsVM
    {
        
        public ObservableCollection<Location> Mockdata { get; set; }
        public UserBookingsVM()
        {
            Mockdata = new ObservableCollection<Location>();
            Mockdata.Add(new Location() { Loc_Id = 1, City = "RO-D1.05", Name = "Virk"});
            Mockdata.Add(new Location() { Loc_Id = 1, City = "RO-D1.06", Name = "Virk"});
            Mockdata.Add(new Location() { Loc_Id = 1, City = "RO-D1.07", Name = "Virk"});
            Mockdata.Add(new Location() { Loc_Id = 1, City = "RO-D1.08", Name = "Virk"});
            Mockdata.Add(new Location() { Loc_Id = 1, City = "RO-D1.09", Name = "Virk"});
            Mockdata.Add(new Location() { Loc_Id = 1, City = "RO-D1.10", Name = "Virk"});
        }
    }
}
