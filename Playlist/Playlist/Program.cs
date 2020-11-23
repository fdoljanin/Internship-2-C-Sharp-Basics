using System;
using System.Collections.Generic;

namespace Playlist
{
    class Program
    {

    static Dictionary<int, string> songs = new Dictionary<int, string>(){ //preloaded
                {1, "Alice" },
                {2, "Enigma" },
                {3, "Replay" },
                {4, "Babylon" }
        };
        
        public static void Main(string[] args)
        {
             

            mainMenu();

        }


        static void cls()
        {
            bool clears=true; //if not wanted, set false
            if (clears) Console.Clear();
        }

        static bool confirmMsg(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg+" (da/ne)");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                string resp = Console.ReadLine().ToLower().Trim();
                if (resp == "da") return true;
                if (resp == "ne") return false;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Odgovor nije prepoznat, upišite ponovno. (da/ne)");
            }
           
        }

        static void mainMenu()
        {
            string options = @"Odaberite akciju:
1 - Ispis cijele liste
2 - Ispis imena pjesme unosom pripadajućeg rednog broja
3 - Ispis rednog broja pjesme unosom pripadajućeg imena
4 - Unos nove pjesme
5 - Brisanje pjesme po rednom broju
6 - Brisanje pjesme po imenu
7 - Brisanje cijele liste
8 - Uređivanje imena pjesme
9 - Uređivanje rednog broja pjesme
0 - Izlaz iz aplikacije";
            Console.WriteLine(options);
            switch (Console.ReadLine().Trim()){
                case "0":
                    cls();
                    Console.WriteLine("Gašenje..");
                    return;
                case "1":
                    songList();
                    break;
                case "2":
                    cls();
                    songFindName();
                    break;
                case "3":
                    cls();
                    songFindIndex();
                    break;
                case "4":
                    cls();
                    songAdd();
                    break;
                case "5":
                    cls();
                    songIndexRemove();
                    break;
                case "6":
                    cls();
                    songNameRemove();
                    break;
                case "7":
                    cls();
                    listDelete();
                    break;
                case "8":
                    cls();
                    songRename();
                    break;
                case "9":
                    cls();
                    songReindex();
                    break;
                default:
                    cls();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Akcija nije prepoznata, pokušajte ponovno:\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    mainMenu();
                    break;

            }

        }

        static void songList()
        {
            cls();
            Console.WriteLine("Lista Vaših pjesama: \n");
            foreach(KeyValuePair<int,string>song in songs)
            {
                Console.WriteLine(song.Key + ". - " + song.Value);
            }
            Console.WriteLine("\n Pritisnite enter za povratak na izbornik.");
            Console.ReadLine();
            cls();
            mainMenu();
        }

        static void songFindName()
        {
            Console.WriteLine("Unesite broj za ispis ili pritisnite enter za povratak na izbornik:");
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            var numInput = int.Parse(input);
            if (numInput > songs.Count || numInput<1)
            {
                Console.WriteLine("Pjesma na tom indeksu ne postoji. \n");
                songFindName();
                return;
            }
            Console.WriteLine("Naziv pjesme: " + songs[numInput] + "\n");
            songFindName();
            

        }

        static void songFindIndex()
        {
            Console.WriteLine("Unesite pjesmu za pretragu ili pritisnite enter za povratak na izbornik:");
            var input = Console.ReadLine().ToLower().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            foreach(KeyValuePair<int,string>song in songs) if(song.Value.ToLower()==input)
            {
                    Console.WriteLine("Indeks pjesme je: " + song.Key + "\n");
                    songFindIndex();
                    return;
            }
            Console.WriteLine("Pjesma nije pronađena. \n");
            songFindIndex();


        }

        static void songAdd()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Unesite ime pjesme ili enter za povratak za izbornik:");
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            foreach (KeyValuePair<int, string> song in songs) if (song.Value.ToLower() == input)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Pjesma već postoji. \n");
                    songAdd();
                    return;
                }
            if (confirmMsg("Želite li dodati pjesmu " + input + "?"))
            {
                songs.Add(songs.Count + 1, input);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pjesma " + input + " dodana na indeks " + songs.Count + ".\n");
            }
            else Console.WriteLine("Akcija poništena.\n");
            songAdd();
        }

        static void songIndexRemove()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Unesite indeks pjesme koju želite obrisati ili enter za povratak na izbornik:");
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            var numInput = int.Parse(input);
            if (numInput > songs.Count || numInput < 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Pjesma na tom indeksu ne postoji. \n");
                songIndexRemove();
                return;
            }
            if(confirmMsg("Želite li uistinu obrisati pjesmu " + songs[numInput] + "?"))
            {
                songs.Remove(numInput);
                for (int i = numInput + 1; i < songs.Count + 2; ++i)
                { //since we want dict with actual index, O(len-input) with deletion is needed
                    songs.Add(i - 1, songs[i]);
                    songs.Remove(i);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pjesma na indeksu obrisana.\n");
                songIndexRemove();
                return;
            } else
            {
                Console.WriteLine("Akcija poništena.\n");
                songIndexRemove();
                return;
            }
        }
        static void songNameRemove()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Unesite ime pjesme koju želite obrisati ili enter za povratak na izbornik:");
            var input = Console.ReadLine().ToLower().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            int songIndex = -1;
            foreach (KeyValuePair<int, string> song in songs) if (song.Value.ToLower() == input)
                {
                    songIndex = song.Key;
                    break;
                }
            if (songIndex == -1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Pjesma ne postoji.\n");
                songNameRemove();
                return;
            }


            if (confirmMsg("Želite li uistinu obrisati pjesmu " + input + "?"))
            {
                songs.Remove(songIndex);
                for (int i = songIndex + 1; i < songs.Count + 2; ++i) //since we want dict with actual index, O(len-input) with deletion is needed
                    songs.Add(i - 1, songs[i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pjesma obrisana.\n");
                songNameRemove();
                return;
            }
            else
            {
                Console.WriteLine("Akcija poništena.\n");
                songIndexRemove();
                return;
            }
        }

        static void listDelete()
        {
            if(confirmMsg("Jeste li sigurni da želite obrisati sve pjesme?"))
            {
                int cnt = songs.Count;
                for (int i = 1; i <= cnt; ++i) songs.Remove(i);
            }
            cls();
            mainMenu();
        }

        static void songRename()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Unesite ime pjesme koju želite obrisati preimenovati ili pritisnite enter za povratak:");
            var input = Console.ReadLine().ToLower().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            int songIndex = -1;
            foreach (KeyValuePair<int, string> song in songs) if (song.Value.ToLower() == input)
                {
                    songIndex = song.Key;
                    break;
                }
            if (songIndex == -1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Pjesma ne postoji.\n");
                songRename();
                return;
            }
            Console.WriteLine("Unesite novo ime pjesme:");
            string newName = Console.ReadLine();
            if(confirmMsg("Želite li sigurno preimenovati pjesmu?"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pjesma preimenovana.\n");
                songs[songIndex] = newName;
                songRename();
            }
            else
            {
                Console.WriteLine("Akcija poništena.\n");
                songRename();
            }
        }

        static void songReindex()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Unesite stari redni broj pjesme ili enter za povratak:");
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return;
            }
            var numInput = int.Parse(input);
            if (numInput > songs.Count || numInput < 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Pjesma na tom indeksu ne postoji. \n");
                songReindex();
                return;
            }
            Console.WriteLine("Unesite novi redni broj:");
            var newIndex = int.Parse(Console.ReadLine());
            if (confirmMsg("Želite li uistinu zamijeniti redni broj?"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Index zamijenjen. \n");
                var replaced = (newIndex, songs[numInput]);
                songs.Remove(numInput);
                if (newIndex > numInput)
                    for (int i = numInput + 1; i <= newIndex; ++i)
                    {
                        songs.Add(i - 1, songs[i]);
                        songs.Remove(i);
                    }
                else
                    for (int i = numInput - 1; i >= newIndex; --i)
                    {
                        songs.Add(i + 1, songs[i]);
                        songs.Remove(i);
                    };
                songs.Add(replaced.Item1, replaced.Item2);

            } else
            {
                Console.WriteLine("Akcija poništena.\n");
            }
            songReindex();
        }
    }
}
