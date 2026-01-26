namespace DemoChatSignalR.Client;

public class SwalAlertService
{
    public enum SwalType
    {
        Info,
        Warning,
        Success,
        Error,
        Loadding,
        Close
    }
    public Action<string, string, SwalType>? OnShow { get; set; }

    private async Task Show(string title, string message, SwalType type)
    {
        await EnsureReady();
        OnShow?.Invoke(title, message, type);
    }

    private async Task EnsureReady()
    {
        int retry = 0;
        while (OnShow == null && retry++ < 10)
            await Task.Delay(100);
    }


    public Task Info(string message, string title = "Thông báo")
        => Show(title, message, SwalType.Info);

    public Task Warning(string message, string title = "Cảnh báo")
        => Show(title, message, SwalType.Warning);

    public Task Success(string message, string title = "Thành công")
        => Show(title, message, SwalType.Success);

    public Task Error(string message, string title = "Báo lỗi")
        => Show(title, message, SwalType.Error);

    public Task Loading(string message = "", string title = "Đang xử lý...")
        => Show(title, message, SwalType.Loadding);

    public Task Close()
        => Show("", "", SwalType.Close);
}
