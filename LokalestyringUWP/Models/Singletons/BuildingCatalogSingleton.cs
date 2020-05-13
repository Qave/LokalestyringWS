using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class BuildingSingleton
    {
        private static BuildingSingleton _instance = null;
        public ObservableCollection<Building> Buildings { get; }
        public static BuildingSingleton Instance { get { return _instance ?? (_instance = new BuildingSingleton()); } }
        public BuildingSingleton()
        {
            Buildings = new ObservableCollection<Building>();
            LoadbuildingsAsync();
        }

        public async void LoadbuildingsAsync()
        {
            ObservableCollection<Building> buildings = await PersistancyService.LoadTableFromJsonAsync<Building>("Buildings");

            foreach (var item in buildings)
            {
                this.Buildings.Add(item);
            }
        }
    }
}
