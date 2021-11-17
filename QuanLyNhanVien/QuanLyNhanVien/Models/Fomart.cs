﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNhanVien.Models
{
    public class Fomart
    {
        public static string Fomartstring(string item)
        {
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
}