using System.Xml;
using static JmalHnd.Util.Conv;

namespace JmalHnd.Util.JMAXML
{
    public class Feed
    {
        public static List<string> entryAlreadyIds = [];
        public static List<Feed> entryList = [];

        public static void FeedParse(XmlDocument xml)
        {
            XmlNamespaceManager ns = new(xml.NameTable);
            ns.AddNamespace("atom", "http://www.w3.org/2005/Atom");

            List<Feed> newEntryList = [];
            foreach (XmlNode entry in xml.SelectNodes("atom:feed/atom:entry", ns))
            {
                DateTime updated = DateTime.Parse(entry.SelectSingleNode("atom:updated", ns).InnerText);
                string id = entry.SelectSingleNode("atom:id", ns).InnerText.Split('/').Last().Replace(".xml", "");
                if (DateTime.Now - updated > TimeSpan.FromMinutes(5))
                {
                    entryAlreadyIds.Remove(id);
                    continue;
                }
                if (entryAlreadyIds.Contains(id))
                    continue;

                newEntryList.Add(new Feed()
                {
                    Title = entry.SelectSingleNode("atom:title", ns).InnerText,
                    Url = entry.SelectSingleNode("atom:id", ns).InnerText,
                    Code = JMAurl2code(entry.SelectSingleNode("atom:id", ns).InnerText),
                    Updated = updated,
                    AuthorName = entry.SelectSingleNode("atom:author/atom:name", ns).InnerText,
                    Content = entry.SelectSingleNode("atom:content", ns).InnerText,
                });
                entryAlreadyIds.Add(id);
            Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"- {updated:MM/dd HH:mm:ss}  {JMAurl2code(entry.SelectSingleNode("atom:id", ns).InnerText)}  {entry.SelectSingleNode("atom:content", ns).InnerText} ({entry.SelectSingleNode("atom:title", ns).InnerText})");

            }
            newEntryList.Reverse();
            entryList.AddRange(newEntryList);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"-- entryList:{entryList.Count} RAM:{GC.GetTotalMemory(false) / 1024 / 1024}MB\n");

        }

        public enum FeedKind
        {
            Null = 0,
            regular = 1,
            extra = 2,
            eqvol = 3,
            other = 4,
            regular_l = 5,
            extra_l = 6,
            eqvol_l = 7,
            other_l = 8
        }

        public string Title { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public DateTime Updated { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
    }
}
