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
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using MahApps.Metro.Controls;
using System.Windows.Controls.Primitives;

namespace hentai
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow {
        hApi api = new hApi();
        int page = 1;

        public MainWindow()
        {
            InitializeComponent();
            List<htObject> ro = api.getList<htObject>(1);
            List<hItem> items = new List<hItem>();
            for (int i = 0; i < ro.Count(); i++) {
                items.Add(new hItem() { name = ro[i].name, rank = ro[i].rank });
            }
            Mainlist.ItemsSource = items;

        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e) {
            List<ScrollBar> scrollBarList = GetVisualChildCollection<ScrollBar>(Mainlist);
            foreach (ScrollBar scrollBar in scrollBarList) {
                if (scrollBar.Orientation == System.Windows.Controls.Orientation.Vertical) {
                    scrollBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(scrollBar_ValueChanged);
                } else {
                }
            }
        }

        private void ListBox_MouseDown(Object sender, MouseButtonEventArgs e) {
            ListBox listBox = (ListBox)sender;
            MessageBox.Show("MouseDown event on " + listBox.Name);
        }
        
        void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            ScrollBar scrollbar = (ScrollBar)sender;
            
            if (scrollbar.Value == Mainlist.Items.Count-10) {
                List<htObject> ro = api.getList<htObject>(page++);
                List<hItem> items = new List<hItem>();
                items = (List<hItem>)Mainlist.ItemsSource;
                for (int i = 0; i < ro.Count(); i++) {
                    items.Add(new hItem() { name = ro[i].name, rank = ro[i].rank });
                }
                Mainlist.ItemsSource = null;
                Mainlist.ItemsSource = items;
            }
        }
        
        public static List<T> GetVisualChildCollection<T>(object parent) where T : UIElement {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }


        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : UIElement {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++) {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T) {
                    visualCollection.Add(child as T);
                } else if (child != null) {
                    GetVisualChildCollection(child, visualCollection);
                }
            }
        }
    }

    public class hItem {
        public string name { get; set; }
        public double rank { get; set; }
    }
}
