namespace RMCLConfigServer.Modules.Classes.HotReload;

public class HotReload
{
    private string FileBody { get; set; } = string.Empty;
    public string file { get; set; } = string.Empty;
    public Action HotReloadAction { get; set; }
    public bool IsRunning { get; set; } = false;
    public HotReload(string file)
    {
        if(!File.Exists(file)) throw new FileNotFoundException($"File {file} does not exist");
        FileBody = File.ReadAllText(file);
        this.file = file;
    }

    public void Start()
    {
        IsRunning = true;
        new Thread(() =>
        {
            while (IsRunning)
            {
                try
                {
                    if (File.ReadAllText(file) != FileBody)
                    {
                        HotReloadAction();
                        FileBody = File.ReadAllText(file);
                    };
                }catch{ }
                Thread.Sleep(200);
            }
        }).Start();
    }

    public void Stop()
    {
        IsRunning = false;
    }
}