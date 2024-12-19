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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly RepairRequestsDBEntities _context = new RepairRequestsDBEntities();

        public AdminPage()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            DataGridRequests.ItemsSource = _context.Requests.Include("Equipment").ToList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRequestPage());
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(entry => entry.Reload());
                LoadRequests();
            }
        }


        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridRequests.SelectedItem is Requests selectedRequest)
            {
                NavigationService.Navigate(new AddRequestPage(selectedRequest));
            }
            else
            {
                MessageBox.Show("Выберите заявку для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequests = DataGridRequests.SelectedItems.Cast<Requests>().ToList();

            if (!selectedRequests.Any())
            {
                MessageBox.Show("Выберите хотя бы одну заявку для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить {selectedRequests.Count} заявок?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаление связанных записей (если необходимо)
                    foreach (var request in selectedRequests)
                    {
                        var relatedRecords = _context.Repairs.Where(c => c.RequestID == request.ID).ToList();
                        _context.Repairs.RemoveRange(relatedRecords);
                        var relatedFeedbackRecords = _context.Feedback.Where(c => c.RequestID == request.ID).ToList();
                        _context.Feedback.RemoveRange(relatedFeedbackRecords);
                        _context.Requests.Remove(request);
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Заявки успешно удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadRequests();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении заявок: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonAssignExecutor_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridRequests.SelectedItem is Requests selectedRequest)
            {
                NavigationService.Navigate(new AssignExecutorPage(selectedRequest));
            }
            else
            {
                MessageBox.Show("Выберите заявку для назначения исполнителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
