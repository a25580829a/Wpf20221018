using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Drink> drinks = new List<Drink>();
        List<Order_item> order = new List<Order_item>();
        private string takeout;
        public MainWindow()
        {
            InitializeComponent();

            AddNewDrink(drinks);

            DisplayDrink(drinks);
        }

        private void DisplayDrink(List<Drink> myDrink)
        {
            foreach (Drink d in myDrink)
            {
                StackPanel sp = new StackPanel();
                CheckBox cb = new CheckBox();
                TextBox tb = new TextBox();
                Slider sl = new Slider();
                Label la = new Label();
                Binding mybinding = new Binding("Value");

                sp.Orientation = Orientation.Horizontal;
                //cb.Content = d.Name + d.Size + d.Price;
                cb.Content = $"{d.Name} {d.Size} {d.Price}";
                cb.Width = 200;
                cb.Height = 40;
                cb.Margin = new Thickness(5);

                //tb.Width = 50;
                //tb.Height = 20;
                sl.Value = 0;
                sl.Width = 100;
                sl.Maximum = 30;
                sl.Minimum = 0;
                sl.TickFrequency = 1;
                sl.IsSnapToTickEnabled = true;
                //sl.ValueChanged += sl_Value_Changed;

                mybinding.Source = sl;


                la.Width = 100;
                la.SetBinding(ContentProperty, mybinding);

                sp.Children.Add(cb);
                //sp.Children.Add(tb);
                sp.Children.Add(sl);
                sp.Children.Add(la);

                stackpanel_DrinkMenu.Children.Add(sp);
            }
        }
        /*
        private void sl_Value_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider targetslider = sender as Slider;
            label.Content = targetslider.Value.ToString();

        }
        */
        private void AddNewDrink(List<Drink> mydrinks)
        {
            mydrinks.Add(new Drink() { Name = "咖啡", Size = "大杯", Price = 60 });
            mydrinks.Add(new Drink() { Name = "咖啡", Size = "小杯", Price = 50 });
            mydrinks.Add(new Drink() { Name = "紅茶", Size = "大杯", Price = 30 });
            mydrinks.Add(new Drink() { Name = "紅茶", Size = "小杯", Price = 20 });
            mydrinks.Add(new Drink() { Name = "綠茶", Size = "大杯", Price = 30 });
            mydrinks.Add(new Drink() { Name = "綠茶", Size = "小杯", Price = 20 });
        }


        private void Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.IsChecked == true)
            {
                takeout = rb.Content.ToString();
                //MessageBox.Show(takeout);
            }
        }

        private void Order_button_Click(object sender, RoutedEventArgs e)
        {
            Place_order(order);

            Display_Order_Detail(order);
        }

        private void Display_Order_Detail(List<Order_item> order)
        {
            displayTextBlock.Text = $"您所訂購的飲品為:{takeout}\n";
            int total = 0;

            int i = 1;
            foreach (Order_item item in order)
            {
                total += item.Subtotal;
                Drink drinkitem = drinks[item.Index];
                displayTextBlock.Text += $"訂購品項:{i} {drinkitem.Name}{drinkitem.Size} X {item.Quantity}杯，每杯{drinkitem.Price}元，小計{item.Subtotal}\n";
                i++;
            }
            displayTextBlock.Text += $"未打折的金額為:{total}\n";
            displayTextBlock.Text += $"獲得的紅利點數(10%):{total / 10}點\n";
            if (total >= 500 && total < 1000) displayTextBlock.Text += $"折扣後金額:{total / 10 * 9}";
            else if (total >= 1000) displayTextBlock.Text += $"折扣後金額:{total / 100 * 85}";
            else displayTextBlock.Text += $"未達金額500元以上 不打折";
            displayTextBlock.TextAlignment = TextAlignment.Center; 
            displayTextBlock.Background = Brushes.AntiqueWhite;
        }

        private void Place_order(List<Order_item> myOrder)
        {
            myOrder.Clear();

            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                StackPanel sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
                CheckBox cb = sp.Children[0] as CheckBox;
                Slider sl = sp.Children[1] as Slider;

                int quantity = Convert.ToInt32(sl.Value);
                if (cb.IsChecked == true && quantity != 0)
                {
                    int price = drinks[i].Price;
                    int subtotal = price * quantity;
                    myOrder.Add(new Order_item(){ Index = i, Quantity = quantity, Subtotal = subtotal });
                }
            }
        }
    }
}
