using OpenQA.Selenium;
using ZaloSelenium.Constants;

namespace ZaloSelenium.ZaloLibrary;

public class ZaloLibrary
{
    public bool QRCodeLoginExpired(IWebDriver driver)
    {
        try
        {
            var ele = driver.FindElement(By.ClassName(AppConst.QrcodeExpiredClass));
            var qrTitle = ele.FindElement(By.TagName("p")).Text;

            if (qrTitle.Equals("Mã QR hết hạn"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            Console.WriteLine("ERROR: Not found qr code expired");
            return false;
        }
    }

    public async Task ReGenerateQRCodeLogin(IWebDriver driver)
    {
        await Task.Delay(3 * 1000);

        try
        {
            var ele = driver.FindElement(By.ClassName(AppConst.QrcodeExpiredClass));
            var btnReGenerate = ele.FindElement(By.ClassName("btn--s"));
            btnReGenerate.Click();

            Console.WriteLine("INFO: Re-generate qr code login");
        }
        catch
        {
            Console.WriteLine("ERROR: Not found qr code expired");
        }
    }

    public async Task SearchAndSelectUser(IWebDriver driver, string text)
    {
        try
        {
            var input = driver.FindElement(By.Id(AppConst.ContactSearchInputId));
            input.SendKeys("");
            input.SendKeys(text);

            Console.WriteLine($"INFO: Search user {text}");

            await Task.Delay(3 * 1000);

            var user = driver.FindElement(By.CssSelector(AppConst.DivFriendItem));
            user.Click();

            await Task.Delay(3 * 1000);

            Console.WriteLine($"INFO: Selected user {text}");
        }
        catch
        {
            Console.WriteLine("ERROR: Not found user");
            await SearchAndSelectUser(driver, text);
        }
    }

    private async Task<bool> ChatBoxIsVisiable(IWebDriver driver)
    {
        await Task.Delay(3 * 1000);

        try
        {
            var chatBox = driver.FindElement(By.Id(AppConst.AvaChatBoxViewId));
            return true;
        }
        catch
        {
            return await ChatBoxIsVisiable(driver);
        }
    }

    public async Task SendMessage(IWebDriver driver, string text)
    {
        try
        {
            if (await ChatBoxIsVisiable(driver))
            {
                var chatInput = driver.FindElement(By.Id(AppConst.ChatInputContentId));
                chatInput.Click();

                await Task.Delay(3 * 1000);

                var richInput = driver.FindElement(By.Id(AppConst.RichInputId));
                richInput.SendKeys(text);
                
                await Task.Delay(3 * 1000);

                var btnSend = driver.FindElement(By.CssSelector(AppConst.ButtonSend));
                btnSend.Click();

                Console.WriteLine($"INFO: Sent message {text}");
            }
        }
        catch
        {
            Console.WriteLine("ERROR: Send message fail");
            await SendMessage(driver, text);
        }
    }
}
