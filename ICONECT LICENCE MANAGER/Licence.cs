using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICONECT_LICENCE_MANAGER
{
    public class Licence
    {
        public int Licence_ID;
        public String Nom;
        public int Code_integrateur;
        public int Code_client;
        public DateTime Date_creation;
        public DateTime Date_expiration;
        public int Nb_variables;
        public int Nb_equipements;

        public Licence(int Licence_ID, String Nom, int Code_integrateur, int Code_client, DateTime Date_creation, DateTime Date_expiration, int Nb_variables, int Nb_equipements)
        {
            this.Licence_ID = Licence_ID;
            this.Nom = Nom;
            this.Code_integrateur = Code_integrateur;
            this.Code_client = Code_client;
            this.Date_creation = Date_creation;
            this.Date_expiration = Date_expiration;
            this.Nb_variables = Nb_variables;
            this.Nb_equipements = Nb_equipements;
        }
    }
}
