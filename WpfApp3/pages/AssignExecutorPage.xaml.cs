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
    /// Логика взаимодействия для AssignExecutorPage.xaml
    /// </summary>
    public partial class AssignExecutorPage : Page
    {
        private readonly RepairRequestsDBEntities _context = new RepairRequestsDBEntities();
        private readonly Requests _selectedRequest;
        private readonly Repairs _currentRepair;

        public AssignExecutorPage(Requests request)
        {
            InitializeComponent();
            _selectedRequest = request ?? throw new ArgumentNullException(nameof(request));
            _currentRepair = _context.Repairs.FirstOrDefault(r => r.RequestID == _selectedRequest.ID);
            LoadExecutors();
        }

        private void LoadExecutors()
        {
            try
            {
                // Загружаем пользователей с ролью "Исполнитель"
                var executors = _context.Users
                    .Where(u => u.Role == "Исполнитель")
                    .ToList();

                ComboBoxExecutors.ItemsSource = executors;

                // Если уже есть исполнитель, выбираем его
                if (_currentRepair != null)
                {
                    ComboBoxExecutors.SelectedValue = _currentRepair.ExecutorID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки исполнителей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxExecutors.SelectedValue == null)
                {
                    MessageBox.Show("Выберите исполнителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int executorId = (int)ComboBoxExecutors.SelectedValue;

                // Создаем запись в таблице Repairs
                Repairs newRepair = new Repairs
                {
                    RequestID = _selectedRequest.ID,
                    ExecutorID = executorId,
                    StartDate = DateTime.Now,
                    Comments = "Исполнитель назначен."
                };

                _context.Repairs.Add(newRepair);
                _context.SaveChanges();

                MessageBox.Show("Исполнитель успешно назначен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при назначении исполнителя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
