﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceReservasi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        string constring = "Data Source=DESKTOP-07IPCUO;Initial Catalog=WCFReservasi;User id=sa;Password=erwinS12";
        SqlConnection connection;
        SqlCommand com;

        public string Login(string username, string password)
        {
            string kategori = " ";

            string sql = "select Kategori from dbo.Login where Username = '" + username + "' and Password = '" + password + "'";
            connection = new SqlConnection(constring);
            com = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                kategori = reader.GetString(0);
            }
            return kategori;
        }

        public string Register(string username, string password, string kategori)
        {
            try
            {
                string sql = "insert into Login values ('" + username + "' , '" + password + "' , '" + kategori + "')";
                connection = new SqlConnection(constring); //fungsi konek ke db
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                return "sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string UpdateRegister(string username, string password, string kategori, int id)
        {
            try
            {
                string sql2 = "update Login SET Username = '" + username + "', Password = '" + password + "', Kategori = '" + kategori + "' where ID_login = " + id + "";
                connection = new SqlConnection(constring); //fungsi koneksi ke db
                com = new SqlCommand(sql2, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                return "sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string DeleteRegister(string username)
        {
            try
            {
                int id = 0;
                string sql = "select ID_login from dbo.Login where Username = '" + username + "'";
                connection = new SqlConnection(constring); //fungsi konek ke db
                com = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                connection.Close();
                string sql2 = "delete from dbo.Pemesanan where ID_login =" + id + "";
                connection = new SqlConnection(constring); //fungsi konek ke db
                com = new SqlCommand(sql2, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                return "sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<DataRegister> DataRegist()
        {
            List<DataRegister> list = new List<DataRegister>(); 
            try
            {
                string sql = "select ID_login, Username, Password, Kategori from Login";
                connection = new SqlConnection(constring); 
                com = new SqlCommand(sql, connection); 
                connection.Open(); 
                SqlDataReader reader = com.ExecuteReader(); 
                while (reader.Read())
                {
                    DataRegister data = new DataRegister(); 
                    data.id = reader.GetInt32(0); 
                    data.username = reader.GetString(1);
                    data.password = reader.GetString(2);
                    data.kategori = reader.GetString(3);
                    list.Add(data); 
                }
                connection.Close(); 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }

        public string deletePemesanan(string IDPemesanan)
        {
            string a = "gagal";
            try
            {
                string sql = "delete from dbo.Pemesanan where ID_Reservasi = '" + IDPemesanan + "'";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();
                a = "sukses";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return a;
        }

        public List<DetailLokasi> DetailLokasi()
        {
            List<DetailLokasi> LokasiFull = new List<DetailLokasi>(); //proces untuk mendeclare nama list yg telah dibuar dengan nama baru
            try
            {
                string sql = "select ID_lokasi, Nama_lokasi, Deskripsi_full, Kuota from dbo.Lokasi"; // declare query
                connection = new SqlConnection(constring); //fungsi konek ke databse
                com = new SqlCommand(sql, connection); //proses execute query
                connection.Open(); //membuka koneksi
                SqlDataReader reader = com.ExecuteReader(); //menampilkan data query
                while (reader.Read())
                {
                    //nama class
                    DetailLokasi data = new DetailLokasi(); //deklarasi data, mengambil persatu dari database
                    //bentuk array
                    data.IDLokasi = reader.GetString(0); 
                    data.NamaLokasi = reader.GetString(1);
                    data.DeskripsiFull = reader.GetString(2);
                    data.Kuota = reader.GetInt32(3);
                    LokasiFull.Add(data); //mengumpulkan data yang awalnya dari array
                }
                connection.Close(); // untuk menutup akses ke database
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return LokasiFull;
        }

        public string editPemesanan(string IDPemesanan, string NamaCustomer, string NoTelp)
        {
            string a = "gagal";
            try
            {
                string sql = "update dbo.Pemesanan set Nama_Customer = '" + NamaCustomer + "', NoTelp = '" + NoTelp + "'" +
                    " where ID_Reservasi = '" + IDPemesanan + "'";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                a = "sukses";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return a;
        }

        public string pemesanan(string IDPemesanan, string NamaCustomer, string NoTelepon, int JumlahPemesanan, string IDLokasi)
        {
            string a = "gagal";
            try
            {
                string sql = "insert into dbo.Pemesanan values ('" + IDPemesanan + "','" + NamaCustomer + "','" + NoTelepon + "','" + JumlahPemesanan + "','" + IDLokasi + "')";
                connection = new SqlConnection(constring); //fungsi konek ke database
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                string sql2 = "update dbo.Lokasi set Kuota = Kuota - " + JumlahPemesanan + " where ID_Lokasi = '" + IDLokasi + "'";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql2, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                a = "sukses";
            }
            catch (Exception es)
            {
                Console.WriteLine(es);
            }
            return a;
        }

        public List<Pemesanan> Pemesanan()
        {
            List<Pemesanan> pemesanans = new List<Pemesanan>();
            try
            {
                string sql = "select ID_Reservasi, Nama_Customer, No_Telp, " +
                    "Jumlah_Pemesanan, Nama_Lokasi from dbo.Pemesanan p join dbo.Lokasi l on p.ID_Lokasi = l.ID_Lokasi";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Pemesanan data = new Pemesanan();
                    data.IDPemesanan = reader.GetString(0);
                    data.NamaCustomer = reader.GetString(1);
                    data.NoTelepon = reader.GetString(2);
                    data.JumlahPemesanan = reader.GetInt32(3);
                    data.Lokasi = reader.GetString(4);
                    pemesanans.Add(data);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return pemesanans;
        }

        public List<CekLokasi> ReviewLokasi()
        {
            throw new NotImplementedException();
        }
    }
}
