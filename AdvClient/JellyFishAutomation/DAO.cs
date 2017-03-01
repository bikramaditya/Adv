using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HelliumClient
{
    public class DAO
    {
        static SqlConnection conn;

        internal void createConnection()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["CRMPerfDB"].ConnectionString;
            String dbPw = ConfigurationManager.AppSettings["dbPassword"];
            
            String dec_pw = Crypto.Decrypt(dbPw, "crmdynamics");
            Console.WriteLine(dec_pw);

            connectionString = connectionString + dec_pw;

            conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                Console.WriteLine("Connection Open ! ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! "+ex.Message);
            }
        }
        internal ObservableCollection<CRMFeature> getAllFeatures()
        {
            String sql = "SELECT ID,FEATURE_NAME FROM [dbo].[FEATURE_ID_NAME_MAP]";

            ObservableCollection<CRMFeature> all_features = new ObservableCollection<CRMFeature>();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                //conn.Open();
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                CRMFeature tc = new CRMFeature();
                                // To avoid unexpected bugs access columns by name.
                                tc.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                                tc.Name = reader.GetString(reader.GetOrdinal("FEATURE_NAME"));

                                all_features.Add(tc);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return all_features;
        }

        internal void parseAndStore(string copiedText)
        {
            String[] lines = copiedText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (String line in lines)
            {
                String tempLine = line.Replace("->", "");
                tempLine = tempLine.Replace("ms", String.Empty);
                tempLine = tempLine.Replace(")", String.Empty);
                
                String[] values = tempLine.Split('(');
                if(values.Length==2)
                { 
                    String markerName = values[0].Trim();
                    String markerValue = values[1].Trim();

                    Console.WriteLine("marker=" + markerName + " --> value=" + markerValue);
                }
            }
        }

        internal ObservableCollection<TestCase> getAll_TestCases(CRMFeature selected_feature)
        {
            String sql = "SELECT [ID] , [TESTCASE_NAME], [DEFAULT_ITR] FROM [dbo].[TEST_CASE_FEATURE_MAPPING] where FEATURE_ID=" + selected_feature.ID;

            ObservableCollection<TestCase> test_cases = new ObservableCollection<TestCase>();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                //conn.Open();
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            int i = 1;
                            while (reader.Read())
                            {
                                TestCase tc = new TestCase();
                                tc.Enabled = false;
                                tc.Seq = i++;
                                tc.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                                tc.TestCaseName = reader.GetString(reader.GetOrdinal("TESTCASE_NAME"));
                                tc.Itr = reader.GetInt32(reader.GetOrdinal("DEFAULT_ITR")); 
                                test_cases.Add(tc);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            foreach(TestCase tc in test_cases)
            {
                tc.Markers = getConcatMarkers(tc.ID);
            }
            return test_cases;
        }

        private string getConcatMarkers(int iD)
        {
            String sql = "SELECT distinct(marker_name) as marker_name FROM [dbo].[FEATURE_ID_MARKER_NAME_MAP] where testcase_id=" + iD;

            String markers = "";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                //conn.Open();
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {                                
                                markers += reader.GetString(reader.GetOrdinal("marker_name"))+", ";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return markers;
        }
    }
}