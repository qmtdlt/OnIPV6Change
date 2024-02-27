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
                            _logger.LogInformation("ip �����仯" + new_ip);
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
                //smtp��������ַ
                EnableSsl = true,//����ssl
                Host = "smtp.qq.com",
                UseDefaultCredentials = false,//�Ƿ�ʹ��Ĭ��ƾ��
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //�����������ڷ���smtp���������û���������
                Credentials = new NetworkCredential(my_qq_email, "pwd")
            };
            //����Ĭ�Ϸ�����Ϣ
            Email.DefaultSender = new SmtpSender(smtp);
            var email = Email
                //������
                .From(my_qq_email)
                //�ռ���
                .To(target_email)
                //������
                //.CC("qq����")
                //�ʼ�����
                .Subject("IP�����仯")
                //�ʼ�����
                // .Body("�ʼ�����");
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
        alert(text + ""�Ѹ���"");
    }}
</script>

</body>
</html>
", true);
            //���ݷ��ͽ���ж��Ƿ��ͳɹ�
            var result = await email.SendAsync();
            //��ʹ���첽�ķ�ʽ����
            //await email.SendAsync();
            if (result.Successful)
            {
                //���ͳɹ��߼�
                return true;
            }
            else
            {
                //����ʧ�ܿ���ͨ��result.ErrorMessages�鿴ʧ��ԭ��
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