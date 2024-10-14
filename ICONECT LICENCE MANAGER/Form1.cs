using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    TODOLIST

    -Faire CRUD pour CLIENTS
    -Faire CRUD pour INTEGRATEURS
    -Faire CRUD pour FONCTIONS
    -Faire page A propos
    -Rajouter logos

 */

/*
    FONCTIONS TERMINEES

    -CRUD LICENCES
    -Foncitonnement du mode ajout/modification
    -Fonction ANNULER modifications pour le mode ajout/modification
    -Génération du fichier de licence
    -Fonction retour pour le mode ajout/modifications
    -Nom de l'application avec version dynamique (à changer dans app.config)
    -Des fenetres de confirmation des actions

*/


namespace ICONECT_LICENCE_MANAGER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());

        //Creation d'une liste d'objet licence
        List<Licence> listeLicences = new List<Licence>();
        //Creation d'une liste d'objet fonction
        List<Fonction> listeFonctions = new List<Fonction>();
        //Creation d'une liste d'objet integrateur
        List<Integrateur> listeIntegrateurs = new List<Integrateur>();
        //Creation d'une liste d'objet client
        List<Client> listeClients = new List<Client>();

        private void Form1_Load(object sender, EventArgs e)
        {
            tc_Licences.Appearance = TabAppearance.FlatButtons;
            tc_Licences.ItemSize = new Size(0, 1);
            tc_Licences.SizeMode = TabSizeMode.Fixed;

            tc_Integrateurs.Appearance = TabAppearance.FlatButtons;
            tc_Integrateurs.ItemSize = new Size(0, 1);
            tc_Integrateurs.SizeMode = TabSizeMode.Fixed;

            #region ajout des colonnes des datagridview
            dgv_R_Licences.ColumnCount = 8;
            dgv_R_Licences.Columns[0].Name = "ID";
            dgv_R_Licences.Columns[1].Name = "Nom";
            dgv_R_Licences.Columns[2].Name = "Nom de l'integrateur";
            dgv_R_Licences.Columns[3].Name = "Nom du client";
            dgv_R_Licences.Columns[4].Name = "Date de création";
            dgv_R_Licences.Columns[5].Name = "Date d'expiration";
            dgv_R_Licences.Columns[6].Name = "Nombre de variables";
            dgv_R_Licences.Columns[7].Name = "Nombre d'equipements";

            dgv_R_LicenceFonctions.ColumnCount = 3;
            dgv_R_LicenceFonctions.Columns[0].Name = "Code";
            dgv_R_LicenceFonctions.Columns[1].Name = "Nom";
            dgv_R_LicenceFonctions.Columns[2].Name = "Description";

            dgv_U_FonctionsDisponibles.ColumnCount = 3;
            dgv_U_FonctionsDisponibles.Columns[0].Name = "Code";
            dgv_U_FonctionsDisponibles.Columns[1].Name = "Nom";
            dgv_U_FonctionsDisponibles.Columns[2].Name = "Description";

            dgv_U_LicenceFonctions.ColumnCount = 3;
            dgv_U_LicenceFonctions.Columns[0].Name = "Code";
            dgv_U_LicenceFonctions.Columns[1].Name = "Nom";
            dgv_U_LicenceFonctions.Columns[2].Name = "Description";

            dgv_R_Integrateurs.ColumnCount = 3;
            dgv_R_Integrateurs.Columns[0].Name = "Code";
            dgv_R_Integrateurs.Columns[1].Name = "Nom";
            dgv_R_Integrateurs.Columns[2].Name = "Description";
            #endregion

            adaptDataGridView();

            tb_AppVersion.Text = "ILM V" + ConfigurationManager.AppSettings["AppVersion"].ToString();

            lb_AdresseEmail.Text = ConfigurationManager.AppSettings["AdresseEmail"].ToString();
            lb_Localisation.Text = ConfigurationManager.AppSettings["Localisation"].ToString();
            lb_NumeroTel.Text = ConfigurationManager.AppSettings["NumeroTel"].ToString();

            lb_AppName.Text = ConfigurationManager.AppSettings["AppName"].ToString();
            lb_Copyrights.Text = ConfigurationManager.AppSettings["Copyrights"].ToString();
            lb_Version.Text = "ILM V" + ConfigurationManager.AppSettings["AppVersion"].ToString();

            fill_dgv_Fonctions();
            fill_dgv_Clients();
            fill_dgv_Integrateurs();
            fill_dgv_Licences();

            #region alimentation des combobox

            foreach (Client client in listeClients)
            {
                cb_U_LicenceClient.Items.Add(client.Nom + "." + client.Code_client.ToString());
            }
            foreach (Integrateur integrateur in listeIntegrateurs)
            {
                cb_U_LicenceIntegrateur.Items.Add(integrateur.Nom + "." + integrateur.Code_integrateur.ToString());
            }
            #endregion
        }

        //Remplie les liste et les datagridviews avec les données de la base
        private void fill_dgv_Fonctions()
        {
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_Fonctions", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            listeFonctions.Clear();
            while (rdr.Read())
            {
                listeFonctions.Add(new Fonction(Convert.ToInt32(rdr[0]), Convert.ToString(rdr[1]), Convert.ToDateTime(rdr[2]), Convert.ToString(rdr[3])));
            }
            con.Close();
        }
        private void fill_dgv_Clients()
        {
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_Clients", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            listeClients.Clear();
            while (rdr.Read())
            {
                listeClients.Add(new Client(Convert.ToInt32(rdr[0]), Convert.ToString(rdr[1]), Convert.ToDateTime(rdr[2]), Convert.ToString(rdr[3])));
            }
            con.Close();
        }
        private void fill_dgv_Integrateurs()
        {
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_Integrateurs", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            listeIntegrateurs.Clear();
            while (rdr.Read())
            {
                listeIntegrateurs.Add(new Integrateur(Convert.ToInt32(rdr[0]), Convert.ToString(rdr[1]), Convert.ToDateTime(rdr[2]), Convert.ToString(rdr[3])));
            }
            con.Close();
        }
        private void fill_dgv_Licences()
        {
            dgv_R_Licences.SelectionChanged -= SelectedLicenceChanged;
            dgv_R_Licences.Rows.Clear();
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_Licences", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            listeLicences.Clear();
            while (rdr.Read())
            {
                listeLicences.Add(new Licence(Convert.ToInt32(rdr[0]), Convert.ToString(rdr[1]), Convert.ToInt32(rdr[2]), Convert.ToInt32(rdr[3]), Convert.ToDateTime(rdr[4]), Convert.ToDateTime(rdr[5]), Convert.ToInt32(rdr[6]), Convert.ToInt32(rdr[7])));
                int rowIndex = dgv_R_Licences.Rows.Add();
                dgv_R_Licences.Rows[rowIndex].Cells[0].Value = rdr[0];
                dgv_R_Licences.Rows[rowIndex].Cells[1].Value = rdr[1];


                foreach (Licence licence in listeLicences)
                {
                    if (Convert.ToInt32(rdr[0]) == licence.Licence_ID)
                    {
                        dgv_R_Licences.Rows[rowIndex].Cells[2].Value = get_Licence_Integrateur(licence).Nom;
                        dgv_R_Licences.Rows[rowIndex].Cells[3].Value = get_Licence_Client(licence).Nom;

                    }
                }
                dgv_R_Licences.Rows[rowIndex].Cells[4].Value = Convert.ToDateTime(rdr[4]).ToString("dd/MM/yyyy");
                dgv_R_Licences.Rows[rowIndex].Cells[5].Value = Convert.ToDateTime(rdr[5]).ToString("dd/MM/yyyy");
                dgv_R_Licences.Rows[rowIndex].Cells[6].Value = rdr[6];
                dgv_R_Licences.Rows[rowIndex].Cells[7].Value = rdr[7];
            }
            con.Close();

            //Permet d'assigner la fonction daffichage des Foncitons de la Licence au datagriview aprés que les licence ont charger
            dgv_R_Licences.ClearSelection();
            dgv_R_Licences.SelectionChanged += SelectedLicenceChanged;
        }


        //Quand une licence est séléctioné, afficher ses fonctions
        private void fill_dgv_LicenceFonctions(int Licence_ID)
        {
            dgv_R_LicenceFonctions.Rows.Clear();
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_LicenceFonctions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = Licence_ID;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int rowIndex = dgv_R_LicenceFonctions.Rows.Add();
                dgv_R_LicenceFonctions.Rows[rowIndex].Cells[0].Value = rdr[0];
                dgv_R_LicenceFonctions.Rows[rowIndex].Cells[1].Value = rdr[1];
                dgv_R_LicenceFonctions.Rows[rowIndex].Cells[2].Value = rdr[3];
            }
            con.Close();
        }
        private void SelectedLicenceChanged(object sender, EventArgs e)
        {
            if (dgv_R_Licences.SelectedRows.Count > 0)
            {
                int Licence_ID = Convert.ToInt32(dgv_R_Licences.SelectedRows[0].Cells[0].Value);
                fill_dgv_LicenceFonctions(Licence_ID);
            }
        }


        //Quand le mode Ajout est choisi
        private void goToAjoutMode(object sender, EventArgs e)
        {
            #region cacher les champs
            btn_U_Licence.Visible = false;
            lb_V1.Visible = false;
            lb_V2.Visible = false;
            lb_V3.Visible = false;
            lb_V4.Visible = false;
            lb_V5.Visible = false;
            lb_V6.Visible = false;
            lb_V7.Visible = false;
            lb_V8.Visible = false;
            lb_V9.Visible = false;

            tb_R_LicenceID.Visible = false;
            tb_R_LicenceIntegrateur.Visible = false;
            tb_R_LicenceClient.Visible = false;
            tb_R_LicenceIntegrateur.Visible = false;
            tb_R_LicenceNbEquipements.Visible = false;
            tb_R_LicenceNbVariables.Visible = false;
            tb_R_LicenceNom.Visible = false;
            dtp_R_LicenceDateCreation.Visible = false;
            dtp_R_LicenceDateExpiration.Visible = false;
            #endregion

            fill_dgv_FonctionsDipsonibles();

            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_nextLicenceID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                tb_U_LicenceID.Text = rdr[0].ToString();
            }
            con.Close();

            dtp_U_LicenceDateCreation.Value = DateTime.Now;
            dtp_U_LicenceDateExpiration.Value = DateTime.Now;

            adaptDataGridView();
            tc_Licences.SelectedIndex = 1;
        }
        private void fill_dgv_FonctionsDipsonibles()
        {
            dgv_U_FonctionsDisponibles.Rows.Clear();
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_Fonctions", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int rowIndex = dgv_U_FonctionsDisponibles.Rows.Add();
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[0].Value = rdr[0];
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[1].Value = rdr[1];
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[2].Value = rdr[3];
            }
            con.Close();
        }
        private void ajouterLicence(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Vous êtes sur le point de créer la licence suivante : " + tb_U_LicenceID.Text + " " + tb_U_LicenceNom.Text + "\n\nconfirmez-vous cette action ?", "Création licence", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                #region Ajout de la licence
                //Definition de la procédure stockée
                SqlCommand cmd = new SqlCommand("Base_ILM.dbo.create_Licence", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@Nom", SqlDbType.VarChar).Value = tb_U_LicenceNom.Text;
                    //Prend la chaine, la coupe au niveau des points et prend la derneriere valeur de la chaine (le code de l'integrateur/client)
                    cmd.Parameters.Add("@Code_integrateur", SqlDbType.Int).Value = Convert.ToInt32(cb_U_LicenceClient.SelectedItem.ToString().Split('.')[cb_U_LicenceClient.SelectedItem.ToString().Split('.').Count() - 1]);
                    cmd.Parameters.Add("@Code_client", SqlDbType.Int).Value = Convert.ToInt32(cb_U_LicenceIntegrateur.SelectedItem.ToString().Split('.')[cb_U_LicenceIntegrateur.SelectedItem.ToString().Split('.').Count() - 1]);
                    cmd.Parameters.Add("@Date_creation", SqlDbType.DateTime).Value = dtp_U_LicenceDateCreation.Value.ToString("dd/MM/yyyy");
                    cmd.Parameters.Add("@Date_expiration", SqlDbType.DateTime).Value = dtp_U_LicenceDateExpiration.Value.ToString("dd/MM/yyyy");
                    cmd.Parameters.Add("@Nb_variables", SqlDbType.Int).Value = Convert.ToInt32(tb_U_LicenceNbVariables.Text);
                    cmd.Parameters.Add("@Nb_equipements", SqlDbType.Int).Value = Convert.ToInt32(tb_U_LicenceNbEquipements.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show("Erreur paramêtres licence : verifier que tous les paramêtres sont bien remplies. ⚠Fermeture du mode Ajout⚠");
                    con.Close();
                }
                #endregion

                #region Ajout des fonctions de la licence
                foreach (DataGridViewRow row in dgv_U_LicenceFonctions.Rows)
                {
                    SqlCommand cmd2 = new SqlCommand("Base_ILM.dbo.create_Liaison_Licence_Fonction", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        int Licence_ID = Convert.ToInt32(tb_U_LicenceID.Text);
                        int Code_fonction = Convert.ToInt32(row.Cells[0].Value);
                        cmd2.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = Licence_ID;
                        cmd2.Parameters.Add("@Code_fonction", SqlDbType.Int).Value = Code_fonction;
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        con.Close();

                    }
                    catch (Exception err)
                    {
                        con.Close();
                    }
                }
                #endregion

                fill_dgv_Licences();
                dgv_R_LicenceFonctions.Rows.Clear();
                dgv_U_FonctionsDisponibles.Rows.Clear();
                dgv_U_LicenceFonctions.Rows.Clear();

                adaptDataGridView();
                tc_Licences.SelectedIndex = 0;
                #region vider les champs
                tb_U_LicenceNom.Text = "";
                cb_U_LicenceIntegrateur.SelectedItem = null;
                cb_U_LicenceClient.SelectedItem = null;
                dtp_R_LicenceDateExpiration.Value = DateTime.Now;
                tb_U_LicenceNbEquipements.Text = "";
                tb_U_LicenceNbVariables.Text = "";
                cb_U_LicenceClient.Text = "";
                cb_U_LicenceIntegrateur.Text = "";
                #endregion
                #region montrer les champs
                btn_U_Licence.Visible = true;
                lb_V1.Visible = true;
                lb_V2.Visible = true;
                lb_V3.Visible = true;
                lb_V4.Visible = true;
                lb_V5.Visible = true;
                lb_V6.Visible = true;
                lb_V7.Visible = true;
                lb_V8.Visible = true;
                lb_V9.Visible = true;

                tb_R_LicenceID.Visible = true;
                tb_R_LicenceIntegrateur.Visible = true;
                tb_R_LicenceClient.Visible = true;
                tb_R_LicenceIntegrateur.Visible = true;
                tb_R_LicenceNbEquipements.Visible = true;
                tb_R_LicenceNbVariables.Visible = true;
                tb_R_LicenceNom.Visible = true;
                dtp_R_LicenceDateCreation.Visible = true;
                dtp_R_LicenceDateExpiration.Visible = true;

                btn_C_Licence.Visible = true;
                btn_U_Licence.Visible = true;
                #endregion
                btn_U_Licence.Visible = true;
            }
        }


        //Quand le mode Modifications est choisi
        private void goToModificationsMode(object sender, EventArgs e)
        {
            if (dgv_R_Licences.SelectedRows.Count > 0)
            {
                int Licence_ID = Convert.ToInt32(dgv_R_Licences.SelectedRows[0].Cells[0].Value);
                foreach(Licence licence in listeLicences)
                {
                    if (Licence_ID == licence.Licence_ID)
                    {
                        #region valeurs de la licence
                        tb_R_LicenceID.Text = licence.Licence_ID.ToString();
                        tb_R_LicenceNom.Text = licence.Nom.ToString();
                        tb_R_LicenceIntegrateur.Text = get_Licence_Integrateur(licence).Nom + "." + licence.Code_integrateur.ToString();
                        tb_R_LicenceClient.Text = get_Licence_Client(licence).Nom + "." + licence.Code_client.ToString();
                        dtp_R_LicenceDateCreation.Value = licence.Date_creation;
                        dtp_R_LicenceDateExpiration.Value = licence.Date_expiration;
                        tb_R_LicenceNbEquipements.Text = licence.Nb_equipements.ToString();
                        tb_R_LicenceNbVariables.Text = licence.Nb_variables.ToString();
                        #endregion

                        #region valeurs modifiables de la licence
                        tb_U_LicenceID.Text = licence.Licence_ID.ToString();
                        tb_U_LicenceNom.Text = licence.Nom.ToString();
                        foreach (string Code_client in cb_U_LicenceClient.Items)
                        {
                            if (licence.Code_client == Convert.ToInt32(Code_client.ToString().Split('.')[Code_client.ToString().Split('.').Count() - 1]))
                            {
                                cb_U_LicenceClient.SelectedItem = Code_client;
                            }
                        }
                        foreach (string Code_integrateur in cb_U_LicenceIntegrateur.Items)
                        {
                            if (licence.Code_integrateur == Convert.ToInt32(Code_integrateur.ToString().Split('.')[Code_integrateur.ToString().Split('.').Count() - 1]))
                            {
                                cb_U_LicenceIntegrateur.SelectedItem = Code_integrateur;
                            }
                        }
                        dtp_U_LicenceDateCreation.Value = licence.Date_creation;
                        dtp_U_LicenceDateExpiration.Value = licence.Date_expiration;
                        tb_U_LicenceNbEquipements.Text = licence.Nb_equipements.ToString();
                        tb_U_LicenceNbVariables.Text = licence.Nb_variables.ToString();
                        #endregion
                    }
                }
                btn_C_Licence.Visible = false;
                fill_dgv_LicenceNoFonctions();
                fill_dgv_LicenceFonctions();
                adaptDataGridView();
                tc_Licences.SelectedIndex = 1;
            }
        }
        private void fill_dgv_LicenceNoFonctions()
        {
            dgv_U_FonctionsDisponibles.Rows.Clear();
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_LicenceNoFonctions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = tb_R_LicenceID.Text.ToString();

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int rowIndex = dgv_U_FonctionsDisponibles.Rows.Add();
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[0].Value = rdr[0];
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[1].Value = rdr[1];
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[2].Value = rdr[3];
            }
            con.Close();
        }
        private void fill_dgv_LicenceFonctions()
        {
            dgv_U_LicenceFonctions.Rows.Clear();
            //Definition de la procédure stockée
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.get_LicenceFonctions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = tb_R_LicenceID.Text.ToString();

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int rowIndex = dgv_U_LicenceFonctions.Rows.Add();
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[0].Value = rdr[0];
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[1].Value = rdr[1];
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[2].Value = rdr[3];
            }
            con.Close();
        }
        private void updateLicence(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Vous êtes sur le point de modifier la licence suivante : " + tb_R_LicenceID.Text + " " + tb_R_LicenceNom.Text + "\n\nconfirmez-vous cette action ?", "Modifications licence", MessageBoxButtons.YesNo);
            bool updatePassed = false;

            #region modification de la licence
            SqlCommand cmd = new SqlCommand("Base_ILM.dbo.update_Licence", con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = Convert.ToInt32(tb_R_LicenceID.Text);
                cmd.Parameters.Add("@Nom", SqlDbType.VarChar).Value = tb_U_LicenceNom.Text;
                cmd.Parameters.Add("@Code_integrateur", SqlDbType.Int).Value = Convert.ToInt32(cb_U_LicenceClient.SelectedItem.ToString().Split('.')[cb_U_LicenceClient.SelectedItem.ToString().Split('.').Count() - 1]);
                cmd.Parameters.Add("@Code_client", SqlDbType.Int).Value = Convert.ToInt32(cb_U_LicenceIntegrateur.SelectedItem.ToString().Split('.')[cb_U_LicenceIntegrateur.SelectedItem.ToString().Split('.').Count() - 1]);
                cmd.Parameters.Add("@Date_expiration", SqlDbType.DateTime).Value = dtp_U_LicenceDateExpiration.Value.ToString("dd/MM/yyyy");
                cmd.Parameters.Add("@Nb_variables", SqlDbType.Int).Value = Convert.ToInt32(tb_U_LicenceNbVariables.Text);
                cmd.Parameters.Add("@Nb_equipements", SqlDbType.Int).Value = Convert.ToInt32(tb_U_LicenceNbEquipements.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                updatePassed = true;
            }
            catch (Exception err)
            {
                MessageBox.Show("Erreur paramêtres licence : verifier que tous les paramêtres sont bien remplies. ⚠Fermeture du mode Modifications⚠");
                con.Close();
            }
            #endregion

            if (updatePassed == true)
            {
                #region suppression des anciennes fonction de la licence
                SqlCommand cmd2 = new SqlCommand("Base_ILM.dbo.delete_LicenceFonctions", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = Convert.ToInt32(tb_R_LicenceID.Text);
                con.Open();
                cmd2.ExecuteNonQuery();
                con.Close();
                #endregion

                #region Ajout des fonctions de la licence
                foreach (DataGridViewRow row in dgv_U_LicenceFonctions.Rows)
                {
                    SqlCommand cmd3 = new SqlCommand("Base_ILM.dbo.create_Liaison_Licence_Fonction", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        int Licence_ID = Convert.ToInt32(tb_U_LicenceID.Text);
                        int Code_fonction = Convert.ToInt32(row.Cells[0].Value);
                        cmd3.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = Licence_ID;
                        cmd3.Parameters.Add("@Code_fonction", SqlDbType.Int).Value = Code_fonction;
                        con.Open();
                        cmd3.ExecuteNonQuery();
                        con.Close();

                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Erreur création fonction pour la licence : " + err.Message + " ⚠Fermeture du mode Ajout⚠");
                        con.Close();
                    }
                }
                #endregion
            }

            fill_dgv_Licences();
            dgv_R_LicenceFonctions.Rows.Clear();
            dgv_U_FonctionsDisponibles.Rows.Clear();
            dgv_U_LicenceFonctions.Rows.Clear();

            adaptDataGridView();
            tc_Licences.SelectedIndex = 0;
            btn_C_Licence.Visible = true;
            #region vider les champs
            tb_U_LicenceNom.Text = "";
            cb_U_LicenceIntegrateur.SelectedItem = null;
            cb_U_LicenceClient.SelectedItem = null;
            dtp_R_LicenceDateExpiration.Value = DateTime.Now;
            tb_U_LicenceNbEquipements.Text = "";
            tb_U_LicenceNbVariables.Text = "";
            cb_U_LicenceClient.Text = "";
            cb_U_LicenceIntegrateur.Text = "";
            #endregion
        }



        //Boutons de selection des fonctions
        private void selectFonction(object sender, EventArgs e)
        {
            if (dgv_U_FonctionsDisponibles.SelectedRows.Count > 0)
            {
                //la fonction choisi
                DataGridViewRow row = (DataGridViewRow)dgv_U_FonctionsDisponibles.SelectedRows[0];
                //la fonction dans le nouveau datagridview
                int rowIndex = dgv_U_LicenceFonctions.Rows.Add();
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[0].Value = row.Cells[0].Value;
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[1].Value = row.Cells[1].Value;
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[2].Value = row.Cells[2].Value;
                //supprimer la fonction dans l'ancien datagridview
                dgv_U_FonctionsDisponibles.Rows.RemoveAt(row.Index);
            }
        }
        private void deselectFonction(object sender, EventArgs e)
        {
            if (dgv_U_LicenceFonctions.SelectedRows.Count > 0)
            {
                //la fonction choisi
                DataGridViewRow row = (DataGridViewRow)dgv_U_LicenceFonctions.SelectedRows[0];
                //la fonction dans le nouveau datagridview
                int rowIndex = dgv_U_FonctionsDisponibles.Rows.Add();
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[0].Value = row.Cells[0].Value;
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[1].Value = row.Cells[1].Value;
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[2].Value = row.Cells[2].Value;
                //supprimer la fonction dans l'ancien datagridview
                dgv_U_LicenceFonctions.Rows.RemoveAt(row.Index);
            }
        }
        private void selectAllFonction(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_U_FonctionsDisponibles.Rows)
            {
                int rowIndex = dgv_U_LicenceFonctions.Rows.Add();
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[0].Value = row.Cells[0].Value;
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[1].Value = row.Cells[1].Value;
                dgv_U_LicenceFonctions.Rows[rowIndex].Cells[2].Value = row.Cells[2].Value;
            }
            dgv_U_FonctionsDisponibles.Rows.Clear();
        }
        private void deselectAllFonction(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_U_LicenceFonctions.Rows)
            {
                int rowIndex = dgv_U_FonctionsDisponibles.Rows.Add();
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[0].Value = row.Cells[0].Value;
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[1].Value = row.Cells[1].Value;
                dgv_U_FonctionsDisponibles.Rows[rowIndex].Cells[2].Value = row.Cells[2].Value;
            }
            dgv_U_LicenceFonctions.Rows.Clear();
        }


        //appuie sur le bouton retour
        private void retour(object sender, EventArgs e) 
        {
            #region montrer les champs
            btn_U_Licence.Visible = true;
            lb_V1.Visible = true;
            lb_V2.Visible = true;
            lb_V3.Visible = true;
            lb_V4.Visible = true;
            lb_V5.Visible = true;
            lb_V6.Visible = true;
            lb_V7.Visible = true;
            lb_V8.Visible = true;
            lb_V9.Visible = true;

            tb_R_LicenceID.Visible = true;
            tb_R_LicenceIntegrateur.Visible = true;
            tb_R_LicenceClient.Visible = true;
            tb_R_LicenceIntegrateur.Visible = true;
            tb_R_LicenceNbEquipements.Visible = true;
            tb_R_LicenceNbVariables.Visible = true;
            tb_R_LicenceNom.Visible = true;
            dtp_R_LicenceDateCreation.Visible = true;
            dtp_R_LicenceDateExpiration.Visible = true;

            btn_C_Licence.Visible = true;
            btn_U_Licence.Visible = true;
            #endregion

            #region vider les champs
            tb_U_LicenceNom.Text = "";
            cb_U_LicenceIntegrateur.SelectedItem = null;
            cb_U_LicenceClient.SelectedItem = null;
            dtp_R_LicenceDateExpiration.Value = DateTime.Now;
            tb_U_LicenceNbEquipements.Text = "";
            tb_U_LicenceNbVariables.Text = "";
            cb_U_LicenceClient.Text = "";
            cb_U_LicenceIntegrateur.Text = "";
            #endregion

            fill_dgv_Licences();
            dgv_R_LicenceFonctions.Rows.Clear();
            dgv_U_FonctionsDisponibles.Rows.Clear();
            dgv_U_LicenceFonctions.Rows.Clear();

            adaptDataGridView();
            tc_Licences.SelectedIndex = 0;
        }


        //Appuie sur le bouton supprimer
        private void delete_Licence(object sender, EventArgs e)
        {
            if (dgv_R_Licences.SelectedRows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Vous êtes sur le point de supprimer la licence suivante : " + dgv_R_Licences.SelectedRows[0].Cells[0].Value.ToString() + " " + dgv_R_Licences.SelectedRows[0].Cells[1].Value.ToString() + "\n\nconfirmez-vous cette action ?", "Suppression licence", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("Base_ILM.dbo.delete_Licence", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Licence_ID", SqlDbType.Int).Value = Convert.ToInt32(dgv_R_Licences.SelectedRows[0].Cells[0].Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    fill_dgv_Licences();
                    dgv_R_LicenceFonctions.Rows.Clear();
                }
            }
        }


        //Appuie sur le bouton annuler modifications
        private void annuler_Modifications(object sender, EventArgs e)
        {
            if (btn_C_Licence.Visible == true)
            {
                DialogResult dr = MessageBox.Show("Vous êtes sur le point d'annuler les modifications \nconfirmez-vous cette action ?", "Annuler modifications", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    fill_dgv_FonctionsDipsonibles();
                    dgv_U_LicenceFonctions.Rows.Clear();
                    adaptDataGridView();

                    tb_U_LicenceNom.Text = "";
                    cb_U_LicenceIntegrateur.SelectedItem = null;
                    cb_U_LicenceClient.SelectedItem = null;
                    dtp_R_LicenceDateExpiration.Value = DateTime.Now;
                    tb_U_LicenceNbEquipements.Text = "";
                    tb_U_LicenceNbVariables.Text = "";
                }
            }

            if (btn_U_Licence.Visible == true)
            {
                DialogResult dr = MessageBox.Show("Vous êtes sur le point d'annuler les modifications \nconfirmez-vous cette action ?", "Annuler modifications", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    int Licence_ID = Convert.ToInt32(tb_R_LicenceID.Text);
                    foreach (Licence licence in listeLicences)
                    {
                        if (Licence_ID == licence.Licence_ID)
                        {
                            #region valeurs de la licence
                            tb_R_LicenceID.Text = licence.Licence_ID.ToString();
                            tb_R_LicenceNom.Text = licence.Nom.ToString();
                            tb_R_LicenceIntegrateur.Text = licence.Code_client.ToString() + "." + get_Licence_Client(licence).Nom;
                            tb_R_LicenceClient.Text = licence.Code_integrateur.ToString() + "." + get_Licence_Integrateur(licence).Nom;
                            dtp_R_LicenceDateCreation.Value = licence.Date_creation;
                            dtp_R_LicenceDateExpiration.Value = licence.Date_expiration;
                            tb_R_LicenceNbEquipements.Text = licence.Nb_equipements.ToString();
                            tb_R_LicenceNbVariables.Text = licence.Nb_variables.ToString();
                            #endregion

                            #region valeurs modifiables de la licence
                            tb_U_LicenceID.Text = licence.Licence_ID.ToString();
                            tb_U_LicenceNom.Text = licence.Nom.ToString();
                            foreach (string Code_client in cb_U_LicenceIntegrateur.Items)
                            {
                                if (licence.Code_client == Convert.ToInt32(Code_client.ToString().Split('.')[Code_client.ToString().Split('.').Count() - 1]))
                                {
                                    cb_U_LicenceIntegrateur.SelectedItem = Code_client;
                                }
                            }
                            foreach (string Code_integrateur in cb_U_LicenceClient.Items)
                            {
                                if (licence.Code_integrateur == Convert.ToInt32(Code_integrateur.ToString().Split('.')[Code_integrateur.ToString().Split('.').Count() - 1]))
                                {
                                    cb_U_LicenceClient.SelectedItem = Code_integrateur;
                                }
                            }
                            dtp_U_LicenceDateCreation.Value = licence.Date_creation;
                            dtp_U_LicenceDateExpiration.Value = licence.Date_expiration;
                            tb_U_LicenceNbEquipements.Text = licence.Nb_equipements.ToString();
                            tb_U_LicenceNbVariables.Text = licence.Nb_variables.ToString();
                            #endregion
                        }
                    }
                }
                fill_dgv_LicenceNoFonctions();
                fill_dgv_LicenceFonctions();
                adaptDataGridView();
            }
        }

        
        //Appuie sur le bouton de Génération de la licence
        private void generateLicence(object sender, EventArgs e)
        {
            if (dgv_R_Licences.SelectedRows.Count > 0)
            {
                string LicenceString = "";
                //Informations de la licence
                LicenceString += dgv_R_Licences.SelectedRows[0].Cells[0].Value.ToString();
                LicenceString += "|" + dgv_R_Licences.SelectedRows[0].Cells[1].Value.ToString();
                LicenceString += "|" + dgv_R_Licences.SelectedRows[0].Cells[2].Value.ToString();
                LicenceString += "|" + dgv_R_Licences.SelectedRows[0].Cells[3].Value.ToString();
                LicenceString += "|" + Convert.ToDateTime(dgv_R_Licences.SelectedRows[0].Cells[5].Value).ToString("dd/MM/yyyy");
                LicenceString += "|" + dgv_R_Licences.SelectedRows[0].Cells[6].Value.ToString();
                LicenceString += "|" + dgv_R_Licences.SelectedRows[0].Cells[7].Value.ToString();
                //10 emplacements de reserve pour de futurs informations
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                LicenceString += "|" + "null";
                //Fonctions de la licence
                LicenceString += "|" + dgv_R_LicenceFonctions.Rows.Count.ToString();
                foreach (DataGridViewRow row in dgv_R_LicenceFonctions.Rows)
                {
                    LicenceString += "|" + row.Cells[0].Value.ToString();
                }

                string key = "";
                using (Aes aesAlgorithm = Aes.Create())
                {
                    aesAlgorithm.KeySize = 128;
                    aesAlgorithm.GenerateKey();
                    string keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
                    key = keyBase64;
                }

                Crypteur crypteur = new Crypteur();
                String encryptedString = crypteur.EncryptString(key, LicenceString);

                selectLicenceFolder.ShowDialog();
                String path = selectLicenceFolder.SelectedPath;
                String name = dgv_R_Licences.SelectedRows[0].Cells[1].Value.ToString();
                String fileName = path + "/" + name + ".IconeLic";

                if (File.Exists(fileName))
                {
                    MessageBox.Show("Nom de fichier déja utilisé");
                    return;
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(encryptedString);
                    sw.WriteLine(key);
                }

                //pour débugger (voir si la chaine reste la même apres le décryptage
                //MessageBox.Show(crypteur.DecryptString(key, encryptedString));

                MessageBox.Show("Licence généré avec succé");
            }
        }


        //permet de récuperer le client/integrateur a partir de la licence
        public Client get_Licence_Client(Licence licence)
        {
            foreach (Client client in listeClients)
            {
                if (client.Code_client == licence.Code_client)
                {
                    return client;
                }
            }

            return null;
        }
        public Integrateur get_Licence_Integrateur(Licence licence)
        {
            foreach (Integrateur integrateur in listeIntegrateurs)
            {
                if (integrateur.Code_integrateur == licence.Code_integrateur)
                {
                    return integrateur;
                }
            }

            return null;
        }

        //Style datagridviews
        private void adaptDataGridView()
        {
            #region reset des style
            dgv_R_Licences.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Licences.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgv_R_LicenceFonctions.DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            dgv_R_LicenceFonctions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_LicenceFonctions.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_R_LicenceFonctions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_LicenceFonctions.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_R_LicenceFonctions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgv_U_FonctionsDisponibles.DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            dgv_U_FonctionsDisponibles.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_U_FonctionsDisponibles.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_U_FonctionsDisponibles.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_U_FonctionsDisponibles.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_U_FonctionsDisponibles.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgv_U_LicenceFonctions.DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            dgv_U_LicenceFonctions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_U_LicenceFonctions.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_U_LicenceFonctions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_U_LicenceFonctions.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_U_LicenceFonctions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgv_R_Integrateurs.DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            dgv_R_Integrateurs.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Integrateurs.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_R_Integrateurs.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_R_Integrateurs.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;
            dgv_R_Integrateurs.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            #endregion

            #region Stylisation des datagridviews
            dgv_R_Licences.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_R_Licences.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv_R_LicenceFonctions.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgv_R_LicenceFonctions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_R_LicenceFonctions.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_R_LicenceFonctions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_R_LicenceFonctions.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_R_LicenceFonctions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv_U_FonctionsDisponibles.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            dgv_U_FonctionsDisponibles.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_U_FonctionsDisponibles.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_U_FonctionsDisponibles.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_U_FonctionsDisponibles.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_U_FonctionsDisponibles.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv_U_LicenceFonctions.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgv_U_LicenceFonctions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_U_LicenceFonctions.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_U_LicenceFonctions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_U_LicenceFonctions.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_U_LicenceFonctions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv_R_Integrateurs.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgv_R_Integrateurs.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_R_Integrateurs.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_R_Integrateurs.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_R_Integrateurs.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_R_Integrateurs.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            #endregion
        }
    }
}
