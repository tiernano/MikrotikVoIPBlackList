using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace MikrotikVoIPBlackList
{
	class Program
	{
		static void Main(string[] args)
		{

			string blackListName = "VoIPBlackList";
			StringBuilder sb = new StringBuilder();
			string message = string.Format(@"#  Last Update: {0} {1}
#  
#  This script imports blacklisted VoIP Ips from http://www.voipbl.org/ to Routeros ip address list
#  ", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

			sb.AppendLine(message);
			string blURL = "http://voipbl.org/update";

			string startLine = $@":log info ""{blackListName} ip address list import started""
/system logging disable 0
/ip firewall address-list remove [find list=""{blackListName}""]
/ip firewall address-list";

			sb.AppendLine(startLine);

			HttpClient client = new HttpClient();

			Console.WriteLine("Downloading BlackList");

			string blList = client.GetStringAsync(blURL).Result;

			var lines = from x in blList.Split('\n')
				where !string.IsNullOrWhiteSpace(x) && !x.StartsWith("#")
				select x;

			Console.WriteLine($"Found {lines.Count()} blacklisted IPs");

			foreach (var x in lines)
			{
					string line = $@"/do {{ ip firewall address-list add list=""{blackListName}"" address={x} }}";
					sb.AppendLine(line);
			}

			string endLine = $@"/system logging enable 0
:log info ""{blackListName} ip address list import completed""";

			sb.AppendLine(endLine);

			string fileName = "voipblacklist.txt";

			if (args.Length > 0)
			{
				if (Directory.Exists(args[0]))
				{
					fileName = Path.Combine(args[0], fileName);
				}
			}

			File.WriteAllText(fileName, sb.ToString());

			Console.WriteLine($"Wrote file to {fileName}");

		}
	}
}
