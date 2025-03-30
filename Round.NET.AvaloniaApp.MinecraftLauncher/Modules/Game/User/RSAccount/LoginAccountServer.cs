using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Avalonia.Threading;

public class LoginAccountServer
{
    private readonly HttpListener _listener;
    private readonly int _port;
    public Action<string> LogiedAction = delegate { };

    public LoginAccountServer(int port)
    {
        _port = port;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"Server started on port {_port}");
        ThreadPool.QueueUserWorkItem((o) =>
        {
            try
            {
                while (_listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem((c) =>
                    {
                        var ctx = c as HttpListenerContext;
                        try
                        {
                            HandleRequest(ctx);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error handling request: {ex.Message}");
                        }
                        finally
                        {
                            try
                            {
                                ctx?.Response.Close();
                            }catch{ }
                        }
                    }, _listener.GetContext());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
        });
    }

    public void Stop()
    {
        _listener.Stop();
        _listener.Close();
    }

    private void HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        Console.WriteLine($"Request: {request.Url}");

        if (request.Url.AbsolutePath == "/login/success")
        {
            // 返回 success.html
            string html = $@"<!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>授权登录至您的 RMCL 账户</title>
    <style>
        :root {{
            --color-blue: #bd00bd;
            --color-white: #fff;
            --curve: cubic-bezier(0.420, 0.000, 0.275, 1.155)
        }}

        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box
        }}

        body {{
            margin: 0;
            padding: 0;
            text-align: center;
            padding: 40px 0;
            background: #fcfcfc;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh
        }}

        .bg-pattern {{
            background: linear-gradient(-45deg, #ee7752, #e73c7e, #23a6d5, #23d5ab);
            background-size: 400% 400%;
            animation: gradientBG 15s ease infinite;
            height: 100vh;
            width: 100vw;
            position: absolute;
            z-index: -10;
        }}

        @keyframes gradientBG {{
            0% {{ background-position: 0% 50%; }}
            50% {{ background-position: 100% 50%; }}
            100% {{ background-position: 0% 50%; }}
        }}

        .bg {{
            height: 100vh;
            width: 100vw;
            z-index: -5;
            position: absolute
        }}

        .bg::after,
        .bg::before {{
            background-image: url(""http://localhost:{_port}/background"");
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            content: """";
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0
        }}

        .bg::after {{
            filter: blur(10px)
        }}

        .container {{
            padding: 40px 0
        }}

        .card {{
            background: #fff;
            padding: 60px;
            border-radius: 15px;
            box-shadow: 0 2px 3px #c8d0d8;
            display: inline-block;
            margin: 0 auto;
            text-align: center
        }}

        .star {{
            position: absolute;
            fill: var(--color-blue);
            opacity: 0;
            animation: grow 3s infinite
        }}

        .star:nth-child(1) {{
            width: 12px;
            height: 12px;
            left: 40px;
            top: 40px;
            animation-delay: 1.5s
        }}

        .star:nth-child(2) {{
            width: 18px;
            height: 18px;
            left: 200px;
            top: 84px;
            animation-delay: 2s
        }}

        .star:nth-child(3) {{
            width: 10px;
            height: 10px;
            left: 22px;
            top: 175px;
            animation-delay: 4s
        }}

        .star:nth-child(4) {{
            width: 20px;
            height: 20px;
            left: 132px;
            top: -12px;
            animation-delay: 6.5s
        }}

        .star:nth-child(5) {{
            width: 114px;
            height: 14px;
            left: 125px;
            top: 162px;
            animation-delay: 8s
        }}

        .star:nth-child(6) {{
            width: 10px;
            height: 10px;
            left: 240px;
            top: 220px;
            animation-delay: 9.5s
        }}

        .checkmark {{
            position: relative;
            padding: 30px;
            animation: checkmark 5m var(--curve) both
        }}

        .checkmark__check {{
            animation: checkmark 5m var(--curve) both;
            position: absolute;
            top: 50%;
            left: 50%;
            z-index: 10;
            transform: translate3d(-50%, -50%, 0);
            fill: var(--color-white)
        }}

        .checkmark__background {{
            position: relative;
            fill: var(--color-blue);
            animation: rotate 15s linear both infinite
        }}

        @keyframes rotate {{
            0% {{ transform: rotate(0) }}
            100% {{ transform: rotate(360deg) }}
        }}

        @keyframes grow {{
            0%, 100% {{ transform: scale(0); opacity: 0 }}
            50% {{ transform: scale(1); opacity: 1 }}
        }}

        @keyframes checkmark {{
            0%, 100% {{ opacity: 0; transform: scale(0) }}
            10%, 50%, 90% {{ opacity: 1; transform: scale(1) }}
        }}

        .title {{
            margin-top: 20px
        }}

        .tips {{
            margin-top: 10px;
            color: #000000;
        }}

        .version-ul {{
            margin-top: 10px;
            color: #4e4e4e;
            position: fixed;
            left: 10px;
            bottom: 10px;
            text-align: start;
            font-size: 11px;
        }}
    </style>
</head>

<body>
    <div class=""bg-pattern""></div>
    <div class=""bg""></div>
    <div class=""container"">
        <div class=""card"">
            <div class=""checkmark"">
                
                <svg class=""checkmark__background"" height=""115"" viewBox=""0 0 120 115"" width=""120"" xmlns=""http://www.w3.org/2000/svg"">
                    <path d=""M107.332 72.938c-1.798 5.557 4.564 15.334 1.21 19.96-3.387 4.674-14.646 1.605-19.298 5.003-4.61 3.368-5.163 15.074-10.695 16.878-5.344 1.743-12.628-7.35-18.545-7.35-5.922 0-13.206 9.088-18.543 7.345-5.538-1.804-6.09-13.515-10.696-16.877-4.657-3.398-15.91-.334-19.297-5.002-3.356-4.627 3.006-14.404 1.208-19.962C10.93 67.576 0 63.442 0 57.5c0-5.943 10.93-10.076 12.668-15.438 1.798-5.557-4.564-15.334-1.21-19.96 3.387-4.674 14.646-1.605 19.298-5.003C35.366 13.73 35.92 2.025 41.45.22c5.344-1.743 12.628 7.35 18.545 7.35 5.922 0 13.206-9.088 18.543-7.345 5.538 1.804 6.09 13.515 10.696 16.877 4.657 3.398 15.91.334 19.297 5.002 3.356 4.627-3.006 14.404-1.208 19.962C109.07 47.424 120 51.562 120 57.5c0 5.943-10.93 10.076-12.668 15.438z""></path>
                </svg>
                <svg class=""checkmark__check"" height=""36"" viewBox=""0 0 48 36"" width=""48"" xmlns=""http://www.w3.org/2000/svg"">
                    <path d=""M47.248 3.9L43.906.667a2.428 2.428 0 0 0-3.344 0l-23.63 23.09-9.554-9.338a2.432 2.432 0 0 0-3.345 0L.692 17.654a2.236 2.236 0 0 0 .002 3.233l14.567 14.175c.926.894 2.42.894 3.342.01L47.248 7.128c.922-.89.922-2.34 0-3.23""></path>
                </svg>
            </div>
            <h1 class=""title"">已登录 Round Studio 账户</h1>
            <p class=""tips"">RMCL 已获得您账户的登录令牌<br>现在可以关闭此页面了</p>
            <p class=""tips"">回到 RMCL 启动器以继续</p>
        </div>
    </div>
    
    <ul class=""version-ul"">
        <p>{Assembly.GetExecutingAssembly().GetName().Version}</p>
        <p>By Round Studio</p>
        <p>RMCL Account</p>
    </ul>
</body>
</html>";

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            response.ContentType = "text/html";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            
            Thread.Sleep(1000);
            this.Stop();
        }
        else if (request.Url.AbsolutePath == "/login" && request.QueryString["uuid"] != null)
        {
            // 获取 UUID 参数
            string uuid = request.QueryString["uuid"];
            Console.WriteLine(uuid);
            Dispatcher.UIThread.Invoke(() => LogiedAction(uuid));
            // 创建跳转页面
            string html = $@"<!DOCTYPE html>
<html>
<head>
    <title>Login Redirect</title>
    <meta http-equiv='refresh' content='0;url=/login/success'>
</head>
<body>
    <h1>Processing Login</h1>
    <p>UUID received: {uuid}</p>
    <p>You will be redirected to the success page in 0 seconds...</p>
</body>
</html>";

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            response.ContentType = "text/html";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        else
        {
            // 404 页面
            string html = @"<!DOCTYPE html>
<html>
<head>
    <title>Not Found</title>
</head>
<body>
    <h1>404 Not Found</h1>
    <p>The requested resource was not found on this server.</p>
</body>
</html>";

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            response.StatusCode = 404;
            response.ContentType = "text/html";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}