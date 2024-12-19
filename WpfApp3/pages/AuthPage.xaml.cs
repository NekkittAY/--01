using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private readonly RepairRequestsDBEntities _dbContext = new RepairRequestsDBEntities();
        public AuthPage()
        {
            InitializeComponent();
        }

        // Метод для хэширования паролей
        private static string GetHash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Concat(data.Select(b => b.ToString("X2"))); // Хэш в верхнем регистре
            }
        }

        // Обработчик кнопки входа
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение данных из полей
            string login = TextBoxLogin.Text.Trim();
            string password = PasswordBoxPassword.Password.Trim();

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowErrorMessage("Введите логин и пароль!");
                return;
            }

            // Хэширование пароля
            string passwordHash = GetHash(password);

            // Поиск пользователя в базе данных
            var user = _dbContext.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == passwordHash);

            if (user == null)
            {
                ShowErrorMessage("Неверный логин или пароль!");
                return;
            }

            // Переход на главную страницу в зависимости от роли
            switch (user.Role)
            {
                case "Администратор":
                    NavigationService.Navigate(new AdminPage());
                    break;
                case "Менеджер":
                    NavigationService.Navigate(new ManagerPage());
                    break;
                case "Исполнитель":
                    NavigationService.Navigate(new ExecutorPage(user.ID));
                    break;
                default:
                    ShowErrorMessage("Неизвестная роль пользователя!");
                    break;
            }
        }

        // Показать сообщение об ошибке
        private void ShowErrorMessage(string message)
        {
            TextBlockError.Text = message;
            TextBlockError.Visibility = Visibility.Visible;
        }
    }
}
