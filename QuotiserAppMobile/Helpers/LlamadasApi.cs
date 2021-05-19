using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuotiserAppMobile.Helpers
{
    class LlamadasApi
    {
        public byte[] ReadByteJson<T>(string url, T objectRequest, string method = "POST")
        {
            
            byte[] result = { };
            try
            {
                
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    //serializamos el objeto
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

                    //peticion
                    WebRequest request = WebRequest.Create(url);
                    //headers
                    request.Method = method;
                    request.PreAuthenticate = true;
                    request.ContentType = "application/json;charset=utf-8'";


                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
             
                        WebResponse myResponse = request.GetResponse();
                        Task.Delay(100);
                        MemoryStream ms = new MemoryStream();
                        myResponse.GetResponseStream().CopyTo(ms);
                        result = ms.ToArray();

               

            }

            catch (Exception e)

            {


            }

            return result;

            


        }

        public string ReadStringJson<T>(string url, T objectRequest, string method = "POST")
        {
            string result = "";
            try
            {
                
                    
                    JavaScriptSerializer js = new JavaScriptSerializer();                
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest); 
                    WebRequest request = WebRequest.Create(url);
                
                    request.Method = method;
                    request.PreAuthenticate = true;
                    request.ContentType = "application/json;charset=utf-8'";


                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
                
                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    Task.Delay(1);

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                           result = streamReader.ReadToEnd();
                    }

                


            }
            catch (Exception e)
            {
                result = e.Message;

            }

            return result;
        }

    }
}
