using System;

namespace QuanLyNhanVien.Models
{
    public class Fomart
    {
        public static string Fomartstring(string item)
        {
            if (item == null) return null;
            string afterFm = "";
            if (item.Length > 0)
            {
                item = item.Trim().ToLower();
                while (item.IndexOf("  ") != -1)
                {
                    item = item.Remove(item.IndexOf("  "), 1);
                }
                string[] s = item.Split(' ');
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        string first = s[i].Substring(0, 1);
                        string another = s[i].Substring(1, s[i].Length - 1);
                        afterFm += first.ToUpper() + another + " ";
                    }
                }
                afterFm = afterFm.Remove(afterFm.LastIndexOf(" "), 1);

            }
            else
            {
                Console.WriteLine("Khong the xoa tiep");
            }
            return afterFm;
        }

    }

    public class ReponseModel
    {

        public object Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public ReponseModel(object data, string message = null, int status = 1)
        {
            Data = data;
            Message = message;
            Status = status;
        }
    }
}