using MarineWizSupporter.DataSupporter;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;

namespace SampleData
{
    /// <summary>
    /// 데이터 접근 클래스
    /// 테스트용 서버 MySQL 및 랜덤 데이터 반환
    /// </summary>
    public static class DataAccessManager
    {
        /// <summary>
        /// 바인딩 데이터그리드
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetWholeData()
        {
            DataTable table;
            if (DatabaseHelper.Instanace.ConfigureDB("encho.duckdns.org", "totoz", "qwer1234", "toztoz"))
            {
                table = new DataTable();
                using (var conn = DatabaseHelper.Instanace.CreateConnection())
                using (var command = new MySqlCommand("SELECT * FROM dummy2;", conn))
                using (var reader = command.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        table.Columns.Add(reader.GetName(i));
                    }

                    while (reader.Read())
                    {
                        table.Rows.Add(new string[]
                        {
                            reader[0].ToString(),
                            reader[1].ToString(),
                            reader[2].ToString(),
                            reader[3].ToString(),
                            reader[4].ToString()
                        });
                    }
                }
                return table;
            }

            return null;
        }

        /// <summary>
        /// 바인딩 List(double 타입)
        /// </summary>
        /// <returns>List&lt;double&gt;</returns>
        public static List<double> GetDataFrom()
        {
            var list = new List<double>();
            if (DatabaseHelper.Instanace.ConfigureDB("encho.duckdns.org", "totoz", "qwer1234", "toztoz"))
            {
                using (var conn = DatabaseHelper.Instanace.CreateConnection())
                using (var command = new MySqlCommand("SELECT " + "height" + " FROM dummy2;", conn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Convert.ToDouble(reader[0]));
                }
            }

            return list;
        }

        /// <summary>
        /// 바인딩 double
        /// </summary>
        /// <returns>double</returns>
        public static double GetDepth()
        {
            double ret = double.NaN;

            if (DatabaseHelper.Instanace.ConfigureDB("encho.duckdns.org", "totoz", "qwer1234", "toztoz"))
            {
                string query =
                    "SELECT depth " +
                    "FROM dummy3 " +
                    "WHERE date IN(SELECT date FROM dummy3 WHERE date = (SELECT MAX(date) FROM dummy3)) " +
                    "ORDER BY date DESC " +
                    "LIMIT 1;";
                using (var conn = DatabaseHelper.Instanace.CreateConnection())
                using (var command = new MySqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        ret = Convert.ToDouble(reader[0]);
                }
            }

            return ret;
        }

        /// <summary>
        /// 바인딩 double
        /// </summary>
        /// <returns>double</returns>
        public static double GetWidth()
        {
            double ret = double.NaN;

            if (DatabaseHelper.Instanace.ConfigureDB("encho.duckdns.org", "totoz", "qwer1234", "toztoz"))
            {
                string query =
                    "SELECT width " +
                    "FROM dummy3 " +
                    "WHERE date IN(SELECT date FROM dummy3 WHERE date = (SELECT MAX(date) FROM dummy3)) " +
                    "ORDER BY date DESC " +
                    "LIMIT 1;";
                using (var conn = DatabaseHelper.Instanace.CreateConnection())
                using (var command = new MySqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        ret = Convert.ToDouble(reader[0]);
                }
            }

            return ret;
        }

        /// <summary>
        /// 바인딩 double
        /// </summary>
        /// <returns>double</returns>
        public static double GetHeight()
        {
            double ret = double.NaN;

            if (DatabaseHelper.Instanace.ConfigureDB("encho.duckdns.org", "totoz", "qwer1234", "toztoz"))
            {
                string query =
                    "SELECT height " +
                    "FROM dummy4 " +
                    "WHERE date IN(SELECT date FROM dummy4 WHERE date = (SELECT MAX(date) FROM dummy4)) " +
                    "ORDER BY date DESC " +
                    "LIMIT 1;";
                using (var conn = DatabaseHelper.Instanace.CreateConnection())
                using (var command = new MySqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        ret = Convert.ToDouble(reader[0]);
                }
            }

            return ret;
        }

        /// <summary>
        /// 바인딩용 Marker 리스트
        /// MarineWizSupporter.DataSupporter.MarkerData
        /// </summary>
        /// <param name="table">생략 가능, Location2 입력 시 다른 데이터 반환됨</param>
        /// <returns>IEnumerable&lt;MarkerData&gt;</returns>
        public static IEnumerable<MarkerData> GetLatAndLong(string table = "Location")
        {
            if (DatabaseHelper.Instanace.ConfigureDB("encho.duckdns.org", "totoz", "qwer1234", "toztoz"))
            {
                string query =
                    "SELECT lat, lon " +
                    "FROM " + table + ";";
                using (var conn = DatabaseHelper.Instanace.CreateConnection())
                using (var command = new MySqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        yield return new MarkerData(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1]));
                }
            }
        }

        /// <summary>
        /// 울산 시내 GPS 데이터
        /// </summary>
        /// <returns>List&lt;MarkerData&gt;(MarineWizSupporter.DataSupporter.MarkerData)</returns>
        public static IEnumerable<MarkerData> GetUlsanDowntownGPSData() => GetLatAndLong();

        /// <summary>
        /// 현대중공업 사내 GPS 데이터
        /// </summary>
        /// <returns>List&lt;MarkerData&gt;(MarineWizSupporter.DataSupporter.MarkerData)</returns>
        public static IEnumerable<MarkerData> GetHyundaiIndustryGPSData() => GetLatAndLong("Location2");
        
        /// <summary>
        /// 4000 ~ 6000 사이의 랜덤 double 값
        /// </summary>
        /// <returns>double</returns>
        public static double GetData_RPM()
            => GetDataFromURL("https://encho.duckdns.org:7890/?min=4000&max=6000");

        /// <summary>
        /// 50 ~ 80 사이의 랜덤 double 값
        /// </summary>
        /// <returns>double</returns>
        public static double GetData_Speed()
            => GetDataFromURL("https://encho.duckdns.org:7890/?min=50&max=80");

        /// <summary>
        /// 800 ~ 1200 사이의 랜덤 double 값
        /// </summary>
        /// <returns>double</returns>
        public static double GetData_Depth()
            => GetDataFromURL("https://encho.duckdns.org:7890/?min=800&max=1200");

        /// <summary>
        /// 400 ~ 500 사이의 랜덤 double 값
        /// </summary>
        /// <returns>double</returns>
        public static double GetData_Power()
            => GetDataFromURL("https://encho.duckdns.org:7890/?min=400&max=500");

        /// <summary>
        /// 100 ~ 120 사이의 랜덤 double 값
        /// </summary>
        /// <returns>double</returns>
        public static double GetData_Wind()
            => GetDataFromURL("https://encho.duckdns.org:7890/?min=100&max=120");

        /// <summary>
        /// 40 ~ 60 사이의 랜덤 double 값
        /// </summary>
        /// <returns>double</returns>
        public static double GetData_Heading()
            => GetDataFromURL("https://encho.duckdns.org:7890/?min=40&max=60");

        /// <summary>
        /// 랜덤 데이터 추출용 함수
        /// </summary>
        /// <param name="url">값 반환 서버 주소 URL(https://encho.duckdns.org:7890/?min={최소값}&amp;max={최대값})</param>
        /// <returns>double</returns>
        private static double GetDataFromURL(string url)
        {
            var resText = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 30000;

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                var status = resp.StatusCode;

                var resStream = resp.GetResponseStream();
                using (var sr = new StreamReader(resStream))
                    resText = sr.ReadToEnd();
            }

            return Convert.ToDouble(resText);
        }
    }
}