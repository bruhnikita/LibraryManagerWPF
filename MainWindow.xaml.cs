using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LibraryManager
{
    public partial class MainWindow : Window
    {
        private List<Book> books;
        private BookService bookService = new BookService();

        public MainWindow()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            books = bookService.LoadBooks();
            BooksDataGrid.ItemsSource = books;
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var searchItem = SearchTextBox.Text.ToLower();
            var filteredBooks = books.Where(b =>
                b.Title.ToLower().Contains(searchItem) ||
                b.Author.ToLower().Contains(searchItem)).ToList();
            BooksDataGrid.ItemsSource = filteredBooks;
        }


        private void AddBookButton_Click(Object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text.Trim();
            var author = AuthorTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(title))
            {
                var newBook = new Book { Author = author, Title = title };

                books.Add(newBook);
                bookService.SaveBooks(books);
                LoadBooks();
                TitleTextBox.Clear();
                AuthorTextBox.Clear();
            }

            else MessageBox.Show("Введите название и автора книги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RemoveBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksDataGrid.SelectedItem as Book;

            if (selectedBook != null)
            {
                books.Remove(selectedBook);
                bookService.SaveBooks(books);
                LoadBooks();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Введите название книги" || textBox.Text == "Введите автора книги")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "TitleTextBox")
                    textBox.Text = "Введите название книги";
                else if (textBox.Name == "AuthorTextBox")
                    textBox.Text = "Введите автора книги";

                textBox.Foreground = Brushes.Gray;
            }
        }
    }
}