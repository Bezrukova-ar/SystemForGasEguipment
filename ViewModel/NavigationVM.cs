using SystemForGasEguipment.Utilities;

namespace SystemForGasEguipment.ViewModel
{
    class NavigationVM : BaseVM
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand InventCommand { get; set; }

        private void Add(object obj) => CurrentView = new AddingAGasEquipmentElementVM();
        private void Edit(object obj) => CurrentView = new EditingAGasEquipmentItemVM();
        private void Remove(object obj) => CurrentView = new RemovingAnElementOfGasEquipmentVM();
        private void Invent(object obj) => CurrentView = new InventoryVM();


        public NavigationVM()
        {
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            RemoveCommand = new RelayCommand(Remove);
            InventCommand = new RelayCommand(Invent);

            //Первая открытая страница
            CurrentView = new InventoryVM();
        }
    }
}
