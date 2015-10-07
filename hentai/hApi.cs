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

namespace hentai {
    public class hApi {

        public List<T> getList<T>(int page){
            try {
                string text;
                List<T> result;
                WebRequest request = WebRequest.Create("http://hentai.lionfree.net/index.php/api/gtlist/" /*+ page*/);
                request.ContentType = "application/json; charset=utf-8";
                WebResponse response = request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
                    text = sr.ReadToEnd();
                    result = Deserialize<T>(text);
                }
                return result;
            }catch(Exception ex){
                return new List<T>();
            }
        }

        private List<T> Deserialize<T>(string json) {
            List<T> result;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
                result = (List<T>)serializer.ReadObject(ms);
            }
            return result;
        }
    }
}
