namespace PMEquipment;

public partial class MainPage : ContentPage
{
    private bool _loaded;

    public MainPage()
    {
        InitializeComponent();
        Loaded += async (_, _) => await LoadLocalAppAsync();
    }

    private async Task LoadLocalAppAsync()
    {
        try
        {
            // فایل‌های HTML و کتابخانهٔ اکسل را کنار هم در فضای محلی برنامه کپی می‌کنیم.
            var appFolder = FileSystem.AppDataDirectory;
            Directory.CreateDirectory(appFolder);

            var target = Path.Combine(appFolder, "index.html");
            var xlsxTarget = Path.Combine(appFolder, "xlsx.full.min.js");

            if (!File.Exists(target))
            {
                await using var input = await FileSystem.OpenAppPackageFileAsync("index.html");
                await using var output = File.Create(target);
                await input.CopyToAsync(output);
            }

            if (!File.Exists(xlsxTarget))
            {
                await using var input = await FileSystem.OpenAppPackageFileAsync("xlsx.full.min.js");
                await using var output = File.Create(xlsxTarget);
                await input.CopyToAsync(output);
            }

            AppWebView.Source = new UrlWebViewSource
            {
                Url = new Uri(target).AbsoluteUri
            };
            _loaded = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("خطا در بارگذاری", ex.Message, "متوجه شدم");
        }
    }

    private async void OnReloadClicked(object sender, EventArgs e)
    {
        if (!_loaded)
            await LoadLocalAppAsync();
        else
            AppWebView.Reload();
    }
}
