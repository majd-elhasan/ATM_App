using System;
 namespace ATM_app
 {
     class MainClass
     {
       
        static short gunluk_Cekme_limiti = 2000;
        static List <Client> clients = new List<Client>(){new Client("Mecid elhasan","123456",5000)};
        static List<string> islemler = new List<string>(){"Para çekme","Para yatırma","Bakiye sorgulama","Hesaptan çık","Uygulamadan çık","gün Sonu"};
        static Client client = new Client("","",0);
        static void Main(string[] args)
        {
            Giris_Yap();
            islem_secimi:
            Console.WriteLine("yapmak istediğiniz işlemi seçiniz.");
            for (int i = 0; i < islemler.Count; i++)
            {
                Console.WriteLine("{0}  {1}", i + 1 , islemler[i]);
            }

            switch (try_short_input((short)islemler.Count))
            {
                case 1:
                    Withdraw(ref client.Balance);
                break;
                case 2:
                    Deposit(ref client.Balance);
                break;
                case 3:
                    ShowBalance(client.Balance);
                break;
                case 4:
                    Main(new string[0]);
                break;
                case 5:
                    Environment.Exit(0);
                break;
                case 6:
                    Gun_Sonu();
                break;
            }
            goto islem_secimi;
        }
        
        static void Withdraw(ref double balance){
            Console.WriteLine("çekmek istediğiniz miktarı giriniz.");
            short Withdrawability = gunluk_Cekme_limiti;
            if (balance < gunluk_Cekme_limiti){
                Withdrawability = (short)balance;
            }
            short amount = try_short_input(Withdrawability);
            if (amount > 0){
                balance -= amount;
                Console.WriteLine("Para çekme işlemi başarıyla tamamlandı.");
                Console.WriteLine("___________________________________________");
                Console.WriteLine();
                Logger.LogMaker("ATM'den "+client.Name+" tarafından "+ amount +" TL çekildi");
            }
        }
        static void Deposit(ref double balance){
            Console.WriteLine("yatırmak istediğiniz miktarı giriniz.");
            short amount = try_short_input(short.MaxValue);
            if (amount >0){
                balance += amount;
                Console.WriteLine("Para yatırma işlemi başarıyla tamamlandı.");
                Console.WriteLine("___________________________________________");
                Console.WriteLine();
                Logger.LogMaker("ATM'ye "+client.Name+" tarafından "+ amount +" TL yatırıldı");
            }
        }
        static void ShowBalance(double balance){
            Console.WriteLine("Bakiyeniz : " + balance.ToString("F3"));
            Console.WriteLine("___________________________________________");
            Console.WriteLine();
        }
        static void Gun_Sonu(){
            Logger.LogShower();
        }
        static short try_short_input(short limit)
        {
            short input = 0;
            void geri_don(){Console.WriteLine("geri dönmek için 'Enter' tuşuna basınız."); Console.ReadKey();}
            try_input_();
            void try_input_(){  
                bool Try = Int16.TryParse(Console.ReadLine(),out input);
                if (input > limit && limit > client.Balance){Console.WriteLine("Bakiyeniz yetersizdir ! , başka miktar deneyiniz");geri_don();}
                else if (input > limit && limit < client.Balance){Console.WriteLine("günlük para çekme limiti "+ gunluk_Cekme_limiti +" TL'dir , onu değiştirmek için banka şubemizi ziyaret edebilirsiniz.");geri_don(); }
                else if (input < 0){Console.WriteLine("0'dan küçük TL çekemezsiniz !"); geri_don(); }
            }
            return input;
        }
        static string try_input()
        {
            string input = Console.ReadLine();
            if (input == ""){Console.WriteLine("boş bırakmayınız");  try_input();}
            return input;
        }
        static string try_input_username()
        {
            string input = Console.ReadLine();
            foreach (Client one in clients)
            {
                if (input == one.Name)
                {
                    Console.WriteLine("siz zaten kayıtlısınız.");
                    Console.WriteLine("Ana menüye dönmek için herhangi bir tuşa basınız.");
                    Console.ReadKey();
                    Main(new string[0]); 
                }
            }
            if (input == ""){Console.WriteLine("boş bırakmayınız");  try_input_username();}
            return input;
        }
        static Client Kayit_Ol(){
            Console.WriteLine("adınızı giriniz.");
            string clientname = try_input_username();
            Console.WriteLine("şifre giriniz.");
            string password = try_input();

            clients.Add(new Client(clientname,password));   ///  bir kullanıcı kayıt oldu.
            return new Client(clientname,password);
        }
        static void Giris_Yap(){
            Console.Write("Lütfen Kullanıcı adınızı giriniz : ");
            string client_input = try_input();
            string password_input ="";
            int counter = 0;
            foreach (var client in clients)
            {
                if (client.Name != client_input)
                    {counter ++;}
                else break;
            }
            if (counter == clients.Count){
                Console.WriteLine("Sistemde kayıtlı değilsiniz !");
                Console.WriteLine("kaydolmak ister misiniz ?  (Evet için  'e', Hayır için  'h') tuşlayınız.");
                if (Console.ReadLine() == "e")
                    client = Kayit_Ol(); 

                else Environment.Exit(0);
            }
            
            else {
                tryingToTypePassword:
                Console.Write("Lütfen şifrenizi giriniz : ");
                password_input = try_input();
                    if (clients[counter].Password == password_input)
                    {
                        Console.WriteLine("şifreniz doğrulandı.");
                        client = clients[counter];
                    }
                    else                                         /// başarısız deneme
                    {
                        Logger.LogMaker(clients[counter].Name + " adlı kullanıcı yanlış şifre girdi. ");
                        Console.WriteLine("Şifreniz yanlıştır !");
                        Console.WriteLine("işlemi sonlandırmak için 'q' , yeniden denemek için 'Enter' tuşuna basınız");
                        if (Console.ReadLine() == "q")
                            Environment.Exit(0);
                        else
                            goto tryingToTypePassword; 
                    }  
            }
        }
     }
 }
