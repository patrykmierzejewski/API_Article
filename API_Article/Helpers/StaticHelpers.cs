using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Helpers
{
    public class StaticHelpers
    {
        public static List<int> GetIdsBySplitName(string name)
        {
            List<int> idsList = new List<int>();
            List<string> idsString = name.Split(",").ToList();
            foreach (string v in idsString)
            {
                try
                {
                    idsList.Add(int.Parse(v));
                }
                catch
                {
                    continue;
                }
            }

            return idsList;
        }
    }
}
