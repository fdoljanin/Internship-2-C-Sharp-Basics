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

        //functions that are used in other options
        static void cls() 
        {
            bool clears = true; //if not wanted, set false
            if (clears) Console.Clear();
        }

        enum color
        {
            RED,YELLOW,GREEN
        }

        static void colorText(string msg, color txtColor) //outputs colored msg using enum
        {
            if (txtColor == color.RED) Console.ForegroundColor = ConsoleColor.Red;
            if (txtColor == color.YELLOW) Console.ForegroundColor = ConsoleColor.Yellow;
            if (txtColor == color.GREEN) Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static bool confirmMsg(string msg) //confirms action with message displayed
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

        static int validNum(string msg) //returns index if input is valid, 0 for menu
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

        static (int Key, string Value) validName(string msg) //returns tuple w index and name of input song, -1 if not found
        {
            Console.WriteLine(msg);
            var input = Console.ReadLine().Trim();
            if (input == "")
            {
                cls();
                mainMenu();
                return (0, input);
            }
            foreach (var song in songs) if (song.Value.ToLower() == input.ToLower())
                {
                    return (song.Key, input);
                }
            return (-1, input);
        }

        static void mainMenu() //menu
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
            catch //if int convert fails or index out of range
            {
                cls();
                colorText("Unos nije prepoznat!\n ", color.YELLOW);
                mainMenu();
            }

        }
        
        //main menu options
        static void songList() //list of all songs
        {
            Console.WriteLine("Lista Vaših pjesama: \n");
            if (songs.Count == 0) colorText("Playlista ne sadrži ni jednu pjesmu!", color.YELLOW);
            for(int i=0;i<songs.Count;++i)
            {
                Console.WriteLine(i+1 + ". - " + songs[i+1]);
            }
            Console.WriteLine("\nPritisnite enter za povratak na izbornik.");
            Console.ReadLine();
            cls();
            mainMenu();
        }
        
        static void songFindName() //find name by index
        {
            var numInput = validNum("Upišite redni broj pjesme ili enter za povratak na izbornik:"); //validate input index
            if (numInput == 0) return;
            Console.WriteLine("Naziv pjesme: " + songs[numInput] + "\n");
            songFindName();
        }

        static void songFindIndex() //find index by name
        {
            var index = validName("Unesite ime pjesme za pretragu ili enter za povratak na izbornik:");
            if (index.Key == 0) return;
            else if (index.Key == -1) Console.WriteLine("Pjesma nije pronađena. \n");
            else Console.WriteLine("Redni broj pjesme je " + index.Key + ".\n");
            songFindIndex();
        }
        
        static void songAdd() //add a song
        {
            var name = validName("Unesite ime pjesme ili enter za povratak za izbornik:");
            if (name.Key == 0) return;
            if (name.Key != -1) colorText("Pjesma već postoji! \n", color.YELLOW); //should not be found 
            else
            {
                if(confirmMsg("Želite li dodati pjesmu " + name.Value + " ?")){
                    songs.Add(songs.Count + 1, name.Value);
                    colorText("Pjesma dodana!\n", color.GREEN);
                }
            }

            songAdd();
        }

        static void songIndexRemove() //remove song by index
        {
            var delIndex = validNum("Upišite redni broj pjesme koju želite obrisati ili enter za izbornik:");
            if (delIndex == 0) return;
            if (confirmMsg("Želite li uistinu obrisati pjesmu " + songs[delIndex] + "?"))
            {
                songs.Remove(delIndex);
                for (int i = delIndex + 1; i < songs.Count + 2; ++i)
                { //since we want dict with actual index, O(len-delIndex) is needed
                    songs.Add(i - 1, songs[i]);
                    songs.Remove(i);
                }
                colorText("Pjesma na indeksu " + delIndex + " obrisana.\n", color.GREEN);
            }
            songIndexRemove();
        }
        
        static void songNameRemove() //remove song by name
        {
            var name = validName("Unesite ime pjesme koju želite obrisati ili enter za povratak na izbornik:");
            if (name.Key == 0) return;
            if (name.Key == -1) colorText("Pjesma ne postoji!\n", color.YELLOW);
            else
            {
                if (confirmMsg("Želite li uistinu obrisati pjesmu " + songs[name.Key] + "?"))
                {
                    songs.Remove(name.Key);
                    for (int i = name.Key + 1; i < songs.Count + 2; ++i)
                    {
                        songs.Add(i - 1, songs[i]);
                        songs.Remove(i);
                    }
                    colorText("Pjesma obrisana.\n", color.GREEN);
                }
            }
            songNameRemove();
        }

        static void listDelete() //delete playlist
        {
            if(confirmMsg("Jeste li sigurni da želite obrisati sve pjesme?"))
            {
                for (int i = songs.Count; i>0; --i) songs.Remove(i);
            }
            cls();
            mainMenu();
        }

        static void songRename() //rename song by name
        {
            var name = validName("Unesite ime pjesme koju želite obrisati preimenovati ili pritisnite enter za povratak:");
            if (name.Key == 0) return;
            else if (name.Key == -1) colorText("Pjesma ne postoji! \n", color.YELLOW);
            else
            {
                var newName = validName("Unesite novo ime pjesme ili enter za povratak:");
                if (newName.Key == 0) return;
                if (newName.Key != -1) colorText("Pjesma s tim imenom već postoji!\n", color.YELLOW);
                else if (confirmMsg("Želite li sigurno preimenovati pjesmu?"))
                {
                    colorText("Pjesma preimenovana.\n",color.GREEN);
                    songs[name.Key] = newName.Value;
                }
            }
            songRename();
        }
        
        static void songReindex() //reindex song by index
        {
            var oldIndex = validNum("Unesite stari redni broj ili enter za povratak:");
            if (oldIndex == 0) return;
            var newIndex = validNum("Unesite novi redni broj ili enter za povratak:");
            if (newIndex == 0) return;
            if (confirmMsg("Želite li uistinu zamijeniti redni broj?"))
            {
                colorText("Index zamijenjen. \n",color.GREEN);
                var replaced = (Key: newIndex, Value: songs[oldIndex]);
                songs.Remove(oldIndex);
                var inc = 1;
                if (oldIndex > newIndex) inc = -1; //if index goes down, increment is negative
                for (int i = oldIndex+inc; i*inc <= newIndex*inc; i+=inc) //change every song index between old and new
                {
                    songs.Add(i-inc, songs[i]);
                    songs.Remove(i);
                }
                songs.Add(replaced.Key, replaced.Value);
            }
            songReindex();
        }

        static void songReshuffle() //randomize list
        {
            if (confirmMsg("Želite li pomiješati pjesme?"))
            {
                var rand = new Random();
                var copyDict = new Dictionary<int, string>();
                var nesto = songs;
                var oldIndex = new List<int>();
                var newIndex = new List<int>(); //list with new song index
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
