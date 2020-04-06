using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using WpfApp1.DataModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserContext db;
        public MainWindow()
        {
            InitializeComponent();
            using (db = new UserContext())
            {
                if (db.Roles.Count() <= 0)
                {
                    db.Roles.Add(new Role { Type = "admin" });
                    db.Roles.Add(new Role { Type = "user" });
                    db.SaveChanges();
                }
                foreach (var item in db.Roles)
                {
                    cbRoles.Items.Add(item.Type);
                }
                cbRoles.SelectedIndex = 0;
                db.Users.Load();
                userGrid.ItemsSource = db.Users.Local.ToBindingList();
            }
        }

        private void Create_Link(object sender, RoutedEventArgs e)
        {
            create_tab.Focus();
        }

        private void View_All_Link(object sender, RoutedEventArgs e)
        {
            all_tab.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbFirstName.Text)
                && !String.IsNullOrWhiteSpace(tbLastName.Text)
                && !String.IsNullOrWhiteSpace(tbEmail.Text))
            {
                using (db = new UserContext())
                {
                    var role = db.Roles.Where(x => x.Type == cbRoles.SelectedItem.ToString()).Single();
                    db.Users.Add(new User() { FirstName = tbFirstName.Text, LastName = tbLastName.Text, Email = tbEmail.Text, RoleId = (role as Role).Id });
                    db.SaveChanges();
                    db.Users.Load();
                    userGrid.ItemsSource = db.Users.Local.ToBindingList();
                }
                cbRoles.SelectedIndex = 0;
                tbEmail.Text = tbFirstName.Text = tbLastName.Text = "";
            }
            else
            {
                foreach (var item in Tab.Children)
                {
                    if (item is StackPanel)
                    {
                        foreach (var child in (item as StackPanel).Children)
                        {
                            if (child is TextBox)
                            {
                                if (String.IsNullOrWhiteSpace((child as TextBox).Text))
                                    (child as TextBox).BorderBrush = Brushes.Red;
                            }
                        }
                    }
                }
                MessageBox.Show("All fields must be filled");
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (db = new UserContext())
            {
                if (userGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < userGrid.SelectedItems.Count; i++)
                    {
                        User user = userGrid.SelectedItems[i] as User;
                        if (user != null)
                        {
                            db.Users.Remove(db.Users.Where(x => x.Id == user.Id).FirstOrDefault());
                        }
                    }
                }
                db.SaveChanges();
                db.Users.Load();
                userGrid.ItemsSource = db.Users.Local.ToBindingList();
            }
        }
        private void Tb_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).BorderBrush = Brushes.Black;
        }
    }
}
