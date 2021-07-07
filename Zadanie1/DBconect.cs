using System;
using System.Data.SqlClient;


namespace zad1
{
    public class DBconect
    {

        //ten int mi sie przyda potem do unikalnego id
        public int temp_id = 0;

        //lacze sie z moja kopia bazy ZNorthwind
        static string conString = @"Data Source=DESKTOP-UK1AL1K;Initial Catalog=ZNorthwind_Kopia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection con1 = new SqlConnection(conString);

        public void display()
        {
            con1.Open();
            string zapytanie = "SELECT * FROM Pracownicy";
            SqlCommand command = new SqlCommand(zapytanie, con1);
            var reader = command.ExecuteReader();

            //tworze reader do wyswietlenia informacji o pracownikach


            // Z czego wyświetlam tylko czesc kolumn
            while (reader.Read())
            {
                Console.WriteLine($" Idpracownika: {reader[0]}\n Nazwisko: {reader[1]}\n " +
                    $"Imie: {reader[2]}\n Stanowisko: {reader[3]}\n ZwrotGrzecznosciowy: {reader[4]}\n" +
                    $" Kraj: {reader[11]}\n\n\n ");


                //nadpisuje temp_id nowymi danymi z readera az do najwiekszego id
                if (temp_id < (int)reader[0]) temp_id = (int)reader[0];


            }

            //zamknę readera aby potem moc zrobic insert-a
            reader.Close();
        }


        //____________________________________
        //"następnie pozwól dodać nowy wpis"
        //-----------------------------------

        public void insert()
        {

            //przypisuje nowe id
            int idpracownika = temp_id + 1;

            //tu podaje dane z klawiatury
            Console.WriteLine("Podaj Nazwisko: ");
            string nazwisko = Console.ReadLine();

            Console.WriteLine("Podaj imie: ");
            string imie = Console.ReadLine();

            Console.WriteLine("Podaj stanowisko: ");
            string stanowisko = Console.ReadLine();

            Console.WriteLine("Podaj zwrot Grzecznosciowy: ");
            string zwrotGrzecznosciowy = Console.ReadLine();

            Console.WriteLine("Podaj kraj: ");
            string kraj = Console.ReadLine();

            /*
            Jakby to miało realne zastosowanie to powinno sie zrobic validacje danych, jakis "sanitizer". 
            Ale w tym przypadku wystarczy sqlparameter zeby zapobiedz sql injesction
             */

            var insert = "INSERT INTO Pracownicy (IDpracownika, Nazwisko, Imię, Stanowisko, ZwrotGrzecznościowy, Kraj) " +
                "VALUES (@ID, @Nazwisko, @Imie, @Stanowisko, @Zwrot, @Kraj)";

            var insert_params = new SqlCommand(insert, con1);
            insert_params.Parameters.Add(new SqlParameter("@ID", idpracownika));
            insert_params.Parameters.Add(new SqlParameter("@Nazwisko", nazwisko));
            insert_params.Parameters.Add(new SqlParameter("@Imie", imie));
            insert_params.Parameters.Add(new SqlParameter("@Stanowisko", stanowisko));
            insert_params.Parameters.Add(new SqlParameter("@Zwrot", zwrotGrzecznosciowy));
            insert_params.Parameters.Add(new SqlParameter("@Kraj", kraj));
            insert_params.ExecuteNonQuery();

        }

        //_______________________
        //updatowanie danych
        //------------------------
        public void update()
        {
            // tu dam tylko opcje edycji nazwiska, ale zasada jest ta sama przy calosci.
            Console.WriteLine("Jakie nazwisko chcesz zedytowac: ");
            string edit_nazwisko = Console.ReadLine();
            Console.WriteLine("Na jakie?: ");
            string edit_nazwisko_nowe = Console.ReadLine();

            var update = $"UPDATE Pracownicy SET Nazwisko = @Nowe_Nazwisko WHERE Nazwisko = @Nazwisko";
            var update_params = new SqlCommand(update, con1);
            update_params.Parameters.Add(new SqlParameter("@Nowe_Nazwisko", edit_nazwisko_nowe));
            update_params.Parameters.Add(new SqlParameter("@Nazwisko", edit_nazwisko));
            update_params.ExecuteNonQuery();

        }


        //________________________
        //" a na koniec usuń go."
        //------------------------
        public void delete()
        {

            Console.Write("Jakiego pracownika chcesz usunac (po nazwisku) ?: ");
            string delete_nazwisko = Console.ReadLine();

            //_______________________
            //usuwanie
            //-----------------------
            var delete = $"DELETE FROM Pracownicy WHERE Nazwisko = @Nazwisko";
            var delete_params = new SqlCommand(delete, con1);
            delete_params.Parameters.Add(new SqlParameter("@Nazwisko", delete_nazwisko));
            delete_params.ExecuteNonQuery();
        }

        public void closeconn()
        {
            con1.Close();
        }
    }
}
