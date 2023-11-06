using biotecgait.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Models
{
    public partial class InsoleModel : ObservableObject, IDisposable
    {
        private static HashSet<int> idsUsed = new HashSet<int>();
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string mac;
        [ObservableProperty]
        public int? battery;
        [ObservableProperty]
        public string fw;
        [ObservableProperty]
        public bool connected;
        [ObservableProperty]
        public Side? side;
        public InsoleModel(string name, string MAC) 
        {
            Id = getNextID();
            Name = name;
            Mac = MAC;
            Connected = false;
            Battery = null;
            Fw = "";
            Side = null;
        }
        private static int getNextID()
        {
            for (int i = 0; i < idsUsed.Count; i++)
            {
                if (!idsUsed.Contains(i))
                {
                    idsUsed.Add(i);
                    return i;
                }
            }
            int id = idsUsed.Count;
            idsUsed.Add(id);
            return id;
        }
        ~InsoleModel()
        {
            idsUsed.Remove(Id);
        }
        public void Dispose()
        {
            idsUsed.Remove(Id);
            GC.SuppressFinalize(this);
        }
    }
}
