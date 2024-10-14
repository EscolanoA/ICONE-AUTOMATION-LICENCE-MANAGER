using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICONECT_LICENCE_MANAGER
{
    public class Fonction
    {
        public int Code_fonction;
        public String Nom;
        public DateTime Date_update;
        public String Description;
        public Fonction(int Code_fonction, String Nom, DateTime Date_update, String Description)
        {
            this.Code_fonction = Code_fonction;
            this.Nom = Nom;
            this.Date_update = Date_update;
            this.Description = Description;
        }
    }
}
