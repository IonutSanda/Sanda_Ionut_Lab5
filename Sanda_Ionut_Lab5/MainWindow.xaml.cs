using AutoLotModel;
using System;
using System.Data;
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

namespace Sanda_Ionut_Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));

            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            
            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            customerOrdersViewSource.Source = ctx.Orders.Local;
            
            ctx.Customers.Load();
            ctx.Inventories.Load();
            ctx.Orders.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "LastName";

            cmbInventory.ItemsSource = ctx.Inventories.Local;
            cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";
        }

        enum ActionState
        {
            New,
            Edit,
            Delete,
            Nothing
        }

        private void btnCustSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if(action == ActionState.New)
            {
                try
                {
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else 
            
                if(action == ActionState.Edit)
                {
                    try
                    {
                        customer = (Customer)customerDataGrid.SelectedItem;
                        customer.FirstName = firstNameTextBox.Text.Trim();
                        customer.LastName = lastNameTextBox.Text.Trim();
                        ctx.SaveChanges();
                    }
                    catch(DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    customerViewSource.View.Refresh();
                    customerViewSource.View.MoveCurrentTo(customer);
                }
            
            else 
            
                if (action == ActionState.Delete)
                {
                    try
                    {
                        customer = (Customer)customerDataGrid.SelectedItem;
                        ctx.Customers.Remove(customer);
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    customerViewSource.View.Refresh();
                }
            
        }
        private void btnCustNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnCustPrev_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnInvSave_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;

            if(action == ActionState.New)
            {
                try
                {
                    inventory = new Inventory()
                    {
                        Color = colorTextBox.Text.Trim(),
                        Make = makeTextBox.Text.Trim()
                    };
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else 
                if(action == ActionState.Edit)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Color = colorTextBox.Text.Trim();
                    inventory.Make = makeTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                inventoryViewSource.View.MoveCurrentTo(inventory);
            }
            else 
                if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
            }
        }


        private void btnInvNext_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }
        private void btnInvPrev_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        private void btnOrdersSave_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;

            if(action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;

                    order = new Order()
                    {
                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };

                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else 
                if(action == ActionState.Edit)
            {
                try
                {
                    order = (Order)ordersDataGrid.SelectedItem;
                    order.CustId = Int32.Parse(custIdTextBox.Text.Trim());
                    order.CarId = Int32.Parse(carIdTextBox.Text.Trim());
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                customerOrdersViewSource.View.Refresh();
                customerOrdersViewSource.View.MoveCurrentTo(order);
            }
            else 
                if(action == ActionState.Delete)
            {
                try
                {
                    order = (Order)ordersDataGrid.SelectedItem;
                    ctx.Orders.Remove(order);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerOrdersViewSource.View.Refresh();
            }
        }
    }
}
