# OnIPV6Change

监控本机ipv6，当ipv6发生变化，将本机ipv6公网地址，通过server酱油和邮箱推送出去，并将变更的记录写入mysql

Orm框架：Freesql

邮箱发送：FluentEmail

# 其他想说的

折腾家里网络，淘宝花钱搞了光猫桥接，然后有一台旧笔记本，安装了centos，想使用ipv6公网ip访问笔记本，为了应对ipv6偶尔变化，写了这个工具

# 发送邮件相关记录在了Nas Note Station，下方链接访问需要支持ipv6
[FluentEmail qq 邮箱相关配置](http://qmtdlt.synology.me:5000/ns/sharing/qSU7s)

# Server酱推送更见简单，参考官网文档即可

# 尾巴

最终还是阿里云买了个域名，十年一百七十多元，搭配docker部署ddns-go，动态域名解析，直接通过域名访问笔记本了，此项目仅供参考。域名访问更优雅，不过需要花域名的钱，哈哈。。

终归还是穷的，开云服务器终归是贵，自己笔记本玩玩linux，docker，纯属兴趣。能把旧电脑用起来，除了域名，成本就只有电费了。