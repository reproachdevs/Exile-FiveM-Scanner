using DiscordMessenger;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Management;
using System.ServiceProcess;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Linq;

using System.Xml.Linq;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Guna.UI2.WinForms;


namespace ExileScanner
{
    public partial class Particles : Form
    {
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();
        private Color backgroundColor = Color.FromArgb(15, 15, 15);

        private string usedpin;
        private string pinhook = "https://discord.com/api/webhooks/2222222222222222222/DCdK-AOWqQB0gTkb4fqhKYyQJ0WPt5szMhTHlheult1QC6DFREUTDmM8oEGqeCvgM0vY";


        public Particles()
        {
            InitializeComponent();
            LittleBootyBitchs();
            timer1.Interval = 1;
            timer1.Start();
            DoubleBuffered = true;
        }

        private void LittleBootyBitchs()
        {
            int numParticles = 25;
            for (int i = 0; i < numParticles; i++)
            {
                double angle = random.NextDouble() * 2 * Math.PI;
                double speed = random.Next(1, 5);
                particles.Add(new Particle()
                {
                    Position = new PointF(random.Next(0, ClientSize.Width), random.Next(0, ClientSize.Height)),
                    Velocity = new PointF((float)(Math.Cos(angle) * speed), (float)(Math.Sin(angle) * speed)),
                    Radius = random.Next(2, 4),
                    Color = Color.FromArgb(80, 80, 80)
                });
            }
        }
        private void BigBootyBitchs()
        {
            foreach (var particle in particles)
            {
                particle.Position = new PointF(particle.Position.X + particle.Velocity.X, particle.Position.Y + particle.Velocity.Y);
                if (particle.Position.X < 0) particle.Position = new PointF(ClientSize.Width, particle.Position.Y);
                if (particle.Position.X > ClientSize.Width) particle.Position = new PointF(0, particle.Position.Y);
                if (particle.Position.Y < 0) particle.Position = new PointF(particle.Position.X, ClientSize.Height);
                if (particle.Position.Y > ClientSize.Height) particle.Position = new PointF(particle.Position.X, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(backgroundColor);
            foreach (var particle in particles)
            {
                e.Graphics.FillEllipse(new SolidBrush(particle.Color),
                    particle.Position.X - particle.Radius,
                    particle.Position.Y - particle.Radius,
                    particle.Radius * 1, particle.Radius * 1);
                foreach (var otherParticle in particles)
                {
                    if (particle != otherParticle)
                    {
                        float dx = particle.Position.X - otherParticle.Position.X;
                        float dy = particle.Position.Y - otherParticle.Position.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        if (distance < 50)
                        {
                            int alpha = (int)((1.0f - (distance / 50.0f)) * 255.0f);
                            e.Graphics.DrawLine(new Pen(Color.FromArgb(alpha, Color.FromArgb(80, 80, 80)), 1),
                                particle.Position, otherParticle.Position);
                        }
                    }
                }
            }
        }

        public int GetService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        var status = service.Status.ToString();
                        ManagementObject wmiService;
                        wmiService = new ManagementObject("Win32_Service.Name='" + $"{serviceName}" + "'");
                        wmiService.Get();
                        var id = Convert.ToInt32(wmiService["ProcessId"]);
                        return id;
                    }
                }
            }
            catch
            {
            }
            return 0;
        }

        static string GetSystemStartTimeAsString()
        {
            DateTime systemStartTime = GetSystemStartTime();
            return systemStartTime.ToString("dd/MM/yyyy - HH:mm:ss");
        }
        static DateTime GetSystemStartTime()
        {

            using (PerformanceCounter uptimeCounter = new PerformanceCounter("System", "System Up Time"))
            {
                uptimeCounter.NextValue();

                float uptimeSeconds = uptimeCounter.NextValue();

                DateTime systemStartTime = DateTime.Now - TimeSpan.FromSeconds(uptimeSeconds);

                return systemStartTime;
            }
        }

        private void downloaddepends()
        {
            string url = "https://files.offshore.cat/r7nyPTFk.exe";
            string path = @"C:\xxstrings.exe";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
            Thread.Sleep(3000);
        }

        private void collectstrings()
        {
            // Gather PIDS
            int explorer = Process.GetProcessesByName("explorer")[0].Id;
            int Lsass = Process.GetProcessesByName("Lsass")[0].Id;
            int dnscache = GetService("Dnscache");
            int dps = GetService("DPS");
            int diagtrack = GetService("DiagTrack");
            int sysmain = GetService("SysMain");
            int pcasvc = GetService("PcaSvc");
            int Discord = Process.GetProcessesByName("Discord")[0].Id;

            // xxstrings Commands
            string lsass_cmd = $"cd C:\\ && xxstrings.exe -p {Lsass} -raw > C:\\lsass.txt";
            string explorer_cmd = $"cd C:\\ && xxstrings.exe -p {explorer} -raw > C:\\explorer.txt";
            string dnscache_cmd = $"cd C:\\ && xxstrings.exe -p {dnscache} -raw > C:\\dnscache.txt";
            string dps_cmd = $"cd C:\\ && xxstrings.exe -p {dps} -raw > C:\\dps.txt";
            string diagtrack_cmd = $"cd C:\\ && xxstrings.exe -p {diagtrack} -raw > C:\\diagtrack.txt";
            string sysmain_cmd = $"cd C:\\ && xxstrings.exe -p {sysmain} -raw > C:\\sysmain.txt";
            string pcasvc_cmd = $"cd C:\\ && xxstrings.exe -p {pcasvc} -raw > C:\\pcasvc.txt";
            string Discord_cmd = $"cd C:\\ && xxstrings.exe -p {Discord} -raw > C:\\Discord.txt";

            // Initializing CMD
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Verb = "runas";
            startInfo.UseShellExecute = true;

            Process process = new Process();
            process.StartInfo = startInfo;

            // Executing Commands
            startInfo.Arguments = "/C" + lsass_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + explorer_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + dnscache_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + dps_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + diagtrack_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + sysmain_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + pcasvc_cmd;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C" + Discord_cmd;
            process.Start();
            process.WaitForExit();

        }

        private void scanner()
        {
            Dictionary<string, string> personalizednames = new Dictionary<string, string> {
                { "keyauth.win", "keyauth" },
                { "dControl.exe", "defcon" },
                { "Defender Control", "defcon" },
                { "imdisk0", "imdisk" },

                //gosth
                { "Launcher.exe", "Possible Gosth" },
                { "pedrin.cc", "Gosth" },
                { "three.pedrin", "Gosth" },
                { "pedrin.ovh", "Gosth" },
                { "pedrin.cc0", "Gosth" },
                { "pedrin.cc0!", "Gosth" },
                { "gosth.gg", "Gosth" },
                { "api-three.pedrin.cc", "Gosth" },
                { "https://cdn.gosth.ltd/launcher.exe", "Gosth" },
                { "ovh-01.pedrin.cc", "Gosth" },
                { "!2023/01/22:01:40:53!0!", "Gosth" },
                { "131.196.198.50" ,"Gosth" },
                { "cdn.gosth.ltd", "Gosth" },
                { "AppCrash_notepad.exe", "Possible Gosth" },

                // Space YX
                {"rage-graphics-five.dll" ,"Possible SpaceYX" },
                
                //susano
                { "cfg.latest", "Possivel Susano" },
                { "favorites.cfg", "Possivel Susano" },
                { "ZltWMtLL5xBgZ2M", "Possivel Susano" },
                
                //skript
                { "http://crl.pki.goog/gtsr1/gtsr1.crl0M", "skrip" },
                { "http://ocsp.pki.goog/s/gts1p5/ghf_lTR8_n801", "skript" },
                {"skript.gg0", "skript" },
                {"http://ocsp.pki.goog/s/gts1p5/ghf_lTR8_n8", "skript" },
                { "20231219164333Z0t0r0J0", "skript" },
                { "1.3.6.1.4.1.11129.2.5.3", "skript" },//vps
                { "skript.gg", "skript" },
                { "s.k.r.i.p.t...g.g.", "skript" },
                { "USBDeview.exe", "skript" },
                { "USBDeview.dll", "skript" },
                { "vps-32704700.vps.ovh.ca", "skript" },
                { "https://skript.gg/favicon.png", "skript" },
                { "https://skript.gg/", "skript" },

                { "GTS CA 1P50", "skript" },
                { "https://pki.goog/repository/0", "skript" },
                { "Software\\Microsoft\\Cryptography", "skript" },

                // TESTO
                {"blitz.exe", "Possible Testo"},
                {"Blitz-2.1.7.exe", "Possible Testo"},

                // TZ
                {"TZProject.com", "TZProject"},
                {"TZProject.win", "TZProject"},


                //monesy e tracks
                { "api.monesy.dev", "monesy" },
                { "!2023/04/12:19:24:40!", "monesy" },
                { "monesy.dev", "monesy" },
                { "api.idandev.xyz", "tracks" },
                { "idandev.xyz", "tracks" },
                { "https://projectcheats.com", "Project Cheats" },
                { "api.projectcheats.com", "Project" },
                { "pc5m.ini", "Project" },
                { "Public.zip", "Project" },
                { "!2099/01/19:13:33:15!36ac9!","Bypass Generic" },
                { "https://cdn.discordapp.com/attachments/1090775912104071188/1186838886098415697/Xrc19-12att.dll", "XRC" },
                { "https://cdn.discordapp.com/attachments/1090775912104071188/1186838886098415697/Xrc", "XRC" },
                { "$url = \"https://cdn.discordapp.com", "XRC" },
                { "", "" },

                // FS
                { "!2023/10/31:13:24:16", "FiveSharp" },
                { "0x29000", "FiveSharp" },

                // Royal
                {"RamonLoader.2.2.exe", "Royal" },
                {"!2023/11/17:00:36:34", "Royal" },

                // Eulen

                {"0x2678000", "Eulen" },
                {"12024/01/19 :14 :55:18", "Eulen" },

                // Susano
                {"Susano.re", "Susano" },

                // Redzone.gg
                {"rz.gg.exe", "Redzone.gg" },

                // CFX MAFIA

                {"cfxmafia_loader.exe", "CFX Mafia" },
                {"0x845000", "CFX Mafia" },
                {"!2023/06/10:15:51:52!", "CFX Mafia" },
                {"C:\\Users\\Public\\cryptography.dll", "CFX Mafia" },

                // RE

                {"HowYouLookSoPerfectOnYourWorstDays.exe", "Red Engine" },
                {"!2023/09/14:02:04:16!", "Red Engine" },
                {"wsauth.redengine.eu", "Red Engine" },
                {"https://i.imgur.com/55ZbAWq.png", "Red Engine Public"},
                 
                // Sicario

                {"ezilax.exe", "Sicario" },
                {"2023/08/16:22:43:26!", "Sicario" },
                {"0x821000 ", "Sicario" },

                // Asgard

                {"!2023/08/05:23:12:33!", "Asgard" },
                {"asg.pomajdorownia.pl", "Asgard" },
                {"!2023/04/13:17:33:54!", "Asgard" },

                // HX
                {"api.hxcheats.com", "hxcheats" },
                {"kzoem.hxcheats.com", "hxcheats" },
                {"0x5a1000", "hxcheats" },

                // TWOODS Slotted

                {"EpicGamesOnline.exe", "Atlantis Bypass" },
                {"0x4c8000", "Atlantis Bypass" },

                // Genesis


                {"0xca2000", "Genesis Bypass" },
                {"2023/10/30:15:45:40", "Genesis Bypass" }






            };

            List<string> StringsWantedD = new List<string> {
                 "keyauth.win",
                 "eauth.us.to",
                 "auth.gg",
                 "licenseauth.site",
                 "dControl.exe",  //defender
                 "Defender Control",
                 "imdisk0",

                 //skript
                 "http://crl.pki.goog/gtsr1/gtsr1.crl0M",
                 "http://ocsp.pki.goog/s/gts1p5/ghf_lTR8_n801",
                 "skript.gg0",
                 "http://ocsp.pki.goog/s/gts1p5/ghf_lTR8_n8",
                 "20231219164333Z0t0r0J0",
                 "1.3.6.1.4.1.11129.2.5.3",
                 "skript.gg",
                 "s.k.r.i.p.t...g.g.",
                 "USBDeview.exe",
                 "vps-32704700.vps.ovh.ca",
                 "https://skript.gg/favicon.png",
                 "https://skript.gg/",
                 "GTS CA 1P50",
                 "https://pki.goog/repository/0",
                 "Software\\Microsoft\\Cryptography",
  
                
                 

                 //gosth
                 "pedrin.cc",
                 "three.pedrin.cc",
                 "pedrin.cc",
                 "pedrin.ovh",
                 "pedrin.cc0!",
                 "gosth.gg",
                 "api-three.pedrin.cc",
                 "https://cdn.gosth.ltd/launcher.exe",
                 "ovh-01.pedrin.cc",
                 "api-three.pedrin.cc",
                 "!2023/01/22:01:40:53!0!",//gosth
                 "AppCrash_notepad.exe",//Possiveel Gosth (notepadcrash)
                 "Launcher.exe",// Possivel Gosth (Launcher.exe)
                 "131.196.198.50",//api gosth


                 //project
                 "projectcheats.com",
                 "api.projectcheats.com",
                 "pc5m.ini",//project
                 "Public.zip",//project

                 //red engine
                 "TypeShit.exe",
                 "Instructions.txt",
                 "Settings.cock",

                 // Monster

                 "Unex Refline Loader (Rename me).exe",

                 // FiveSharp
                 "FS Update.exe",
 
                 //eulen
                 "loader_prod.exe",
                

                 //tz project
                 "TZX.zip",
                

                 //bypass etc
                 "api.monesy.dev", //monesy
                 "!2023/04/12:19:24:40!",//monesy
                 "monesy.dev", //monesy
                 "api.idandev.xyz", //tracks
                 "idandev.xyz", //tracks
                 "!2099/01/19:13:33:15!36ac9!", //bypass generico
                 "https://cdn.discordapp.com/attachments/1090775912104071188/1186838886098415697/Xrc19-12att.dll",// discord xrc
                 "https://cdn.discordapp.com/attachments/1090775912104071188/1186838886098415697/Xrc", //discord xrc
                 "$url = \"https://cdn.discordapp.com", //powershell xrc
                 "--inject",//stopped
                 "ChromeCrashReporter.exe",
                 "plutolauncher.exe",
                 "vgtray.exe",




            };

            string InfoFound = "";

            if (File.Exists("C:\\explorer.txt"))
            {
                string contents = File.ReadAllText("C:\\explorer.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }
                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nEXP -> " + PersonalizedConcatenatedNames + "\n";
            }

            if (File.Exists("C:\\Lsass.txt"))
            {
                string contents = File.ReadAllText("C:\\lsass.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }
                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nLSA -> " + PersonalizedConcatenatedNames + "\n";
            }

            if (File.Exists("C:\\dnscache.txt"))
            {
                string contents = File.ReadAllText("C:\\dnscache.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }
                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nDNS -> " + PersonalizedConcatenatedNames + "\n";
            }



            if (File.Exists("C:\\dps.txt"))
            {
                string contents = File.ReadAllText("C:\\dps.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }
                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nDPS -> " + PersonalizedConcatenatedNames + "\n";
            }

            if (File.Exists("C:\\sysmain.txt"))
            {
                string contents = File.ReadAllText("C:\\sysmain.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }
                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nSYS -> " + PersonalizedConcatenatedNames + "\n";
            }

            if (File.Exists("C:\\Discord.txt"))
            {
                string contents = File.ReadAllText("C:\\Discord.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }
                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nDIS -> " + PersonalizedConcatenatedNames + "\n";
            }

            if (File.Exists("C:\\pcasvc.txt"))
            {
                string contents = File.ReadAllText("C:\\pcasvc.txt");

                List<string> StringsFound = new List<string>();

                foreach (string StringsWanted in StringsWantedD)
                {
                    if (contents.Contains(StringsWanted))
                    {
                        StringsFound.Add(StringsWanted);
                    }
                }

                string PersonalizedConcatenatedNames = "";
                foreach (string StringFound in StringsFound)
                {
                    string personalizedname = personalizednames.ContainsKey(StringFound) ? personalizednames[StringFound] : StringFound;

                    PersonalizedConcatenatedNames += personalizedname + ", ";
                }

                PersonalizedConcatenatedNames = PersonalizedConcatenatedNames.TrimEnd(',', ' ');

                InfoFound += "\nPCA -> " + PersonalizedConcatenatedNames + "\n";
            }

            if (!string.IsNullOrEmpty(InfoFound))
            {
                string pinformat = $"||{usedpin}|| ";
                string currentDateTime = $"__{GetCurrentDateTime()}__";
                string PCNAME = $"__{Dns.GetHostEntry(Environment.MachineName).HostName.ToString()}__";
                string Traces = $"```{InfoFound}```";

                string[] serviceNames = { "Sysmain", "Pcasvc", "DPS", "Diagtrack" };
                string resultString = GetServiceStatusString(serviceNames);

                var message = new DiscordMessage()
                    .SetUsername("Exile Scanner")
                    .SetAvatar("https://cdn.discordapp.com/attachments/2222222222222222222/1223417851210236037/hellangelic.jpg?ex=6619c7a9&is=660752a9&hm=8253d9d36a30580485988f049167cb5fd36c551ced71e326f30fdef1d9341aa6&")
                    .AddEmbed()
                        .SetTimestamp(DateTime.Now)
                        .SetTitle("\nExile Scanner - Results")
                        .SetDescription(
                            "\n > **Pc Name:** " + PCNAME +
                            "\n > **Data:** " + currentDateTime +
                            //"\n > **IP:** " + ipv4Address +
                            //"\n > **HWID:** " + HWID +

                            "\n > **StartTime:** " + $"__{GetSystemStartTimeAsString()}__" + "\n" + $"{resultString}" +
                            "\n > **Detected:**" + "\n" + Traces

                        )
                        .SetColor(00000000)
                        .SetFooter("Exile Scanner - Results", "https://cdn.discordapp.com/attachments/2222222222222222222/1223417851210236037/hellangelic.jpg?ex=6619c7a9&is=660752a9&hm=8253d9d36a30580485988f049167cb5fd36c551ced71e326f30fdef1d9341aa6&")
                        .Build();

                message.SendMessage(pinhook);
                doothershit();
                string[] filesToDelete =
            {
                @"C:\lsass.txt",
                @"C:\diagtrack.txt",
                @"C:\Discord.txt",
                @"C:\dnscache.txt",
                @"C:\dps.txt",
                @"C:\explorer.txt",
                @"C:\pcasvc.txt",
                @"C:\xxstrings.exe"
            };

                deletefilesfromarray(filesToDelete);
            }
            else
            {
                string HWID = $"__{System.Security.Principal.WindowsIdentity.GetCurrent().User.Value}__";
                string PCNAME = $"__{Dns.GetHostEntry(Environment.MachineName).HostName.ToString()}__";
                string UserS = $"__{Environment.UserName.ToString()}__";
                string currentDateTime = $"__{GetCurrentDateTime()}__";


                var message = new DiscordMessage()
                   .SetUsername("Exile Scanner - Clean")
                   .SetAvatar("https://cdn.discordapp.com/attachments/2222222222222222222/1223417851210236037/hellangelic.jpg?ex=6619c7a9&is=660752a9&hm=8253d9d36a30580485988f049167cb5fd36c551ced71e326f30fdef1d9341aa6&")
                   .AddEmbed()
                       .SetTimestamp(DateTime.Now)
                .SetTitle("\nExile Scanner")
                       .SetDescription(
                           "> **User:** " + UserS +
                            "\n > **Pc Name:** " + PCNAME +
                            "\n > **Data:** " + currentDateTime +
                           // "\n > **IP:** " + ipv4Address +
                           //"\n > **HWID:** " + HWID +
                           "\n > **Resultado** " + $"__{"Clean"}__"
                       )
                       .SetColor(00000000)
                       .SetFooter("Exile Scanner - Logs", "https://cdn.discordapp.com/attachments/2222222222222222222/1223417851210236037/hellangelic.jpg?ex=6619c7a9&is=660752a9&hm=8253d9d36a30580485988f049167cb5fd36c551ced71e326f30fdef1d9341aa6&")
                       .Build();

                message.SendMessage(pinhook);
                doothershit();
                string[] filesToDelete =
             {
                @"C:\lsass.txt",
                @"C:\diagtrack.txt",
                @"C:\Discord.txt",
                @"C:\dnscache.txt",
                @"C:\dps.txt",
                @"C:\explorer.txt",
                @"C:\pcasvc.txt",
                @"C:\xxstrings.exe"
            };

                deletefilesfromarray(filesToDelete);
            }
        }

        static string GetServiceStatusString(string[] serviceNames)
        {
            string result = "";

            foreach (var serviceName in serviceNames)
            {
                string serviceStatus = GetServiceStatus(serviceName);

                result += $"{serviceStatus}\n";
            }

            return result;
        }


        static string GetServiceStatus(string serviceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    return $" > **{serviceName}** : __{sc.Status.ToString().ToLower()}__";
                }
            }
            catch (Exception ex)
            {
                return $" > **{serviceName}** : Erro ao obter status: {ex.Message.ToLower()}";
            }
        }

        static string generatepin()
        {
            Random random = new Random();
            int pin = random.Next(10000, 99999);
            return pin.ToString();

        }

        static string GetCurrentDateTime()
        {
            DateTime currentDateTime = DateTime.Now;

            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return formattedDateTime;
        }

        private async void sendpintohook(string pin)
        {
            string pinformat = $"||{pin}||";
            string currentDateTime = $"__{GetCurrentDateTime()}__";
            string PCNAME = $"__{Dns.GetHostEntry(Environment.MachineName).HostName.ToString()}__";

            var message = new DiscordMessage()
                .SetUsername("Exile Scanner")
                .SetAvatar("https://cdn.discordapp.com/attachments/2222222222222222222/1223417851210236037/hellangelic.jpg?ex=6619c7a9&is=660752a9&hm=8253d9d36a30580485988f049167cb5fd36c551ced71e326f30fdef1d9341aa6&")
                .AddEmbed()
                   .SetTimestamp(DateTime.Now)
                   .SetTitle("Pin Generated - Exile Scanner")
                   .SetDescription(
                      "> **Pc Name:** " + PCNAME +
                      "\n **Date:** " + currentDateTime +
                      "\n **Pin** " + pinformat
                )
                .SetColor(00000000)
                .SetFooter("Exile Scanner - Pin Gen", "https://cdn.discordapp.com/attachments/2222222222222222222/1223417851210236037/hellangelic.jpg?ex=6619c7a9&is=660752a9&hm=8253d9d36a30580485988f049167cb5fd36c551ced71e326f30fdef1d9341aa6&")
                .Build();

            message.SendMessage(pinhook);

            

        }

        private void Particles_Load(object sender, EventArgs e)
        {
            usedpin = generatepin();
            sendpintohook(usedpin);
            this.BackColor = Color.FromArgb(16, 16, 16);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        

        public static void deletefilesfromarray(string[] filePaths)
        {
           

          foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
        }

        public async void doothershit()
        {
            if (guna2Button1.InvokeRequired)
            {
                guna2Button1.Invoke((MethodInvoker)delegate
                {
                    
                    guna2CircleProgressBar1.Visible = false;
                    label2.Visible = true;
                });
            }
            else
            {
                guna2CircleProgressBar1.Visible = false;
                label2.Visible = true;
            }
        }

        public async void doshit()
        {
            if (guna2Button1.InvokeRequired)
            {
                guna2Button1.Invoke((MethodInvoker)delegate
                {
                    guna2Button1.Visible = false;
                    pinbox.Visible = false;
                    guna2CircleProgressBar1.Visible = true;  
                });
            }
            else
            {
                guna2Button1.Visible = false;
                pinbox.Visible = false;
                guna2CircleProgressBar1.Visible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BigBootyBitchs();
            Invalidate();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {

                if (pinbox.Text.Length == 5 && pinbox.Text == usedpin)
                {
                    doshit();
                    Thread.Sleep(1000);
                    downloaddepends();
                    Thread.Sleep(2000);
                    scanner();


                }
                else
                {
                    Application.Exit();
                }

            });
        }
    }

    public class Particle
    {
        public PointF Position { get; set; }
        public PointF Velocity { get; set; }
        public int Radius { get; set; }
        public Color Color { get; set; }
    }

}
