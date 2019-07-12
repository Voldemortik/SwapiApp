using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace SwapiApp
{
    
    class SwapiHelper
    {
        private string apiUrl = "http://swapi.co/api";
        private string _proxyName = null;
        private HttpClient client;

        private enum HttpMethod
        {
            GET,
            POST
        }
        
        public SwapiHelper()
        {
            client = new HttpClient();
        }

        public SwapiHelper(string proxyName)
        {
            _proxyName = proxyName;
        }

        
        public async Task<SwapiPeople> GetUser(int id)
        {
            var person = await client.GetStringAsync(apiUrl + $"/people/{id}");
            return JsonConvert.DeserializeObject<SwapiPeople>(person);
        }
        
        public async Task<SwapiFilm> GetFilm(string filmPath)
        {
            var film = await client.GetStringAsync(filmPath);
            return JsonConvert.DeserializeObject<SwapiFilm>(film);
        }
       
        public async Task<SwapiPlanet> GetPlanet(string planetPath)
        {
            var planet = await client.GetStringAsync(planetPath);
            return JsonConvert.DeserializeObject<SwapiPlanet>(planet);
        }
        
        private string Request(string url, HttpMethod httpMethod)
        {
            return Request(url, httpMethod, null, false);
        }
        
        private string Request(string url, HttpMethod httpMethod, string data, bool isProxyEnabled)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = httpMethod.ToString();

            if (!string.IsNullOrEmpty(_proxyName))
            {
                httpWebRequest.Proxy = new WebProxy(_proxyName, 80);
                httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            if (data != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
                httpWebRequest.ContentLength = bytes.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Dispose();
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
            result = reader.ReadToEnd();
            reader.Dispose();

            return result;
        }
        
        private string SerializeDictionary(Dictionary<string, string> dictionary)
        {
            StringBuilder parameters = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                parameters.Append(keyValuePair.Key + "=" + keyValuePair.Value + "&");
            }
            return parameters.Remove(parameters.Length - 1, 1).ToString();
        }
      
        private T GetSingle<T>(string endpoint, Dictionary<string, string> parameters = null) where T : SwapiEntity
        {
            string serializedParameters = "";
            if (parameters != null)
            {
                serializedParameters = "?" + SerializeDictionary(parameters);
            }

            return GetSingleByUrl<T>(url: string.Format("{0}{1}{2}", apiUrl, endpoint, serializedParameters));
        }
       
        private SwapiEntityResults<T> GetMultiple<T>(string endpoint) where T : SwapiEntity
        {
            return GetMultiple<T>(endpoint, null);
        }
        
        private SwapiEntityResults<T> GetMultiple<T>(string endpoint, Dictionary<string, string> parameters) where T : SwapiEntity
        {
            string serializedParameters = "";
            if (parameters != null)
            {
                serializedParameters = "?" + SerializeDictionary(parameters);
            }

            string json = Request(string.Format("{0}{1}{2}", apiUrl, endpoint, serializedParameters), HttpMethod.GET);
            SwapiEntityResults<T> swapiResponse = JsonConvert.DeserializeObject<SwapiEntityResults<T>>(json);
            return swapiResponse;
        }
       
        private NameValueCollection GetQueryParameters(string dataWithQuery)
        {
            NameValueCollection result = new NameValueCollection();
            string[] parts = dataWithQuery.Split('?');
            if (parts.Length > 0)
            {
                string QueryParameter = parts.Length > 1 ? parts[1] : parts[0];
                if (!string.IsNullOrEmpty(QueryParameter))
                {
                    string[] p = QueryParameter.Split('&');
                    foreach (string s in p)
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(temp[0], temp[1]);
                        }
                        else
                        {
                            result.Add(s, string.Empty);
                        }
                    }
                }
            }
            return result;
        }
       
        private SwapiEntityResults<T> GetAllPaginated<T>(string entityName, string pageNumber = "1") where T : SwapiEntity
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("page", pageNumber);

            SwapiEntityResults<T> result = GetMultiple<T>(entityName, parameters);

            result.NextPageNo = String.IsNullOrEmpty(result.Next) ? null : GetQueryParameters(result.Next)["page"];
            result.PreviousPageNo = String.IsNullOrEmpty(result.Previous) ? null : GetQueryParameters(result.Previous)["page"];

            return result;
        }
        
        public T GetSingleByUrl<T>(string url) where T : SwapiEntity
        {
            string json = Request(url, HttpMethod.GET);
            T swapiResponse = JsonConvert.DeserializeObject<T>(json);
            return swapiResponse;
        }
        
        public SwapiEntityResults<SwapiPeople> GetAllPeople(string pageNumber = "1")
        {
            SwapiEntityResults<SwapiPeople> result = GetAllPaginated<SwapiPeople>("/people/", pageNumber);

            return result;
        }
        
       

    }
}
