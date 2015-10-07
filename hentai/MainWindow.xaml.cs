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
using System.Windows.Threading;

namespace hentai
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow {
        hApi api = new hApi();
        List<htObject> htObjectList = new List<htObject>();
        htDetailObject htdo = new htDetailObject();

        public MainWindow() {
            InitializeComponent();
            #region initialize htObjectList and update mainList
            htObjectList = api.getList<htObject>(1);
            List<hItem> items = new List<hItem>();
            for (int i = 0; i < htObjectList.Count(); i++) {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(htObjectList[i].img, UriKind.RelativeOrAbsolute);
                bi3.CacheOption = BitmapCacheOption.OnLoad;
                bi3.EndInit();
                
                items.Add(new hItem() { name = htObjectList[i].name, rank = htObjectList[i].rank, img=bi3});
            }
            mainList.ItemsSource = items;
            mainList.Tag = 1;
            #endregion

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\htviewer";
            System.IO.Directory.CreateDirectory(path);
            lbPath.Content = path;

        }

        private void btnDownload_Click(object sender, RoutedEventArgs e) {
            string imgPath = lbPath.Content + "\\" + htdo.name;
            System.IO.Directory.CreateDirectory(imgPath);

            List<htDetailObject> htdoList = new List<htDetailObject>();
            htdoList.Add(htdo);
            downloadList.ItemsSource = htdoList;
            mainTab.SelectedIndex = 2;

            htImgObject htio = api.getObject<htImgObject>(htdo.start);
            int total = Convert.ToInt32(htio.total);
            for (int i = 1; i <= total; i++) {
                downloadImage(htio.image, imgPath + "\\" + i.ToString() + ".png");

                /*WebRequest request = WebRequest.Create(htio.image);
                testPath = imgPath + "\\" + i.ToString() + ".png";
                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ReadCallBack), request);
                while (!result.IsCompleted) {
                    System.Windows.Forms.Application.DoEvents();
                }*/

                System.Windows.Forms.Application.DoEvents();
                if (i != total) {
                    htio = api.getObject<htImgObject>(htio.next);
                }
            }
            MessageBox.Show("Download Success");
        }

        private void btnPath_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = lbPath.Content.ToString();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                lbPath.Content = dialog.SelectedPath;
            }
        }

        private void ListBox_MouseDoubleClick(Object sender, MouseEventArgs e) {
            #region get detail and show in detailTab
            ListBox listBox = (ListBox)sender;
            int index = listBox.SelectedIndex;
            string url = htObjectList[index].href;
            htdo = api.getObject<htDetailObject>(url);
            lbName.Text = htdo.name;
            lbPosted.Content = htdo.information.posted;
            lbRank.Content = htdo.rank_avg + " rank / " + htdo.rank_times + " times";
            lbSizePages.Content = htdo.information.file_size + " / " + htdo.information.length;

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(htdo.img, UriKind.RelativeOrAbsolute);
            bi3.CacheOption = BitmapCacheOption.OnLoad;
            bi3.EndInit();
            imgPreview.Source = bi3;

            詳細.IsEnabled = true;
            mainTab.SelectedIndex = 1;
            #endregion
        }

        void downloadImage(string url,string path) {
            
            WebRequest request = WebRequest.Create(url);
            /*WebResponse response = request.GetResponse();
            System.Drawing.Image img;
            using (Stream imgStream = response.GetResponseStream()) {
                img = System.Drawing.Image.FromStream(imgStream); 
            }
            img.Save(path);*/
            request.BeginGetResponse(new AsyncCallback(ReadCallBack), request);
        }

        private void ReadCallBack(IAsyncResult asyncResult) {
            WebResponse response = (asyncResult.AsyncState as HttpWebRequest).EndGetResponse(asyncResult) as WebResponse;
            System.Drawing.Image img;
            using (Stream imgStream = response.GetResponseStream()) {
                img = System.Drawing.Image.FromStream(imgStream);
            }
            img.Save("");
        }

        void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            ScrollBar scrollbar = (ScrollBar)sender;
            if (scrollbar.Value == mainList.Items.Count - scrollbar.ViewportSize) {
                mainList.Tag = (int)mainList.Tag + 1; //mainList.Tag -> Latest Page
                List<htObject> newhtObjectList = api.getList<htObject>((int)mainList.Tag);
                htObjectList.AddRange(newhtObjectList);
                List<hItem> items = new List<hItem>();
                for (int i = 0; i < htObjectList.Count(); i++) {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(htObjectList[i].img, UriKind.RelativeOrAbsolute);
                    bi3.CacheOption = BitmapCacheOption.OnLoad;
                    bi3.EndInit();

                    items.Add(new hItem() { name = htObjectList[i].name, rank = htObjectList[i].rank, img=bi3});
                }
                mainList.ItemsSource = null;
                mainList.ItemsSource = items;
            }
        }

        #region find ScrollBar and add handler
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e) {
            List<ScrollBar> scrollBarList = GetVisualChildCollection<ScrollBar>(mainList);
            foreach (ScrollBar scrollBar in scrollBarList) {
                if (scrollBar.Orientation == System.Windows.Controls.Orientation.Vertical) {
                    scrollBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(scrollBar_ValueChanged);
                }
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
        #endregion
    }


    public class hItem {
        public string name { get; set; }
        public double rank { get; set; }
        public BitmapImage img { get; set; }
    }
}
