using GoturBeniIstedigimYere.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class ArffGenerator
    {
        public class ArffData
        {
            public string Name { get; set; }
            public Dictionary<string, int> Categorie { get; set; }
            public int CheckinCount { get; set; }
            public ArffData()
            {
                this.Categorie = new Dictionary<string, int>();
            }
        }

        string[] replaceChar = { "&", "(", ")", "[", "]", "{", "}", "-", "*", "?", "\\", "!", "'", "#", "+", "$", "%", "½" };

        private string Name { get; set; }
        private string STANDART_ARFF = "@RELATION\n@ATTRIBUTE\n@DATA\n";

        public ArffGenerator(string name)
        {
            this.Name = name;
        }

        public string GenerateRelation(string arff)
        {
            arff = arff.Replace("@RELATION", "@RELATION " + this.Name + "\n\n");
            return arff;
        }

        public string GenerateAttibutes(string arff)
        {
            var categories = CheckinOperations.getCategorie();
            string attributes = string.Empty;
            attributes += "@ATTRIBUTE Name string\n";
            foreach (var cat in categories)
            {
                string name = cat.NAME;
                foreach (var item in replaceChar)
                    name = name.Replace(item, "");
                attributes += "@ATTRIBUTE " + name.Replace(" ", "_") + " numeric\n";
            }

            arff = arff.Replace("@ATTRIBUTE", attributes + "\n\n");

            return arff;
        }

        public string GenerateData(string arff)
        {
            string data = "@data\n";

            var userList = CheckinOperations.getUsers();
            List<ArffData> datas = new List<ArffData>();
            foreach (var user in userList)
            {
                ArffData d = new ArffData();
                d.Name = user.USERNAME;
                d.Categorie = getDefaultData();
                var checkins = CheckinOperations.getCheckIns(user.USERID);
                foreach (var checkin in checkins)
                {
                    var categories = CheckinOperations.getPlaceCategories(checkin.PLACEID.Value);
                    foreach (var catPlace in categories)
                    {
                        var cat = CheckinOperations.getCategory(catPlace.CATEGORIEID.Value);
                        d.Categorie[cat.NAME] += 1;
                    }
                }
                d.CheckinCount = checkins.Count;
                datas.Add(d);
            }
            foreach (var d in datas)
            {
                string row = string.Empty;

                row += d.Name + ",";
                foreach (var cat in d.Categorie)
                {
                    if (cat.Value == 0)
                        row += cat.Value.ToString().Replace(",", ".") + ",";
                    else
                    {
                        float result = ((float)cat.Value) / (float)d.CheckinCount;
                        row += result.ToString().Replace(",", ".") + ",";
                    }
                }

                row = row.Substring(0, row.Length - 1);
                data += row + "\n";
            }
            arff = arff.Replace("@DATA", data + "\n\n");
            return arff;
        }

        public string GetDefaultArff()
        {
            return this.STANDART_ARFF;
        }
        private Dictionary<string, int> getDefaultData()
        {
            Dictionary<string, int> data = new Dictionary<string, int>();

            var categories = CheckinOperations.getCategorie();
            foreach (var cat in categories)
                data.Add(cat.NAME, 0);
            return data;
        }

    }
}
