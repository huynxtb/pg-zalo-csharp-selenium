using OpenQA.Selenium.Chrome;
using ZaloSelenium.ZaloLibrary;

string path = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net8.0", string.Empty);

var driver = new ChromeDriver(path + @"\Drivers"); // Tạo thư mục Drivers và chứa file chromedriver.exe

driver.Navigate().GoToUrl("https://chat.zalo.me/");

await Task.Delay(20 * 1000);

var zaloLib = new ZaloLibrary();

while (true)
{
    var currentUrl = driver.Url;
    var isCurrentUrl = currentUrl.StartsWith("https://id.zalo.me/");
    var qrCodeLoginExpired = zaloLib.QRCodeLoginExpired(driver);

    if (qrCodeLoginExpired)
    {
        await zaloLib.ReGenerateQRCodeLogin(driver);
    }

    if (!isCurrentUrl) break;

    await Task.Delay(15 * 1000);
}

await zaloLib.SearchAndSelectUser(driver, "0399059205");

await zaloLib.SendMessage(driver, "Hello! I am ProG Coder");

