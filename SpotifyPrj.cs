using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject
{
    class Song
    {
        private string name;
        private string author;
        private double length;
        private string category;
        private string album;

        public Song(string name, string author, double length, string category, string album = "")
        {
            this.name = name;
            this.author = author;
            this.length = length;
            this.category = category;
            this.album = album;
        }

        public string getName()
        {
            return this.name;
        }

        public string getCategory()
        {
            return this.category;
        }

        public string getAuthor()
        {
            return this.author;
        }

        public string getAlbum()
        {
            return this.album;
        }
    }
    class Album
    {
        private string name;
        private List<Song> songs;

        public Album(string name, List<Song> songs)
        {
            this.name = name;
            this.songs = songs;
        }

        public List<Song> getSongs()
        {
            return this.songs;
        }

        public List<Song> searchSongs(string name)
        {
            List<Song> res = new List<Song>();
            for (int i = 0; i < this.songs.Count; i++)
            {
                if (this.songs[i].getName().Contains(name))
                    res.Add(this.songs[i]);
            }

            return res;
        }

        public string getCategory()
        {
            return this.songs[0].getCategory();
        }

        public int numberOfSongs()
        {
            return this.songs.Count;
        }
    }
    class MusicPlayer
    {
        private Queue<Song> playlist;
        private bool playing;

        public MusicPlayer()
        {
            this.playlist = new Queue<Song>();
            this.playing = false;
        }

        public void Play(Song s)
        {
            if (s != null && this.playlist.Contains(s))
            {
                Queue<Song> temp = new Queue<Song>();
                Queue<Song> newPlaylist = new Queue<Song>();

                while (this.playlist.Peek() != s)
                {
                    temp.Enqueue(this.playlist.Dequeue());
                }

                newPlaylist.Enqueue(this.playlist.Dequeue());

                while (this.playlist.Count != 0)
                {
                    temp.Enqueue(this.playlist.Dequeue());
                }

                while (temp.Count != 0)
                {
                    newPlaylist.Enqueue(temp.Dequeue());
                }
                this.playlist = newPlaylist;
            }
            else
            {
                List<Song> l = new List<Song>(new Song[] { s });
                l.AddRange(this.playlist.ToArray());
                this.playlist = new Queue<Song>(l);
            }

            this.playing = true;
        }

        public void Stop()
        {
            this.playing = false;
        }

        public Song Next()
        {
            if (this.playlist.Count == 0)
            {
                return null;
            }

            if (this.playlist.Count == 1)
            {
                this.playing = false;
            }

            return this.playlist.Dequeue();
        }

        public void Add(Song s)
        {
            if (!this.playlist.Contains(s))
                this.playlist.Enqueue(s);
        }
        public Song[] getPlaylist()
        {
            return this.playlist.ToArray();
        }

        public Song currentSong()
        {
            if (this.playing && this.playlist.Count > 0)
                return this.playlist.Peek();
            else
                return null;
        }
    }
    enum UserType { Regular, Premium, Admin }
    class User
    {
        private string name;
        private MusicPlayer mp;
        private List<Song> likedSongs;
        private List<Song> history;
        private List<Album> likedAlbums;
        private UserType type;
        private string password;
        private string email;
        private List<string> authors;
        private List<int> authorsCounters;
        private List<string> categories;
        private List<int> categoriesCounters;

        public User(string name, UserType type, string password = "", string email = "")
        {
            this.name = name;
            this.mp = new MusicPlayer();
            this.likedSongs = new List<Song>();
            this.history = new List<Song>();
            this.likedAlbums = new List<Album>();
            this.type = type;
            this.password = password;
            this.email = email;
            this.authors = new List<string>();
            this.authorsCounters = new List<int>();
            this.categories = new List<string>();
            this.categoriesCounters = new List<int>();
        }

        public void Play(Song s = null)
        {
            mp.Play(s);
        }

        public void Stop()
        {
            mp.Stop();
        }

        public void Next()
        {
            Song s = mp.Next();
            if (s == null)
            {
                return;
            }

            this.history.Add(s);

            string author = s.getAuthor();
            if (this.authors.Contains(author))
            {
                int index = this.authors.IndexOf(author);
                this.authorsCounters[index]++;
            }
            else
            {
                this.authors.Add(author);
                this.authorsCounters.Add(1);
            }

            string category = s.getCategory();
            if (this.categories.Contains(category))
            {
                int index = this.categories.IndexOf(category);
                this.categoriesCounters[index]++;
            }
            else
            {
                this.categories.Add(category);
                this.categoriesCounters.Add(1);
            }
        }

        public void Like(Song s)
        {
            if (this.likedSongs.Count > 10 && this.type == UserType.Regular)
                Console.WriteLine("Regular user can like only up to 10 songs");

            if (this.likedSongs.Contains(s))
                Console.WriteLine("You already liked this song");

            this.likedSongs.Add(s);
        }

        public Song[] getHistory()
        {
            return this.history.ToArray();
        }

        public Song[] getPlaylist()
        {
            return mp.getPlaylist();
        }

        public string getName()
        {
            return this.name;
        }

        public void Add(Song s)
        {
            this.mp.Add(s);
        }

        public List<string> getTopAuthors()
        {
            return this.authors;
        }

        public List<string> getTopCategories()
        {
            return this.categories;
        }
    }
    class SpotifyPrj
    {
        private static List<Album> allAlbums;
        private static User user;

        static void SearchSongByName(string name)
        {
            List<Song> searchResult = new List<Song>();

            for (int i = 0; i < allAlbums.Count; i++)
            {
                searchResult.AddRange(allAlbums[i].searchSongs(name));
            }

            for (int i = 0; i < searchResult.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + searchResult[i].getName());
            }
            if (searchResult.Count == 0)
            {
                Console.WriteLine("No songs");
                return;
            }

            Console.WriteLine("Choose a song: ");
            int index = int.Parse(Console.ReadLine());

            if (index > searchResult.Count || index < 1)
                Console.WriteLine("Incorrect");

            Console.WriteLine("Choose an option: ");
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Add to playlist");
            Console.WriteLine("3. Like this song");
            Console.WriteLine("4. Back");
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    user.Play(searchResult[index - 1]);
                    break;
                case 2:
                    user.Add(searchResult[index - 1]);
                    break;
                case 3:
                    user.Like(searchResult[index - 1]);
                    break;
                case 4:
                    break;
            }

        }

        static void SearchSongByCategory(string category)
        {
            List<Song> searchResult = new List<Song>();

            for (int i = 0; i < allAlbums.Count; i++)
            {
                List<Song> songs = allAlbums[i].getSongs();
                for (int j = 0; j < songs.Count; j++)
                {
                    if (songs[j].getCategory() == category)
                    {
                        searchResult.Add(songs[j]);
                        Console.WriteLine((searchResult.Count) + ". " + songs[j].getName());
                    }
                }

            }
            if (searchResult.Count == 0)
            {
                Console.WriteLine("No songs");
                return;
            }

            Console.WriteLine("Choose a song: ");
            int index = int.Parse(Console.ReadLine());

            if (index > searchResult.Count || index < 1)
                Console.WriteLine("Incorrect");

            Console.WriteLine("Choose an option: ");
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Add to playlist");
            Console.WriteLine("3. Like this song");
            Console.WriteLine("4. Back");
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    user.Play(searchResult[index - 1]);
                    break;
                case 2:
                    user.Add(searchResult[index - 1]);
                    break;
                case 3:
                    user.Like(searchResult[index - 1]);
                    break;
                case 4:
                    break;
            }

        }

        static void PrintPlaylist()
        {
            Console.WriteLine("Your playlist: ");
            Song[] arr = user.getPlaylist();
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i].getName());
            }
        }

        static void PrintLast10Songs()
        {
            Console.WriteLine("Your last songs:");
            Song[] arr = user.getHistory();
            for (int i = 0; i < arr.Length && i < 10; i++)
            {
                Console.WriteLine(arr[i].getName());
            }
        }

        static void PrintStats()
        {
            List<string> albumNames = new List<string>();
            Song[] arr = user.getHistory();

            Console.WriteLine("Your last albums:");
            for (int i = 0; i < arr.Length; i++)
            {
                string album = arr[i].getAlbum();
                if (!albumNames.Contains(album) && albumNames.Count < 4)
                {
                    Console.WriteLine(album);
                    albumNames.Add(album);
                }
            }

            Console.WriteLine("Your favorite categories:");
            List<string> cl = user.getTopCategories();
            for (int i = 0; i < cl.Count; i++)
                Console.WriteLine(cl[i]);

            Console.WriteLine("Your favorite authors:");
            List<string> al = user.getTopAuthors();
            for (int i = 0; i < al.Count; i++)
                Console.WriteLine(al[i]);
        }

        static void Main(string[] args)
        {
            Song[] radioHeadAlbumArray =
            {
                new Song("Everything in Its Right Place", "Radiohead", 4.11, "Post-Rock", "Kid A"),
                new Song("Kid A", "Radiohead", 4.44, "Post-Rock","Kid A"),
                new Song("The National Anthem", "Radiohead", 5.51, "Post-Rock","Kid A"),
                new Song("How to Disappear Completely", "Radiohead", 5.56, "Post-Rock","Kid A"),
                new Song("Treefingers", "Radiohead", 3.42, "Post-Rock","Kid A"),
                new Song("Optimistic", "Radiohead", 5.15, "Post-Rock","Kid A"),
                new Song("In Limbo", "Radiohead", 3.31, "Post-Rock","Kid A"),
                new Song("Idioteque", "Radiohead", 5.09, "Post-Rock","Kid A"),
                new Song("Morning Bell", "Radiohead", 4.35, "Post-Rock","Kid A"),
                new Song("Motion Picture Soundtrack", "Radiohead", 7.01, "Post-Rock", "Kid A")
            };

            List<Song> radioHeadAlbum = new List<Song>(radioHeadAlbumArray);
            Album a1 = new Album("Kid A", radioHeadAlbum);

            Song[] theBeatlesAlbumArray =
            {
                new Song("Come Together", "The Beatles", 4.19, "Rock","Abbey Road"),
                new Song("Something", "The Beatles", 3.02, "Rock","Abbey Road"),
                new Song("Maxwell's Silver Hammer", "The Beatles", 3.27, "Rock","Abbey Road"),
                new Song("Oh! Darling", "The Beatles", 3.27, "Rock","Abbey Road"),
                new Song("Because", "The Beatles", 2.45, "Rock","Abbey Road"),
                new Song("Golden Slumbers", "The Beatles", 1.31, "Rock", "Abbey Road")
            };

            List<Song> theBeatlesAlbum = new List<Song>(theBeatlesAlbumArray);
            Album a2 = new Album("Abbey Road", theBeatlesAlbum);

            Song[] theMJAlbumArray =
            {
                new Song("Wanna Be Startin' Somethin'", "Michael Jackson", 6.03, "R&B","Thriller"),
                new Song("Baby Be Mine", "Michael Jackson", 4.21, "R&B","Thriller"),
                new Song("The Girl Is Mine" , "Michael Jackson", 3.42, "R&B","Thriller"),
                new Song("Thriller", "Michael Jackson", 5.58, "R&B","Thriller"),
                new Song("Beat It", "Michael Jackson", 4.18, "R&B","Thriller"),
                new Song("Billie Jean", "Michael Jackson", 4.54, "R&B","Thriller"),
                new Song("Human Nature", "Michael Jackson", 4.06, "R&B","Thriller"),
                new Song("P.Y.T. (Pretty Young Thing)", "Michael Jackson", 3.59, "R&B","Thriller"),
                new Song("The Lady in My Life", "Michael Jackson", 5.00, "R&B", "Thriller")
            };

            List<Song> theMJAlbum = new List<Song>(theMJAlbumArray);
            Album a3 = new Album("Thriller", theMJAlbum);

            Song[] theKWAlbumArray =
            {
                new Song("Power", "Kanye West", 4.52, "Hip-Hop", "My Beautiful Dark Twisted Fantasy"),
                new Song("All of the Lights", "Kanye West", 4.59, "Hip-Hop","My Beautiful Dark Twisted Fantasy"),
                new Song("Monster", "Kanye West", 6.18, "Hip-Hop","My Beautiful Dark Twisted Fantasy"),
                new Song("Runaway", "Kanye West", 9.07, "Hip-Hop","My Beautiful Dark Twisted Fantasy"),
                new Song("Blame Game", "Kanye West", 7.49, "Hip-Hop", "My Beautiful Dark Twisted Fantasy")
            };

            List<Song> theKWAlbum = new List<Song>(theKWAlbumArray);
            Album a4 = new Album("My Beautiful Dark Twisted Fantasy", theKWAlbum);

            Song[] theKLAlbumArray =
            {
                new Song("King Kunta", "Kendrick Lamar", 3.54, "Experimental Hip-Hop","To Pimp a Butterfly"),
                new Song("Alright", "Kendrick Lamar", 3.39, "Experimental Hip-Hop","To Pimp a Butterfly"),
                new Song("The Blacker the Berry", "Kendrick Lamar", 5.28, "Experimental Hip-Hop","To Pimp a Butterfly"),
                new Song("U", "Kendrick Lamar", 4.28, "Experimental Hip-Hop","To Pimp a Butterfly"),
                new Song("I", "Kendrick Lamar", 5.36, "Experimental Hip-Hop","To Pimp a Butterfly")
            };

            List<Song> theKLAlbum = new List<Song>(theKLAlbumArray);
            Album a5 = new Album("To Pimp a Butterfly", theKLAlbum);
            allAlbums = new List<Album>(new Album[] { a1, a2, a3, a4, a5 });

            Console.WriteLine("Hello! Welcome to our version of Spotify! Enjoy!");
            Console.WriteLine("Sign-up, insert name:");
            string name = Console.ReadLine();
            user = new User(name, UserType.Regular);
            bool run = true;
            Console.WriteLine("Hi " + name + "!");

            while (run)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Search a song by name");
                Console.WriteLine("2. Search a song by category");
                Console.WriteLine("3. Play");
                Console.WriteLine("4. Stop");
                Console.WriteLine("5. Next");
                Console.WriteLine("6. Show playlist");
                Console.WriteLine("7. Show my stats");
                Console.WriteLine("8. Exit");

                int option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Enter the name of the song:");
                        string sName = Console.ReadLine();
                        SearchSongByName(sName);
                        break;
                    case 2:
                        Console.WriteLine("Enter the category:");
                        string category = Console.ReadLine();
                        SearchSongByCategory(category);
                        break;
                    case 3:
                        user.Play();
                        break;
                    case 4:
                        user.Stop();
                        break;
                    case 5:
                        user.Next();
                        break;
                    case 6:
                        PrintPlaylist();
                        break;
                    case 7:
                        PrintLast10Songs();
                        PrintStats();
                        break;
                    case 8:
                        Console.WriteLine("Bye Bye!");
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Wrong option");
                        break;
                }
            }
        }
    }
}