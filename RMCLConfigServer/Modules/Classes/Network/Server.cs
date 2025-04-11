using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace RMCLConfigServer.Modules.Classes.Network
{
    public class Server
    {
        private readonly int _port;
        private HttpListener _listener;
        private bool _isRunning;

        public Server(int port)
        {
            _port = port;
        }

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{_port}/");
            _listener.Start();
            _isRunning = true;

            Log.Information($"HTTP Server started on http://*:{_port}/");

            Task.Run(() => ListenForRequests());
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            Log.Information("HTTP Server stopped");
        }

        private async Task ListenForRequests()
        {
            try
            {
                while (_isRunning)
                {
                    var context = await _listener.GetContextAsync();
                    Task.Run(() => HandleRequest(context));
                }
            }
            catch (Exception ex)
            {
                if (_isRunning) // Only log if we didn't expect this
                    Log.Error($"Listener error: {ex.Message}");
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;

                // Only handle POST requests
                if (request.HttpMethod != "GET")
                {
                    SendErrorResponse(response, HttpStatusCode.MethodNotAllowed, "Only GET method is supported");
                    return;
                }

                // Read request body
                string requestJson;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    requestJson = await reader.ReadToEndAsync();
                }

                Log.Information($"Received request: {requestJson}");

                // Process the request and get response
                var responseJson = ProcessRequest(requestJson);

                // Send response
                var buffer = Encoding.UTF8.GetBytes(responseJson);
                response.ContentType = "application/json";
                response.ContentEncoding = Encoding.UTF8;
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Log.Error($"Request handling error: {ex.Message}");
                if (context.Response.OutputStream.CanWrite)
                {
                    SendErrorResponse(context.Response, HttpStatusCode.InternalServerError, "Internal server error");
                }
            }
        }

        private void SendErrorResponse(HttpListenerResponse response, HttpStatusCode statusCode, string message)
        {
            response.StatusCode = (int)statusCode;
            var errorResponse = CreateErrorResponse(message);
            var buffer = Encoding.UTF8.GetBytes(errorResponse);
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        private string ProcessRequest(string requestJson)
        {
            try
            {
                using (var doc = JsonDocument.Parse(requestJson))
                {
                    if (!doc.RootElement.TryGetProperty("Type", out var typeProperty))
                    {
                        return CreateErrorResponse("Missing 'Type' field in request");
                    }

                    var type = typeProperty.GetString();
                    Log.Information($"Received request: {type}");
                    return type switch
                    {
                        "Config" => HandleConfigRequest(doc.RootElement),
                        _ => CreateErrorResponse($"Unknown request type: {type}")
                    };
                }
            }
            catch (JsonException)
            {
                return CreateErrorResponse("Invalid JSON format");
            }
        }

        private string HandleConfigRequest(JsonElement root)
        {
            return CreateResponse("Config", Config.MainConfig.LauncherConfig);
        }

        private string CreateResponse(string Type, object Data)
        {
            var response = new
            {
                Type,
                Status = "Success",
                Data,
                Timestamp = DateTime.UtcNow
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }

        private string CreateErrorResponse(string Message)
        {
            var response = new
            {
                Type = "Error",
                Status = "Error",
                Message,
                Timestamp = DateTime.UtcNow
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}