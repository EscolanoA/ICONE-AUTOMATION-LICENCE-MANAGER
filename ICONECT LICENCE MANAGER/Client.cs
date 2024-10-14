using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICONECT_LICENCE_MANAGER
{
    public class Client
    {
        public int Code_client;
        public String Nom;
        public DateTime Date_update;
        public String Description;

        public Client(int Code_client, String Nom, DateTime Date_update, String Description)
        {
            this.Code_client = Code_client;
            this.Nom = Nom;
            this.Date_update = Date_update;
            this.Description = Description;
        }
    }
}
