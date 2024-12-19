using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp3.pages
{
    /// <summary>
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        private readonly RepairRequestsDBEntities _context = new RepairRequestsDBEntities();

        public ManagerPage()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            // Загружаем заявки с включением связанных данных
            DataGridRequests.ItemsSource = _context.Requests
                .Include("Equipment")
                .Include("Repairs.Users")
                .ToList();
        }

        private void ButtonAssignSpecialist_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridRequests.SelectedItem is Requests selectedRequest)
            {
                // Открываем окно для выбора исполнителя
                var assignPage = new AssignExecutorPage(selectedRequest);
                NavigationService.Navigate(assignPage);
            }
            else
            {
                MessageBox.Show("Выберите заявку для назначения специалиста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonExtendDeadline_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridRequests.SelectedItem is Requests selectedRequest)
            {
                var inputDialog = new InputDialog("Введите новую дату окончания (ГГГГ-ММ-ДД):");
                if (inputDialog.ShowDialog() == true)
                {
                    if (DateTime.TryParse(inputDialog.Input, out DateTime newDeadline))
                    {
                        selectedRequest.Deadline = newDeadline;
                        _context.SaveChanges();
                        LoadRequests();
                        MessageBox.Show("Срок выполнения продлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Неправильный формат даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для продления срока!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonGenerateQR_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
