using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/*
    Most of this code is modeled after automatically generated example 
    by Azure Machine Learning Services after making a web service.
    https://services.azureml.net/workspaces/06628f2290a04f489e85f4fea38117cb/webservices/e4c6a47ac0624212a1fcbbc445bbafca/endpoints/default/consume
 */


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    var content = await req.Content.ReadAsStringAsync();
    log.Info(content);                                                  // Body of request
    var title_start = content.IndexOf("\"Col1\":") + 8;
    var title_end = content.IndexOf("\"Col2\":") - 2;
    var url_start = content.IndexOf("\"Col2\":") + 8;
    var url_end = content.IndexOf("\"Col3\":") - 2;
    var body_start = content.IndexOf("\"Col3\":") + 8;
    var body_end = content.IndexOf("\"Col4\":") - 2;
    var title = content.Substring(title_start, title_end-title_start);  // Extract title from body
    var url = content.Substring(url_start, url_end-url_start);          // Extract url from body
    var body = content.Substring(body_start, body_end-body_start);      // Extract body from body
    log.Info(" " + title + " " + url + " " + body);

    const string apiKey = "fVknxM9XnpcHs4CLG7tlU4H/9oIcdGPxWP6A0kxkPC8mVUpT+6tLImD7FhwoHGf6qm2KVpj8VeO81sFazXpOGQ==";
    const string uri = "https://ussouthcentral.services.azureml.net/workspaces/06628f2290a04f489e85f4fea38117cb/services/ed3bac539d9740a696c8994de708974f/execute?api-version=2.0&format=swagger";
    // Uri and key for Azure Machine Learning Services to post request to
    var client = new HttpClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", apiKey);
    client.BaseAddress = new Uri(uri);

    
    var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>> () {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "Col1", title
                                            },
                                            {
                                                "Col2", url
                                            },
                                            {
                                                "Col3", body
                                            },
                                            {
                                                "Col4", ""
                                            },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>() {
                    }
                };

    HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);
    string result = await response.Content.ReadAsStringAsync();
    if (response.IsSuccessStatusCode) 
                {
                    Console.WriteLine("Result: {0}", result);
                    log.Info("Result: {0}", result);
                } 
                else
                {
                    Console.WriteLine(string.Format("Request failed: {0}", response.StatusCode));
                    log.Info(string.Format("Request failed: {0}", response.StatusCode));
                    Console.WriteLine(response.Headers.ToString());

                    Console.WriteLine(result);
                }
    return response.IsSuccessStatusCode
        ? req.CreateResponse(HttpStatusCode.OK, result)
        : req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request!");
}
