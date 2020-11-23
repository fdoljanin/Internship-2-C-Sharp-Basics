using System;
using System.Collections.Generic;

namespace Playlist
{
    class Program
    {

    static Dictionary<int, string> songs = new Dictionary<int, string>(){ //playlist with default songs
                {1, "Alice" },
                {2, "Enigma" },
                {3, "Replay" },
                {4, "Babylon" },
                {5, "Heaven" },
                {6, "SOS" },
                {7, "Freak" },
                {8, "Fades Away" }
        };
        
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            mainMenu();
        }


        static void cls()
        {
            bool clears=true; //if not wanted, set false
            if (clears) Console.Clear();
        }

        enum color
        {
            RED,YELLOW,GREEN
        }

        static void colorText(string msg, color txtColor)
        {
            if (txtColor == color.RED) Console.ForegroundColor = ConsoleColor.Red;
            if (txtColor == color.YELLOW) Console.ForegroundColor = ConsoleColor.Yellow;
            if (txtColor == color.GREEN) Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static bool confirmMsg(string msg)
        {
            colorText(msg+" (da/ne)",color.RED);
            while (true)
            {
                string resp = Console.ReadLine().ToLower().Trim();
                if (resp == "da") return true;
                if (resp == "ne")
                {
                    Console.WriteLine("Akcija poništena.\n");
                    return false;
                }
                colorText("Odgovor nije prepoznat, upišite ponovno. (da/ne)",color.YELLOW);
            }
           
        }

        static int validNum(string msg)
        {
            Console.WriteLine(msg);
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return 0;
            }
            int numInput;
            try
            {
                numInput = int.Parse(input);
                if (numInput > songs.Count || numInput < 1)
                {
                    colorText("Pjesma na tom indeksu ne postoji. \n", color.YELLOW);
                    return validNum(msg);
                }
            }
            catch
            {
                colorText("Unos nije valjan. \n", color.YELLOW);
                return validNum(msg);
            }
            return numInput;
        }

        static (int, string) validName(string msg)
        {
            Console.WriteLine(msg);
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return (0, input);
            }
            foreach (KeyValuePair<int, string> song in songs) if (song.Value.ToLower() == input)
                {
                    return (song.Key, input);
                }
            return (-1, input);
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
10 - Pomiješaj pjesme
0 - Izlaz iz aplikacije
";
            Console.WriteLine(options);
            var optionCall = new List<Action>()
            {
                songList, songFindName, songFindIndex, songAdd, songIndexRemove, songNameRemove, listDelete, songRename, songReindex, songReshuffle
            };
            var choice = Console.ReadLine().Trim();
            try
            {
                var choiceInt = int.Parse(choice);
                if (choiceInt == 0)
                {
                    cls();
                    Console.WriteLine("Gašenje...\n");
                }
                else
                {
                    cls();
                    optionCall[choiceInt-1]();
                }  
            }
            catch
            {
                cls();
                colorText("Unos nije prepoznat!\n ", color.YELLOW);
                mainMenu();
            }

        }
        
        static void songList()
        {
            Console.WriteLine("Lista Vaših pjesama: \n");
            for(int i=0;i<songs.Count;++i)
            {
                Console.WriteLine(i+1 + ". - " + songs[i+1]);
            }
            Console.WriteLine("\n Pritisnite enter za povratak na izbornik.");
            Console.ReadLine();
            cls();
            mainMenu();
        }
        
        static void songFindName()
        {
            var numInput = validNum("Upišite redni broj pjesme:");
            if (numInput == 0) return;
            Console.WriteLine("Naziv pjesme: " + songs[numInput] + "\n");
            songFindName();
        }

        static void songFindIndex()
        {
            var index = validName("Unesite ime pjesme za pretragu ili enter za povratak na izbornik:");
            if (index.Item1 == 0) return;
            else if (index.Item1 == -1) Console.WriteLine("Pjesma nije pronađena. \n");
            else Console.WriteLine("Redni broj pjesme je " + index.Item1 + ".\n");
            songFindIndex();
        }
        
        static void songAdd()
        {
            var name = validName("Unesite ime pjesme ili enter za povratak za izbornik:");
            if (name.Item1 == 0) return;
            if (name.Item1 != -1) colorText("Pjesma već postoji! \n", color.YELLOW);
            else
            {
                if(confirmMsg("Želite li dodati pjesmu " + name.Item2 + " ?")){
                    songs.Add(songs.Count + 1, name.Item2);
                    colorText("Pjesma dodana!\n", color.GREEN);
                }
            }

            songAdd();
        }

        static void songIndexRemove()
        {
            var delIndex = validNum("Upišite redni broj pjesme koju želite obrisati ili enter za izbornik:");
            if (delIndex == 0) return;
            if (confirmMsg("Želite li uistinu obrisati pjesmu " + songs[delIndex] + "?"))
            {
                songs.Remove(delIndex);
                for (int i = delIndex + 1; i < songs.Count + 2; ++i)
                { //since we want dict with actual index, O(len-input) with deletion is needed
                    songs.Add(i - 1, songs[i]);
                    songs.Remove(i);
                }
                colorText("Pjesma na indeksu " + delIndex + " obrisana.\n", color.GREEN);
            }
            songIndexRemove();
        }
        
        static void songNameRemove()
        {
            var name = validName("Unesite ime pjesme koju želite obrisati ili enter za povratak na izbornik:");
            if (name.Item1 == 0) return;
            if (name.Item1 == -1) colorText("Pjesma ne postoji!\n", color.YELLOW);
            else
            {
                if (confirmMsg("Želite li uistinu obrisati pjesmu " + songs[name.Item1] + "?"))
                {
                    songs.Remove(name.Item1);
                    for (int i = name.Item1 + 1; i < songs.Count + 2; ++i)
                    {//since we want dict with actual index, O(len-input) with deletion is needed
                        songs.Add(i - 1, songs[i]);
                        songs.Remove(i);
                    }
                    colorText("Pjesma obrisana.\n", color.GREEN);
                }
            }
            songNameRemove();
        }

        static void listDelete()
        {
            if(confirmMsg("Jeste li sigurni da želite obrisati sve pjesme?"))
            {
                for (int i = songs.Count; i>0; --i) songs.Remove(i);
            }
            cls();
            mainMenu();
        }

        static void songRename()
        {
            var name = validName("Unesite ime pjesme koju želite obrisati preimenovati ili pritisnite enter za povratak:");
            if (name.Item1 == 0) return;
            else if (name.Item1 == -1) colorText("Pjesma ne postoji! \n", color.YELLOW);
            else
            {
                var newName = validName("Unesite novo ime pjesme ili enter za povratak:");
                if (newName.Item1 == 0) return;
                if (newName.Item1 != -1) colorText("Pjesma s tim imenom već postoji!\n", color.YELLOW);
                else if (confirmMsg("Želite li sigurno preimenovati pjesmu?"))
                {
                    colorText("Pjesma preimenovana.\n",color.GREEN);
                    songs[name.Item1] = newName.Item2;
                }
            }
            songRename();
        }
        
        static void songReindex()
        {
            var oldIndex = validNum("Unesite stari redni broj ili enter za povratak:");
            if (oldIndex == 0) return;
            var newIndex = validNum("Unesite novi redni ili enter za povratak:");
            if (newIndex == 0) return;
            if (confirmMsg("Želite li uistinu zamijeniti redni broj?"))
            {
                colorText("Index zamijenjen. \n",color.GREEN);
                var replaced = (newIndex, songs[oldIndex]);
                songs.Remove(oldIndex);
                var inc = 1;
                if (oldIndex > newIndex) inc = -1;
                for (int i = oldIndex+inc; i*inc <= newIndex*inc; i+=inc)
                {
                    songs.Add(i-inc, songs[i]);
                    songs.Remove(i);
                }
                songs.Add(replaced.Item1, replaced.Item2);
            }
            songReindex();
        }

        static void songReshuffle()
        {
            if (confirmMsg("Želite li pomiješati pjesme?"))
            {
                var rand = new Random();
                var copyDict = new Dictionary<int, string>();
                var nesto = songs;
                var oldIndex = new List<int>();
                var newIndex = new List<int>();
                for (int i = 0; i < songs.Count; ++i) oldIndex.Add(i);
                while (oldIndex.Count > 0)
                {
                    var tempIndex = rand.Next(0, oldIndex.Count);
                    newIndex.Add(oldIndex[tempIndex]);
                    oldIndex.RemoveAt(tempIndex);
                }
                foreach (var song in songs)
                {
                    copyDict.Add(song.Key, song.Value);
                    songs.Remove(song.Key);
                }
                for (int i = 0; i < copyDict.Count; ++i)
                {
                    songs.Add(i + 1, copyDict[newIndex[i] + 1]);
                }
            }
            cls();
            mainMenu();
        }
    }
}
