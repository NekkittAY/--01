using System;
using System.CodeDom.Compiler;
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
using System.Data.Entity;


namespace WpfApp3.pages
{
    /// <summary>
    /// Логика взаимодействия для ExecutorPage.xaml
    /// </summary>
    public partial class ExecutorPage : Page
    {
        private readonly RepairRequestsDBEntities _context = new RepairRequestsDBEntities();
        private int _executorId; // ID текущего исполнителя
        public string[] StatusList { get; } = { "В ожидании", "В работе", "Выполнено" };
        public ExecutorPage(int executorId)
        {
            InitializeComponent();

            _executorId = executorId;

            DataContext = this; // Устанавливаем DataContext для привязки StatusList
            LoadRequests();
        }
        private void LoadRequests()
        {
            try
            {
                var executorId = _executorId; // ID текущего исполнителя
                DataGridRequests.ItemsSource = _context.Repairs
                    .Where(r => r.ExecutorID == executorId)
                    .Include(r => r.Requests) // Подгружаем связанные заявки
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var repair in DataGridRequests.Items.OfType<Repairs>())
                {
                    // Устанавливаем EndDate при статусе "Выполнено"
                    if (repair.Requests.Status == "Выполнено" && repair.EndDate == null)
                    {
                        repair.EndDate = DateTime.Now;
                    }
                    else if (repair.Requests.Status != "Выполнено")
                    {
                        repair.EndDate = null; // Сбрасываем EndDate, если статус изменен
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем таблицу после сохранения
                LoadRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
