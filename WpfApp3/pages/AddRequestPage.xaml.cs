using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3.pages
{
    /// <summary>
    /// Логика взаимодействия для AddRequestPage.xaml
    /// </summary>
    public partial class AddRequestPage : Page
    {

        private Requests _currentRequest;
        private readonly RepairRequestsDBEntities _context = new RepairRequestsDBEntities();

        public AddRequestPage(Requests selectedRequest = null)
        {
            InitializeComponent();

            // Если передана заявка, редактируем её, иначе создаём новую
            _currentRequest = selectedRequest ?? new Requests();
            DataContext = _currentRequest;

            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            // Заполнение ComboBoxEquipment
            ComboBoxEquipment.ItemsSource = _context.Equipment.ToList();
            ComboBoxEquipment.DisplayMemberPath = "Name";
            ComboBoxEquipment.SelectedValuePath = "ID";

            // Заполнение ComboBoxClient
            ComboBoxClient.ItemsSource = _context.Users.ToList();
            ComboBoxClient.DisplayMemberPath = "FullName"; // Отображаем ФИО
            ComboBoxClient.SelectedValuePath = "ID";

            // Если заявка существует, устанавливаем текущие значения
            if (_currentRequest.ID != 0)
            {
                ComboBoxEquipment.SelectedValue = _currentRequest.EquipmentID;
                ComboBoxClient.SelectedValue = _currentRequest.ClientID;
                TextBoxFaultType.Text = _currentRequest.FaultType;
                TextBoxDescription.Text = _currentRequest.Description;
                ComboBoxStatus.Text = _currentRequest.Status;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на пустые поля
                if (ComboBoxEquipment.SelectedValue == null || ComboBoxClient.SelectedValue == null)
                {
                    MessageBox.Show("Пожалуйста, выберите оборудование и клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Заполнение текущей заявки
                _currentRequest.EquipmentID = (int)ComboBoxEquipment.SelectedValue;
                _currentRequest.ClientID = (int)ComboBoxClient.SelectedValue;
                _currentRequest.FaultType = TextBoxFaultType.Text.Trim();
                _currentRequest.Description = TextBoxDescription.Text.Trim();
                _currentRequest.Status = ComboBoxStatus.Text;
                _currentRequest.DateAdded = _currentRequest.ID == 0 ? DateTime.Now : _currentRequest.DateAdded;

                if (_currentRequest.ID == 0)
                {
                    // Генерация RequestNumber
                    int nextId = _context.Requests.Any()
                        ? _context.Requests.Max(r => r.ID) + 1
                        : 1;

                    _currentRequest.RequestNumber = $"REQ-{nextId:D3}";
                    _context.Requests.Add(_currentRequest); // Добавляем новую заявку
                }
                else
                {
                    // Обновляем существующую заявку
                    var existingRequest = _context.Requests.Find(_currentRequest.ID);
                    if (existingRequest != null)
                    {
                        existingRequest.EquipmentID = _currentRequest.EquipmentID;
                        existingRequest.ClientID = _currentRequest.ClientID;
                        existingRequest.FaultType = _currentRequest.FaultType;
                        existingRequest.Description = _currentRequest.Description;
                        existingRequest.Status = _currentRequest.Status;
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Заявка успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заявки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
