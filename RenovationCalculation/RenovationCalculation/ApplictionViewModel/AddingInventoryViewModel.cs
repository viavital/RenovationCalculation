using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Service;
using RenovationCalculation.Model;

namespace RenovationCalculation.ApplictionViewModel
{
    class AddingInventoryViewModel : INotifyPropertyChanged, IDisposable    
    {
        private readonly InventoryService _inventoryService;
        public ObservableCollection<InventoryModel> InventoryList { get; }

        public AddingInventoryViewModel()
        {
             _inventoryService = InventoryService.GetInstance();
            InventoryList = new ObservableCollection<InventoryModel>(_inventoryService.GetAllInventory());
            _inventoryService.InventoryAddedEvent += OnInventoryAdded;
            _inventoryService.InventoryDeletedEvent += OnInventoryDeleted;
        }

        private string enteredNameOfInventory;
        public string EnteredNameOfInventory
        {
            get { return enteredNameOfInventory; }
            set
            {
                enteredNameOfInventory = value;
                OnPropertyChanged();
            }
        }

        private string enteredpriceOfInventory;
        public string EnteredPriceOfInventory
        {
            get { return enteredpriceOfInventory; }
            set
            {
                enteredpriceOfInventory = value;
                OnPropertyChanged();
                OnEnteredPriceOfInventory();
            }
        }

        private int priceOfInventory;
        public int PriceOfInventory
        {
            get { return priceOfInventory; }
            set
            {
                priceOfInventory = value;               
            }
        }

        private InventoryModel selectedInventory;
        public InventoryModel SelectedInventory
        {
            get { return selectedInventory; }
            set
            {
                selectedInventory = value;                
                OnPropertyChanged();
            }
        }
        private void OnEnteredPriceOfInventory()
        {
            if (EnteredPriceOfInventory != null && EnteredPriceOfInventory.Length > 0)
            {
                int ParsedPrice;
                if (int.TryParse(EnteredPriceOfInventory, out ParsedPrice))
                {
                    PriceOfInventory = ParsedPrice;
                }
                else
                {
                    EnteredPriceOfInventory = EnteredPriceOfInventory.Remove(EnteredPriceOfInventory.Length - 1);
                }
            }
        }
        private void OnInventoryAdded(InventoryModel inventory)
        {
            InventoryList.Add(inventory);
        }

        private void OnInventoryDeleted(InventoryModel inventory)
        {
            if (InventoryList.Contains(inventory))
            {
                InventoryList.Remove(inventory);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private RelayCommand addInventoryCommand;
        public RelayCommand AddInventoryCommand
        {
            get
            {
                return addInventoryCommand ??
                    (addInventoryCommand = new RelayCommand(_ =>
                    {
                        InventoryModel NewInventory = new();

                        NewInventory.Name = EnteredNameOfInventory;
                        NewInventory.PriceOfInventory = PriceOfInventory;
                        _inventoryService.AddInventory(NewInventory);

                        EnteredNameOfInventory = null;
                        EnteredPriceOfInventory = null;
                    }));
            }
        }
        
        private RelayCommand removeInventoryCommand;
        public RelayCommand RemoveInventoryCommand
        {
            get
            {
                return removeInventoryCommand ??
                    (removeInventoryCommand = new RelayCommand(obj =>
                    {
                        var inventoryToRemove = obj as InventoryModel;
                        if (inventoryToRemove != null)
                        {
                            _inventoryService.DeleteInventory(inventoryToRemove);
                        }
                    }
                    ));
            }
        }
        public event Action CloseAddInventoryWindowEvent;
        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ?? (closeWindowCommand = new RelayCommand(_ => CloseAddInventoryWindowEvent()));
            }
        }
        public void Dispose()
        {
            _inventoryService.InventoryAddedEvent -= OnInventoryAdded;
            _inventoryService.InventoryDeletedEvent -= OnInventoryDeleted;
        }
    }
}
