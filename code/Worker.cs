using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Net;

namespace OnIPv6ChangeSend
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFreeSql _freeSql;
        public Worker(ILogger<Worker> logger, IFreeSql freeSql)
        {
            _logger = logger;
            _freeSql = freeSql;
        }

        static string new_ip = "";
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await SendEmail("hahaha");
            //_freeSql.Insert<ip_change_log>(new ip_change_log { new_ip = "111", time = DateTime.Now }).ExecuteAffrows();
            while (!stoppingToken.IsCancellationRequested)
            {
                List<string> ipv6_ips = GetLocalIpAddress("InterNetworkV6");
                foreach (var item in ipv6_ips)
                {
                    if (item.Contains("::") && !item.StartsWith("fe") && !item.Contains("%"))
                    {
                        if (item != new_ip)
                        {
                            new_ip = item;
                            _logger.LogInformation("ip 发生变化" + new_ip);
                            await sendWeChat(new_ip);
                            await SendEmail(new_ip);
                            _freeSql.Insert<ip_change_log>(new ip_change_log { new_ip = new_ip, time = DateTime.Now }).ExecuteAffrows();
                        }
                    }
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
        public async Task<bool> SendEmail(string content)
        {
            string my_qq_email = "you qq email";
            string target_email = "your receive email";
            SmtpClient smtp = new SmtpClient
            {
                //smtp服务器地址
                EnableSsl = true,//启用ssl
                Host = "smtp.qq.com",
                UseDefaultCredentials = false,//是否使用默认凭据
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //这里输入你在发送smtp服务器的用户名和密码
                Credentials = new NetworkCredential(my_qq_email, "pwd")
            };
            //设置默认发送信息
            Email.DefaultSender = new SmtpSender(smtp);
            var email = Email
                //发送人
                .From(my_qq_email)
                //收件人
                .To(target_email)
                //抄送人
                //.CC("qq邮箱")
                //邮件标题
                .Subject("IP发生变化")
                //邮件内容
                // .Body("邮件内容");
                .Body($@"<!DOCTYPE html>
<html>
<head>
    <title>Copy to Clipboard</title>
    <style>
        .copy-container {{
            display: flex;
            align-items: center;
            justify-content: start;
            margin: 20px;
        }}

        .copybtn {{
            margin-left: 5px;
        }}
    </style>
</head>
<body>

<div class=""copy-container"">
    <span id=""textToCopy"">{content}</span>
    <input type=""button"" class=""copybtn"" onclick=""copyToClipboard()"" value=""copy""></input>
</div>

<script>
    function copyToClipboard() {{
        var text = document.getElementById('textToCopy').innerText;
        var elem = document.createElement(""textarea"");
        document.body.appendChild(elem);
        elem.value = text;
        elem.select();
        document.execCommand(""copy"");
        document.body.removeChild(elem);
        alert(text + ""已复制"");
    }}
</script>

</body>
</html>
", true);
            //依据发送结果判断是否发送成功
            var result = await email.SendAsync();
            //或使用异步的方式发送
            //await email.SendAsync();
            if (result.Successful)
            {
                //发送成功逻辑
                return true;
            }
            else
            {
                //发送失败可以通过result.ErrorMessages查看失败原因
                return false;
            }
        }
        async Task sendWeChat(string content)
        {
            var url = $"https://sctapi.ftqq.com/{"your key"}.send?title={content}";
            using (var hc = new HttpClient())
            {
                var response = await hc.PostAsync(url, null);
                await Console.Out.WriteLineAsync(response.ReasonPhrase);
            }
        }
        public static List<string> GetLocalIpAddress(string netType)
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            List<string> IPList = new List<string>();
            if (netType == string.Empty)
            {
                for (int i = 0; i < addresses.Length; i++)
                {
                    IPList.Add(addresses[i].ToString());
                }
            }
            else
            {
                for (int i = 0; i < addresses.Length; i++)
                {
                    if (addresses[i].AddressFamily.ToString() == netType)
                    {
                        IPList.Add(addresses[i].ToString());
                    }
                }
            }
            return IPList;
        }
    }
}