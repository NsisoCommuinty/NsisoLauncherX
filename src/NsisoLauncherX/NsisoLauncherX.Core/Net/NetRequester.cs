using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NsisoLauncherX.Core.Net
{
    public class NetRequester: IDisposable
    {

        /// <summary>
        /// NsisoLauncher目前名称.
        /// </summary>
        private string ClientName { get; set; } = "NsisoLauncherX";

        /// <summary>
        /// Set the DefaultRequestHeaders 'AcceptLanguage' in the requester.
        /// </summary>
        private string AcceptLanguageName { get; set; } = CultureInfo.CurrentCulture.Name;

        /// <summary>
        /// NsisoLauncher目前版本号.
        /// </summary>
        private string? ClientVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString(2);
        
        /// <summary>
        /// 表示Web请求中使用的http客户端.
        /// </summary>
        public HttpClient Client { get; private set; }
        
        /// <summary>
        /// 表示http客户端handler
        /// </summary>
        public HttpClientHandler ClientHandler { get; private set; }
        
        /// <summary>
        /// 使用的代理服务
        /// </summary>
        public IWebProxy? NetProxy
        {
            get => this.ClientHandler.Proxy;
            set => this.ClientHandler.Proxy = value;
        }

        public NetRequester()
        {
            this.ClientHandler = new HttpClientHandler();
            this.Client = new HttpClient(this.ClientHandler) {/* Timeout = NetRequester.Timeout */};
            
            // set the default headers
            this.Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(ClientName, ClientVersion));
            this.Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(this.AcceptLanguageName));
        }

        public void Dispose()
        {
            Client.Dispose();
            ClientHandler.Dispose();
            //GC.SuppressFinalize(this);
        }
    }
}