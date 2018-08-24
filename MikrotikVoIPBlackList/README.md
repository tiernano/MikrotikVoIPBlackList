# Mikrotik VoIP BlackList

##What is it?
Microsoft VoIP BlackList is a .NET Core app that downloads a list of Blacklisted IPs from http://www.voipbl.org and creates a script to run on your Mikrotik Devices. With some firewall rules, you can then block the blacklisted traffic to your VoIP server at the firewall, not at the box itself.

## Why?
I noticed i was getting a lot of authentication requests to my VoIP Server ([3cx](http://www.3cx.com)) that was being blacklisted on the VoIP server itself, so i went looking, and found VoipBL.org. But they only have code for [Fail2Ban](https://www.fail2ban.org) on [Asterisk](https://www.asterisk.org). So, thats how this was born!

## How do i use it?
You need .NET Core 2.0 on your machine. Binaries will be avaialbe soon, but till then, you will either need to build it with Visual Studio 2017 or .NET Core SDK 2.0. 

The following steps might help:

`git clone https://github.com/tiernano/MikrotikVoIPBlackList.git
cd MikrotikVoIPBlackList
cd MikrotikVoIPBlackList
dotnet restore
dotnet build
dotnet run`

this will produce an TXT File (script for Mikrotik) in the folder you are in. Yes, i did CD in to the folder twice... the first is the solution folder, second is the project...

Copy that RSC file to the mikrotik and run:

`import voipblacklist.txt`

Alternatively, you can add a param of an output folder you want to write to. For example, you could run:

`dotnet run c:\inetpub\wwwroot`

On a Windows box with IIS, this will write the file to IIS. Then, you can run the following script on your mikrotik device:

`/tool fetch url="http://<iisserver>/voipblacklist.txt"
import voipblacklist.txt`

Where <iisserver> is the IP of your server. That could be any box, but i ran this on Windows, so thats what i had!

you may also want to add the following Firewall rule in Mikrotik:

`add action=drop chain=forward comment="VoIP BlackList" dst-port=5060 log=yes log-prefix=FW_VoIP_BL protocol=udp \
    src-address-list=VoIPBlackList`

you can remove the Logging parts if not required. 

## Questions
If you have questions, ask here. Any problems, leave a issue on the site. 

Thanks!
