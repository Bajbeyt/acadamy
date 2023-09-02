using System.Data.SqlClient;


internal class Program
{
    private static SqlConnection connection;

    private static string connectionString = "Server=localhost; Database=Ogrenci_BilgiDb; MultipleActiveResultSets=true; TrustServerCertificate=true;";

    private static void Main(string[] args)
    {
        Console.WriteLine("ÖĞRENCİ BİLGİ SİSTEMİ");
        Console.WriteLine("1 - Öğrenci Kayıt: ");
        Console.WriteLine("2 - Öğretmen Kayıt: ");
        Console.WriteLine("3 - Öğrenci Girişi: ");
        Console.WriteLine("4 - Öğretmen Girişi: ");
        Console.WriteLine("5 - Öğrenci Listesi: ");
        Console.WriteLine("6 - Öğrenci Güncelleme: ");
        Console.WriteLine("7 - Öğretmen Güncelleme: ");
        Console.WriteLine("8 - Derslerimiz: ");
        Console.WriteLine("9 - Öğretmen Kaydı Silme: ");
        Console.WriteLine("10 - Öğrenci Kaydı Silme: ");
        Console.WriteLine("==YAPMAK İSTREDİĞİNİZ İŞLEM NUMARASINI GİRİNİZ==");
        string sec = Console.ReadLine();
        connection = new SqlConnection(connectionString);
        connection.Open();
        switch (sec)
        {
            case "1":
                OgrenciEkle();
                break;
            case "2":
                OgretmenEkle();
                break;
            case "3":
                OgrenciGiris();
                break;
            case "4":
                OgretmenGiris();
                break;
            case "5":
                OgrenciList();
                break;
            case "6":
                OgrenciBilgiGencelle();
                break;
            case "7":
                OgretmenBilgiGuncelle();
                break;
            case "8":
                Dersler();
                break;
            case "9":
                OgretmenSil();
                break;
            case "10":
                OgrenciSil();
                break;
            case "11":
                OgretmenAta();
                break;
        }
        connection.Close();
    }
    public static void OgrenciAta()
    {
        OgrenciList();
        Console.WriteLine("Öğrenci Id Numaranızı Giriniz:");
        int ogrenciId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Ders Id si Giriniz:");
        int dersId = Convert.ToInt32(Console.ReadLine());
        string query = "INSERT INTO Veriler(OgrenciId,DersId) VALUES (@OgrenciId,@DersId)";
        using (SqlCommand sqlCommand = new SqlCommand(query, connection))
        {
            sqlCommand.Parameters.AddWithValue("@OgrenciId", ogrenciId);
            sqlCommand.Parameters.AddWithValue("@DersId", dersId);
            int count = sqlCommand.ExecuteNonQuery();
            if (count > 0) Console.WriteLine("Kayıt Başarılı");
            else Console.WriteLine("Kayıt Başarısız Tekrar Deneyiniz");
            Console.WriteLine("Kayıt Yapılmıştır");
        }
    }
    public static void OgretmenAta()
    {

        Console.WriteLine("Öğretmen Id Numaranızı Giriniz");
        int ogretmenId = Convert.ToInt32(Console.ReadLine());
        Dersler();
        Console.WriteLine("Lütfen Ders  Id si seçiniz");
        int dersId = Convert.ToInt32(Console.ReadLine());
        string query = "UPDATE Veriler SET OgretmenId=@OgretmenId,DersId=@DersId WHERE DersId=@DersId";
        using (SqlCommand sqlCommand = new SqlCommand(query, connection))
        {
            sqlCommand.Parameters.AddWithValue("@DersId", dersId);
            sqlCommand.Parameters.AddWithValue("@OgretmenId", ogretmenId);
            int count = sqlCommand.ExecuteNonQuery();
            if (count > 0) Console.WriteLine("Kayıt Başarılı");
            else Console.WriteLine("Kaytı Başarısız");
            Console.WriteLine("İşleminiz Tamamlanmıştır");
        }
    }
    public static void OgrenciEkle()
    {
        Console.WriteLine("Öğrencinin Adı:");
        string name = Console.ReadLine();
        Console.WriteLine("Öğrencinin Soyadı:");
        string surname = Console.ReadLine();
        string query = "INSERT INTO OgrenciKayit(Name,Surname)VALUES (@Name,@Surname)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Surname", surname);
            int count = command.ExecuteNonQuery();
            if (count > 0) Console.WriteLine("Kayıt Başarılı");
            else Console.WriteLine("Kayıt Başarısız Tekrar Deneyiniz");
            Console.WriteLine("Kayıt Yapılmıştır");
        }
        OgrenciAta();
    }
    public static void OgrenciList()
    {
        SqlCommand ogrenciList = new SqlCommand();
        ogrenciList.Connection = connection;
        ogrenciList.CommandText = "SELECT * FROM OgrenciKayit";
        SqlDataReader ogrenciReader = ogrenciList.ExecuteReader();
        Console.WriteLine("OgrenciId \tName \tSurname");
        while (ogrenciReader.Read())
        {
            Console.WriteLine($"{ogrenciReader[0]}\t{ogrenciReader[1]}\t{ogrenciReader[2]}");
        }
        Console.ReadKey();
    }
    public static void OgretmenEkle()
    {
        Console.WriteLine("Öğretmenin Adı:");
        string ogretmenAdi = Console.ReadLine();
        string query = "INSERT INTO OgretmenKayit(OgretmenAdi)VALUES(@OgretmenAdi)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@OgretmenAdi", ogretmenAdi);
            int count = command.ExecuteNonQuery();
            if (count > 0) Console.WriteLine("Kayıtınız Başarılı Öğretmenim");
            else Console.WriteLine("Kayıt Başarısız Tekrar Deneyiniz");
        }
        Console.ReadKey();
    }
    public static void OgrenciGiris()
    {
        OgrenciList();
        Console.WriteLine("Hoşgeldin Sevgili Öğrenci");
        Console.WriteLine("Kendi Öğrenci Id Numaranızı Giriniz: ");
        int ogrenciId = Convert.ToInt32(Console.ReadLine());
        SqlCommand dersler = new SqlCommand();
        dersler.Connection = connection;
        dersler.CommandText = "SELECT V.*, DBK.LessonName " +
                           "FROM [dbo].[Veriler] V " +
                           "JOIN [dbo].[DersBilgiKaydi] DBK ON V.DersId = DBK.DersId " +
                           "WHERE V.OgrenciId = @OgrenciId";
        dersler.Parameters.AddWithValue("@OgrenciId", ogrenciId);
        SqlDataReader derslerReader = dersler.ExecuteReader();
        Console.WriteLine("OgrenciId\tLessonName");
        while (derslerReader.Read())
        {
            Console.WriteLine($"{derslerReader["OgrenciId"]}\t{derslerReader["LessonName"]}");
        }

        string query = "SELECT OgrenciId,NotOrtalamasi FROM Veriler WHERE OgrenciId=@OgrenciId";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@OgrenciId", ogrenciId);
            command.ExecuteNonQuery();
        }
        Console.ReadKey();
    }
    public static void OgretmenGiris()
    {
        SqlCommand ogrenciList = new SqlCommand();
        ogrenciList.Connection = connection;
        ogrenciList.CommandText = "SELECT * FROM OgrenciKayit";
        SqlDataReader ogrenciReader = ogrenciList.ExecuteReader();
        Console.WriteLine("OgrenciId\tName");
        while (ogrenciReader.Read())
        {
            Console.WriteLine($"{ogrenciReader[0]}\t{ogrenciReader[1]}");
        }

        Console.WriteLine("Hoşgeldin Saygıdeğer Öğretmen");
        Console.WriteLine("Notunu Girmek İstediğiniz Öğrenci Numarası(ID):");
        string ogrenciId = Console.ReadLine();
        Console.WriteLine("Hangi Ders için Not Girilicek:");
        int dersId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Öğrencinin 1.Notu: ");
        int not1 = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Öğrencinin 2.Notu: ");
        int not2 = Convert.ToInt32(Console.ReadLine());
        int notortalamasi = (not1 + not2) / 2;
        Console.WriteLine("Öğrencinin Ortalaması" + notortalamasi);

        if (notortalamasi >= 50) Console.WriteLine("Öğrenci Geçti");
        else Console.WriteLine("Öğrenci Kaldı");
        string query = "UPDATE Veriler SET NotOrtalamasi=@NotOrtalamasi WHERE DersId=@DersId AND OgrenciId=@OgrenciId";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Notortalamasi", notortalamasi);
            command.Parameters.AddWithValue("@OgrenciId", ogrenciId);
            command.Parameters.AddWithValue("@DersId", dersId);
            int count = command.ExecuteNonQuery();

            if (count > 0) Console.WriteLine("Not Ortalamaları Girişi Başarılı");
            else Console.WriteLine("Not Ortalamalarında Hatalı Girişi");
            Console.ReadKey();
        }
    }
    public static void Dersler()
    {
        SqlCommand dersveNotList = new SqlCommand();
        dersveNotList.Connection = connection;
        dersveNotList.CommandText = "SELECT * FROM DersBilgiKaydi";
        SqlDataReader dersveNotReader = dersveNotList.ExecuteReader();
        Console.WriteLine("DersId\tLessonName");
        while (dersveNotReader.Read())
        {
            Console.WriteLine($"{dersveNotReader[0]}\t{dersveNotReader[1]}");
        }
        Console.ReadKey();
    }
    public static void OgrenciBilgiGencelle()
    {
        SqlCommand ogrenciList = new SqlCommand();
        ogrenciList.Connection = connection;
        ogrenciList.CommandText = "SELECT * FROM OgrenciKayit";
        SqlDataReader ogrenciReader = ogrenciList.ExecuteReader();
        Console.WriteLine("OgrenciId\tName\tSurname");
        while (ogrenciReader.Read())
        {
            Console.WriteLine($"{ogrenciReader[0]}\t{ogrenciReader[1]}\t{ogrenciReader[2]}");
        }
        Console.WriteLine("Güncellemek İstediğiniz Öğrenci Id'sini Giriniz:");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Öğrencinin Adı:");
        string name = Console.ReadLine();
        Console.WriteLine("Öğrencinin Soyadı:");
        string surname = Console.ReadLine();
        string query = "UPDATE OgrenciKayit SET Name=@Name,Surname=@Surname WHERE OgrenciId=@OgrenciId";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@OgrenciId", id);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Surname", surname);
            command.ExecuteNonQuery();
        }
        OgrenciAta();
        Console.WriteLine("Güncelleme Onaylandı");
        Console.ReadKey();
    }
    public static void OgretmenBilgiGuncelle()
    {
        SqlCommand ogretmenList = new SqlCommand();
        ogretmenList.Connection = connection;
        ogretmenList.CommandText = "SELECT * FROM OgretmenKayit";
        SqlDataReader ogretmenReader = ogretmenList.ExecuteReader();
        Console.WriteLine("OgretmenId\tOgretmenAdi");
        while (ogretmenReader.Read())
        {
            Console.WriteLine($"{ogretmenReader[0]}\t {ogretmenReader[1]}");
        }

        Console.WriteLine("Öğretmenin Adı: ");
        string ogretmenAdi = Console.ReadLine();

        string query = "UPDATE OgretmenKayıt SET ogretmenAdi=@OgretmenAdi WHERE ogretmenId=@OgretmenId";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@OgretmenAdi", ogretmenAdi);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Güncelleme Onaylandı");
    }
    public static void OgretmenSil()
    {
        SqlCommand ogretmenList = new SqlCommand();
        ogretmenList.Connection = connection;
        ogretmenList.CommandText = "SELECT * FROM OgretmenKayit";
        SqlDataReader ogretmenReader = ogretmenList.ExecuteReader();
        Console.WriteLine("OgretmenId\tOgretmenAdi");
        while (ogretmenReader.Read())
        {
            Console.WriteLine($"{ogretmenReader[0]}\t {ogretmenReader[1]}");
        }
        Console.WriteLine("Silmek İstediğiniz Öğretmenin Id Numarası: ");
        int ogretmenId = Convert.ToInt32(Console.ReadLine());
        string query = "DELETE FROM OgretmenKayit WHERE OgretmenId=@OgretmenId";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@OgretmenId", ogretmenId);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Silme İşleminiz Başarılı");
        Console.ReadKey();
    }
    public static void OgrenciSil()
    {
        SqlCommand ogrenciList = new SqlCommand();
        ogrenciList.Connection = connection;
        ogrenciList.CommandText = "SELECT * FROM OgrenciKayit";
        SqlDataReader ogrenciReader = ogrenciList.ExecuteReader();
        Console.WriteLine("OgrenciId \tName \tSurname ");
        while (ogrenciReader.Read())
        {
            Console.WriteLine($"{ogrenciReader[0]}\t{ogrenciReader[1]}\t{ogrenciReader[2]}");
        }
        Console.WriteLine("Silmek İstediğiniz Öğrenci Id Numarasını Giriniz: ");
        int ogrenciId = Convert.ToInt32(Console.ReadLine());
        string query = "DELETE FROM OgrenciKayit WHERE OgrenciId=@OgrenciId";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@OgrenciId", ogrenciId);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Silme İşleminiz Başarılı");
        Console.ReadKey();
    }
}



