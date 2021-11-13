using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNhanVien.Extensions
{
    public static class SessionExtension
    {
        public static void Set<T>(string key, T data) where T : class
        {
            HttpContext.Current.Session[key] = data;
        }
        public static void SetList<T>(string key, List<T> data) where T : class
        {
            HttpContext.Current.Session[key] = data;
        }
        public static T Get<T>(string key) where T:class
        {
             return (T)HttpContext.Current.Session[key];
        }
        public static List<T> GetList<T>(string key) where T : class
        {
            return (List<T>)HttpContext.Current.Session[key];
        }
    }
}