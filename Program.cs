using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace TestXDocument
{
    class Program
    {
        static void Main(string[] args)
        {
            string ChaineConnexion1 = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=GESTION_HOTEL;User ID=sa;Password=1234";
            string reqContenuTable = "SELECT * FROM ";
            string reqContenuTableTemp;
            string reqChampsTables = "SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME;";

            SqlConnection oconnexion1 = new SqlConnection(ChaineConnexion1);
            {
                SqlCommand Tables = new SqlCommand(reqChampsTables, oconnexion1);

                oconnexion1.Open();

                SqlDataReader readerTables = Tables.ExecuteReader();
                {

                    List<String[]> ComparaisonChampsTables = new List<String[]>();
                    List<String> NomChamps = new List<String>();
                    while (readerTables.Read())
                    {
                        String[] ContenuTables = new String[2];
                        ContenuTables[0] = readerTables[0].ToString();
                        ContenuTables[1] = readerTables[1].ToString();
                        ComparaisonChampsTables.Add(ContenuTables);
                    }


                    bool continuer = true;
                    int index = 0;

                    while (continuer == true)
                    {
                        if (ComparaisonChampsTables.ElementAt(index + 1)[0] == ComparaisonChampsTables.ElementAt(index)[0] && continuer == true)
                        {
                            NomChamps.Add(ComparaisonChampsTables.ElementAt(index)[1]);
                            index++;

                            if (index == ComparaisonChampsTables.Count - 1)
                            {
                                reqContenuTableTemp = reqContenuTable + ComparaisonChampsTables.ElementAt(index)[0];
                                oconnexion1.Close();
                                SqlCommand TestTables = new SqlCommand(reqContenuTableTemp, oconnexion1);
                                oconnexion1.Open();
                                SqlDataReader readerTest = TestTables.ExecuteReader();

                                XElement element = new XElement(ComparaisonChampsTables.ElementAt(index)[0]);
                                while (readerTest.Read())
                                {
                                    for (int i = 0; i < NomChamps.Count - 1; i++)
                                    {
                                        if (i != NomChamps.Count)
                                        {
                                            element.Add(new XElement(NomChamps[i].ToString(), readerTest[i].ToString()));
                                        }
                                    }
                                }
                                element.Save(ComparaisonChampsTables.ElementAt(index)[0] + ".xml");

                                return;
                            }
                        }
                        else
                        {
                            reqContenuTableTemp = reqContenuTable + ComparaisonChampsTables.ElementAt(index)[0];
                            oconnexion1.Close();
                            SqlCommand TestTables = new SqlCommand(reqContenuTableTemp, oconnexion1);
                            oconnexion1.Open();
                            SqlDataReader readerTest = TestTables.ExecuteReader();

                            XElement element = new XElement(ComparaisonChampsTables.ElementAt(index)[0]);
                            while (readerTest.Read())
                            {
                                for (int i = 0; i < NomChamps.Count - 1; i++)
                                {
                                    if (i != NomChamps.Count)
                                    {
                                        element.Add(new XElement(NomChamps[i].ToString(), readerTest[i].ToString()));
                                    }
                                }
                            }
                            element.Save(ComparaisonChampsTables.ElementAt(index)[0] + ".xml");
                            reqContenuTableTemp = reqContenuTable;
                            index++;
                            NomChamps.Clear();
                        }
                    }
                }
            }
        }
    }
}