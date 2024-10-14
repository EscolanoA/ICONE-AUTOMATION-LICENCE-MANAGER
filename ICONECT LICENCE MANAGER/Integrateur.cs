using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICONECT_LICENCE_MANAGER
{
    public class Integrateur
    {
        public int Code_integrateur;
        public String Nom;
        public DateTime Date_update;
        public String Description;

        public Integrateur(int Code_integrateur, String Nom, DateTime Date_update, String Description)
        {
            this.Code_integrateur = Code_integrateur;
            this.Nom = Nom;
            this.Date_update = Date_update;
            this.Description = Description;
        }
    }
}
